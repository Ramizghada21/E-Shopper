using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectUI.Admin
{
    public partial class Category : System.Web.UI.Page
    {
        SqlCommand cmd;
        SqlConnection con;
        SqlDataAdapter da;
        DataTable dt;

        // Pagination variables
        int pageIndex = 0;
        int pageSize = 5; // Number of categories per page
        PagedDataSource pg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadCumbTitle"] = "Manage Category";
                Session["breadCumbPage"] = "Category";
                lblMsg.Visible = false;

                // Set default page index if not set
                if (Session["PageIndex"] == null)
                {
                    Session["PageIndex"] = 0;
                }

                getCategories();
            }
        }

        void getCategories()
        {
            con = new SqlConnection(Util.getConnection());
            cmd = new SqlCommand("Category_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "GETALL");
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);

            // Implement Pagination
            pg = new PagedDataSource();
            pg.DataSource = dt.DefaultView;
            pg.AllowPaging = true;
            pg.PageSize = pageSize;

            // Get current page index
            if (Session["PageIndex"] != null)
            {
                pageIndex = (int)Session["PageIndex"];
            }
            pg.CurrentPageIndex = pageIndex;

            // Enable/Disable pagination buttons
            btnPrevPage.Enabled = !pg.IsFirstPage;
            btnNextPage.Enabled = !pg.IsLastPage;

            // Display current page number
            lblPageNumber.Text = "Page " + (pg.CurrentPageIndex + 1) + " of " + pg.PageCount;

            rCategory.DataSource = pg;
            rCategory.DataBind();
        }

        protected void btnNextPage_Click(object sender, EventArgs e)
        {
            if (Session["PageIndex"] != null)
            {
                pageIndex = (int)Session["PageIndex"];
            }
            pageIndex++; // Move to next page
            Session["PageIndex"] = pageIndex;
            getCategories();
        }

        protected void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (Session["PageIndex"] != null)
            {
                pageIndex = (int)Session["PageIndex"];
            }
            if (pageIndex > 0)
            {
                pageIndex--; // Move to previous page
            }
            Session["PageIndex"] = pageIndex;
            getCategories();
        }

        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            string imagePath = string.Empty, fileExtension = string.Empty;
            bool isValidToExecute = false;
            int categoryId = string.IsNullOrEmpty(hfCategoryId.Value) ? 0 : Convert.ToInt32(hfCategoryId.Value);

            con = new SqlConnection(Util.getConnection());
            cmd = new SqlCommand("Category_Crud", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", categoryId == 0 ? "INSERT" : "UPDATE");
            cmd.Parameters.AddWithValue("@CategoryId", categoryId);
            cmd.Parameters.AddWithValue("@CategoryName", txtCategoryName.Text.Trim());
            cmd.Parameters.AddWithValue("@IsActive", cbIsActive.Checked);

            if (fuCategoryImage.HasFile)
            {
                if (Util.isValidExtension(fuCategoryImage.FileName))
                {
                    string newImageName = Util.getUniqueId();
                    fileExtension = Path.GetExtension(fuCategoryImage.FileName);
                    imagePath = "images/category/" + newImageName + fileExtension;
                    fuCategoryImage.PostedFile.SaveAs(Server.MapPath("~/images/category/") + newImageName + fileExtension);
                    cmd.Parameters.AddWithValue("@CategoryImageUrl", imagePath);
                    isValidToExecute = true;
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Please select a .jpg, .png, or .jpeg format file";
                    lblMsg.CssClass = "alert alert-danger";
                    isValidToExecute = false;
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@CategoryImageUrl", DBNull.Value);
                isValidToExecute = true;
            }

            if (isValidToExecute)
            {
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    lblMsg.Visible = true;
                    lblMsg.Text = "Category " + (categoryId == 0 ? "Inserted" : "Updated") + " Successfully!";
                    lblMsg.CssClass = "alert alert-success";
                    getCategories();
                    clear();
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = ex.Message;
                    lblMsg.CssClass = "alert alert-danger";
                }
                finally
                {
                    con.Close();
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        void clear()
        {
            txtCategoryName.Text = string.Empty;
            cbIsActive.Checked = false;
            hfCategoryId.Value = "0";
            btnAddOrUpdate.Text = "Add";
            imagePreview.ImageUrl = string.Empty;
        }

        protected void rCategory_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lblMsg.Visible = false;
            if (e.CommandName == "edit")
            {
                con = new SqlConnection(Util.getConnection());
                cmd = new SqlCommand("Category_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "GETBYID");
                cmd.Parameters.AddWithValue("@CategoryID", e.CommandArgument);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                txtCategoryName.Text = dt.Rows[0]["CategoryName"].ToString();
                cbIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
                imagePreview.ImageUrl = string.IsNullOrEmpty(dt.Rows[0]["CategoryImageUrl"].ToString()) ? "../images/no_image.png" : "../" + dt.Rows[0]["CategoryImageUrl"].ToString();
                imagePreview.Height = 200;
                imagePreview.Width = 200;
                hfCategoryId.Value = dt.Rows[0]["CategoryId"].ToString();
                btnAddOrUpdate.Text = "Update";
            }
            else if (e.CommandName == "delete")
            {
                con = new SqlConnection(Util.getConnection());
                cmd = new SqlCommand("Category_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@CategoryID", e.CommandArgument);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    lblMsg.Visible = true;
                    lblMsg.Text = "Category deleted Successfully!";
                    lblMsg.CssClass = "alert alert-success";
                    getCategories();
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = ex.Message;
                    lblMsg.CssClass = "alert alert-danger";
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}
