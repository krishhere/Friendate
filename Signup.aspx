<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="WebApplication.Signup" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign up</title>
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
    <div class="row justify-content-center align-items-center vh-100 py-5">
      <div class="col-sm-10 col-md-8 col-lg-7 col-xl-6 col-xxl-5">
        <div class="card card-body rounded-3 p-4 p-sm-5" style="background-color:#000000e8;">
          <div class="text-center">
            <h1 class="mb-2">Sign up</h1>
          </div>
          <form class="mt-4">
            <div class="mb-3 input-group-lg">
              <input type="email" class="form-control" placeholder="Enter email">
              <%--<small>We'll never share your email with anyone else.</small>--%>
            </div>
            <div class="mb-3 position-relative">
              <div class="input-group input-group-lg">
                <input class="form-control fakepassword" type="password" id="psw-input" placeholder="Enter new password">
                <span class="input-group-text p-0">
                  <i class="fakepasswordicon fa-solid fa-eye-slash cursor-pointer p-2 w-40px"></i>
                </span>
              </div>
              <div id="pswmeter" class="mt-2"></div>
              <div class="d-flex mt-1">
                <div id="pswmeter-message" class="rounded"></div>
                <div class="ms-auto">
                  <i class="bi bi-info-circle ps-1" data-bs-container="body" data-bs-toggle="popover" data-bs-placement="top" data-bs-content="Include at least one uppercase, one lowercase, one special character, one number and 8 characters long." data-bs-original-title="" title=""></i>
                </div>
              </div>
            </div>
            <div class="mb-3 input-group-lg">
              <input class="form-control" type="password" placeholder="Confirm password">
            </div>
            <div class="mb-3 text-start">
            <span class="d-block" style="text-align:right;">Already have an account? <a href="Login.aspx">Sign in here</a></span>
              <%--<input type="checkbox" class="form-check-input" id="keepsingnedCheck">
              <label class="form-check-label" for="keepsingnedCheck"> Keep me signed in</label>--%>
            </div>
            <div class="d-grid"><button type="submit" class="btn btn-lg btn-primary">Sign me up</button></div>
            
            <p class="mb-0 mt-3">©<%: DateTime.Now.Year %> <a target="_blank" href="#">MyView</a> All rights reserved</p>
          </form>
        </div>
      </div>
    </div>
  </div>
</main>

<script src="assets/vendor/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
<script src="assets/vendor/pswmeter/pswmeter.min.js"></script>
<script src="assets/js/functions.js"></script>
</form>
</body>
</html>