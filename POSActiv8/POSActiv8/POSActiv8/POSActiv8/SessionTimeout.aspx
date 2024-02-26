<%@ Page Title="Session Timeout" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="SessionTimeout.aspx.cs" Inherits="POSActiv8.SessionTimeout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<br /><br /><br />

<div id="divSessionTimeout" runat="server" class="pagetitle my-lg-5">

    <h1>Session Timeout</h1>

    <p>
        Your session has expired due to inactivity. Please login again.
    </p>

    <p>
        <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-primary" Text="Login Again" OnClick="btnLogin_Click"/>
    </p>

</div>

</asp:Content>

