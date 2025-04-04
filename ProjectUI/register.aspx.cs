using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace ProjectUI
{
    public partial class register : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;

        string str = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
        string ch1, ch2, ch3;
        string[] ch = new string[3];

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                hobby();

                // Set default values before using them
                ch1 = "Null"; ch2 = "Null"; ch3 = "Null";

                if (ch.Contains("Reading")) ch1 = "Reading";
                if (ch.Contains("Travelling")) ch2 = "Travelling";
                if (ch.Contains("Playing")) ch3 = "Playing";

                con = new SqlConnection(str);

                
               string checkQuery = "SELECT COUNT(*) FROM [User_tbl] WHERE Username = @Username";
                cmd = new SqlCommand(checkQuery, con);
                cmd.Parameters.AddWithValue("@Username", txtUserName.Text.Trim());
                con.Open();

                int userExists = (int)cmd.ExecuteScalar();
                if (userExists > 0)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "<b>" + txtUserName.Text.Trim() + "</b> username already exists, try a new one..!";
                    lblMsg.CssClass = "alert alert-danger";
                    return;
                }
                bool isRegistered = Util.RegisterUser(
                txtUserName.Text.Trim(),
                txtConfirmPassword.Text.Trim(),
                txtFullName.Text.Trim(),
                rblSalesMode.SelectedValue.Trim(),
                txtEmail.Text.Trim(),
                txtMobile.Text.Trim(),
                txtAddress.Text.Trim(),
                ch1.Trim(),
                ch2.Trim(),
                ch3.Trim(),
                ddlCountry.SelectedValue
            );

                if (isRegistered)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Registration Successful!";
                    lblMsg.CssClass = "alert alert-success";
                    clear(); // Clear input fields
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Registration failed. Please try again.";
                    lblMsg.CssClass = "alert alert-danger";
                }

            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY constraint"))
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "<b>" + txtUserName.Text.Trim() + "</b> username already exists, try a new one..!";
                    lblMsg.CssClass = "alert alert-danger";
                }
                else
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
            finally
            {
                con.Close();
            }
        }

        private void clear()
        {
            txtUserName.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
            txtFullName.Text = string.Empty;
            rblSalesMode.SelectedValue = string.Empty;
            rblSalesMode.ClearSelection();
            txtEmail.Text = string.Empty;
            txtMobile.Text = string.Empty;
            txtAddress.Text = string.Empty;
            ddlCountry.ClearSelection();

            foreach (ListItem item in cblProductType.Items)
            {
                item.Selected = false;
            }
        }

        void hobby()
        {
            int index = 0;
            foreach (ListItem item in cblProductType.Items)
            {
                if (item.Selected && index < ch.Length)
                {
                    ch[index] = item.Text;
                    index++;
                }
            }
        }
    }
}
