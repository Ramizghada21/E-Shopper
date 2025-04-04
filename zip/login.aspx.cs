using System;
using System.Data.SqlClient;
using System.Configuration;

namespace ProjectUI
{
    public partial class login : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                lblMsg.Text = "Email and password are required!";
                lblMsg.Visible = true;
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                string query = "SELECT Name, Email FROM User_tbl WHERE Email = @Email AND Password = @Password";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read()) // If user exists
                    {
                        string name = dr["Name"].ToString();
                        lblMsg.Text = "Welcome, " + name + "!";
                        lblMsg.ForeColor = System.Drawing.Color.Green;
                        lblMsg.Visible = true;
                        Response.Redirect("WebForm1.aspx");
                    }
                    else
                    {
                        lblMsg.Text = "Invalid email or password!";
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        lblMsg.Visible = true;
                    }
                    dr.Close();
                }
            }
        }
    }
}
