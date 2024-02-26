<%@ Page Title="Home" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="POSActiv8.HomePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<div id="divErrorMessage" runat="server" class="pagetitle">

    <h1 class="header-name">Ooops! Something went wrong.</h1>

    <p>
        <asp:label id="lblErrorMessage" runat="server"></asp:label>
    </p>

    <p>
        <asp:Button ID="btnReload" runat="server" CssClass="btn btn-primary" Text="Try again" OnClick="btnReload_Click"/>
    </p>

</div>

<div id="divPageTitle" runat="server" class="pagetitle">

    <h1>
        <asp:label id="lblWelcomeMessage" runat="server">Welcome</asp:label>
    </h1>

    <br />

    <p>
        <asp:label id="lblDateToday" runat="server"></asp:label>
    </p>

    <p>
        <asp:label id="lblLoginAttempts" runat="server"></asp:label>
    </p> 

</div>

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
