<%@ Page Title="Login" Language="C#" MasterPageFile="~/LoginMaster.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="POSActiv8.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="section register min-vh-100 d-flex flex-column align-items-center justify-content-center py-4">
    <div class="container">
        <div class="row justify-content-center">
        <div class="col-lg-4 col-md-6 d-flex flex-column align-items-center justify-content-center">

            <div id="divErrorMessage" runat="server" class="container-fluid">

                <h2>Ooops! The system has encountered a problem.</h2>

                <p>
                    <label id="lblErrorMessage" runat="server"></label>
                </p>

                <p>
                    <asp:Button ID="btnReload" runat="server" CssClass="btn btn-primary" Text="Try again" OnClick="btnReload_Click"/>
                </p>

            </div>

             <div class="d-flex justify-content-center py-4">
                <a href="#" class="logo d-flex align-items-center w-auto">
                  <img src="Images/activ8-black.jpg" />
                  <span class="d-none d-lg-block"></span>
                </a>
              </div>

            <div id="divLogin" runat="server" class="card shadow" style="width:25rem;">

                <div class="card-body">

                    <div class="pt-4 pb-2">
                        <h5 class="card-title text-center pb-0 fs-4">Login - POS</h5>
                        <p class="text-center small">Enter your userid & password to login</p>
                    </div>

                    <div class="form-floating mb-3">
                        <%--<input id="txtUserName" runat="server" type="text" class="form-control" placeholder="name">--%>
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" placeholder="name"></asp:TextBox>
                        <label for="txtUserName">User ID</label>
                    </div>

                    <div class="form-floating mb-3">
                        <%--<input id="txtPassword" runat="server" type="password" class="form-control"  placeholder="name@example.com">--%>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="name" TextMode="Password"></asp:TextBox>
                        <label for="txtPassword">Password</label>
                    </div>

                    <p style="text-align:center;">
                        <asp:Button ID="btnLogin" runat="server" CssClass="w-100 btn btn-lg btn-primary" OnClick="btnLogin_Click" Text="Login" OnClientClick="LoadingProgress()"/>
                        <label id="lblLoadingMessage" runat="server"></label>
                        <img id="imgLoadingMessage" runat="server" alt="" src="Images/11x11progress.gif" style="display:none;"/>
                    </p>

                </div>

            </div>

            <div class="credits">
                <asp:label id="lblFooter" runat="server">test</asp:label>
                <asp:Label id="lblHostName" runat="server" Visible="false"></asp:Label>
                <asp:Label id="lblIPAddress" runat="server" Visible="false"></asp:Label>
                <label id="lblLoginAttempts" runat="server" Visible="false">0</label>
            </div>

            </div>
        </div>
    </div>
</section>

<%--User Disclaimer--%>

<div id="modalUserDisclaimer" class="modal fade" role="dialog">
    <div class="modal-dialog modal-dialog-centered modal-lg">

            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">
                        User Disclaimer
                    </h5>
                </div>

                <div class="modal-body">

                    <p>
                        Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
                    </p>

                </div>

                <div class="modal-footer">
                    <asp:Button ID="btnCloseUserDisclaimer" runat="server" CssClass="btn btn-default" Text="Close"/>
                </div>
            </div>

        </div>
    </div>
    

  
 <%--</ContentTemplate>
   <Triggers>
        <asp:PostBackTrigger ControlID="btnLogin"/>
        <asp:PostBackTrigger ControlID="btnAcceptDisclaimer"/>
    </Triggers>
</asp:UpdatePanel>--%>

<script type="text/javascript">
    function LoadingProgress()
    {
        var btn = document.getElementById("<%=btnLogin.ClientID%>");
        btn.hidden = true;

        var lbl = document.getElementById("<%=lblLoadingMessage.ClientID%>");
        lbl.innerText = "Verifying user, Please wait...";

        var imgloadingmessage = document.getElementById("<%=imgLoadingMessage.ClientID%>");
        imgloadingmessage.style.display = "inline";
    }
</script>

</asp:Content>
