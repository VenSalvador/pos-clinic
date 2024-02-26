<%@ Page Title="Sales Invoice" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="SalesInvoice.aspx.cs" Inherits="POSActiv8.SalesInvoice" %>
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

    <h1>Sales Invoice</h1>
    <asp:HiddenField ID="hfControlNumber" runat="server" />

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

<div id="divSalesInvoice" runat="server" class="section">

    <p>
        <label id="lblSalesInvoice" runat="server"></label>
    </p>

    <div class="table-responsive">

        <asp:GridView ID="gvSalesInvoice" runat="server" CssClass="table table-bordered table-hover align-middle" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false" GridLines="None" ToolTip="Showing all the sales invoice based on the selected transaction date" OnRowCommand="gvSalesInvoice_RowCommand">
        <Columns>

            <asp:TemplateField HeaderText="">
                <HeaderStyle />
                <ItemTemplate>
                    <medium>
                        <asp:LinkButton ID="link" runat="server" Text="View" CommandArgument='<%# Eval("ControlNumber")%>' ToolTip="Click to view the details"></asp:LinkButton>
                    </medium>
                </ItemTemplate>
                <ItemStyle Width="1px" CssClass="text-center"/>
            </asp:TemplateField>

            <asp:BoundField DataField="ControlNumber" HeaderText="Control Number">
                <HeaderStyle CssClass="text-center"/>
                <ItemStyle CssClass="text-center"/>
            </asp:BoundField>

            <asp:BoundField DataField="SubTotal" HeaderText="Sub Total">
                <HeaderStyle CssClass="text-end"/>
                <ItemStyle CssClass="text-end"/>
            </asp:BoundField>

            <asp:TemplateField HeaderText="Discount">
                <HeaderStyle />
                <ItemTemplate>
                    <div>
                        <%# Eval("Discount")%>
                    </div>
                    <small class="text-muted"><%# Eval("DiscountRemarks") %></small>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="left"/>
            </asp:TemplateField>

            <asp:BoundField DataField="DiscountedPrice" HeaderText="Discount Price">
                <HeaderStyle CssClass="text-end"/>
                <ItemStyle CssClass="text-end"/>
            </asp:BoundField>

            <asp:BoundField DataField="GrandTotal" HeaderText="Grand Total">
                <HeaderStyle CssClass="text-end"/>
                <ItemStyle CssClass="text-end"/>
            </asp:BoundField>

            <asp:BoundField DataField="PaymentType" HeaderText="Payment">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left"/>
            </asp:BoundField>

            <asp:BoundField DataField="OrderStatus" HeaderText="Status">
                <HeaderStyle CssClass="text-center"/>
                <ItemStyle CssClass="text-center"/>
            </asp:BoundField>

        </Columns>
        </asp:GridView>

    </div>

    <br />

</div>

    <%--Sales Invoice Details--%>

    <div id="modalSalesInvoiceDetails" class="modal fade" role="dialog">
        <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
    
          <!-- Modal content-->
          <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">
                    <label id="lblSalesInvoiceDetailsTitle" runat="server"></label>
                </h5>
                <asp:label id="lblRecordID" runat="server" visible="false"></asp:label>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>

            <div class="modal-body">

                <div class="table-responsive">

                    <asp:GridView ID="gvSalesInvoiceDetails" runat="server" CssClass="table table-bordered table-hover align-middle" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false" GridLines="None">
                    <Columns>

                        <asp:TemplateField HeaderText="Table Name">
                            <HeaderStyle />
                            <ItemTemplate>
                                <div>
                                    <%# Eval("TableName")%>
                                </div>
                                <small class="text-muted"><%# Eval("FloorName") %></small>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left"/>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Item Name">
                            <HeaderStyle />
                            <ItemTemplate>
                                <div>
                                    <%# Eval("ItemName")%>
                                </div>
                                <small class="text-muted"><%# Eval("CategoryName") %></small>
                            </ItemTemplate>
                            <ItemStyle/>
                        </asp:TemplateField>

                        <asp:BoundField DataField="ItemPrice" HeaderText="Item Price">
                            <HeaderStyle CssClass="text-end"/>
                            <ItemStyle CssClass="text-end"/>
                        </asp:BoundField>

                        <asp:BoundField DataField="ItemQuantity" HeaderText="Item Quantity">
                            <HeaderStyle CssClass="text-center"/>
                            <ItemStyle CssClass="text-center"/>
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="Discount">
                            <HeaderStyle />
                            <ItemTemplate>
                                <div>
                                    <%# Eval("DiscountName")%>
                                </div>
                                <small class="text-muted"><%# Eval("DiscountRemarks") %></small>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left"/>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Item Amount">
                            <HeaderStyle CssClass="text-end" />
                            <ItemTemplate>
                                <div>
                                    <%# Eval("ItemAmount")%>
                                </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="text-end"/>
                            <%--<FooterTemplate>
                                <asp:Label ID="lblTotalItemAmount" runat="server" ToolTip="Total Items Amount"></asp:Label>
                            </FooterTemplate>
                            <FooterStyle HorizontalAlign="right" Font-Bold="true"/>--%>
                        </asp:TemplateField>

                        <%--<asp:BoundField DataField="PaymentType" HeaderText="Payment">
                            <HeaderStyle HorizontalAlign="left"/>
                            <ItemStyle HorizontalAlign="left"/>
                        </asp:BoundField>

                        <asp:BoundField DataField="OrderStatus" HeaderText="Status">
                            <HeaderStyle CssClass="text-center"/>
                            <ItemStyle CssClass="text-center"/>
                        </asp:BoundField>--%>

                    </Columns>
                    </asp:GridView>

                </div>

            </div>

            <div class="modal-footer" style="justify-content: space-between;">
                <label id="lblGrandTotal" runat="server"></label>
            </div>

            <div class="modal-footer">
                <button class="btn btn-default" data-bs-dismiss="modal">Close</button>
            </div>

          </div>
      
        </div>
    </div>

    <%--Show Modal--%>
    <script type="text/javascript">
        function showModal() {
            var myModal = new bootstrap.Modal(document.getElementById('modalSalesInvoiceDetails'), { keyboard: false });
            myModal.show();
        }
    </script>

</asp:Content>