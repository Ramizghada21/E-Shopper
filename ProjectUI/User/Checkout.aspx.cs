using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

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
                string query = @"SELECT c.ProductId, p.ProductName, p.Price, c.Quantity 
                                 FROM Cart c
                                 INNER JOIN Product p ON c.ProductId = p.ProductId
                                 WHERE c.UserId = @UserId";

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
            if (Session["UserId"] == null) return;

            int userId = Convert.ToInt32(Session["UserId"]);
            string orderNo = "ORD-" + DateTime.Now.Ticks.ToString(); // Unique Order Number

            int paymentId = InsertPaymentDetails();
            if (paymentId == 0)
            {
                Response.Write("<script>alert('Payment failed. Please try again.');</script>");
                return;
            }

            using (SqlConnection conn = new SqlConnection(Util.getConnection()))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    List<CartItem> cart = GetCartItems(userId);

                    foreach (var item in cart)
                    {
                        string insertOrderQuery = @"
                            INSERT INTO OrderDetails (OrderNo, ProductId, Quantity, UserId, Status, PaymentId, OrderDate, IsCancel)
                            VALUES (@OrderNo, @ProductId, @Quantity, @UserId, 'Pending', @PaymentId, GETDATE(), 0)";

                        SqlCommand cmd = new SqlCommand(insertOrderQuery, conn, transaction);
                        cmd.Parameters.AddWithValue("@OrderNo", orderNo);
                        cmd.Parameters.AddWithValue("@ProductId", item.productId);
                        cmd.Parameters.AddWithValue("@Quantity", item.quantity);
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@PaymentId", paymentId);

                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Response.Write("<script>alert('Order failed: " + ex.Message + "');</script>");
                    return;
                }
            }

            Response.Redirect("Default.aspx?OrderNo=" + orderNo);
        }

        private int InsertPaymentDetails()
        {
            int paymentId = 0;
            using (SqlConnection conn = new SqlConnection(Util.getConnection()))
            {
                string insertPaymentQuery = @"
                    INSERT INTO Payment (Name, CardNo, ExpiryDate, CvvNo, Address, PaymentMode)
                    OUTPUT INSERTED.PaymentId
                    VALUES (@Name, @CardNo, @ExpiryDate, @CvvNo, @Address, @PaymentMode)";

                SqlCommand cmd = new SqlCommand(insertPaymentQuery, conn);
                cmd.Parameters.AddWithValue("@Name", txtCardName.Text);
                cmd.Parameters.AddWithValue("@CardNo", txtCardNumber.Text);
                cmd.Parameters.AddWithValue("@ExpiryDate", txtExpiryDate.Text);
                cmd.Parameters.AddWithValue("@CvvNo", Convert.ToInt32(txtCVV.Text));
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@PaymentMode", ddlPaymentMode.SelectedValue);

                conn.Open();
                paymentId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return paymentId;
        }
    }
}