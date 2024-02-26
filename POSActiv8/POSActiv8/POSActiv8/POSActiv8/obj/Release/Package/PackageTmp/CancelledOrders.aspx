<%@ Page Title="Cancelled Orders" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="CancelledOrders.aspx.cs" Inherits="POSActiv8.CancelledOrders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="divErrorMessage" runat="server" class="pagetitle">

    <h1>Ooops! Something went wrong.</h1>

    <p>
        <label id="lblErrorMessage" runat="server"></label>
    </p>

    <p>
        <asp:Button ID="btnReload" runat="server" CssClass="btn btn-primary" Text="Try again" OnClick="btnReload_Click"/>
    </p>

</div>

<div id="divPageTitle" runat="server" class="pagetitle">

    <h1>Cancelled Orders</h1>

</div>

<div id="divButtonControls" runat="server" class="section">

    <div class="row">
        
        <div class="col-md-4 mb-3">
            <asp:TextBox ID="txtTransactionDate" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtTransactionDate_TextChanged" type="date"></asp:TextBox>
        </div>

        <div class="col-md-8 mb-3" style="text-align:right;">
              <asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary" Text="Export" OnClick="btnExport_Click" Visible="false" />
        </div>

    </div>

</div>

<div id="divCancelledOrders" runat="server" class="section">

    <p>
        <label id="lblCancelledOrders" runat="server"></label>
    </p>

    <div class="table-responsive">

        <asp:GridView ID="gvCancelledOrders" runat="server" CssClass="table table-bordered table-hover align-middle" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false" GridLines="None" ToolTip="Showing all the voided items based on the selected transaction date">
        <Columns>

            <asp:BoundField DataField="ControlNumber" HeaderText="Control Number">
                <HeaderStyle CssClass="text-center"/>
                <ItemStyle CssClass="text-center"/>
            </asp:BoundField>

            <asp:TemplateField HeaderText="Table">
                <HeaderStyle />
                <ItemTemplate>
                    <div>
                        <%# Eval("TableName")%>
                    </div>
                    <small class="text-muted"><%# Eval("FloorName") %></small>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="left"/>
            </asp:TemplateField>

            <asp:BoundField DataField="OrderCount" HeaderText="Order Count">
                <HeaderStyle CssClass="text-center"/>
                <ItemStyle CssClass="text-center"/>
            </asp:BoundField>

            <asp:BoundField DataField="GrandTotal" HeaderText="Grand Total">
                <HeaderStyle CssClass="text-end"/>
                <ItemStyle CssClass="text-end"/>
            </asp:BoundField>

            <asp:BoundField DataField="CancellationType" HeaderText="Cancellation Type">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left"/>
            </asp:BoundField>

            <asp:BoundField DataField="CanceldAuthorizedBy" HeaderText="Authorized By">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left"/>
            </asp:BoundField>

            <asp:BoundField DataField="CancelledBy" HeaderText="Cancelled By">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left"/>
            </asp:BoundField>


        </Columns>
        </asp:GridView>

    </div>

    <br />

</div>


</asp:Content>