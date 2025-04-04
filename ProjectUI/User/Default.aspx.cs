using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace ProjectUI.User
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
            }
            if (Session["UserId"] == null) // If user is not logged in
            {
                Response.Redirect("login.aspx");
            }
        }
        private void LoadCategories()
        {
           
            using (SqlConnection con = new SqlConnection(Util.getConnection()))
            {
                string query = "SELECT CategoryId, CategoryName, CategoryImageUrl FROM Category WHERE IsActive = 1 ORDER BY createdDate DESC";
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