﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication.Login" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Social - Network, Community and Event Theme</title>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
	<meta name="author" content="">
	<meta name="description" content="">
	<link rel="shortcut icon" href="assets/images/favicon.ico">
	<link rel="preconnect" href="https://fonts.googleapis.com/">
  <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&amp;display=swap">
	<link rel="stylesheet" type="text/css" href="assets/vendor/font-awesome/css/all.min.css">
	<link rel="stylesheet" type="text/css" href="assets/vendor/bootstrap-icons/bootstrap-icons.css">
	<link rel="stylesheet" type="text/css" href="assets/css/style.css" />
</head>
<body>
    <form id="form1" runat="server">
       <main style="background-color:black;">
  <div class="container">
    <div class="row justify-content-center align-items-center vh-100">
      <div class="col-sm-10 col-md-8 col-lg-7 col-xl-6 col-xxl-5">
        <div class="card card-body text-center p-4 p-sm-5" style="background-color:#000000e8;">
          <h1 class="mb-2">Sign in</h1>
          <form class="mt-sm-4">
            <div class="mb-3 input-group-lg">
              <input type="email" class="form-control" placeholder="Enter email">
            </div>
            <div class="mb-3 position-relative">
              <div class="input-group input-group-lg">
                <input class="form-control fakepassword" type="password" id="psw-input" placeholder="Enter new password">
                <span class="input-group-text p-0">
                  <i class="fakepasswordicon fa-solid fa-eye-slash cursor-pointer p-2 w-40px"></i>
                </span>
              </div>
            </div>
            <div class="mb-3 d-sm-flex justify-content-between">
              <div>
                <input type="checkbox" class="form-check-input" id="rememberCheck">
                <label class="form-check-label" for="rememberCheck">Remember me?</label>
              </div>
              <a href="forgot-password.html">Forgot password?</a>
            </div>
            <div class="d-grid"><button type="submit" class="btn btn-lg btn-primary">Login</button></div><br />
            <div class="d-grid">
                <asp:Button ID="btnSignInGoogle" runat="server" Text="Sign in with Google" class="btn btn-lg btn-primary" OnClick="btnSignInGoogle_Click" /> </div>
              <br />
          <p class="mb-0">Don't have an account?<a href="Signup.aspx"> Click here to sign up</a></p>
            <p class="mb-0 mt-3">©<%: DateTime.Now.Year %> <a target="_blank" href="#">MySite</a> All rights reserved</p>
          </form>
        </div>
      </div>
    </div>
  </div>

<script src="assets/vendor/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
<script src="assets/vendor/pswmeter/pswmeter.min.js"></script>
<script src="assets/js/functions.js"></script>
</main>
    </form>
</body>
</html>