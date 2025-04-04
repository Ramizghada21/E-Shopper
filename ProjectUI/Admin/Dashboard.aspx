<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="ProjectUI.Admin.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid">
        <h2 class="mt-4">Admin Dashboard</h2>
        <hr />

        <div class="row">
            <div class="col-md-3">
                <div class="card bg-primary text-white">
                    <div class="card-body">
                        <h5>Total Products</h5>
                        <h3><asp:Label ID="lblTotalProducts" runat="server" Text="0"></asp:Label></h3>
                    </div>
                </div>
            </div>

            <div class="col-md-3">
                <div class="card bg-success text-white">
                    <div class="card-body">
                        <h5>Total Users</h5>
                        <h3><asp:Label ID="lblTotalUsers" runat="server" Text="0"></asp:Label></h3>
                    </div>
                </div>
            </div>

            <div class="col-md-3">
                <div class="card bg-warning text-dark">
                    <div class="card-body">
                        <h5>Total Orders</h5>
                        <h3><asp:Label ID="lblTotalOrders" runat="server" Text="0"></asp:Label></h3>
                    </div>
                </div>
            </div>

            <div class="col-md-3">
                <div class="card bg-danger text-white">
                    <div class="card-body">
                        <h5>Total Sales ($)</h5>
                        <h3><asp:Label ID="lblTotalSales" runat="server" Text="0.00"></asp:Label></h3>
                    </div>
                </div>
            </div>
        </div>

        <h4 class="mt-4">Recent Orders</h4>
        <div class="table-responsive">
            <asp:Repeater ID="rptRecentOrders" runat="server">
                <HeaderTemplate>
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th>Order No</th>
                                <th>User</th>
                                <th>Total Amount</th>
                                <th>Status</th>
                                <th>Order Date</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("OrderNo") %></td>
                        <td><%# Eval("UserName") %></td>
                        <td>$<%# Eval("TotalAmount", "{0:0.00}") %></td>
                        <td><%# Eval("Status") %></td>
                        <td><%# Eval("OrderDate", "{0:dd MMM yyyy}") %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>

</asp:Content>
