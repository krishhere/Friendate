<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--srolling css and java files--%>
	<link rel="stylesheet" type="text/css" href="assets/css/SwipeBundle.css">
	<script src="https://cdn.jsdelivr.net/npm/swiper@8/swiper-bundle.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<main>
  <div class="container-fluid">
    <div class="row justify-content-between g-0">
      <div class="col-md-2 col-lg-3 col-xxl-4 mt-n4">    </div>

      <!-- Main content START -->
      <div class="col-md-8 col-lg-6 col-xxl-4 vstack gap-4">
        <div class="card-body">
          <ul class="nav nav-pills nav-stack small fw-normal">
            <li class="nav-item">
              <a class="nav-link bg-light py-1 px-2 mb-0" href="#!" data-bs-toggle="modal" data-bs-target="#feedActionPhoto"> <i class="bi bi-image-fill text-success pe-2"></i>Friendship</a>
            </li>
            <li class="nav-item">
              <a class="nav-link bg-light py-1 px-2 mb-0" href="#!" data-bs-toggle="modal" data-bs-target="#feedActionVideo"> <i class="bi bi-camera-reels-fill text-info pe-2"></i>Date</a>
            </li>
            <li class="nav-item">
              <a href="#" class="nav-link bg-light py-1 px-2 mb-0" data-bs-toggle="modal" data-bs-target="#modalCreateEvents"> <i class="bi bi-calendar2-event-fill text-danger pe-2"></i>Join Event </a>
            </li>
            <li class="nav-item dropdown ms-lg-auto">
              <a class="nav-link bg-light py-1 px-2 mb-0" href="#" id="feedActionShare" data-bs-toggle="dropdown" aria-expanded="false">
                Filter<!-- <i class="bi bi-three-dots"></i> -->
              </a>
              <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="feedActionShare">
                <li><a class="dropdown-item" href="#"> <i class="bi bi-envelope fa-fw pe-2"></i>Men</a></li>
                <li><a class="dropdown-item" href="#"> <i class="bi bi-bookmark-check fa-fw pe-2"></i>Female</a></li>
              </ul>
            </li>
          </ul>
        </div>
        <!-- Card feed item START -->

    <asp:Repeater ID="rptUsers" runat="server" OnItemDataBound="rptUsers_ItemDataBound">
        <ItemTemplate>
            <div class="card">
              <div class="card-header border-0 pb-0">
                <div class="d-flex align-items-center justify-content-between">
                  <div class="d-flex align-items-center">
                    <div>
                      <div class="nav nav-divider">
                        <h6 class="nav-item card-title mb-0">
                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                        </h6>
                        <span class="nav-item small"> <asp:Label ID="lblCity" runat="server" Text='<%# Eval("city") %>'></asp:Label></span>
                      </div>
                      <p class="mb-0 small">
                          <asp:Label ID="lblLookFor" runat="server" Text="" class="badge bg-danger bg-opacity-10 text-danger mb-2 fw-bold"></asp:Label>
                      </p>
                    </div>
                  </div>
                  <div class="dropdown">
                    <a href="#" class="text-secondary btn btn-secondary-soft-hover py-1 px-2" id="cardFeedAction" data-bs-toggle="dropdown" aria-expanded="false">
                      <i class="bi bi-three-dots"></i>
                    </a>
                    <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="cardFeedAction" style="padding:0px;">
                      <li><a class="dropdown-item" href="#"> <i class="bi bi-flag fa-fw pe-2"></i>Report</a></li>
                    </ul>
                  </div>
                </div>
              </div>
          
              <div class="card-body">
                <div class="swiper sample-slider">
                    <div class="swiper-wrapper">
                        <div class="swiper-slide" style="text-align:center;">
                            <asp:Image ID="imgUserPost" runat="server" class="card-img" AlternateText="Image" style="Width:fit-content;" />
                        </div>
                        <div class="swiper-slide">
			                <span class="list-inline mb-0 d-flex flex-wrap gap-2" style="padding: 2% 4% 0 10%;height: 100px;overflow: overlay;border-bottom:1px solid;">
                                <asp:Label ID="lblAbout" runat="server" Text='<%# Eval("about") %>'></asp:Label>
			                </span>
			                <div style="text-align:center;"><b>Interests:</b></div><br/>

			                <ul class="list-inline mb-0 d-flex flex-wrap gap-2" style="padding: 1% 4% 0 10%;">
                                <asp:Repeater ID="rptUserInterests" runat="server" OnItemDataBound="rptUserInterests_ItemDataBound">
                                    <ItemTemplate>
                                      <li class="list-inline-item m-0">
                                          <asp:Label ID="lblInterest" runat="server" Text="" class="btn btn-sm" style="border: 1px solid;"></asp:Label>
                                      </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
		                </div>
                    </div>
                    <div class="swiper-button-prev"></div>
                    <div class="swiper-button-next"></div>
                </div>
                  <%--srolling javascript--%>
                  <script type="text/javascript">
                    function initSwiper() {
                        var mySwiper = new Swiper('.sample-slider', {
                            loop: false,                         //loop
                                pagination: {                       //pagination(dots)
                                    el: '.swiper-pagination',
                                },
                                navigation: {                       //navigation(arrows)
                                    nextEl: ".swiper-button-next",
                                    prevEl: ".swiper-button-prev",
                                },
                        });
                    }
                  </script>

                  <div class="nav nav-pills nav-pills-light nav-fill nav-stack small border-top my-top">
                    <asp:LinkButton ID="lnkFriend" runat="server" Text="<i class='bi bi-heart pe-1'></i> Friend" class="nav-link mb-0" style="float:left;" />
                    <asp:LinkButton ID="lnkDate" runat="server" Text="<i class='bi bi-heart pe-1'></i> Date" class="nav-link mb-0" style="float:left;" />
                    <li class="nav-item dropdown">
                        <a href="#" class="nav-link mb-0" id="cardShareAction" data-bs-toggle="dropdown" aria-expanded="false">
                            <i class="bi bi-reply-fill flip-horizontal ps-1"></i>Share
                        </a>
                        <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="cardShareAction">
                            <li><a class="dropdown-item" href="#"><i class="bi bi-envelope fa-fw pe-2"></i>Facebook</a></li>
                            <li><a class="dropdown-item" href="#"><i class="bi bi-bookmark-check fa-fw pe-2"></i>whatsapp</a></li>
                        </ul>
                    </li>
                  </div>
                <%--<ul class="nav nav-pills nav-pills-light nav-fill nav-stack small border-top my-top">
                  <li class="nav-item" ID="liLnkFriend" runat = "server">
                      <asp:LinkButton ID="lnkFriend1" runat="server" Text="<i class='bi bi-heart pe-1'></i> Friend" class="nav-link mb-0" />
                  </li>
                  <li class="nav-item" ID="liLnkDate" runat = "server">
                      <asp:LinkButton ID="lnkDate1" runat="server" Text="<i class='bi bi-heart pe-1'></i> Date" class="nav-link mb-0" />
                  </li>
                  <li class="nav-item dropdown">
                    <a href="#" class="nav-link mb-0" id="cardShareAction" data-bs-toggle="dropdown" aria-expanded="false">
                      <i class="bi bi-reply-fill flip-horizontal ps-1"></i>Share
                    </a>
                      <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="cardShareAction">
                      <li><a class="dropdown-item" href="#"> <i class="bi bi-envelope fa-fw pe-2"></i>Facebook</a></li>
                      <li><a class="dropdown-item" href="#"> <i class="bi bi-bookmark-check fa-fw pe-2"></i>whatsapp </a></li>
                    </ul>
                  </li>
                </ul>--%>
              </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
          <!-- Card feed item END -->

        <!-- Card feed item START -->
        <div class="card">
          <div class="card-header">
            <div class="d-flex align-items-center justify-content-between">
              <div class="d-flex align-items-center">
                <div class="avatar me-2">
                  <a href="#!"> <img class="avatar-img rounded-circle" src="assets/images/logo/12.svg" alt=""> </a>
                </div>
                <div>
                  <h6 class="card-title mb-0"><a href="#!"> Advertise to make your business big </a></h6>
                  <a href="#!" class="mb-0 text-body">Sponsored <i class="bi bi-info-circle ps-1" data-bs-container="body" data-bs-toggle="popover" data-bs-placement="top" data-bs-content="You're seeing this ad because your activity meets the intended audience of our site."></i> </a>
                </div>
              </div>
              <div class="dropdown">
                <a href="#" class="text-secondary btn btn-secondary-soft-hover py-1 px-2" id="cardShareAction2" data-bs-toggle="dropdown" aria-expanded="false">
                  <i class="bi bi-three-dots"></i>
                </a>
                <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="cardShareAction2">
                  <li><a class="dropdown-item" href="#"> <i class="bi bi-flag fa-fw pe-2"></i>Report post</a></li>
                </ul>
              </div>
            </div>
          </div>
          <div class="card-body">
            <p class="mb-0">Quickly design and customize responsive mobile-first sites with Bootstrap.</p>
          </div>
          <img src="assets/images/post/3by2/02.jpg" alt="">
          <div class="card-footer border-0 d-flex justify-content-between align-items-center">
            <p class="mb-0">Currently v5.1.3 </p>
            <a class="btn btn-primary-soft btn-sm" href="#"> Download now </a>
          </div>
        </div>
        <!-- Card feed item END -->

          <!-- Load more button START -->
          <a href="#!" role="button" class="btn btn-loader btn-primary-soft" data-bs-toggle="button" aria-pressed="true">
            <span class="load-text"> Load more </span>
            <div class="load-icon">
              <div class="spinner-grow spinner-grow-sm" role="status">
                <span class="visually-hidden">Loading...</span>
              </div>
            </div>
          </a>

      </div>
      <!-- Main content END -->

      <!-- Right sidebar START -->
      <div class="col-md-2 col-lg-3 col-xxl-4">      </div>
    </div>
  </div>
</main>
<script src="assets/vendor/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
<script src="assets/vendor/tiny-slider/dist/tiny-slider.js"></script>
<script src="assets/vendor/OverlayScrollbars-master/js/OverlayScrollbars.min.js"></script>
<script src="assets/vendor/choices.js/public/assets/scripts/choices.min.js"></script>
<script src="assets/vendor/glightbox-master/dist/js/glightbox.min.js"></script>
<script src="assets/vendor/flatpickr/dist/flatpickr.min.js"></script>
<script src="assets/vendor/plyr/plyr.js"></script>
<script src="assets/vendor/dropzone/dist/min/dropzone.min.js"></script>
<script src="assets/vendor/zuck.js/dist/zuck.min.js"></script>
<script src="assets/js/zuck-stories.js"></script>
<script src="assets/js/functions.js"></script>
</asp:Content>