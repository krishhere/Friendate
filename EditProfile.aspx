<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" Inherits="WebApplication.EditProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<main>
<div class="container">
<div class="row g-4">
    <div class="col-lg-8 vstack gap-4">
    <div class="card">
    <div class="card-header border-0 pb-0">
        <h5 class="card-title">Update profile</h5> 
    </div>
    <div class="card-body">
        <div class="rounded border-0 px-3 py-2 mb-3"> 
            <div class="d-flex align-items-center justify-content-between">
                <asp:Label ID="lblProfileName" runat="server" Text="" Font-Bold="true" Font-Size="Larger"></asp:Label>
            </div>
        </div>
        <div class="row g-4">
        <div class="col-sm-6">
            <div class="d-flex align-items-center rounded border px-3 py-2">
            <p class="mb-0">
                <i class="bi bi-calendar-date fa-fw me-2"></i><strong> <asp:Label ID="lblDob" runat="server" Text=""></asp:Label> </strong>
            </p>
            </div>
        </div>
        <div class="col-sm-6">
            <div class="d-flex align-items-center rounded border px-3 py-2">
            <p class="mb-0">
                <i class="bi bi-gender-ambiguous"></i><strong> <asp:Label ID="lblGender" runat="server" Text=""></asp:Label></strong>
            </p>
            </div>
        </div>
        <div class="col-sm-6">
            <div class="d-flex align-items-center rounded border px-3 py-2">Looking For:
                <asp:DropDownList ID="ddlLookingFor" runat="server" style="border:none;background-color:transparent;">
                    <asp:ListItem Value="Both">Friend / Date</asp:ListItem>
                    <asp:ListItem Value="Friend">Friend</asp:ListItem>
                    <asp:ListItem Value="Date">Date</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-sm-6">
            <div class="d-flex align-items-center rounded border px-3 py-2"> 
            <p class="mb-0">
                <i class="bi bi-geo-alt fa-fw me-2"></i> Lives in: <strong> <asp:Label ID="lblCity" runat="server" Text=""></asp:Label></strong>
            </p>
            </div>
        </div>
        <div class="col-sm-6">
            <div class="d-flex align-items-center rounded border px-3 py-2"> 
            <p class="mb-0">
                <i class="bi bi-geo-alt fa-fw me-2"></i> About: <strong> <asp:Label ID="lblAbout" runat="server" Text=""></asp:Label></strong>
            </p>
            </div>
        </div>
        <div class="col-sm-6">
            <div class="d-flex align-items-center rounded border px-3 py-2"> 
            <p class="mb-0">
                <i class="bi bi-geo-alt fa-fw me-2"></i> Interests:<br /> <strong> <asp:Label ID="lblInterests" runat="server" Text=""></asp:Label></strong>
            </p>
            </div>
        </div>
        </div>
    </div>
    </div>
</div>
    <div class="col-lg-4">
        <div class="row g-4">
        <div class="col-sm-6 col-lg-12">
            <div class="card">
            <div class="card-body position-relative pt-0 align-items-center ">
                <asp:Image ID="ImgProfile" runat="server" />
            </div>
            </div>
            </div>
        </div>
    </div>
</div>
</div>
</main>
<script src="assets/vendor/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
<script src="assets/vendor/choices.js/public/assets/scripts/choices.min.js"></script>
<script src="assets/js/functions.js"></script>
</asp:Content>