<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="WebApplication.Signup" Async="true" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign up</title>
    <meta charset="utf-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
	<meta name="author" content=""/>
	<meta name="description" content=""/>
	<link rel="shortcut icon" href="assets/images/favicon.ico"/>
	<link rel="preconnect" href="https://fonts.googleapis.com/"/>
  <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&amp;display=swap"/>
	<link rel="stylesheet" type="text/css" href="assets/vendor/font-awesome/css/all.min.css"/>
	<link rel="stylesheet" type="text/css" href="assets/vendor/bootstrap-icons/bootstrap-icons.css"/>
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
          <div class="mt-4">
              <asp:Panel ID="pnlMail" runat="server">
                <div class="mb-3 input-group-lg">
                    <asp:TextBox ID="txtEmail" runat="server" class="form-control" placeholder="Enter email" AutoPostBack="true" OnTextChanged="txtEmail_TextChanged"></asp:TextBox>
                    <asp:Label ID="lblEmailMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                  <%--<small>We'll never share your email with anyone else.</small>--%>
                  <div class="d-grid">
                      <asp:Button ID="btnMailConfirm" runat="server" Text="Confirm" class="btn btn-lg btn-primary" OnClick="btnMailConfirm_Click" />
                  </div>
                    <div class="d-grid">
                        <asp:TextBox ID="txtCode" runat="server" class="form-control" placeholder="Enter 6 digit code sent to above email id" Visible="false"></asp:TextBox>
                  </div>
                    <div class="d-grid">
                    <asp:Button ID="btnCodeConfirm" runat="server" Text="Confirm" class="btn btn-lg btn-primary" Visible="false" OnClick="btnCodeConfirm_Click"/>
                  </div>
                </div>
              </asp:Panel>
              <asp:Panel ID="pnlPswd" runat="server" Visible="false">
                <div class="mb-3 position-relative">
                  <div class="input-group input-group-lg">
                      <asp:TextBox ID="txtPswd" runat="server" class="form-control fakepassword" placeholder="Enter new password" TextMode="Password" data-bs-container="body" data-bs-toggle="popover" data-bs-placement="top" data-bs-content="Include at least one uppercase, one lowercase, one special character, one number and 8 characters long." data-bs-original-title="" title="" MaxLength="12"></asp:TextBox>
                      <span class="input-group-text p-0">
                      <i class="fakepasswordicon fa-solid fa-eye-slash cursor-pointer p-2 w-40px"></i>
                    </span>
                  </div>
                </div>
                <div class="mb-3 input-group-lg">
                    <asp:TextBox ID="txtConPswd" runat="server" placeholder="Confirm password" class="form-control" TextMode="Password" MaxLength="12"></asp:TextBox>
                </div>
                <div class="mb-3 text-start">
                    <span class="d-block" style="text-align:right;">Already have an account? <a href="Login.aspx">Sign in here</a></span>
                </div>
                <div class="d-grid">
                    <asp:Button ID="btnSignUp" runat="server" Text="Sign me up" class="btn btn-lg btn-primary" OnClick="btnSignUp_Click" />
                </div>
            </asp:Panel>
              <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
            <p class="mb-0 mt-3">©<%: DateTime.Now.Year %> <a target="_blank" href="#">frinDate</a> All rights reserved</p>
          </div>
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