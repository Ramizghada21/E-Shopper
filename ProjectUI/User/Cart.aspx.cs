using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectUI.User
{
    public partial class Cart : System.Web.UI.Page
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
            List<CartItem> cart = GetCartItemsFromDB(userId);

            rptCart.DataSource = cart;
            rptCart.DataBind();

            decimal subtotal = cart.Sum(item => item.price * item.quantity);
            lblSubtotal.Text = subtotal.ToString("F2");

            decimal shipping = cart.Count > 0 ? 10 : 0;
            lblShipping.Text = shipping.ToString("F2");

            decimal total = subtotal + shipping;
            lblTotal.Text = total.ToString("F2");
        }

        private List<CartItem> GetCartItemsFromDB(int userId)
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

        protected void UpdateQuantity_Click(object sender, CommandEventArgs e)
        {
            if (Session["UserId"] == null) return;

            int userId = Convert.ToInt32(Session["UserId"]);
            int productId = Convert.ToInt32(e.CommandArgument);
            string action = e.CommandName;

            using (SqlConnection conn = new SqlConnection(Util.getConnection()))
            {
                string query = action == "IncreaseQty"
                    ? "UPDATE Cart SET Quantity = Quantity + 1 WHERE UserId = @UserId AND ProductId = @ProductId"
                    : "UPDATE Cart SET Quantity = Quantity - 1 WHERE UserId = @UserId AND ProductId = @ProductId AND Quantity > 1";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProductId", productId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadCart();
        }

        protected void RemoveItem_Click(object sender, CommandEventArgs e)
        {
            if (Session["UserId"] == null) return;

            int userId = Convert.ToInt32(Session["UserId"]);
            int productId = Convert.ToInt32(e.CommandArgument);

            using (SqlConnection conn = new SqlConnection(Util.getConnection()))
            {
                string query = "DELETE FROM Cart WHERE UserId = @UserId AND ProductId = @ProductId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProductId", productId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadCart();
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            Response.Redirect("Checkout.aspx");
        }
    }
}