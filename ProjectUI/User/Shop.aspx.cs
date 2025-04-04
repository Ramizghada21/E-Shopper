using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace ProjectUI.User
{
    public partial class Shop : System.Web.UI.Page
    {
        private int pageSize = 9;
        private int totalPages = 0;
        

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                ViewState["CurrentPage"] = 1;
                LoadCategories();
                BindAllProducts();
            }
        }

        private int GetUserIdFromSession()
        {
            if (Session["UserId"] != null)
            {
                int userId;
                if (int.TryParse(Session["UserId"].ToString(), out userId))
                {
                    return userId;
                }
            }
            return -1; // Return -1 if userId is not found or invalid
        }

        private void LoadCategories()
        {
            using (SqlConnection con = new SqlConnection(Util.getConnection()))
            {
                SqlCommand cmd = new SqlCommand("SELECT CategoryId, CategoryName FROM Category WHERE IsActive = 1", con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    ddlCategory.DataSource = reader;
                    ddlCategory.DataTextField = "CategoryName";
                    ddlCategory.DataValueField = "CategoryId";
                    ddlCategory.DataBind();
                }
            }
            ddlCategory.Items.Insert(0, new ListItem("-- Select Category --", "0"));
        }

        private void LoadSubCategories(int categoryId)
        {
            using (SqlConnection con = new SqlConnection(Util.getConnection()))
            {
                SqlCommand cmd = new SqlCommand("SELECT SubCategoryId, SubCategoryName FROM SubCategory WHERE CategoryId = @CategoryId AND IsActive = 1", con);
                cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = categoryId;
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    ddlSubCategory.DataSource = reader;
                    ddlSubCategory.DataTextField = "SubCategoryName";
                    ddlSubCategory.DataValueField = "SubCategoryId";
                    ddlSubCategory.DataBind();
                }
            }
            ddlSubCategory.Items.Insert(0, new ListItem("-- Select SubCategory --", "0"));
        }

        private DataTable GetProducts(string action, int? categoryId = null, int? subCategoryId = null, string searchTerm = null)
        {
            using (SqlConnection con = new SqlConnection(Util.getConnection()))
            {
                SqlCommand cmd = new SqlCommand("Product_Crud", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", action);

                if (categoryId.HasValue && categoryId.Value > 0)
                    cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = categoryId.Value;

                if (subCategoryId.HasValue && subCategoryId.Value > 0)
                    cmd.Parameters.Add("@SubCategoryId", SqlDbType.Int).Value = subCategoryId.Value;

                if (!string.IsNullOrEmpty(searchTerm))
                    cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 100).Value = searchTerm;

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
        }

        private void BindAllProducts()
        {
            DataTable dt = GetProducts("ACTIVEPRODUCT");
            BindProductsWithPagination(dt);
        }

        private void BindProductsWithPagination(DataTable dt)
        {
            int currentPage = Convert.ToInt32(ViewState["CurrentPage"]);
            totalPages = (int)Math.Ceiling((double)dt.Rows.Count / pageSize);
            ViewState["TotalPages"] = totalPages;

            if (dt.Rows.Count > 0)
            {
                DataTable pagedData = dt.AsEnumerable()
                                        .Skip((currentPage - 1) * pageSize)
                                        .Take(pageSize)
                                        .CopyToDataTable();

                rptProducts.DataSource = pagedData;
                rptProducts.DataBind();

                // Set up pagination
                rptPagination.DataSource = Enumerable.Range(1, totalPages).ToList();
                rptPagination.DataBind();
                rptPagination.Visible = totalPages > 1;
                noProductsPanel.Visible = false;
            }
            else
            {
                rptProducts.DataSource = null;
                rptProducts.DataBind();
                rptPagination.Visible = false;
                noProductsPanel.Visible = true;
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSubCategory.Items.Clear();
            int categoryId = Convert.ToInt32(ddlCategory.SelectedValue);

            if (categoryId > 0)
            {
                LoadSubCategories(categoryId);
                BindProductsWithPagination(GetProducts("PRDTBYCATEGORY", categoryId));
            }
            else
            {
                ddlSubCategory.Items.Insert(0, new ListItem("-- Select SubCategory --", "0"));
                BindAllProducts();
            }

            ViewState["CurrentPage"] = 1;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                BindProductsWithPagination(GetProducts("PRDTSEARCH", null, null, searchTerm));
            }
            else
            {
                BindAllProducts();
            }
            ViewState["CurrentPage"] = 1;
        }

        protected void lnkPage_Click(object sender, EventArgs e)
        {
            ViewState["CurrentPage"] = Convert.ToInt32(((LinkButton)sender).CommandArgument);
            BindAllProducts();
        }

        protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int productId;
            if (int.TryParse(e.CommandArgument.ToString(), out productId))
            {
                if (e.CommandName == "ViewDetails")
                {
                    Response.Redirect($"ProductDetails.aspx?id={productId}");
                }
                else if (e.CommandName == "AddToCart")
                {
                    int userId = GetUserIdFromSession();
                    if (userId == -1)
                    {
                        lblUserId.Text =  "user id" + userId;
                        ScriptManager.RegisterStartupScript(this, GetType(), "SessionExpired",
                            "alert('Please log in to add products to the cart.');", true);
                        return;
                    }
                    AddToCart(productId,userId);
                }
            }
        }

        private void AddToCart(int productId,int userId)
        {
            using (SqlConnection con = new SqlConnection(Util.getConnection()))
            {
                con.Open();

                // 🔹 Check if UserId exists
                SqlCommand checkUserCmd = new SqlCommand("SELECT COUNT(*) FROM User_tbl WHERE UserId = @UserId", con);
                checkUserCmd.Parameters.AddWithValue("@UserId", userId);
                int userExists = (int)checkUserCmd.ExecuteScalar();

                if (userExists == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "UserNotFound",
                        "alert('User does not exist. Please log in again.');", true);
                    return;
                }

                // 🔹 Check if ProductId exists in Cart for this UserId
                SqlCommand checkCmd = new SqlCommand("SELECT Quantity FROM Cart WHERE ProductId = @ProductId AND UserId = @UserId", con);
                checkCmd.Parameters.AddWithValue("@ProductId", productId);
                checkCmd.Parameters.AddWithValue("@UserId", userId);

                object result = checkCmd.ExecuteScalar();

                if (result != null) // Product already in cart
                {
                    int currentQuantity = Convert.ToInt32(result);
                    SqlCommand updateCmd = new SqlCommand("UPDATE Cart SET Quantity = @Quantity WHERE ProductId = @ProductId AND UserId = @UserId", con);
                    updateCmd.Parameters.AddWithValue("@ProductId", productId);
                    updateCmd.Parameters.AddWithValue("@UserId", userId);
                    updateCmd.Parameters.AddWithValue("@Quantity", currentQuantity + 1);
                    updateCmd.ExecuteNonQuery();
                }
                else // Product not in cart, insert new
                {
                    SqlCommand insertCmd = new SqlCommand("INSERT INTO Cart (ProductId, Quantity, UserId, CreatedDate) VALUES (@ProductId, @Quantity, @UserId, @CreatedDate)", con);
                    insertCmd.Parameters.AddWithValue("@ProductId", productId);
                    insertCmd.Parameters.AddWithValue("@Quantity", 1);
                    insertCmd.Parameters.AddWithValue("@UserId", userId);
                    insertCmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    insertCmd.ExecuteNonQuery();
                }

                // Update cart UI
                UpdateSessionCart(userId);

                ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMessage",
                    "alert('Product added to cart successfully!');", true);
            }
        }



        private void UpdateSessionCart(int userId)
        {
            using (SqlConnection con = new SqlConnection(Util.getConnection()))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(@"SELECT c.ProductId, p.ProductName, p.Price,c.Quantity 
                                          FROM Cart c 
                                          JOIN Product p ON c.ProductId = p.ProductId 
                                          WHERE c.UserId = @UserId", con);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                List<CartItem> cart = new List<CartItem>();

                foreach (DataRow row in dt.Rows)
                {
                    //string imageUrl = "";
                    //string[] images = row["Image1"].ToString().Split(';');
                    //if (images.Length > 0)
                    //{
                    //    imageUrl = images[0];  // Pick the first image
                    //}

                    cart.Add(new CartItem
                    {
                        productId = Convert.ToInt32(row["ProductId"]),
                        productName = row["ProductName"].ToString(),
                        price = Convert.ToDecimal(row["Price"]),
                        //imageUrl = imageUrl,
                        quantity = Convert.ToInt32(row["Quantity"])
                    });
                }

                // Store updated cart in session
                Session["Cart"] = cart;
            }
        }

        protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int subCategoryId = Convert.ToInt32(ddlSubCategory.SelectedValue);

            if (subCategoryId > 0)
            {
                BindProductsWithPagination(GetProducts("PRDTBYSUBCATEGORY", null, subCategoryId));
            }
            else
            {
                BindAllProducts();
            }

            ViewState["CurrentPage"] = 1;
        }
    }

    public class CartItem
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public decimal price { get; set; }
        public string imageUrl { get; set; }
        public int quantity { get; set; }
        public decimal SubTotal
        {
            get { return price * quantity; }
        }
    }
}
