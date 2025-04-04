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
    public partial class SubCategory : System.Web.UI.Page
    {
        SqlCommand cmd;
        SqlConnection con;
        SqlDataAdapter da;
        DataTable dt;


        protected void Page_Load(object sender, EventArgs e)
        {
            Session["breadCumbTitle"] = "Manage SubCategory";
            Session["breadCumbPage"] = "Sub-Category";
            if (!IsPostBack)
            {
                getCategories();
                getSubCategories();
            }
            lblMsg.Visible = false;
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
            ddlCategry.DataSource = dt;
            ddlCategry.DataTextField = "CategoryName";
            ddlCategry.DataValueField = "CategoryId";
            ddlCategry.DataBind();


        }
        void getSubCategories()
        {
            con = new SqlConnection(Util.getConnection());
            cmd = new SqlCommand("SubCategory_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "GETALL");
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            rSubCategory.DataSource = dt;
            rSubCategory.DataBind();


        }
        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            string actionName = string.Empty;
            int subcategoryId = string.IsNullOrEmpty(hfSubCategoryId.Value) ? 0 : Convert.ToInt32(hfSubCategoryId.Value);

            con = new SqlConnection(Util.getConnection());
            cmd = new SqlCommand("SubCategory_Crud", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", subcategoryId == 0 ? "INSERT" : "UPDATE");
            cmd.Parameters.AddWithValue("@SubCategoryId", subcategoryId);
            cmd.Parameters.AddWithValue("@SubCategoryName", txtSubCategoryName.Text.Trim());
            cmd.Parameters.AddWithValue("@CategoryId", Convert.ToInt32(ddlCategry.SelectedValue));
            cmd.Parameters.AddWithValue("@IsActive", cbIsActive.Checked);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                lblMsg.Visible = true;
                lblMsg.Text = "Sub-Category " + (subcategoryId == 0 ? "Inserted" : "Updated") + " Successfully!";
                lblMsg.CssClass = "alert alert-success";
                getSubCategories();
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

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        void clear()
        {
            txtSubCategoryName.Text = string.Empty;
            cbIsActive.Checked = false;
            hfSubCategoryId.Value = "0";
            btnAddOrUpdate.Text = "Add";
            ddlCategry.ClearSelection();
        }

        protected void rSubCategory_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lblMsg.Visible = false;
            if (e.CommandName == "edit")
            {
                con = new SqlConnection(Util.getConnection());
                cmd = new SqlCommand("SubCategory_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "GETBYID");
                cmd.Parameters.AddWithValue("@SubCategoryId", e.CommandArgument);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                txtSubCategoryName.Text = dt.Rows[0]["SubCategoryName"].ToString();
                cbIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
                ddlCategry.SelectedValue = dt.Rows[0]["CategoryId"].ToString();
                hfSubCategoryId.Value = dt.Rows[0]["SubCategoryId"].ToString();
                btnAddOrUpdate.Text = "Update";
            }
            else if (e.CommandName == "delete")
            {
                con = new SqlConnection(Util.getConnection());
                cmd = new SqlCommand("SubCategory_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@CategoryID", e.CommandArgument);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    lblMsg.Visible = true;
                    lblMsg.Text = "Sub-Category deleted Successfully!";
                    lblMsg.CssClass = "alert alert-success";
                    getSubCategories();
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