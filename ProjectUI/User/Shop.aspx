<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Shop.aspx.cs" EnableEventValidation="false" Inherits="ProjectUI.User.Shop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <h2 class="text-center my-4">Our Products</h2>
                <asp:Label ID="lblUserId" runat="server" Text="Label"></asp:Label>
            </div>
        </div>

        <!-- Search and Filter Section -->
        <div class="row mb-4">
            <div class="col-md-6">
                <div class="input-group">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search products..."></asp:TextBox>
                    <div class="input-group-append">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>

        <!-- Products Display -->
        <div class="row">
            <asp:Repeater ID="rptProducts" runat="server" OnItemCommand="rptProducts_ItemCommand">
                <ItemTemplate>
                    <div class="col-md-4 mb-4">
                        <div class="card h-100">
                            <%--<img src='<%# Eval("ImageUrl") %>' class="card-img-top product-image" alt='<%# Eval("ProductName") %>'>--%>
                            <asp:Image src='<%# Eval("ImageUrl") %>' class="card-img-top product-image" alt='<%# Eval("ProductName") %>' runat="server"/>
                            <div class="card-body">
                                <h5 class="card-title"><%# Eval("ProductName") %></h5>
                                <p class="card-text text-truncate"><%# Eval("ShortDescription") %></p>
                                <p class="card-text"><strong>Price: $<%# Eval("Price", "{0:0.00}") %></strong></p>
                                <p class="card-text"><small class="text-muted"><%# Eval("CategoryName") %> - <%# Eval("SubCategoryName") %></small></p>
                            </div>
                            <div class="card-footer">
                                <asp:LinkButton ID="lnkViewDetails" runat="server" CssClass="btn btn-info btn-sm" CommandName="ViewDetails" CommandArgument='<%# Eval("ProductId") %>'>
                                    View Details
                                </asp:LinkButton>
                                <asp:LinkButton ID="lnkAddToCart" runat="server" CssClass="btn btn-primary btn-sm float-right" CommandName="AddToCart" CommandArgument='<%# Eval("ProductId") %>'>
                                    <i class="fa fa-shopping-cart"></i> Add to Cart
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- No Products Message -->
        <div class="row" id="noProductsPanel" runat="server" visible="false">
            <div class="col-md-12">
                <div class="alert alert-info text-center">
                    No products found! Please try a different search or category.
                </div>
            </div>
        </div>

        <!-- Pagination -->
        <div class="row">
            <div class="col-md-12">
                <ul class="pagination justify-content-center">
                    <asp:Repeater ID="rptPagination" runat="server">
                        <ItemTemplate>
                            <li class="page-item <%# Container.ItemIndex + 1 == Convert.ToInt32(ViewState["CurrentPage"]) ? "active" : "" %>">
                                <asp:LinkButton ID="lnkPage" runat="server" CssClass="page-link" CommandName="Page" CommandArgument='<%# Container.ItemIndex + 1 %>' OnClick="lnkPage_Click">
                                    <%# Container.ItemIndex + 1 %>
                                </asp:LinkButton>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>
    </div>

    <!-- Custom CSS for Shop Page -->
    <style>
        .product-image {
            height: 200px;
            object-fit: cover;
        }
        .card {
            transition: transform 0.3s;
        }
        .card:hover {
            transform: translateY(-5px);
            box-shadow: 0 10px 20px rgba(0,0,0,0.1);
        }
    </style>
</asp:Content>
