<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="ProjectUI.User.Contact" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

      <!-- Page Header Start -->
    <div class="container-fluid bg-secondary mb-5">
      <div
        class="d-flex flex-column align-items-center justify-content-center"
        style="min-height: 300px"
      >
        <h1 class="font-weight-semi-bold text-uppercase mb-3">Contact Us</h1>
        <div class="d-inline-flex">
          <p class="m-0"><a href="index.html">Home</a></p>
          <p class="m-0 px-2">-</p>
          <p class="m-0">Contact</p>
        </div>
      </div>
    </div>
    <!-- Page Header End -->

    <!-- Contact Start -->
    <div class="container mt-5">
            <div class="row">
                <!-- Left Side: Contact Form -->
                <div class="col-md-7">
                    <h2 class="text-center mb-4">Contact For Any Queries</h2>
                    <div class="mb-3">
                        <asp:TextBox ID="txtName" CssClass="form-control" placeholder="Your Name" runat="server"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <asp:TextBox ID="txtEmail" CssClass="form-control" placeholder="Your Email" runat="server"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <asp:TextBox ID="txtSubject" CssClass="form-control" placeholder="Subject" runat="server"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <asp:TextBox ID="txtMessage" CssClass="form-control" TextMode="MultiLine" placeholder="Message" runat="server"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnSubmit" CssClass="btn btn-danger" runat="server" Text="Send Message" OnClick="btnSubmit_Click" />
                    <asp:Label ID="lblMessage" CssClass="text-success mt-3 d-block" runat="server" Visible="false"></asp:Label>
                </div>

                <!-- Right Side: Contact Info -->
                <div class="col-md-5">
                    <h4 class="text-dark">Get In Touch</h4>
                    <p>Justo sed diam ut sed amet duo amet lorem amet stet sea ipsum.</p>
                    <h5>Store 1</h5>
                    <p><i class="fa fa-map-marker"></i> 123 Street, New York, USA</p>
                    <p><i class="fa fa-envelope"></i> info@example.com</p>
                    <p><i class="fa fa-phone"></i> +012 345 67890</p>

                    <h5>Store 2</h5>
                    <p><i class="fa fa-map-marker"></i> 123 Street, New York, USA</p>
                    <p><i class="fa fa-envelope"></i> info@example.com</p>
                    <p><i class="fa fa-phone"></i> +012 345 67890</p>
                </div>
            </div>
        </div>
    
    <!-- Contact End -->

</asp:Content>
