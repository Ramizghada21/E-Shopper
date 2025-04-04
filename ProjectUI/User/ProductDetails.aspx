<%--<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="ProductDetails.aspx.cs" Inherits="ProjectUI.User.ProductDetails" %>--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="ProductDetails.aspx.cs" Inherits="ProjectUI.User.ProductDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container">
        <div class="row">
            <div class="col-md-12">
                <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="~/User/Shop.aspx" CssClass="btn btn-outline-secondary mb-3">
                    <i class="fa fa-arrow-left"></i> Back to Shop
                </asp:HyperLink>
            </div>
        </div>

        <div class="row" id="productDetails" runat="server">
            <div class="col-md-6">
                <!-- Main Product Image -->
                <div class="main-product-image mb-3">
                    <asp:Image ID="mainImage" runat="server" class="img-fluid" alt="Product" />
                    <%--<img id="mainImage" runat="server" class="img-fluid" alt="Product" />--%>
                </div>
                
                <!-- Product Thumbnails -->
                <div class="product-thumbnails">
                    <asp:Repeater ID="rptImages" runat="server">
                        <ItemTemplate>
                            <img src='<%# Eval("ImageUrl") %>' class="img-thumbnail thumbnail-image" onclick="changeMainImage(this.src)" alt="Thumbnail" />
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
            
            <div class="col-md-6">
                <div class="product-details-info">
                    <h2><asp:Literal ID="litProductName" runat="server"></asp:Literal></h2>
                    <div class="product-category mb-2">
                        <span class="badge badge-info">
                            <asp:Literal ID="litCategory" runat="server"></asp:Literal>
                        </span>
                    </div>
                    
                    <div class="product-price mb-3">
                        <h3>$<asp:Literal ID="litPrice" runat="server"></asp:Literal></h3>
                    </div>
                    
                    <div class="product-short-description mb-3">
                        <asp:Literal ID="litShortDescription" runat="server"></asp:Literal>
                    </div>
                    
                    <div class="product-details mb-4">
                        <table class="table table-bordered">
                            <tr>
                                <td><strong>Color:</strong></td>
                                <td><asp:Literal ID="litColor" runat="server"></asp:Literal></td>
                            </tr>
                            <tr>
                                <td><strong>Size:</strong></td>
                                <td><asp:Literal ID="litSize" runat="server"></asp:Literal></td>
                            </tr>
                            <tr>
                                <td><strong>Available:</strong></td>
                                <td>
                                    <asp:Literal ID="litQuantity" runat="server"></asp:Literal> in stock
                                    <span class="text-muted ml-2">(<asp:Literal ID="litSold" runat="server"></asp:Literal> sold)</span>
                                </td>
                            </tr>
                            <tr>
                                <td><strong>Brand:</strong></td>
                                <td><asp:Literal ID="litCompany" runat="server"></asp:Literal></td>
                            </tr>
                        </table>
                    </div>
                    
                    <div class="product-quantity mb-3">
                        <div class="form-group">
                            <label for="txtQuantity">Quantity:</label>
                            <div class="input-group" style="width: 150px;">
                                <div class="input-group-prepend">
                                    <button type="button" class="btn btn-outline-secondary" onclick="decrementQty()">-</button>
                                </div>
                                <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control text-center" Text="1" min="1"></asp:TextBox>
                                <div class="input-group-append">
                                    <button type="button" class="btn btn-outline-secondary" onclick="incrementQty()">+</button>
                                </div>
                            </div>
                            <asp:HiddenField ID="hdnMaxQuantity" runat="server" />
                        </div>
                    </div>
                    
                    <div class="product-actions mb-4">
                        <asp:Button ID="btnAddToCart" runat="server" Text="Add to Cart" CssClass="btn btn-primary btn-lg" OnClick="btnAddToCart_Click" />
                        <asp:Button ID="btnBuyNow" runat="server" Text="Buy Now" CssClass="btn btn-success btn-lg ml-2"  OnClick="btnBuyNow_Click"/>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Product Description Tabs -->
        <div class="row mt-4">
            <div class="col-md-12">
                <ul class="nav nav-tabs" id="productTabs" role="tablist">
                    <li class="nav-item">
                        <a class="nav-link active" id="description-tab" data-toggle="tab" href="#description" role="tab">Description</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" id="additional-tab" data-toggle="tab" href="#additional" role="tab">Additional Info</a>
                    </li>
                </ul>
                
                <div class="tab-content p-3 border border-top-0" id="productTabsContent">
                    <div class="tab-pane fade show active" id="description" role="tabpanel">
                        <asp:Literal ID="litLongDescription" runat="server"></asp:Literal>
                    </div>
                    <div class="tab-pane fade" id="additional" role="tabpanel">
                        <asp:Literal ID="litAdditionalInfo" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Related Products -->
        <div class="row mt-5">
            <div class="col-md-12">
                <h3 class="mb-4">You May Also Like</h3>
                <div class="row">
                    <asp:Repeater ID="rptRelatedProducts" runat="server">
                        <ItemTemplate>
                            <div class="col-md-3">
                                <div class="card h-100">
                                    <img src='<%# Eval("ImageUrl") %>' class="card-img-top related-product-image" alt='<%# Eval("ProductName") %>'>
                                    <div class="card-body">
                                        <h5 class="card-title"><%# Eval("ProductName") %></h5>
                                        <p class="card-text"><strong>$<%# Eval("Price", "{0:0.00}") %></strong></p>
                                    </div>
                                    <div class="card-footer">
                                        <a href='ProductDetails.aspx?id=<%# Eval("ProductId") %>' class="btn btn-sm btn-outline-primary">View Details</a>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
        
        <!-- Empty Product Panel -->
        <div class="row" id="noProductPanel" runat="server" visible="false">
            <div class="col-md-12">
                <div class="alert alert-warning">
                    <h4>Product Not Found</h4>
                    <p>The product you are looking for is not available. Please browse our <a href="Shop.aspx">shop</a> for other products.</p>
                </div>
            </div>
        </div>
    </div>

    <!-- Custom CSS for Product Details -->
    <style>
        .main-product-image {
            border: 1px solid #ddd;
            padding: 10px;
            height: 400px;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        
        .main-product-image img {
            max-height: 100%;
            object-fit: contain;
        }
        
        .thumbnail-image {
            width: 80px;
            height: 80px;
            object-fit: cover;
            cursor: pointer;
            margin-right: 10px;
            transition: border-color 0.2s ease;
        }
        
        .thumbnail-image:hover {
            border-color: #007bff;
        }
        
        .related-product-image {
            height: 150px;
            object-fit: cover;
        }
    </style>

    <!-- Custom JavaScript for Product Details -->
    <script type="text/javascript">
        function changeMainImage(src) {
            document.getElementById('<%= mainImage.ClientID %>').src = src;
        }
        
        function incrementQty() {
            var txtQty = document.getElementById('<%= txtQuantity.ClientID %>');
            var maxQty = document.getElementById('<%= hdnMaxQuantity.ClientID %>').value;
            var currentQty = parseInt(txtQty.value);
            
            if (currentQty < parseInt(maxQty)) {
                txtQty.value = currentQty + 1;
            }
        }
        
        function decrementQty() {
            var txtQty = document.getElementById('<%= txtQuantity.ClientID %>');
            var currentQty = parseInt(txtQty.value);
            
            if (currentQty > 1) {
                txtQty.value = currentQty - 1;
            }
        }
    </script>
</asp:Content>
