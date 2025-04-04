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
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardData();
                LoadRecentOrders();
            }
        }

        private void LoadDashboardData()
        {
            using (SqlConnection con = new SqlConnection(Util.getConnection()))
            {
                string query = @"
            SELECT 
                (SELECT COUNT(*) FROM Product) AS TotalProducts,
                (SELECT COUNT(*) FROM User_tbl WHERE Email != 'admin@gmail.com') AS TotalUsers,
                (SELECT COUNT(*) FROM Orders) AS TotalOrders,
                (SELECT COUNT(*) FROM Orders WHERE Status = 'Completed') AS TotalSales,
                (SELECT SUM(p.Price * o.Quantity) 
                 FROM Orders o
                 INNER JOIN Product p ON o.ProductId = p.ProductId
                 WHERE o.Status = 'Completed') AS TotalRevenue";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lblTotalProducts.Text = reader["TotalProducts"].ToString();
                        lblTotalUsers.Text = reader["TotalUsers"].ToString();
                        lblTotalOrders.Text = reader["TotalOrders"].ToString();
                        lblTotalSales.Text = reader["TotalSales"].ToString();
                        lblTotalSales.Text = reader["TotalRevenue"] != DBNull.Value ? reader["TotalRevenue"].ToString() : "0"; // Handle NULL values
                    }
                }
            }
        }

        private void LoadRecentOrders()
        {
            using (SqlConnection con = new SqlConnection(Util.getConnection()))
            {
                string query = @"
                    SELECT TOP 5 OrderNo, User_tbl.Name AS UserName,Orders.Status, Orders.OrderDate 
                    FROM Orders
                    INNER JOIN User_tbl ON Orders.UserId = User_tbl.UserId
                    ORDER BY Orders.OrderDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    rptRecentOrders.DataSource = dt;
                    rptRecentOrders.DataBind();
                }
            }
        }
    }
}