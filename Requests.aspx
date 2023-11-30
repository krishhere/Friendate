<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Requests.aspx.cs" Inherits="WebApplication.Requests" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManagerRequests" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanelRequests" runat="server">
<ContentTemplate>
    <main>
         <section class="pt-0">
    <div class="container">
      <div class="row g-4">
        <div class="col-12 vstack gap-4">
            <div class="card">
                <a href="##" onClick="history.go(-1); return false;"><i class="bi bi-house-heart-fill"></i> <span style="text-decoration:underline;">Home</span></a>
              <div class="card-header d-sm-flex align-items-center text-center justify-content-sm-between border-0 pb-0">
                <h2 class="h4 card-title">Requests</h2>
              </div>
              <div class="card-body">
                <div class="row g-4">
                    <asp:Repeater ID="rptUsers" runat="server" OnItemDataBound="rptUsers_ItemDataBound" OnItemCommand="rptUsers_ItemCommand">
                    <ItemTemplate>
                  <div class="col-sm-6 col-xl-4">
                    <div class="card h-100" style="padding:5px;">
                      <div class="position-relative">
                          <asp:Image ID="imgSenderPost" runat="server" class="img-fluid rounded-top" style="width:-webkit-fill-available;text-align:center;" />
                        <div class="badge mt-1 me-1 position-absolute bottom-0 end-0">
                          <asp:Label ID="lblRequestedFor" runat="server" Text='<%# Eval("RequestType") %>' CssClass="badge bg-danger text-white"></asp:Label>
                        </div>
                      </div>
                      <div class="card-body position-relative pt-0">
                        <span class="btn btn-xs btn-primary mt-n3">
                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("senderId") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblRequestAccept" runat="server" Text='<%# Eval("accept") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("Name") %>' Font-Bold="false" Font-Size="15px"></asp:Label>
                        </span>
                        <p class="mb-0 small" style="display: flex; justify-content: space-between; align-items: center;width:100%;"> 
                             <asp:Label ID="lblCity" runat="server" Text='<%# "<i class=\"bi bi-geo-alt pe-1\"></i> " + Eval("city") %>'></asp:Label>
                            <asp:Label ID="lblDOB" runat="server" Text='<%# "<i class=\"bi bi-person-fill\"></i> " + Eval("age") + " years" %>'></asp:Label> 
                            <asp:Label ID="lblGender" runat="server" Text='<%# "<i class=\"bi bi-gender-ambiguous\"></i> " + Eval("gender") %>'></asp:Label> </p>
                        <h6 class="mt-2" style="border-bottom:1px solid gray;"> <a > <asp:Label ID="lblAbout" runat="server" Text='<%# Eval("about") %>'></asp:Label> </a> </h6>
                        <h6>Interests:</h6>
                          <div>
                        <asp:Repeater ID="rptUserInterests" runat="server" OnItemDataBound="rptUserInterests_ItemDataBound">
                        <ItemTemplate>
                            <asp:Label ID="lblInterest" runat="server" Text="" class="small text-secondary" style="border: 1px solid gray;padding:5px;border-radius:3px;display: inline-block; margin:0px 0px 5px 0px; box-sizing: border-box;"></asp:Label>
                        </ItemTemplate>
                        </asp:Repeater>
                              </div>
                          <asp:Panel ID="pnlAcceptReject" runat="server">
                        <div class="d-flex mt-3 justify-content-between">
                          <div class="w-50" style="padding-right:5px;">
                              <asp:LinkButton ID="lbAccept" runat="server" class="btn btn-sm btn-outline-success d-block" CommandName="Accept"><i class="fa-solid fa-thumbs-up me-1"></i> Accept</asp:LinkButton>
                          </div>
                          <div class="w-50">
                              <asp:LinkButton ID="lbReject" runat="server" class="btn btn-sm btn-outline-danger d-block" CommandName="Reject"><i class="fa-solid fa-thumbs-down me-1"></i> Reject</asp:LinkButton>
                          </div>
                        </div>
                              </asp:Panel>
                          <asp:Panel ID="pnlMsg" runat="server" Visible="false">
                        <div class="d-flex mt-3 justify-content-between">
                          <div class="w-50">
                              <asp:LinkButton ID="lbMessage" runat="server" class="btn btn-sm btn-outline-success d-block"><i class="bi bi-envelope fa-fw pe-1"></i> Message</asp:LinkButton>
                          </div>
                        </div>
                              </asp:Panel>
                      </div>
                  </div>
                  </div>
        </ItemTemplate>
        </asp:Repeater>
                </div>
            </div>
          </div>
        </div>
      </div>
    </div>
</section>
</main>
<script src="assets/vendor/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
<script src="assets/vendor/dropzone/dist/dropzone.js"></script>
<script src="assets/vendor/flatpickr/dist/flatpickr.min.js"></script>
<script src="assets/vendor/choices.js/public/assets/scripts/choices.min.js"></script>
<script src="assets/js/functions.js"></script>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>