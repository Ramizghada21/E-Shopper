<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="ProjectUI.Admin.Category" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        window.onload = function () {
            var s = 5;
            setTimeout(function () {
                document.getElementById("<%= lblMsg.ClientID%>").style.display = "none";
            }, s * 1000)
        }
    </script>
    <script>
        function ImagePreview(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%= imagePreview.ClientID %>').prop('src', e.target.result).width(200).height(200);
                };
                reader.readAsDataURL(input.files[0]);
            }
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
                    <h4 class="card-title">Category</h4>
                    <hr />

                    <div class="form-body">
                        <label>Category Name :</label>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control" placeholder="Enter category name"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvCategory" ForeColor="Red" runat="server" Font-Size="Small" Display="Dynamic" ControlToValidate="txtCategoryName" SetFocusOnError="true" ErrorMessage="category name is required"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>

                        <label>Category Image :</label>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:FileUpload ID="fuCategoryImage" CssClass="form-control" runat="server" onchange="ImagePreview(this);" />
                                    <asp:HiddenField ID="hfCategoryId" runat="server" Value="0" />
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

                    <div>
                        <asp:Image ID="imagePreview" runat="server" CssClass="img-thumbnail" />
                    </div>
                </div>
            </div>
        </div>

        <div class="col-sm-12 col-md-8">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Category List</h4>
                    <hr />

                    <div class="table-responsive">
                        <asp:Repeater ID="rCategory" runat="server" OnItemCommand="rCategory_ItemCommand">
                            <HeaderTemplate>
                                <table class="table data-table-export table-hover nowrap">
                                    <thead>
                                        <tr>
                                            <th class="table-plus">Name</th>
                                            <th>Image</th>
                                            <th>IsActive</th>
                                            <th>CreatedDate</th>
                                            <th class="datatable-nosort">Action</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr>
                                    <td class="table-plus"><%# Eval("CategoryName") %></td>
                                    <td>
                                        <img height="40" src="<%# ProjectUI.Util.getImageUrl(Eval("CategoryImageUrl")) %>" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblActive"
                                            Text='<%# (bool)Eval("IsActive") == true ? "Active":"In-Active"%>'
                                            CssClass='<%# (bool)Eval("IsActive") == true ? "badge badge-success":"badge badge-danger"%>'></asp:Label>
                                    </td>
                                    <td><%# Eval("CreatedDate") %></td>
                                    <td>
                                        <asp:LinkButton ID="lbEdit" runat="server" Text="Edit" CssClass="badge badge-primary"
                                            CommandArgument='<%# Eval("CategoryID") %>' CommandName="edit" CausesValidation="false">
                                            <i class="fas fa-edit"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lbDelete" runat="server" Text="Delete" CssClass="badge badge-danger"
                                            CommandArgument='<%# Eval("CategoryID") %>' CommandName="delete" CausesValidation="false">
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

                        <!-- Pagination Controls -->
                        <div class="pagination-container text-center mt-3">
                            <asp:Button ID="btnPrevPage" runat="server" CssClass="btn btn-primary mx-2"
                                Text="Previous" OnClick="btnPrevPage_Click" CausesValidation="false" />

                            <asp:Label ID="lblPageNumber" runat="server" CssClass="mx-2"></asp:Label>

                            <asp:Button ID="btnNextPage" runat="server" CssClass="btn btn-primary mx-2"
                                Text="Next" OnClick="btnNextPage_Click" CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
