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
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Visible = true;
                return;
            }

            // ✅ Special case for Admin Login
            if (email == "admin@gmail.com" && password == "AdminEcom")
            {
                Session["UserId"] = 0; // Admin doesn't have UserId in DB
                Session["UserName"] = "Admin";
                Session["UserEmail"] = email;
                Session["IsAdmin"] = true; // Admin flag
                Response.Redirect("~/Admin/Dashboard.aspx");
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                string query = "SELECT UserId, Name, Email, Password FROM User_tbl WHERE Email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read()) // User found
                    {
                        string dbPassword = dr["Password"].ToString();

                        if (dbPassword == password) // ✅ Correct password
                        {
                            // Retrieve user details
                            int userId = Convert.ToInt32(dr["UserId"]);
                            string name = dr["Name"].ToString();
                            string userEmail = dr["Email"].ToString();

                            // Store user details in Session
                            Session["UserId"] = userId;
                            Session["UserName"] = name;
                            Session["UserEmail"] = userEmail;
                            Session["IsLoggedIn"] = true;

                            Response.Redirect("~/User/Default.aspx"); // ✅ Redirect to Home
                        }
                        else // ❌ Incorrect password
                        {
                            lblMsg.Text = "Incorrect password!";
                            lblMsg.ForeColor = System.Drawing.Color.Red;
                            lblMsg.Visible = true;
                        }
                    }
                    else // ❌ No user found
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
