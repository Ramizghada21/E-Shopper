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
    public partial class User : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] != null) 
                {
                    lblWelcome.Text = "Welcome, " + Session["UserName"];
                    pnlAuthButtons.Visible = false;
                    pnlLogout.Visible = true; 
                }
                else
                {
                    lblWelcome.Text = "";
                    pnlAuthButtons.Visible = true; 
                    pnlLogout.Visible = false; 
                }
                UpdateCartCount();
                LoadCategories();
            }

            if (Request.Url.AbsoluteUri.ToString().Contains("Default.aspx"))
            {
                Control sc = (Control)Page.LoadControl("SliderUserControl.ascx");
                pnlSliderUC.Controls.Add(sc);
            }
            if (Session["UserId"] == null)
            {
                Response.Redirect("login.aspx");
            }
            else
            {
                lblWelcome.Text = "Welcome, " + Session["UserName"].ToString();

            }
            //if (Session["Cart"] != null)
            //{
            //    List<CartItem> cart = (List<CartItem>)Session["Cart"];
            //    rptCart.DataSource = cart;
            //    rptCart.DataBind();
            //}
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/login.aspx"); 
        }
        private void UpdateCartCount()
        {
            if (Session["Cart"] != null)
            {
                List<CartItem> cart = (List<CartItem>)Session["Cart"];
                int totalItems = cart.Sum(item => item.quantity); // Sum up all quantities
                lblCartCount.Text = totalItems.ToString();
            }
            else
            {
                lblCartCount.Text = "0";
            }
        }
        private void LoadCategories()
        {
            
            using (SqlConnection con = new SqlConnection(Util.getConnection()))
            {
                string query = "SELECT CategoryId, CategoryName FROM Category WHERE IsActive = 1 ORDER BY createdDate DESC";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        rptCategories.DataSource = dt;
                        rptCategories.DataBind();

                    }
                }
            }
        }
    }
}