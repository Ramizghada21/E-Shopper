<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="ProjectUI.Admin.Product" %>

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
                    var controlName = input.id.substr(input.id.indexOf("_") + 1);
                    if (controlName == 'fuFirstImage') {
                        $('#<%= imageProduct1.ClientID %>').show();
                    $('#<%= imageProduct1.ClientID %>').prop('src', e.target.result).width(200).height(200);
                    } else if (controlName == 'fuSecondImage') {
                        $('#<%= imageProduct2.ClientID %>').show();
                        $('#<%= imageProduct2.ClientID %>').prop('src', e.target.result).width(200).height(200);
                    } else if (controlName == 'fuThirdImage') {
                        $('#<%= imageProduct3.ClientID %>').show();
                        $('#<%= imageProduct3.ClientID %>').prop('src', e.target.result).width(200).height(200);
                    } else{
                        $('#<%= imageProduct4.ClientID %>').show();
                        $('#<%= imageProduct4.ClientID %>').prop('src', e.target.result).width(200).height(200);
                    }
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
        <div class="col-sm-12 col-md-12">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Product</h4>
                    <hr />
                    <div class="form-body">
                        <div class="row">
                            <div class="col-md-6">
                                <label>Product Name :</label>
                                <div class="form-group">
                                    <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" placeholder="Enter Product name"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvProduct" ForeColor="Red" runat="server" Font-Size="Small" Display="Dynamic" ControlToValidate="txtProductName" SetFocusOnError="true" ErrorMessage="Product name is required"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <label>Category</label>
                                <div class="form-group">
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"
                                          AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select Category</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvCategory" ForeColor="Red" runat="server" Font-Size="Small" Display="Dynamic" ControlToValidate="ddlCategory" InitialValue="0" SetFocusOnError="true" ErrorMessage="Category is required"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <label>Sub Category</label>
                                <div class="form-group">
                                    <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="true">
                                        
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvSubCategory" ForeColor="Red" runat="server" Font-Size="Small" Display="Dynamic" ControlToValidate="ddlSubCategory" InitialValue="0" SetFocusOnError="true" ErrorMessage="Sub Category is required"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label>Price</label>
                                <div class="form-group">
                                    <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" placeholder="Enter Product Price"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvPrice" ForeColor="Red" runat="server" Font-Size="Small" Display="Dynamic"
                                        ControlToValidate="txtPrice" SetFocusOnError="true" ErrorMessage="Product Price is required"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revPrice" runat="server"
                                        ErrorMessage="Product Price is invalid" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Smaller" ControlToValidate="txtPrice" ValidationExpression="\d+(?:.\d{1,2})?"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <label>Color</label>
                                <div class="form-group">
                                    <asp:ListBox ID="lboxColor" runat="server" CssClass="form-control" SelectionMode="Multiple"
                                        ToolTip="Use CTRL key to select multiple items">
                                        <asp:ListItem Value="1">Blue</asp:ListItem>
                                        <asp:ListItem Value="2">Red</asp:ListItem>
                                        <asp:ListItem Value="3">Pink</asp:ListItem>
                                        <asp:ListItem Value="4"> Purple</asp:ListItem>
                                        <asp:ListItem Value="5">Brown</asp:ListItem>
                                        <asp:ListItem Value="6">Gray</asp:ListItem>
                                        <asp:ListItem Value="7">Green</asp:ListItem>
                                        <asp:ListItem Value="8">White</asp:ListItem>
                                        <asp:ListItem Value="9">Black</asp:ListItem>
                                    </asp:ListBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <label>Size</label>
                                <div class="form-group">
                                    <asp:ListBox ID="lboxSize" runat="server" CssClass="form-control" SelectionMode="Multiple"
                                        ToolTip="Use CTRL key to select multiple items">
                                        <asp:ListItem Value="1">XS</asp:ListItem>
                                        <asp:ListItem Value="2">SM</asp:ListItem>
                                        <asp:ListItem Value="3">M</asp:ListItem>
                                        <asp:ListItem Value="4">L</asp:ListItem>
                                        <asp:ListItem Value="5">XL</asp:ListItem>
                                        <asp:ListItem Value="6">XLL</asp:ListItem>
                                    </asp:ListBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <label>Quantity</label>
                                <div class="form-group">
                                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" placeholder="Enter Product Quantity"
                                        TextMode="Number"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvQuantity" ForeColor="Red" runat="server" Font-Size="Small"
                                        Display="Dynamic" ControlToValidate="txtQuantity" SetFocusOnError="true" ErrorMessage="Product Quantity    is required"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <label>Company Name</label>
                                <div class="form-group">
                                    <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" placeholder="Enter Product's companyname"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvtxtCompanyName" ForeColor="Red" runat="server" Font-Size="Small"
                                        Display="Dynamic" ControlToValidate="txtCompanyName" SetFocusOnError="true" ErrorMessage="Company Name is required"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <label>Short Description</label>
                                <div class="form-group">
                                    <asp:TextBox ID="txtShortDescription" runat="server" CssClass="form-control"
                                        placeholder="Enter Short Description" TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvShortDescription" ForeColor="Red" runat="server" Font-Size="Small"
                                        Display="Dynamic" ControlToValidate="txtShortDescription" SetFocusOnError="true" ErrorMessage="Short Description is required"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <label>Long Description</label>
                                <div class="form-group">
                                    <asp:TextBox ID="txtLongDescription" runat="server" CssClass="form-control"
                                        placeholder="Enter Long Description" TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvLongDescription" ForeColor="Red" runat="server" Font-Size="Small"
                                        Display="Dynamic" ControlToValidate="txtLongDescription" SetFocusOnError="true" ErrorMessage="Long Description is required"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <label>Additional Description</label>
                                <div class="form-group">
                                    <asp:TextBox ID="txtAdditionalDescription" runat="server" CssClass="form-control"
                                        placeholder="Enter Additional Description" TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvAdditionalDescription" ForeColor="Red" runat="server" Font-Size="Small"
                                        Display="Dynamic" ControlToValidate="txtAdditionalDescription" SetFocusOnError="true" ErrorMessage="Additional Description is required"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <%--<div class="row">
                            <div class="col-md-12">
                                <label>Tags(Search keyword)</label>
                                <div class="form-group">
                                    <asp:TextBox ID="txtTags" runat="server" CssClass="form-control"
                                        placeholder="Enter Tags"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvTags" ForeColor="Red" runat="server" Font-Size="Small"
                                        Display="Dynamic" ControlToValidate="txtTags" SetFocusOnError="true" ErrorMessage="Tags                                                                                                                                                                                                                  is required"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>--%>
                        <div class="row">
                            <div class="col-md-6">
                                <label>Product Image 1</label>
                                <div class="form-group">
                                    <asp:FileUpload ID="fuFirstImage" runat="server" CssClass="form-control" ToolTip=".jpg, .png, .jpeg image only"
                                        onchange="ImagePreview(this);" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <label>Product Image 2</label>
                                <div class="form-group">
                                    <asp:FileUpload ID="fuSecondImage" runat="server" CssClass="form-control" ToolTip=".jpg, .png, .jpeg image only"
                                        onchange="ImagePreview(this);" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <label>Product Image 3</label>
                                <div class="form-group">
                                    <asp:FileUpload ID="fuThirdImage" runat="server" CssClass="form-control" ToolTip=".jpg, .png, .jpeg image only"
                                        onchange="ImagePreview(this);" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <label>Product Image 4</label>
                                <div class="form-group">
                                    <asp:FileUpload ID="fuFourthImage" runat="server" CssClass="form-control" ToolTip=".jpg, .png, .jpeg image only"
                                        onchange="ImagePreview(this);" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <label>Default Image</label>
                                <div class="form-group">
                                    <asp:RadioButtonList ID="rblDefaultImage" runat="server"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1">&nbsp;First&nbsp;</asp:ListItem>
                                        <asp:ListItem Value="2">&nbsp;Second&nbsp;</asp:ListItem>
                                        <asp:ListItem Value="3">&nbsp;Third&nbsp;</asp:ListItem>
                                        <asp:ListItem Value="4">&nbsp;Fourth&nbsp;</asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:RequiredFieldValidator ID="rfvDefaultImage" ForeColor="Red" runat="server" Font-Size="Small"
                                        Display="Dynamic" ControlToValidate="rblDefaultImage" SetFocusOnError="true" ErrorMessage="Default Image is required"></asp:RequiredFieldValidator>
                                    <asp:HiddenField ID="hfDefImagePos" runat="server" Value="0" />
                                </div>
                            </div>
                            <div class="col-md-3">
                                <label>Customized</label>
                                <div class="form-group">
                                    <asp:CheckBox ID="cbIsCustomized" Text="&nbsp; IsCustomized" runat="server" />
                                </div>
                            </div>
                            <div class="col-md-3">
                                <label>Active</label>
                                <div class="form-group">
                                    <asp:CheckBox ID="cbIsActive" Text="&nbsp; IsActive" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 align-content-sm-between pl-3">
                                <span>
                                    <asp:Image id="imageProduct1" runat="server" CssClass="img-thumbnail" 
                                        AlternateText="image" style="display:none"/>
                                </span>
                                <span>
                                    <asp:Image id="imageProduct2" runat="server" CssClass="img-thumbnail" 
                                        AlternateText="image" style="display:none"/>
                                </span>
                                <span>
                                    <asp:Image id="imageProduct3" runat="server" CssClass="img-thumbnail" 
                                        AlternateText="image" style="display:none"/>
                                </span>
                                <span>
                                    <asp:Image id="imageProduct4" runat="server" CssClass="img-thumbnail" 
                                        AlternateText="image" style="display:none"/>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="form-action pb-4">
                        <div class="text-left">
                            <asp:Button ID="btnAddOrUpdate" Text="Add" runat="server" CssClass="btn btn-info" OnClick="btnAddOrUpdate_Click" />
                            <asp:Button ID="btnClear" Text="Reset" runat="server" CssClass="btn btn-dark" CausesValidation="false" OnClick="btnClear_Click"/>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
