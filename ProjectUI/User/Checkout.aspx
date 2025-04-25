<%@ Page Title="Checkout" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="ProjectUI.User.Checkout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="https://checkout.razorpay.com/v1/checkout.js"></script>
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

                <div class="d-flex justify-content-center mt-4">
        <asp:Button ID="btnPlaceOrder" runat="server" Text="Pay Now" CssClass="btn btn-primary w-50 py-3" OnClientClick="return initiateRazorpay();" />
    </div>

    <asp:HiddenField ID="hdnAmount" runat="server" />
    <asp:HiddenField ID="hdnOrderNo" runat="server" />
    <asp:HiddenField ID="hdnPaymentId" runat="server" />
            </div>
        </div>
    </div>
    

    <script type="text/javascript">
        function initiateRazorpay() {
            var amount = document.getElementById('<%= hdnAmount.ClientID %>').value;
            var orderNo = document.getElementById('<%= hdnOrderNo.ClientID %>').value;

            var options = {
                "key": "rzp_test_LWvBuAmAHDdJS8", // Your Razorpay Key
                "amount": amount * 100, // in paise
                "currency": "INR",
                "name": "Online Store",
                "description": "Order Payment",
                "handler": function (response) {
                    document.getElementById('<%= hdnPaymentId.ClientID %>').value = response.razorpay_payment_id;
                    __doPostBack('<%= btnPlaceOrder.UniqueID %>', '');
                },
                "prefill": {
                    "name": "<%= txtFirstName.Text %> <%= txtLastName.Text %>",
                    "email": "<%= txtEmail.Text %>",
                    "contact": "<%= txtMobile.Text %>"
                },
                "theme": {
                    "color": "#3399cc"
                }
            };
            var rzp = new Razorpay(options);
            rzp.open();
            return false;
        }
    </script>
</asp:Content>
