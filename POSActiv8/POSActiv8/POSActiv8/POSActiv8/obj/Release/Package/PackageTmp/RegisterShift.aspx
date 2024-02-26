<%@ Page Title="Register Shift" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="RegisterShift.aspx.cs" Inherits="POSActiv8.RegisterShift" %>
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

    <h1>Register Shift</h1>

</div>

<div id="divButtonControls" runat="server" class="section">

    <div class="row">

        <div class="col-md-4 mb-3">
             <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" placeholder="Search..." ToolTip="Type in to find pos transaction"></asp:TextBox>
        </div>

       <div class="col-md-8 mb-3" style="text-align:right;">
            <asp:Button ID="btnCreate" runat="server" CssClass="btn btn-primary" Text="Create New" OnClick="btnCreate_Click" ToolTip="Click to open or close register" />
       </div>

    </div>

</div>

<div id="divRegisterShift" runat="server" class="section">

    <p>
        <label id="lblRegisterShift" runat="server"></label>
    </p>

    <div class="table-responsive">
   
        <asp:GridView ID="gvRegisterShift" runat="server" CssClass="table table-bordered table-hover align-middle" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No records to display" EmptyDataRowStyle-HorizontalAlign="center" ShowHeader="true" ShowFooter="false" GridLines="None" OnRowDataBound="gvRegisterShift_RowDataBound">
        <Columns>

            <asp:BoundField DataField="TransactionDate" HeaderText="Transaction Date">
                <HeaderStyle CssClass="text-center"/>
                <ItemStyle CssClass="text-center" Width="120px"/>
            </asp:BoundField>

            <asp:BoundField DataField="ControlNumber" HeaderText="Control Number">
                <HeaderStyle CssClass="text-center"/>
                <ItemStyle CssClass="text-center"/>
            </asp:BoundField>

            <asp:TemplateField HeaderText="Opening Amount">
                <HeaderStyle CssClass="text-end"/>
                <ItemTemplate>

                    &#8369; <label id="lblOpeningAmount" runat="server"><%# Eval("OpeningAmount") %></label>
                    <%--<div>
                        <medium class="text-muted">
                            <%# Eval("OpeningRemarks") %>
                        </medium>
                    </div>--%>
                </ItemTemplate>
                <ItemStyle CssClass="text-end"/>
            </asp:TemplateField>


            <asp:TemplateField HeaderText="Closing Amount">
                <HeaderStyle CssClass="text-end"/>
                <ItemTemplate>
                    &#8369; <label id="lblClosingAmount" runat="server"><%# Eval("ClosingAmount") %></label>
                    <%--<p>
                        <%# Eval("ClosingRemarks") %>
                    </p>--%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="right"/>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Status">
                <HeaderStyle CssClass="text-center"/>
                <ItemTemplate>
                    <asp:label id="lblTransactionStatus" runat="server" Text='<%# Eval("TransactionStatus") %>'></asp:label>
                </ItemTemplate>
                <ItemStyle CssClass="text-center" Font-Bold="true"/>
            </asp:TemplateField>

        </Columns>
        </asp:GridView>

    </div>

    <br />

</div>

    <!--Register Shift-->
    <div id="modalRegisterShift" class="modal fade" role="dialog">
        <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable">
    
          <!-- Modal content-->
          <div class="modal-content">

            <div class="modal-header">
                <h5 class="modal-title">
                    <asp:Label ID="lblRegisterShiftTitle" runat="server"></asp:Label>
                </h5>
                <asp:label id="lblRecordID" runat="server" visible="false"></asp:label>
            </div>

            <div class="modal-body">

                <asp:HiddenField ID="hfControlNumber" runat="server" />

                Transaction Date
                <br />
                <input type="date" id="txtTransactionDate" runat="server" class="form-control" tabindex="1"/>
                <br />

                Opening Amount
                <br />
                <asp:TextBox ID="txtOpeningAmount" runat="server" CssClass="form-control text-end" TabIndex="2"></asp:TextBox>
                <br />

                <div id="divClosing" runat="server">
                    Closing Amount
                    <br />
                    <asp:TextBox ID="txtClosingAmount" runat="server" CssClass="form-control text-end" TabIndex="3"></asp:TextBox>
                    <br />
                </div>

                Remarks
                <br />
                <textarea id="txtRemarks" runat="server" class="form-control" tabindex="3" rows="3" maxlength="200" placeholder="Input your remarks here"></textarea>
                <br />
                
            </div>

            <div class="modal-footer" style="justify-content: space-between;">
                <label ID="lblValidationMessage" runat="server"></label>
            </div>

            <div class="modal-footer">
                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-default" data-bs-dismiss="modal" Text="Cancel"/>
                <asp:Button ID="btnPost" runat="server" CssClass="btn btn-primary" OnClick="btnPost_Click"/>
            </div>

          </div>
      
        </div>
    </div>

    <%--Show Modal--%>
    <script type="text/javascript">
        function showModal() {
            var myModal = new bootstrap.Modal(document.getElementById('modalRegisterShift'), { keyboard: false });
            myModal.show();
        }
    </script>


</asp:Content>
