<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="ProjectUI.User.Cart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<!-- Page Header Start -->
    <div class="container-fluid bg-secondary mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center" style="min-height: 300px">
            <h1 class="font-weight-semi-bold text-uppercase mb-3">Shopping Cart</h1>
            <div class="d-inline-flex">
                <p class="m-0"><a href="index.aspx">Home</a></p>
                <p class="m-0 px-2">-</p>
                <p class="m-0">Shopping Cart</p>
            </div>
        </div>
    </div>
    <!-- Page Header End -->

    <!-- Cart Start -->
    <div class="container-fluid pt-5">
        <div class="row px-xl-5">
            <div class="col-lg-8 table-responsive mb-5">
                <table class="table table-bordered text-center mb-0">
                    <thead class="bg-secondary text-dark">
                        <tr>
                            <th>Products</th>
                            <th>Price</th>
                            <th>Quantity</th>
                            <th>Total</th>
                            <th>Remove</th>
                        </tr>
                    </thead>
                    <tbody class="align-middle">
                        <asp:Repeater ID="rptCart" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td class="align-middle">
                                        <img src='<%# Eval("ImageUrl") %>' alt="" style="width: 50px" />
                                        <%# Eval("ProductName") %>
                                    </td>
                                    <td class="align-middle">$<%# Eval("Price") %></td>
                                    <td class="align-middle">
                                        <div class="input-group quantity mx-auto" style="width: 100px">
                                            <div class="input-group-btn">
                                                <asp:Button ID="btnMinus" runat="server" CssClass="btn btn-sm btn-primary btn-minus" CommandArgument='<%# Eval("ProductId") %>' CommandName="DecreaseQty" Text="-" OnCommand="UpdateQuantity_Click" />
                                            </div>
                                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control form-control-sm bg-secondary text-center" Text='<%# Eval("Quantity") %>' ReadOnly="true"></asp:TextBox>
                                            <div class="input-group-btn">
                                                <asp:Button ID="btnPlus" runat="server" CssClass="btn btn-sm btn-primary btn-plus" CommandArgument='<%# Eval("ProductId") %>' CommandName="IncreaseQty" Text="+" OnCommand="UpdateQuantity_Click" />
                                            </div>
                                        </div>
                                    </td>
                                    <td class="align-middle">$<%# Convert.ToDecimal(Eval("Price")) * Convert.ToInt32(Eval("Quantity")) %></td>
                                    <td class="align-middle">
                                        <asp:Button ID="btnRemove" runat="server" CssClass="btn btn-sm btn-primary" CommandArgument='<%# Eval("ProductId") %>' CommandName="Remove" Text="X" OnCommand="RemoveItem_Click" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
            
            <div class="col-lg-4">
                <div class="card border-secondary mb-5">
                    <div class="card-header bg-secondary border-0">
                        <h4 class="font-weight-semi-bold m-0">Cart Summary</h4>
                    </div>
                    <div class="card-body">
                        <div class="d-flex justify-content-between mb-3 pt-1">
                            <h6 class="font-weight-medium">Subtotal</h6>
                            <h6 class="font-weight-medium">$<asp:Label ID="lblSubtotal" runat="server" Text="0"></asp:Label></h6>
                        </div>
                        <div class="d-flex justify-content-between">
                            <h6 class="font-weight-medium">Shipping</h6>
                            <h6 class="font-weight-medium">$<asp:Label ID="lblShipping" runat="server" Text="10"></asp:Label></h6>
                        </div>
                    </div>
                    <div class="card-footer border-secondary bg-transparent">
                        <div class="d-flex justify-content-between mt-2">
                            <h5 class="font-weight-bold">Total</h5>
                            <h5 class="font-weight-bold">$<asp:Label ID="lblTotal" runat="server" Text="0"></asp:Label></h5>
                        </div>
                        <asp:Button ID="btnCheckout" runat="server" CssClass="btn btn-block btn-primary my-3 py-3" Text="Proceed To Checkout" OnClick="btnCheckout_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Cart End -->
</asp:Content>
