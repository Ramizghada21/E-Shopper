using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace ProjectUI
{
    
    public class ProductObj
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string AdditionalDescription { get; set; }
        public decimal Price{ get; set; }
        public int Quantity { get; set; }
        public string Size{ get; set; }
        public string Color { get; set; }
        public string CompanyName{ get; set; }
        //public string Tags { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public bool IsCustomized { get; set; }
        public bool IsActive { get; set; }
        public DateTime createdDate { get; set; }
        public List<ProductImageObj> ProductImages { get; set; } = new List<ProductImageObj>();
        public int DefaultImagePosition { get; set; }
    }
    public class ProductImageObj
    {
        public int ImageId{ get; set; }
        public string ImageUrl{ get; set; }
        public int ProductId { get; set; }
        public bool DefaultImage{ get; set; }
    }
    public class ProductDal
    {
        private const string V = "image";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader sdr;
        SqlDataAdapter da;
        DataTable dt;
        SqlTransaction transaction = null;

        public int AddUpdateProduct(ProductObj productBO)
        {
            int result = 0;
            int productId = 0;
            string type = "insert";
            using(con = new SqlConnection(Util.getConnection()))
            {
                try
                {
                    var productImages = productBO.ProductImages;
                    #region insert product
                    con.Open();
                    transaction = con.BeginTransaction();
                    productId = productBO.ProductId;
                    cmd = new SqlCommand("Product_Crud", con, transaction);
                    cmd.Parameters.AddWithValue("@Action", productId == 0 ? "INSERT" : "UPDATE");
                    cmd.Parameters.AddWithValue("@ProductName", productBO.ProductName);
                    cmd.Parameters.AddWithValue("@ShortDescription", productBO.ShortDescription);
                    cmd.Parameters.AddWithValue("@LongDescription", productBO.LongDescription);
                    cmd.Parameters.AddWithValue("@AdditionalDescription", productBO.AdditionalDescription);
                    cmd.Parameters.AddWithValue("@Price", productBO.Price);
                    cmd.Parameters.AddWithValue("@Quantity", productBO.Quantity);
                    cmd.Parameters.AddWithValue("@Size", productBO.Size);
                    cmd.Parameters.AddWithValue("@Color", productBO.Color);
                    cmd.Parameters.AddWithValue("@CompanyName", productBO.CompanyName);
                    cmd.Parameters.AddWithValue("@CategoryId", productBO.CategoryId);
                    cmd.Parameters.AddWithValue("@SubCategoryId", productBO.SubCategoryId);
                    cmd.Parameters.AddWithValue("@IsCustomized", productBO.IsCustomized);
                    cmd.Parameters.AddWithValue("@IsActive", productBO.IsActive);
                    if(productId > 0)
                    {
                        cmd.Parameters.AddWithValue("@ProductId", productBO.ProductId);
                        type = "update";
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    if (productId == 0)
                    {
                        cmd = new SqlCommand("Product_Crud", con, transaction);
                        cmd.Parameters.AddWithValue("@Action", "RECENT_PRODUCT");
                        cmd.CommandType = CommandType.StoredProcedure;
                        sdr = cmd.ExecuteReader();
                        while (sdr.Read())
                        {
                            productId = (int)sdr["ProductId"];
                        }
                        sdr.Close();
                    }
                    #endregion

                    #region insert product images
                    if(productId > 0)
                    {
                        if (type == "insert")
                        {
                            foreach (var i in productImages)
                            {
                                cmd = new SqlCommand("Product_Crud", con, transaction);
                                cmd.Parameters.AddWithValue("@Action", "INSERT_PROD_IMG");
                                cmd.Parameters.AddWithValue("@ImageUrl", i.ImageUrl);
                                cmd.Parameters.AddWithValue("@ProductId", productId);
                                cmd.Parameters.AddWithValue("@DefaultImage", i.DefaultImage);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.ExecuteNonQuery();
                                result = 1;
                            }
                        }
                        else
                        {
                            bool isTrue = false;
                            if(productImages.Count != 0)
                            {
                                cmd = new SqlCommand("Product_Crud", con, transaction);
                                cmd.Parameters.AddWithValue("@Action", "DELETE_PROD_IMG");
                                cmd.Parameters.AddWithValue("@ProductId", productId);
                                cmd.CommandType = CommandType.StoredProcedure;
                                isTrue = true;
                            }
                            else
                            {
                                int defaultImagePos = productBO.DefaultImagePosition;
                                if(defaultImagePos > 0)
                                {
                                    cmd = new SqlCommand("Product_Crud", con, transaction);
                                    cmd.Parameters.AddWithValue("@Action", "UPDATE_IMG_POS");
                                    cmd.Parameters.AddWithValue("@ProductId", productId);
                                    cmd.Parameters.AddWithValue("@DefaultImagePos", defaultImagePos);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.ExecuteNonQuery();
                                    result = 1;
                                }
                                else
                                {
                                    result = 1;
                                }
                            }
                            if (isTrue)
                            {
                                foreach (var i in productImages)
                                {
                                    cmd = new SqlCommand("Product_Crud", con, transaction);
                                    cmd.Parameters.AddWithValue("@Action", "INSERT_PROD_IMG");
                                    cmd.Parameters.AddWithValue("@ImageUrl", i.ImageUrl);
                                    cmd.Parameters.AddWithValue("@ProductId", productId);
                                    cmd.Parameters.AddWithValue("@DefaultImage", i.DefaultImage);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.ExecuteNonQuery();
                                    result = 1;
                                }
                            }
                        }
                    }
                    #endregion
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        result = 0;
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                    throw;
                }
            }
            return result;
        }
        public DataTable ProductByIdWithImages(int productId)
        {
            try
            {
                DataTable dt = ProductById(productId);
                dt.Columns.Add("Image2");
                dt.Columns.Add("Image3");
                dt.Columns.Add("Image4");
                dt.Columns.Add("DefaultImage");

                if (dt.Rows.Count == 0) return dt; // Handle case where no rows exist

                DataRow dr = dt.Rows[0]; // Get the first row

                string images = dr["Image1"].ToString();
                string[] imgArr = images.Split(';');

                // Find default image index
                int rb = Array.IndexOf(imgArr, "1");
                if (rb == -1) rb = 0; // Default to 0 if "1" is not found

                // Assign image values
                for (int i = 0; i < 3; i++) // Assign Image2, Image3, Image4
                {
                    if (i + 1 < imgArr.Length)
                        dr["Image" + (i + 2)] = imgArr[i + 1].Trim();
                }

                dr["DefaultImage"] = rb;

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in ProductByIdWithImages: " + ex.Message);
            }
        }

        public DataTable ProductById(int pId)
        {
            try
            {
                using(con = new SqlConnection(Util.getConnection()))
                {
                    con.Open();
                    cmd = new SqlCommand("Product_Crud", con);
                    cmd.Parameters.AddWithValue("@Action", "GETBYID");
                    cmd.Parameters.AddWithValue("@ProductId", pId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}