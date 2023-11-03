<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserEntry.aspx.cs" Inherits="WebApplication.UserEntry" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" data-bs-theme="dark">
<head runat="server">
    <title>Social - Network, Community and Event Theme</title>
	<meta charset="utf-8"/>
	<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
	<meta name="author" content=""/>
	<meta name="description" content=""/>
	<link rel="shortcut icon" href="assets/images/favicon.ico"/>
	<link rel="preconnect" href="https://fonts.googleapis.com/"/>
  <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&amp;display=swap"/>
	<link rel="stylesheet" type="text/css" href="assets/vendor/font-awesome/css/all.min.css"/>
	<link rel="stylesheet" type="text/css" href="assets/vendor/bootstrap-icons/bootstrap-icons.css"/>
  <link rel="stylesheet" type="text/css" href="assets/vendor/choices.js/public/assets/styles/choices.min.css"/>
	<link rel="stylesheet" type="text/css" href="assets/css/style.css"/>
    <script>
        $(document).ready(function () {
            window.history.replaceState('', '', window.location.href)
        });
    </script>
    
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
	<script type="text/javascript">
        $(document).ready(() => {
            $('#FileUpload1').change(function () {
                const file = this.files[0];
                console.log(file);
                if (file) {
                    let reader = new FileReader();
                    reader.onload = function (event) {
                        console.log(event.target.result);
                        $('#imgPreview').attr('src', event.target.result);
                    }
                    reader.readAsDataURL(file);
                }
            });
        });
    </script>
</head>
<body>
<form id="form1" runat="server">
<main>
  <div class="container">
    <asp:Panel ID="pnlUserDetails" runat="server">
    <div class="row g-4" style="padding-top:10px;">
      <div class="col-md-8 col-lg-6 vstack gap-4">
        <div class="card">
          <div class="card-header border-0 pb-0">
            <h1 class="h4 card-title mb-0">step 1 of <span style="color:gray;">2</span></h1>
          </div>
          <div class="card-body">
            <div class="row g-3">
              <div class="col-sm-6 col-lg-3">
                <label class="form-label">Name (Required)</label>
                  <asp:TextBox ID="txtName" runat="server" class="form-control" placeholder="Enter Name"></asp:TextBox>
              </div>
              <div class="col-sm-6 col-lg-3">
                <label class="form-label">City (Required)</label>
                  <asp:TextBox ID="txtCity" runat="server" class="form-control" placeholder="City" ReadOnly></asp:TextBox>
              </div>
              <div class="col-sm-6 col-lg-2">
                <label class="form-label">Date of birth (Required)</label>
                  <asp:TextBox ID="txtDob" runat="server" class="form-control" placeholder="dd/mm/yyyy"></asp:TextBox>
              </div>
              <div class="col-sm-6 col-lg-2">
                <label class="form-label">Gender (required)</label>
                  <asp:DropDownList ID="ddlGen" runat="server" class="form-select js-choice">
                      <asp:ListItem Value="male">Male</asp:ListItem>
                      <asp:ListItem Value="Female">Female</asp:ListItem>
                  </asp:DropDownList>
              </div>
              <div class="col-sm-6 col-lg-2">
                <label class="form-label">Looking for (required)</label>
                  <asp:DropDownList ID="ddlLookingFor" runat="server" class="form-select js-choice" >
                      <asp:ListItem Value="Both">Both</asp:ListItem>
                      <asp:ListItem Value="Friend">Friend</asp:ListItem>
                      <asp:ListItem Value="Date">Date</asp:ListItem>
                  </asp:DropDownList>
              </div>
              <div class="col-sm-8 col-lg-8">
                <label class="form-label">About you (Character limit: 10 - 300) (Required)</label>
                  <asp:TextBox ID="txtAbout" runat="server" class="form-control" rows="3" TextMode="MultiLine" placeholder="Example: I am a techie"></asp:TextBox>
              </div>
              <div class="col-12 text-end">
                <button type="submit" class="btn btn-primary mb-0">Next</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    </asp:Panel>
    <asp:Panel ID="pnlUserInterests" runat="server">
    <div class="row g-4" style="padding-top:10px;">
      <div class="col-md-8 col-lg-6 vstack gap-4">
        <div class="card">
          <div class="card-header border-0 pb-0">
            <h1 class="h4 card-title mb-0">step 2 of 2</h1>
          </div>
          <div class="card-body">
            <div class="row g-3">
              <div class="col-md-6 col-lg-6">
                <label class="form-label">Select your interests (atleast 3) (Required)</label>
                 <asp:ListBox ID="multiSelectListBox" runat="server" SelectionMode="Multiple" class="form-select js-choice" data-search-enabled="true">
                    <asp:ListItem Text="reading" Value="reading" />
                    <asp:ListItem Text="trekking" Value="trekking" />
                    <asp:ListItem Text="hiking" Value="hiking" />
                    <asp:ListItem Text="art" Value="art" />
                    <asp:ListItem Text="singing" Value="singing" />
                    <asp:ListItem Text="dancing" Value="dancing" />
                    <asp:ListItem Text="listen Music" Value="listenMusic" />
                    <asp:ListItem Text="gardening" Value="gardening" />
                    <asp:ListItem Text="cooking" Value="cooking" />
                    <asp:ListItem Text="gym" Value="gym" />
                    <asp:ListItem Text="foodie" Value="foodie" />
                    <asp:ListItem Text="travelling" Value="travelling" />
                    <asp:ListItem Text="photography" Value="photography" />
                    <asp:ListItem Text="teaching" Value="teaching" />
                    <asp:ListItem Text="technology" Value="technology" />
                    <asp:ListItem Text="out door Gaming" Value="outdoorGaming" />
                    <asp:ListItem Text="in door Gaming" Value="indoorGaming" />
                    <asp:ListItem Text="petCaring" Value="petCaring" />
                    <asp:ListItem Text="coding" Value="coding" />
                    <asp:ListItem Text="fashion" Value="fashion" />
                    <asp:ListItem Text="night Life" Value="nightLife" />
                    <asp:ListItem Text="day Life" Value="daylife" />
                </asp:ListBox>
              </div>
              <div class="col-md-6 col-lg-6">
                <label class="form-label">Upload your pic (Required)</label><br />
                  <asp:FileUpload ID="FileUpload1" runat="server" /><br />
						<asp:Image ID="imgPreview" Height="260px" Width="200px" runat="server" />
              </div>
              <div class="col-12 text-end">
                    <asp:Button ID="BtnSubmit" runat="server" Text="Go ahead" class="btn btn-primary mb-0" OnClick="BtnSubmit_Click" />
              </div>
              <div class="col-12 text-end">
                  <asp:Label ID="lblMsg" runat="server" Text="" Visible="false" ForeColor="Red" Font-Bold="true"></asp:Label>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    </asp:Panel>
  </div>
</main>
<script src="assets/vendor/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
<script src="assets/vendor/choices.js/public/assets/scripts/choices.min.js"></script>
<script src="assets/js/functions.js"></script>
</form>
</body>
</html>