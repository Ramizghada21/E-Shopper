<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SubCategory.aspx.cs" Inherits="ProjectUI.Admin.SubCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        window.onload = function () {
            var s = 5;
            setTimeout(function () {
                document.getElementById("<%= lblMsg.ClientID%>").style.display = "none";
            }, s * 1000)
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="mb-4">
        <asp:Label ID="lblMsg" runat="server"></asp:Label>
    </div>
    <div class="row">
        <div class="col-sm-12 col-md-4">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Sub-Category</h4>
                    <hr />

                    <div class="form-body">
                        <label>SubCategory Name :</label>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:TextBox ID="txtSubCategoryName" runat="server" CssClass="form-control" placeholder="Enter SubCategory name"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvSubCategory" ForeColor="Red" runat="server" Font-Size="Small" Display="Dynamic" ControlToValidate="txtSubCategoryName" SetFocusOnError="true" ErrorMessage="Sub-Category name is required"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <label>Category</label>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:DropDownList ID="ddlCategry" runat="server" AppendDataBoundItems="true" cssClass="form-control">
                                        <asp:ListItem Value="0">--Select Category--</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvCategory" ForeColor="Red" runat="server" Font-Size="Small" Display="Dynamic" ControlToValidate="ddlCategry" SetFocusOnError="true" ErrorMessage="Category is required" IntialValue="0"></asp:RequiredFieldValidator>
                                    <asp:HiddenField ID="hfSubCategoryId" runat="server" Value="0" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:CheckBox ID="cbIsActive" Text="&nbsp; IsActive" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-action pb-5">
                        <div class="text-left">
                            <asp:Button ID="btnAddOrUpdate" runat="server" CssClass="btn btn-info" Text="Add" OnClick="btnAddOrUpdate_Click" />
                            <asp:Button ID="btnClear" runat="server" CssClass="btn btn-dark" Text="Reset" OnClick="btnClear_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-sm-12 col-md-8">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Sub-Category List</h4>
                    <hr />
                    <div class="table-responsive">
                        <asp:Repeater ID="rSubCategory" runat="server" OnItemCommand="rSubCategory_ItemCommand">
                            <headertemplate>
                                <table class="table data-table-export table-hover nowrap">
                                    <thead>
                                        <tr>
                                            <th class="table-plus">SubCategory</th>
                                            <th>Category</th>
                                            <th>IsActive</th>
                                            <th>CreatedDate</th>
                                            <th class="datatable-nosort">Action</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </headertemplate>
                            <itemtemplate>
                                <tr>
                                    <td class="table-plus"><%# Eval("SubCategoryName") %></td>
                                    <td><%# Eval("CategoryName") %></td>
                                    <td>
                                        <asp:Label runat="server" ID="lblActive"
                                            Text='<%# (bool)Eval("IsActive") == true ? "Active":"In-Active"%>'
                                            cssClass='<%# (bool)Eval("IsActive") == true ? "badge badge-success":"badge badge-danger"%>'></asp:Label>
                                    </td>
                                    <td><%# Eval("CreatedDate") %></td>
                                    <td>
                                        <asp:LinkButton ID="lbEdit" runat="server" Text="Edit" cssClass="badge badge-primary"
                                            CommandArgument='<%# Eval("SubCategoryID") %>' CommandName="edit" CausesValidation="false">
                                            <i class="fas fa-edit"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lbDelete" runat="server" Text="Delete" cssClass="badge badge-danger"
                                            CommandArgument='<%# Eval("SubCategoryID") %>' CommandName="delete" CausesValidation="false">
                                            <i class="fas fa-trash-alt"></i>
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </itemtemplate>
                            <footertemplate>
                                </tbody>
                                </table>
                            </footertemplate>
                        </asp:Repeater>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>


