<%@ Page Title="Error Logs" Language="C#" MasterPageFile="~/Masterpage.Master" AutoEventWireup="true" CodeBehind="ErrorLogs.aspx.cs" Inherits="POSActiv8.ErrorLogs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="divErrorMessage" runat="server" class="pagetitle">

    <h2>Ooops! Something went wrong.</h2>

    <p>
        <label id="lblErrorMessage" runat="server"></label>
    </p>

    <p>
        <asp:Button ID="btnReload" runat="server" CssClass="btn btn-primary" Text="Try again" OnClick="btnReload_Click"/>
    </p>

</div>

<div id="divPageTitle" runat="server" class="pagetitle">

    <h1>Error Logs</h1>

</div>

<div id="divButtonControls" runat="server" class="section">

    <div class="row">
    
        <div class="col-md-4 mb-3">
            <asp:TextBox ID="txtReferenceDate" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtReferenceDate_TextChanged" type="date"></asp:TextBox>
        </div>

        <div class="col-md-8 mb-3" style="text-align:right;">
              <asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary" Text="Export" OnClick="btnExport_Click" />
        </div>

    </div>

</div>

<div id="divErrorLogs" runat="server" class="section">

    <p>
        <label id="lblErrorLogs" runat="server"></label>
    </p>

    <div class="table-responsive">

        <asp:GridView ID="grdErrorLogs" runat="server" CssClass="table table-bordered table-hover align-middle" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false" GridLines="None" ToolTip="Showing all the error logs based on the selected reference date">
        <Columns>

            <asp:BoundField DataField="SourceFile" HeaderText="Source">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left"/>
            </asp:BoundField>

            <asp:BoundField DataField="ErrorName" HeaderText="Section">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left"/>
            </asp:BoundField>

            <asp:BoundField DataField="ErrorDescription" HeaderText="Description">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left" Width="350px"/>
            </asp:BoundField>

            <asp:BoundField DataField="DateTimePosted" HeaderText="Date and Time Posted">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left"/>
            </asp:BoundField>

            <asp:BoundField DataField="PostedBy" HeaderText="Posted By">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left"/>
            </asp:BoundField>

        </Columns>
        </asp:GridView>

    </div>

    <br />

</div>

   <%--Export--%>
    
    <div id="modalExportErrorLogs" class="modal fade" role="dialog">
        <div class="modal-dialog modal-dialog-centered">
    
          <!-- Modal content-->
          <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">
                    Export Error Logs
                </h5>
                <asp:label id="lblRecordID" runat="server" visible="false"></asp:label>
            </div>

            <div class="modal-body">
                Date From
                <br />
                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" TabIndex="1" placeholder="mm/dd/yyyy" type="date"></asp:TextBox>
                <br />

                Date To
                <br />
                <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" TabIndex="2" placeholder="mm/dd/yyyy" type="date"></asp:TextBox>
                <br />

                <p style="text-align:left;">
                    <asp:label id="lblValidationExportErrorLogs" runat="server"></asp:label>
                </p>

            </div>

            <div class="modal-footer">
                <asp:Button ID="btnCancelExportErrorLogs" runat="server" CssClass="btn btn-default" Text="Cancel" OnClick="btnCancelExportErrorLogs_Click"/>
                <asp:Button ID="btnExportErrorLogs" runat="server" CssClass="btn btn-primary" Text="Export" OnClick="btnExportErrorLogs_Click" OnClientClick="LoadingProgress()"/>
                <label id="lblMessage" runat="server"></label>
                <img id="imgLoadingImage" runat="server" alt="" src="Images/11x11progress.gif" style="display:none;"/>
            </div>
          </div>
      
        </div>
    </div>

    <%--Show Modal--%>
    <script type="text/javascript">
        function showModal() {
            var myModal = new bootstrap.Modal(document.getElementById('modalExportSystemLogs'), { keyboard: false });
            myModal.show();
        }
    </script>

    <%--Hide Modal--%>
    <script type="text/javascript">
        function hideModal() {
            var myModal = new bootstrap.Modal(document.getElementById('modalExportSystemLogs'), { keyboard: false });
            myModal.hide();
        }
    </script>
   
    <script type="text/javascript">
        function LoadingProgress() {
            var btnexport = document.getElementById("<%=btnExportErrorLogs.ClientID%>");
        btnexport.style.display = "none";

        var btncancelexport = document.getElementById("<%=btnCancelExportErrorLogs.ClientID%>");
        btncancelexport.style.display = "none";

        var lbl = document.getElementById("<%=lblMessage.ClientID%>");
        lbl.innerText = "Generating report, Please wait...";

        var img = document.getElementById("<%=imgLoadingImage.ClientID%>");
            img.style.display = "inline";
        }
</script>

</asp:Content>