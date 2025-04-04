using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web.UI.WebControls;

namespace ProjectUI
{
    public class Util
    {
        SqlConnection con;
        SqlDataAdapter da;
        DataTable dt;
        SqlDataReader dr;
        SqlCommand cmd;

        string s = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;

        public static string getConnection()
        {
            return ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
        }
        public static bool isValidExtension(string fileName)
        {
            bool isValid = false;
            string[] filextension = { ".jpg", ".png", ".jpeg" };
            foreach (string f in filextension)
            {
                if (fileName.Contains(f))
                {
                    isValid = true;
                    break;
                }
            }
            return isValid;
        }
        public static string getUniqueId()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }
        public static string getImageUrl(object url)
        {
            string url1 = string.Empty;
            if (string.IsNullOrEmpty(url.ToString()) || url == DBNull.Value)
            {
                url1 = "../images/no_image.png";
            }
            else
            {
                url1 = string.Format("../{0}", url);
            }
            return url1;
        }
        public static string[] getImagesPath(string[] images)
        {
            List<string> list = new List<string>();
            string fileExtension = string.Empty;
            for (int i = 0; i < images.Length; i++)
            {
                fileExtension = Path.GetExtension(images[i]);
                list.Add("images/product/" + getUniqueId().ToString() + fileExtension);
            }
            return list.ToArray();
        }
        public static string getItemWithCommaSeparater(ListBox listBox)
        {
            string selectItems = string.Empty;
            foreach (var i in listBox.GetSelectedIndices())
            {
                selectItems += listBox.Items[i].Text + ",";
            }
            return selectItems;
        }
        public static bool saveContact(string name, string email, string subject, string message)
        {
            using (SqlConnection con = new SqlConnection(Util.getConnection()))
            {
                string query = "INSERT INTO Contact (Name, Email, Subject, Mesaage, CreatedDate) VALUES (@Name, @Email, @Subject, @Message, @CreatedDate)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Subject", subject);
                    cmd.Parameters.AddWithValue("@Message", message);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        public static bool RegisterUser(
       string username, string password, string name, string gender,
       string email, string mobile, string address, string reading,
       string travelling, string playing, string country)
        {
            using (SqlConnection con = new SqlConnection(Util.getConnection()))
            {
                string query = @"
            INSERT INTO [User_tbl] 
                (Username, Password, Name, Gender, Email, Mobile, Address, Reading, Travelling, Playing, Country) 
            VALUES 
                (@Username, @Password, @Name, @Gender, @Email, @Mobile, @Address, @Reading, @Travelling, @Playing, @Country)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password); 
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Mobile", mobile);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@Reading", reading);
                    cmd.Parameters.AddWithValue("@Travelling", travelling);
                    cmd.Parameters.AddWithValue("@Playing", playing);
                    cmd.Parameters.AddWithValue("@Country", country);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0; 
                }
            }

        }
    }
}