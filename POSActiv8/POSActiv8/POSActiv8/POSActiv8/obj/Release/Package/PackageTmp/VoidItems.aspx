<%@ Page Title="Void Items" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="VoidItems.aspx.cs" Inherits="POSActiv8.VoidItems" %>
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

    <h1>Void Items</h1>

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

<div id="divVoidItems" runat="server" class="section">

    <p>
        <label id="lblVoidItems" runat="server"></label>
    </p>

    <div class="table-responsive">

        <asp:GridView ID="gvVoidItems" runat="server" CssClass="table table-bordered table-hover align-middle" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false" GridLines="None" ToolTip="Showing all the voided items based on the selected transaction date">
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

            <asp:TemplateField HeaderText="Item">
                <HeaderStyle />
                <ItemTemplate>
                    <div>
                        <%# Eval("ItemName")%>
                    </div>
                    <small class="text-muted"><%# Eval("CategoryName") %></small>
                </ItemTemplate>
                <ItemStyle/>
            </asp:TemplateField>

            <asp:BoundField DataField="ItemPrice" HeaderText="Price">
                <HeaderStyle CssClass="text-end"/>
                <ItemStyle CssClass="text-end"/>
            </asp:BoundField>

            <asp:BoundField DataField="ItemQuantity" HeaderText="Quantity">
                <HeaderStyle CssClass="text-center"/>
                <ItemStyle CssClass="text-center"/>
            </asp:BoundField>

            <asp:BoundField DataField="ItemAmount" HeaderText="Amount">
                <HeaderStyle CssClass="text-end"/>
                <ItemStyle CssClass="text-end"/>
            </asp:BoundField>

            <asp:BoundField DataField="VoidAuthorizedBy" HeaderText="Authorized By">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left"/>
            </asp:BoundField>

            <asp:BoundField DataField="VoidedBy" HeaderText="Voided By">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left"/>
            </asp:BoundField>


        </Columns>
        </asp:GridView>

    </div>

    <br />

</div>


</asp:Content>