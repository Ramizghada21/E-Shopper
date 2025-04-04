using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectUI.Admin
{
    public partial class ProductList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }
        private void LoadProducts()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Util.getConnection()))
                {
                    // ✅ Fixed SQL Query: Joining Products with Categories and SubCategories
                    string query = @"
                        SELECT 
                            p.ProductId, p.ProductName, p.Price, p.Quantity, p.Sold, p.IsActive,
                            c.CategoryName, sc.SubCategoryName 
                        FROM Product p
                        LEFT JOIN Category c ON p.CategoryId = c.CategoryId
                        LEFT JOIN SubCategory sc ON p.SubCategoryId = sc.SubCategoryId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            rptProducts.DataSource = dt;
                            rptProducts.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error: " + ex.Message;
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int productId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "edit")
            {
                Response.Redirect("Product.aspx?id=" + productId);
            }
            else if (e.CommandName == "delete")
            {
                DeleteProduct(productId);
            }
        }

        private void DeleteProduct(int productId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Util.getConnection()))
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Product WHERE ProductId = @ProductId", con))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", productId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                lblMsg.Text = "Product deleted successfully!";
                lblMsg.ForeColor = System.Drawing.Color.Green;
                LoadProducts(); // Refresh the product list after deletion
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error: " + ex.Message;
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }
        }

    }
}