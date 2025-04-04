<%@ Page Title="Checkout" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="ProjectUI.User.Checkout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!-- Page Header Start -->
    <div class="container-fluid bg-secondary mb-5">
      <div
        class="d-flex flex-column align-items-center justify-content-center"
        style="min-height: 300px"
      >
        <h1 class="font-weight-semi-bold text-uppercase mb-3">Checkout</h1>
        <div class="d-inline-flex">
          <p class="m-0"><a href="index.html">Home</a></p>
          <p class="m-0 px-2">-</p>
          <p class="m-0">Checkout</p>
        </div>
      </div>
    </div>
    <!-- Page Header End -->

    <div class="container my-5">
        <div class="row justify-content-center">
            <div class="col-lg-10 checkout-container">
                <div class="checkout-header">Billing Details</div>
                <div class="row g-3 mt-4">
                    <div class="col-md-6">
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" placeholder="First Name" required></asp:TextBox>
                    </div>
                    <div class="col-md-6">
                        <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" placeholder="Last Name" required></asp:TextBox>
                    </div>
                    <div class="col-md-6">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Email" required></asp:TextBox>
                    </div>
                    <div class="col-md-6">
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" placeholder="Mobile No" required></asp:TextBox>
                    </div>
                    <div class="col-md-12">
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Address" required></asp:TextBox>
                    </div>
                </div>

                <div class="checkout-header mt-5">Payment Details</div>
                <div class="row g-3 mt-4">
                    <div class="col-md-12">
                        <asp:TextBox ID="txtCardName" runat="server" CssClass="form-control" placeholder="Cardholder Name" required></asp:TextBox>
                    </div>
                    <div class="col-md-12">
                        <asp:TextBox ID="txtCardNumber" runat="server" CssClass="form-control" placeholder="Card Number" required></asp:TextBox>
                    </div>
                    <div class="col-md-6">
                        <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control" placeholder="MM/YY" required></asp:TextBox>
                    </div>
                    <div class="col-md-6">
                        <asp:TextBox ID="txtCVV" runat="server" CssClass="form-control" placeholder="CVV" required></asp:TextBox>
                    </div>
                </div>

                <div class="checkout-header mt-5">Payment Mode</div>
                <div class="mt-3">
                    <asp:DropDownList ID="ddlPaymentMode" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Credit Card" Value="Credit Card"></asp:ListItem>
                        <asp:ListItem Text="Debit Card" Value="Debit Card"></asp:ListItem>
                        <asp:ListItem Text="Paypal" Value="Paypal"></asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="checkout-header mt-5">Order Summary</div>
                <div class="order-summary mt-4">
                    <asp:Repeater ID="rptOrderSummary" runat="server">
                        <ItemTemplate>
                            <div class="d-flex justify-content-between">
                                <p class="m-0"><%# Eval("ProductName") %> (x<%# Eval("Quantity") %>)</p>
                                <p class="m-0">$<%# Eval("Price") %></p>
                            </div>
                            <hr />
                        </ItemTemplate>
                    </asp:Repeater>
                    <div class="d-flex justify-content-between fw-bold">
                        <p>Total:</p>
                        <p>$<asp:Label ID="lblTotal" runat="server" CssClass="text-danger fw-bold"></asp:Label></p>
                    </div>
                </div>

                <!-- Place Order Button -->
                <div class="d-flex justify-content-center mt-4">
                    <asp:Button ID="btnPlaceOrder" runat="server" Text="Place Order" CssClass="btn btn-primary w-50 py-3" OnClick="btnPlaceOrder_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
