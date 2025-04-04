using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectUI.User
{
    public partial class ProductDetails : System.Web.UI.Page
    {
        private int productId; // Declare at class level

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int productId;
                    if (int.TryParse(Request.QueryString["id"], out productId))
                    {
                        // Use productId as needed
                        LoadProductDetails(productId);
                    }
                    else
                    {
                        // Handle invalid productId scenario
                        Response.Redirect("ProductList.aspx");
                    }
                }
                else
                {
                    // Handle missing productId scenario
                    Response.Redirect("ProductList.aspx");
                }
            }
        }
        private void LoadProductDetails(int productId)
        {
            using (SqlConnection con = new SqlConnection(Util.getConnection()))
            {
                SqlCommand cmd = new SqlCommand("Product_Crud", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GETBYID");
                cmd.Parameters.AddWithValue("@ProductId", productId);

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    // Display product information
                    litProductName.Text = row["ProductName"].ToString();
                    litCategory.Text = row["CategoryName"].ToString();
                    litPrice.Text = string.Format("{0:0.00}", row["Price"]);
                    litShortDescription.Text = row["ShortDescription"].ToString();
                    litLongDescription.Text = row["LongDescription"].ToString();
                    litAdditionalInfo.Text = row["AdditionalDescription"].ToString();
                    litColor.Text = row["Color"].ToString();
                    litSize.Text = row["Size"].ToString();
                    litQuantity.Text = row["Quantity"].ToString();
                    litSold.Text = row["Sold"].ToString();
                    litCompany.Text = row["CompanyName"].ToString();

                    // Set max quantity for inventory control
                    hdnMaxQuantity.Value = row["Quantity"].ToString();

                    // Process images
                    string imageString = row["Image1"].ToString();
                    List<ProductImage> images = new List<ProductImage>();

                    if (!string.IsNullOrEmpty(imageString))
                    {
                        string[] imageArray = imageString.Split(';');
                        foreach (string img in imageArray)
                        {
                            if (!string.IsNullOrEmpty(img))
                            {
                                string[] parts = img.Split(':');
                                string url = parts[0];
                                bool isDefault = parts.Length > 1 && parts[1] == "1";

                                images.Add(new ProductImage
                                {
                                    ImageUrl = url,
                                    IsDefault = isDefault
                                });

                                // Set default image
                                if (isDefault || images.Count == 1)
                                {
                                    mainImage.ImageUrl= url;
                                }
                            }
                        }
                    }

                    // Bind image thumbnails
                    rptImages.DataSource = images;
                    rptImages.DataBind();

                    productDetails.Visible = true;
                    noProductPanel.Visible = false;
                }
                else
                {
                    ShowNoProductPanel();
                }
            }
        }

        private void LoadRelatedProducts()
        {
            using (SqlConnection con = new SqlConnection(Util.getConnection()))
            {
                SqlCommand cmd = new SqlCommand("Product_Crud", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "TOP_5_PRODUCTS");

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                rptRelatedProducts.DataSource = dt;
                rptRelatedProducts.DataBind();
            }
        }

        private void ShowNoProductPanel()
        {
            productDetails.Visible = false;
            noProductPanel.Visible = true;
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            int quantity = 1;
            if (int.TryParse(txtQuantity.Text, out quantity))
            {
                if (quantity < 1)
                    quantity = 1;
            }

            AddToCart(quantity,productId);

            // Show success message
            ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMessage",
                "alert('Product added to cart successfully!');", true);
        }

        protected void btnBuyNow_Click(object sender, EventArgs e)
        {
            int quantity = 1;
            if (int.TryParse(txtQuantity.Text, out quantity))
            {
                if (quantity < 1)
                    quantity = 1;
            }

            AddToCart(quantity,productId);

            // Redirect to checkout
            Response.Redirect("Checkout.aspx");
        }
        private void AddToCart(int quantity,int productId)
        {
            using (SqlConnection con = new SqlConnection(Util.getConnection()))
            {
                SqlCommand cmd = new SqlCommand("Product_Crud", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GETBYID");
                cmd.Parameters.AddWithValue("@ProductId", productId);

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    // Get or create shopping cart
                    List<CartItem> cart;
                    if (Session["Cart"] == null)
                    {
                        cart = new List<CartItem>();
                    }
                    else
                    {
                        cart = (List<CartItem>)Session["Cart"];
                    }

                    // Check if product is already in cart
                    CartItem existingItem = cart.FirstOrDefault(p => p.productId == productId);
                    if (existingItem != null)
                    {
                        existingItem.quantity += quantity;
                    }
                    else
                    {
                        string imageUrl = "";
                        string[] images = row["Image1"].ToString().Split(';');
                        if (images.Length > 0)
                        {
                            string[] imgParts = images[0].Split(':');
                            imageUrl = imgParts[0];
                        }

                        cart.Add(new CartItem
                        {
                            productId = productId,
                            productName = row["ProductName"].ToString(),
                            price = Convert.ToDecimal(row["Price"]),
                            imageUrl = imageUrl,
                            quantity = quantity
                        });
                    }

                    Session["Cart"] = cart;
                }
            }
        }
    }

    public class ProductImage
    {
        public string ImageUrl { get; set; }
        public bool IsDefault { get; set; }
    }

}