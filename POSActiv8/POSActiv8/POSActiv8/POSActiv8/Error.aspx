<%@ Page Title="Error" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="POSActiv8.Error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="section register min-vh-100 d-flex flex-column align-items-center justify-content-center py-4">
    
    <img src="Images/nsjbi.jpg" class="img-fluid py-3" width="400px"/>

    <h2>Ooops! Something went wrong</h2>

    <p>
        <label id="lblErrorMessage" runat="server"></label>
    </p>

    <p>
        <asp:Button ID="btnHome" runat="server" CssClass="btn btn-primary" Text="Back to Home" OnClick="btnHome_Click"/>
    </p>

</section>

</asp:Content>
