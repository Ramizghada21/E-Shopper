<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="ProjectUI.login" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <title>EShopper - Find What You Love</title>
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta content="Free HTML Templates" name="keywords" />
    <meta content="Free HTML Templates" name="description" />

    <!-- Favicon -->
    <link href="img/favicon.ico" rel="icon" />

    <!-- Google Web Fonts -->
    <link rel="preconnect" href="https://fonts.gstatic.com" />
    <link
        href="https://fonts.googleapis.com/css2?family=Poppins:wght@100;200;300;400;500;600;700;800;900&display=swap"
        rel="stylesheet" />

    <!-- Font Awesome -->
    <link
        href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.10.0/css/all.min.css"
        rel="stylesheet" />

    <!-- Libraries Stylesheet -->
    <link href="lib/owlcarousel/assets/owl.carousel.min.css" rel="stylesheet" />

    <!-- Customized Bootstrap Stylesheet -->
    <link href="css/style.css" rel="stylesheet" />
</head>

<body>
    <form id="form1" runat="server">
        <!-- Topbar Start -->
        <div class="container-fluid">
            <div class="row align-items-center py-3 px-xl-5">
                <div class="col-lg-3 d-none d-lg-block">
                    <a href="Default.aspx" class="text-decoration-none">
                        <h1 class="m-0 display-5 font-weight-semi-bold">
                            <span class="text-primary font-weight-bold border px-3 mr-1">E</span>Shopper
                        </h1>
                    </a>
                </div>
                <div class="col-lg-6 col-6 text-left">

                    <div class="input-group">
                        <input type="text"
                            class="form-control"
                            placeholder="Search for products" />
                        <div class="input-group-append">
                            <span class="input-group-text bg-transparent text-primary">
                                <i class="fa fa-search"></i>
                            </span>
                        </div>
                    </div>

                </div>
                <div class="col-lg-3 col-6 text-right">
                    <a href="Cart.aspx" class="btn border">
                        <i class="fas fa-shopping-cart text-primary"></i>
                        <span class="badge">0</span>
                    </a>
                </div>
            </div>
        </div>
        <!-- Topbar End -->
        <!-- Navbar Start -->
        <div class="container-fluid">
            <div class="row border-top px-xl-5">
                <div class="col-lg-9">
                    <nav class="navbar navbar-expand-lg bg-light navbar-light py-3 py-lg-0 px-0">
                        <a href="index.html" class="text-decoration-none d-block d-lg-none">
                            <h1 class="m-0 display-5 font-weight-semi-bold">
                                <span class="text-primary font-weight-bold border px-3 mr-1">E</span>Shopper
                            </h1>
                        </a>
                        <button type="button"
                            class="navbar-toggler"
                            data-toggle="collapse"
                            data-target="#navbarCollapse">
                            <span class="navbar-toggler-icon"></span>
                        </button>
                        <div class="collapse navbar-collapse justify-content-between"
                            id="navbarCollapse">
                            <div class="navbar-nav mr-auto py-0">
                                <a href="Default.aspx" class="nav-item nav-link">Home</a>
                                <a href="Shop.aspx" class="nav-item nav-link">Shop</a>
                                <a href="ProductDetails.aspx" class="nav-item nav-link">Shop Detail</a>
                                <div class="nav-item dropdown">
                                    <a href="#"
                                        class="nav-link dropdown-toggle active"
                                        data-toggle="dropdown">Pages</a>
                                    <div class="dropdown-menu rounded-0 m-0">
                                        <a href="Cart.aspx" class="dropdown-item">Shopping Cart</a>
                                        <a href="Checkout.aspx" class="dropdown-item">Checkout</a>
                                    </div>
                                </div>
                                <a href="Contact.aspx" class="nav-item nav-link">Contact</a>
                            </div>
                            <div class="navbar-nav ml-auto py-0">
                                <a href="login.aspx" class="nav-item nav-link">Login</a>
                                <a href="register.aspx" class="nav-item nav-link">Register</a>
                            </div>
                        </div>
                    </nav>
                </div>
            </div>
        </div>
        <!-- Navbar End -->
        <!-- Page Header Start -->
        <div class="container-fluid bg-secondary mb-5">
            <div class="d-flex flex-column align-items-center justify-content-center"
                style="min-height: 300px">
                <h1 class="font-weight-semi-bold text-uppercase mb-3">Login</h1>
                <div class="d-inline-flex">
                    <p class="m-0"><a href="index.html">Home</a></p>
                    <p class="m-0 px-2">-</p>
                    <p class="m-0">Login</p>
                </div>
            </div>
        </div>
        <!-- Page Header End -->
        <!-- Login Start -->
        <div class="container mt-5">
            <div class="row justify-content-center">
                <div class="col-md-6">
                    <div class="card">
                        <div class="card-header text-center bg-primary text-white">
                            <h4>Login</h4>
                            <asp:Label ID="lblMsg" runat="server" CssClass="error" Visible="false"></asp:Label>
                        </div>
                        <div class="card-body">

                            <div class="form-group">
                                <label for="email">Email Address</label>
                                <div class="input-group">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text">
                                            <i class="fas fa-envelope"></i>
                                        </span>
                                    </div>
                                    <asp:TextBox ID="txtEmail"
                                        CssClass="form-control"
                                        runat="server"
                                        TextMode="Email"
                                        placeholder="Enter your email"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="password">Password</label>
                                <div class="input-group">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text">
                                            <i class="fas fa-lock"></i>
                                        </span>
                                    </div>
                                    <asp:TextBox ID="txtPassword"
                                        CssClass="form-control"
                                        runat="server"
                                        TextMode="Password"
                                        placeholder="Enter your password"></asp:TextBox>
                                </div>
                            </div>
                            <asp:Button ID="btnLogin"
                                CssClass="btn btn-primary btn-block"
                                runat="server"
                                Text="Login" OnClick="btnLogin_Click" />
                        </div>
                        <div class="card-footer text-center">
                            <div class="mt-2">
                                Don't have an account?
                                  <a href="register.aspx" class="text-primary">Register Now</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Login End -->
        <!-- Footer Start -->
        <div class="container-fluid bg-secondary text-dark mt-5 pt-5">
            <div class="row px-xl-5 pt-5">
                <div class="col-lg-4 col-md-12 mb-5 pr-3 pr-xl-5">
                    <a href="index.html" class="text-decoration-none">
                        <h1 class="mb-4 display-5 font-weight-semi-bold">
                            <span class="text-primary font-weight-bold border border-white px-3 mr-1">E</span>Shopper
                        </h1>
                    </a>
                    <p>
                        Dolore erat dolor sit lorem vero amet. Sed sit lorem magna, ipsum no
                          sit erat lorem et magna ipsum dolore amet erat.
                    </p>
                    <p class="mb-2">
                        <i class="fa fa-map-marker-alt text-primary mr-3"></i>Rajkot,Gujrat
                    </p>
                    <p class="mb-2">
                        <i class="fa fa-envelope text-primary mr-3"></i>mohsindodhiya0@gmail.com
                    </p>
                    <p class="mb-0">
                        <i class="fa fa-phone-alt text-primary mr-3"></i>+91 8780337880
                    </p>
                </div>
                <div class="col-lg-8 col-md-12">
                    <div class="row">
                        <div class="col-md-4 mb-5">
                            <h5 class="font-weight-bold text-dark mb-4">Quick Links</h5>
                            <div class="d-flex flex-column justify-content-start">
                                <a class="text-dark mb-2" href="index.html"><i class="fa fa-angle-right mr-2"></i>Home</a>
                                <a class="text-dark mb-2" href="shop.html"><i class="fa fa-angle-right mr-2"></i>Our Shop</a>
                                <a class="text-dark mb-2" href="detail.html"><i class="fa fa-angle-right mr-2"></i>Shop Detail</a>
                                <a class="text-dark mb-2" href="cart.html"><i class="fa fa-angle-right mr-2"></i>Shopping Cart</a>
                                <a class="text-dark mb-2" href="checkout.html"><i class="fa fa-angle-right mr-2"></i>Checkout</a>
                                <a class="text-dark" href="contact.html"><i class="fa fa-angle-right mr-2"></i>Contact Us</a>
                            </div>
                        </div>
                        <div class="col-md-4 mb-5">
                            <h5 class="font-weight-bold text-dark mb-4">Quick Links</h5>
                            <div class="d-flex flex-column justify-content-start">
                                <a class="text-dark mb-2" href="index.html"><i class="fa fa-angle-right mr-2"></i>Home</a>
                                <a class="text-dark mb-2" href="shop.html"><i class="fa fa-angle-right mr-2"></i>Our Shop</a>
                                <a class="text-dark mb-2" href="detail.html"><i class="fa fa-angle-right mr-2"></i>Shop Detail</a>
                                <a class="text-dark mb-2" href="cart.html"><i class="fa fa-angle-right mr-2"></i>Shopping Cart</a>
                                <a class="text-dark mb-2" href="checkout.html"><i class="fa fa-angle-right mr-2"></i>Checkout</a>
                                <a class="text-dark" href="contact.html"><i class="fa fa-angle-right mr-2"></i>Contact Us</a>
                            </div>
                        </div>
                        <div class="col-md-4 mb-5">
                            <h5 class="font-weight-bold text-dark mb-4">Newsletter</h5>

                            <div class="form-group">
                                <input type="text"
                                    class="form-control border-0 py-4"
                                    placeholder="Your Name"
                                    />
                            </div>
                            <div class="form-group">
                                <input type="email"
                                    class="form-control border-0 py-4"
                                    placeholder="Your Email"
                                   />
                            </div>
                            <div>
                                <button class="btn btn-primary btn-block border-0 py-3"
                                    type="submit">
                                    Subscribe Now
                                </button>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <div class="row border-top border-light mx-xl-5 py-4">
                <div class="col-md-6 px-xl-0">
                    <p class="mb-md-0 text-center text-md-left text-dark">
                        &copy;
                          <a class="text-dark font-weight-semi-bold" href="#">E Shopper</a>.
                          All Rights Reserved. Designed by
                          <br />
                        <a href="" target="_blank">Mohsin Dodhiya</a>
                    </p>
                </div>
                <div class="col-md-6 px-xl-0 text-center text-md-right">
                    <img class="img-fluid" src="img/payments.png" alt="" />
                </div>
            </div>
        </div>
        <!-- Footer End -->
        <!-- Back to Top -->
        <a href="#" class="btn btn-primary back-to-top">
            <i class="fa fa-angle-double-up"></i>
        </a>

        <!-- JavaScript Libraries -->
        <script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>
        <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.bundle.min.js"></script>
        <script src="lib/easing/easing.min.js"></script>
        <script src="lib/owlcarousel/owl.carousel.min.js"></script>

        <!-- Contact Javascript File -->
        <script src="mail/jqBootstrapValidation.min.js"></script>
        <script src="mail/contact.js"></script>

        <!-- Template Javascript -->
        <script src="js/main.js"></script>
    </form>
</body>
</html>
