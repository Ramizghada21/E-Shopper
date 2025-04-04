using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectUI.Admin
{
    public partial class Product : System.Web.UI.Page
    {
        SqlCommand cmd;
        SqlConnection con;
        SqlDataAdapter da;
        DataTable dt, dt1;
        string[] imagePath;
        ProductDal productDal;
        ProductObj productObj;
        List<ProductImageObj> productImages = new List<ProductImageObj>();
        int defaultImageAfterEdit = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["breadCumbTitle"] = "Product";
            Session["breadCumbPage"] = "Product";
            if (!IsPostBack)
            {
                getCategories();
                if (Request.QueryString["id"] != null)
                {
                    GetProductDetails();
                }
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
            ddlCategory.DataSource = dt;
            ddlCategory.DataTextField = "CategoryName";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();
        }



        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            getSubCategories(Convert.ToInt32(ddlCategory.SelectedValue));
        }
        void getSubCategories(int categoryId)
        {
            con = new SqlConnection(Util.getConnection());
            cmd = new SqlCommand("SubCategory_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "SUBCATEGORYBYID");
            cmd.Parameters.AddWithValue("@CategoryId", categoryId);
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);
            dt1 = new DataTable();
            da.Fill(dt1);
            ddlSubCategory.Items.Clear();
            ddlSubCategory.DataSource = dt1;
            ddlSubCategory.DataTextField = "SubCategoryName";
            ddlSubCategory.DataValueField = "SubCategoryId";
            ddlSubCategory.DataBind();
            ddlSubCategory.Items.Insert(0, "Select SubCategory");
        }

        void GetProductDetails()
        {
            if (Request.QueryString["id"] != null)
            {

                int productId = Convert.ToInt32(Request.QueryString["id"]);
                productDal = new ProductDal();
                dt = productDal.ProductByIdWithImages(productId);
                if (dt.Rows.Count > 0)
                {
                    txtProductName.Text = dt.Rows[0]["ProductName"].ToString();
                    txtPrice.Text = dt.Rows[0]["Price"].ToString();
                    txtQuantity.Text = dt.Rows[0]["Quantity"].ToString();
                    txtShortDescription.Text = dt.Rows[0]["ShortDescription"].ToString();
                    txtLongDescription.Text = dt.Rows[0]["LongDescription"].ToString();
                    txtAdditionalDescription.Text = dt.Rows[0]["AdditionalDescription"].ToString();
                    string[] color = dt.Rows[0]["Color"].ToString().Split('\u002C');
                    string[] size = dt.Rows[0]["Size"].ToString().Split('\u002C');
                    for (int i = 0; i < color.Length -1; i++)
                    {
                        lboxColor.Items.FindByText(color[i]).Selected = true;
                    }
                    for (int i = 0; i < size.Length-1; i++)
                    {
                        lboxSize.Items.FindByText(size[i]).Selected = true;
                    }
                    txtCompanyName.Text = dt.Rows[0]["CompanyName"].ToString();
                    ddlCategory.SelectedValue = dt.Rows[0]["CategoryId"].ToString();
                    getSubCategories(Convert.ToInt32(dt.Rows[0]["CategoryId"]));
                    ddlSubCategory.SelectedValue = dt.Rows[0]["SubCategoryId"].ToString();
                    //cbIsCustomized.Checked = Convert.ToBoolean(dt.Rows[0]["IsCustomized"]);
                  //  cbIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
                    rblDefaultImage.SelectedIndex = Convert.ToInt32(dt.Rows[0]["DefaultImage"]);
                    hfDefImagePos.Value = (Convert.ToInt32(dt.Rows[0]["SubCategoryId"]) + 1).ToString();
                    imageProduct1.ImageUrl = "../" + dt.Rows[0]["Image1"].ToString().Substring(0, dt.Rows[0]["Image1"].ToString().IndexOf(":"));
                    imageProduct2.ImageUrl = "../" + dt.Rows[0]["Image1"].ToString().Substring(0, dt.Rows[0]["Image1"].ToString().IndexOf(":"));
                    imageProduct3.ImageUrl = "../" + dt.Rows[0]["Image1"].ToString().Substring(0, dt.Rows[0]["Image1"].ToString().IndexOf(":"));
                    imageProduct4.ImageUrl = "../" + dt.Rows[0]["Image1"].ToString().Substring(0, dt.Rows[0]["Image1"].ToString().IndexOf(":"));
                    imageProduct1.Width = 200;
                    imageProduct2.Width = 200;
                    imageProduct3.Width = 200;
                    imageProduct4.Width = 200;
                    imageProduct1.Style.Remove("display");
                    imageProduct2.Style.Remove("display");
                    imageProduct3.Style.Remove("display");
                    imageProduct4.Style.Remove("display");
                    btnAddOrUpdate.Text = "Update";
                }
            }
        }
        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {

            try
            {
                string selectedColor = string.Empty;
                string selectedSize = string.Empty;
                bool isValid = false;
                bool isValidToExecute = false;
                List<string> list = new List<string>();
                bool isImageSaved = false;
                if (Request.QueryString["id"] == null)
                {
                    if (fuFirstImage.HasFile && fuSecondImage.HasFile && fuThirdImage.HasFile && fuFourthImage.HasFile)
                    {
                        list.Add(fuFirstImage.FileName);
                        list.Add(fuSecondImage.FileName);
                        list.Add(fuThirdImage.FileName);
                        list.Add(fuFourthImage.FileName);
                        string[] fu = list.ToArray();
                        #region validate images
                        for (int i = 0; i < fu.Length; i++)
                        {
                            if (Util.isValidExtension(fu[i]))
                            {
                                isValid = true;
                            }
                            else
                            {
                                isValid = false;
                                break;
                            }
                        }
                        #endregion
                        #region
                        if (isValid)
                        {
                            imagePath = Util.getImagesPath(fu);
                            for (int i = 0; i < imagePath.Length; i++)
                            {
                                for (int j = i; j <= rblDefaultImage.Items.Count - 1;)
                                {
                                    productImages.Add
                                    (
                                        new ProductImageObj()
                                        {
                                            ImageUrl = imagePath[i],
                                            DefaultImage = Convert.ToBoolean(rblDefaultImage.Items[j].Selected)
                                        }
                                    );
                                    break;
                                }
                                #region saving all images
                                if (i == 0)
                                {
                                    fuFirstImage.PostedFile.SaveAs(Server.MapPath("~/images/product/") + imagePath[i].Replace("images/product/", ""));
                                    isImageSaved = true;
                                }
                                else if (i == 1)
                                {
                                    fuSecondImage.PostedFile.SaveAs(Server.MapPath("~/images/product/") + imagePath[i].Replace("images/product/", ""));
                                    isImageSaved = true;
                                }
                                else if (i == 2)
                                {
                                    fuThirdImage.PostedFile.SaveAs(Server.MapPath("~/images/product/") + imagePath[i].Replace("images/product/", ""));
                                    isImageSaved = true;
                                }
                                else if (i == 3)
                                {
                                    fuFourthImage.PostedFile.SaveAs(Server.MapPath("~/images/product/") + imagePath[i].Replace("images/product/", ""));
                                    isImageSaved = true;
                                }
                                #endregion
                            }
                            #region saving new product
                            if (isImageSaved)
                            {
                                selectedColor = Util.getItemWithCommaSeparater(lboxColor);
                                selectedSize = Util.getItemWithCommaSeparater(lboxSize);
                                productDal = new ProductDal();
                                productObj = new ProductObj()
                                {
                                    ProductId = Request.QueryString["id"] == null ? 0 : Convert.ToInt32(Request.QueryString["id"]),
                                    ProductName = txtProductName.Text.Trim(),
                                    ShortDescription = txtShortDescription.Text.Trim(),
                                    LongDescription = txtLongDescription.Text.Trim(),
                                    AdditionalDescription = txtAdditionalDescription.Text.Trim(),
                                    Price = Convert.ToDecimal(txtPrice.Text.Trim()),
                                    Quantity = Convert.ToInt32(txtQuantity.Text.Trim()),
                                    Color = selectedColor,
                                    Size = selectedSize,
                                    CompanyName = txtCompanyName.Text.Trim(),
                                    CategoryId = Convert.ToInt32(ddlCategory.SelectedValue),
                                    SubCategoryId = Convert.ToInt32(ddlSubCategory.SelectedValue),
                                    IsCustomized = cbIsCustomized.Checked,
                                    IsActive = cbIsActive.Checked,
                                    ProductImages = productImages
                                };
                                int r = productDal.AddUpdateProduct(productObj);
                                if (r > 0)
                                {
                                    DisplayMessage("Product inserted successfully!", "success");
                                    Response.AddHeader("REFRESH", "2;URL=ProductList.aspx");
                                    Clear();
                                }
                                else
                                {
                                    DeleteFile(imagePath);
                                    DisplayMessage("Cannot save record right now!", "warning");
                                }
                            }
                            else
                            {
                                DeleteFile(imagePath);
                            }
                            #endregion
                        }
                        else
                        {
                            DisplayMessage("Please Select .jpg, .jpeg, .png file for image!", "warning");
                        }
                        #endregion

                    }
                    else
                    {
                        DisplayMessage("Please Select all the product images", "warning");
                    }
                }
                else
                {
                    if (fuFirstImage.HasFile && fuSecondImage.HasFile && fuThirdImage.HasFile && fuFourthImage.HasFile)
                    {
                        list.Add(fuFirstImage.FileName);
                        list.Add(fuSecondImage.FileName);
                        list.Add(fuThirdImage.FileName);
                        list.Add(fuFourthImage.FileName);
                        string[] fu = list.ToArray();
                        #region validate images
                        for (int i = 0; i < fu.Length; i++)
                        {
                            if (Util.isValidExtension(fu[i]))
                            {
                                isValid = true;
                            }
                            else
                            {
                                isValid = false;
                                break;
                            }
                        }
                        #endregion
                        if (isValid)
                        {
                            imagePath = Util.getImagesPath(fu);
                            for (int i = 0; i < imagePath.Length; i++)
                            {
                                for (int j = i; j <= rblDefaultImage.Items.Count - 1;)
                                {
                                    productImages.Add
                                    (
                                        new ProductImageObj()
                                        {
                                            ImageUrl = imagePath[i],
                                            DefaultImage = Convert.ToBoolean(rblDefaultImage.Items[j].Selected)
                                        }
                                    );
                                    break;
                                }
                                #region saving all images
                                if (i == 0)
                                {
                                    fuFirstImage.PostedFile.SaveAs(Server.MapPath("~/images/product/") + imagePath[i].Replace("images/product/", ""));
                                    isImageSaved = true;
                                }
                                else if (i == 1)
                                {
                                    fuSecondImage.PostedFile.SaveAs(Server.MapPath("~/images/product/") + imagePath[i].Replace("images/product/", ""));
                                    isImageSaved = true;
                                }
                                else if (i == 2)
                                {
                                    fuThirdImage.PostedFile.SaveAs(Server.MapPath("~/images/product/") + imagePath[i].Replace("images/product/", ""));
                                    isImageSaved = true;
                                }
                                else if (i == 3)
                                {
                                    fuFourthImage.PostedFile.SaveAs(Server.MapPath("~/images/product/") + imagePath[i].Replace("images/product/", ""));
                                    isImageSaved = true;
                                }
                            }
                            #endregion
                            if (isImageSaved)
                            {
                                isValidToExecute = true;
                            }
                            else
                            {
                                DeleteFile(imagePath);
                            }
                        }
                    }
                    else if (fuFirstImage.HasFile || fuSecondImage.HasFile || fuThirdImage.HasFile || fuFourthImage.HasFile)
                    {
                        DisplayMessage("Please add all 4 product images if want to update images", "warning");
                    }
                    else
                    {
                        // Update product without images
                        if (Convert.ToInt32(hfDefImagePos.Value) != Convert.ToInt32(rblDefaultImage.SelectedValue))
                        {
                            defaultImageAfterEdit = Convert.ToInt32(rblDefaultImage.SelectedValue);
                        }
                        isValidToExecute = true;
                    }
                    #region updating product
                    if (isValidToExecute)
                    {
                        selectedColor = Util.getItemWithCommaSeparater(lboxColor);
                        selectedSize = Util.getItemWithCommaSeparater(lboxSize);
                        productDal = new ProductDal();
                        productObj = new ProductObj()
                        {
                            ProductId = Convert.ToInt32(Request.QueryString["id"]),
                            ProductName = txtProductName.Text.Trim(),
                            ShortDescription = txtShortDescription.Text.Trim(),
                            LongDescription = txtLongDescription.Text.Trim(),
                            AdditionalDescription = txtAdditionalDescription.Text.Trim(),
                            Price = Convert.ToDecimal(txtPrice.Text.Trim()),
                            Quantity = Convert.ToInt32(txtQuantity.Text.Trim()),
                            Color = selectedColor,
                            Size = selectedSize,
                            CompanyName = txtCompanyName.Text.Trim(),
                            CategoryId = Convert.ToInt32(ddlCategory.SelectedValue),
                            SubCategoryId = Convert.ToInt32(ddlSubCategory.SelectedValue),
                            IsCustomized = cbIsCustomized.Checked,
                            IsActive = cbIsActive.Checked,
                            ProductImages = productImages,
                            DefaultImagePosition = defaultImageAfterEdit
                        };
                        int r = productDal.AddUpdateProduct(productObj);
                        if (r > 0)
                        {
                            DisplayMessage("Product Updated successfully!", "success");
                            Response.AddHeader("REFRESH", "2;URL=ProductList.aspx");
                            Clear();
                        }
                        else
                        {
                            DeleteFile(imagePath);
                            DisplayMessage("Cannot update record right now!", "warning");
                        }
                    }
                    else
                    {
                        DisplayMessage("Something went wrong !", "danger");
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
                throw;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        void DeleteFile(string[] filePath)
        {
            for (int i = 0; i <= filePath.Length - 1; i++)
            {
                if (File.Exists(Server.MapPath("~/" + filePath[i])))
                {
                    File.Delete(Server.MapPath("~/" + filePath[i]));
                }
            }
        }
        void DisplayMessage(string ms, string cl)
        {
            lblMsg.Visible = true;
            lblMsg.Text = ms;
            lblMsg.CssClass = "alert alert-" + cl;
        }

        private void Clear()
        {
            txtProductName.Text = string.Empty;
            txtShortDescription.Text = string.Empty;
            txtLongDescription.Text = string.Empty;
            txtAdditionalDescription.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            txtCompanyName.Text = string.Empty;
            lboxColor.ClearSelection();
            lboxSize.ClearSelection();
            lboxSize.ClearSelection();
            ddlCategory.ClearSelection();
            ddlSubCategory.ClearSelection();
            rblDefaultImage.ClearSelection();
            cbIsCustomized.Checked = false;
            cbIsActive.Checked = false;
            hfDefImagePos.Value = "0";
        }
    }
}
