<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ProductList.aspx.cs" Inherits="ProjectUI.Admin.ProductList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mb-4">
        <asp:Label ID="lblMsg" runat="server"></asp:Label>
    </div>
    <div class="col-sm-12 col-md-8">
        <div class="card">
            <div class="card-body">
                <h4 class="card-title">Product List</h4>
                <hr />
                <div class="table-responsive">
                    <asp:Repeater ID="rptProducts" runat="server" OnItemCommand="rptProducts_ItemCommand">
                        <HeaderTemplate>
                            <table class="table data-table-export table-hover nowrap">
                                <thead>
                                    <tr>
                                        <th class="table-plus">Product Name</th>
                                        <th>Price</th>
                                        <th>Qty</th>
                                        <th>Category</th>
                                        <th>Sub-Category</th>
                                        <th>Sold</th>
                                        <th>IsActive</th>
                                        <th class="datatable-nosort">Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="table-plus"><%# Eval("ProductName") %></td>
                                <td>$<%# Eval("Price", "{0:0.00}") %></td>
                                <td><%# Eval("Quantity") %></td>
                                <td><%# Eval("CategoryName") %></td>
                                <td><%# Eval("SubCategoryName") %></td>
                                <td><%# Eval("Sold") %></td>
                                <td>
                                    <asp:Label runat="server" ID="lblActive"
                                        Text='<%# (bool)Eval("IsActive") == true ? "Active":"In-Active"%>'
                                        CssClass='<%# (bool)Eval("IsActive") == true ? "badge badge-success":"badge badge-danger"%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbEdit" runat="server" Text="Edit" CssClass="badge badge-primary"
                                        CommandArgument='<%# Eval("ProductId") %>' CommandName="edit" CausesValidation="false">
                                            <i class="fas fa-edit"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lbDelete" runat="server" Text="Delete" CssClass="badge badge-danger"
                                        CommandArgument='<%# Eval("ProductId") %>' CommandName="delete" CausesValidation="false"
                                        OnClientClick="return confirm('Are you sure you want to delete this product?');">
                                            <i class="fas fa-trash-alt"></i>
                                    </asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody>
                                </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
