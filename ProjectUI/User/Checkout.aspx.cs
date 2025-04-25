using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Razorpay.Api;

namespace ProjectUI.User
{
    public partial class Checkout : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCart();
            }
        }

        private void LoadCart()
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            List<CartItem> cart = GetCartItems(userId);

            rptOrderSummary.DataSource = cart;
            rptOrderSummary.DataBind();

            decimal total = cart.Sum(item => item.price * item.quantity);
            lblTotal.Text = total.ToString("F2");
        }

        private List<CartItem> GetCartItems(int userId)
        {
            List<CartItem> cartItems = new List<CartItem>();

            using (SqlConnection conn = new SqlConnection(Util.getConnection()))
            {
                string query = "SELECT c.ProductId, p.ProductName, p.Price, c.Quantity FROM Cart c INNER JOIN Product p ON c.ProductId = p.ProductId WHERE c.UserId = @UserId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cartItems.Add(new CartItem
                    {
                        productId = Convert.ToInt32(reader["ProductId"]),
                        productName = reader["ProductName"].ToString(),
                        price = Convert.ToDecimal(reader["Price"]),
                        quantity = Convert.ToInt32(reader["Quantity"])
                    });
                }
            }
            return cartItems;
        }

        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            string orderNo = "ORD-" + DateTime.Now.Ticks;
            decimal totalAmount = Convert.ToDecimal(lblTotal.Text);

            var client = new RazorpayClient("rzp_test_LWvBuAmAHDdJS8", "wZHmdNuX039PuLqc3RT96CXV");
            Dictionary<string, object> options = new Dictionary<string, object>
            {
                { "amount", totalAmount * 100 }, // Amount in paise
                { "currency", "INR" },
                { "receipt", orderNo },
                { "payment_capture", 1 }
            };

            Order order = client.Order.Create(options);
            string paymentId = order["id"].ToString();

            if (!string.IsNullOrEmpty(paymentId))
            {
                ProcessOrder(userId, orderNo, paymentId);
                Response.Redirect("OrderSuccess.aspx?OrderNo=" + orderNo);
            }
            else
            {
                // Handle payment failure
            }
        }

        private void ProcessOrder(int userId, string orderNo, string paymentId)
        {
            using (SqlConnection conn = new SqlConnection(Util.getConnection()))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    List<CartItem> cart = GetCartItems(userId);
                    foreach (var item in cart)
                    {
                        string insertOrderQuery = @"INSERT INTO OrderDetails (OrderNo, ProductId, Quantity, UserId, Status, PaymentId, OrderDate, IsCancel) VALUES (@OrderNo, @ProductId, @Quantity, @UserId, 'Confirmed', @PaymentId, GETDATE(), 0)";
                        SqlCommand cmd = new SqlCommand(insertOrderQuery, conn, transaction);
                        cmd.Parameters.AddWithValue("@OrderNo", orderNo);
                        cmd.Parameters.AddWithValue("@ProductId", item.productId);
                        cmd.Parameters.AddWithValue("@Quantity", item.quantity);
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@PaymentId", paymentId);
                        cmd.ExecuteNonQuery();
                    }

                    string deleteCartQuery = "DELETE FROM Cart WHERE UserId = @UserId";
                    SqlCommand deleteCmd = new SqlCommand(deleteCartQuery, conn, transaction);
                    deleteCmd.Parameters.AddWithValue("@UserId", userId);
                    deleteCmd.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // Handle exception
                }
            }
        }
        public class CartItem
        {
            public int productId { get; set; }
            public string productName { get; set; }
            public decimal price { get; set; }
            public int quantity { get; set; }
        }
    }
}