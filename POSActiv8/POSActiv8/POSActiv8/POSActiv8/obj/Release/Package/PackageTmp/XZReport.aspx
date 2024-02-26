<%@ Page Title="XZ Report" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="XZReport.aspx.cs" Inherits="POSActiv8.XZReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container">

    <div class="row d-flex justify-content-center align-items-center" style="height:auto;margin-top:80px;">

        <div id="divErrorMessage" runat="server" class="pagetitle">

            <h1>Ooops! Something went wrong.</h1>

            <p>
                <label id="lblErrorMessage" runat="server"></label>
            </p>

            <p>
                <asp:Button ID="btnReload" runat="server" CssClass="btn btn-primary" Text="Try again" OnClick="btnReload_Click"/>
            </p>

        </div>

        <div id="divXZReport" runat="server" class="card shadow" style="width:30rem;">

            <div class="card-body">

                <h5 class="card-title">
                    <label id="lblPageTitle" runat="server"></label>
                </h5>

                Transaction Date
                <br />
                <asp:TextBox ID="txtTransactionDate" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtTransactionDate_TextChanged" type="date"></asp:TextBox>
                <br />

                Employees
                <br />
                <asp:DropDownList ID="ddlPOSTerminal" runat="server" CssClass="form-select"></asp:DropDownList>
                <br />

                <p class="d-grid gap-2">
                    <asp:Button ID="btnGenerate" runat="server" CssClass="btn btn-primary" Text="Generate" OnClick="btnGenerate_Click"/>
                </p>
            
            </div>

        </div>
    
    </div>

</div>

    <!--XZ Report-->
    <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasXZReport" aria-labelledby="offcanvasXZReportLabel">
        <div class="offcanvas-header">
            <h5 class="offcanvas-title" id="offcanvasXZReportLabel">
                
            </h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>

        <div id="div1" runat="server" class="offcanvas-body">

            <div class="row text-center">
                <div class="col-md-12">
                    <h5>
                        <b><label id="lblXZReportTitle" runat="server"></label></b>
                    </h5>
                </div>
                <div class="col-md-12">
                    <label id="lblXZReportTransactionDate" runat="server"></label>
                </div>
            </div>

            <br />

            <div class="row mb-1">
                <div class="col-md-6 label">
                    <medium>POS Terminal</medium>
                </div>
                <div class="col-md-6 text-end">
                    <label id="lblPOSTerminal" runat="server"></label>
                </div>
            </div>
            <div class="row mb-1">
                <div class="col-md-6 label">
                    <medium>POS User</medium>
                </div>
                <div class="col-md-6 text-end">
                    <label id="lblPOSUser" runat="server"></label>
                </div>
            </div>

            <hr>

            <div class="row mb-1">
                <div class="col-md-8 label">
                    <medium>Number of Orders</medium>
                </div>
                <div class="col-md-4 text-end">
                    <label id="lblTotalOrders" runat="server">0.00</label>
                </div>
            </div>

            <div class="row mb-1">
                <div class="col-md-8 label">
                    <medium>
                        <b>Total Net Sales</b>
                    </medium>
                </div>
                <div class="col-md-4 text-end">
                    <b>
                        <label id="lblTotalNetSales" runat="server">0.00</label>
                    </b>
                </div>
            </div>

            <hr>

            <div class="row mb-1">
                <div class="col-md-8 label">
                    <medium>Opening Amount</medium>
                </div>
                <div class="col-md-4 text-end">
                    <label id="lblOpeningAmount" runat="server">0.00</label>
                </div>

                <div id="divExpectedDrawer" runat="server" class="col-md-8 label">
                    <medium>Expected Drawer</medium>
                </div>
                <div id="divExpectedDrawerAmount" runat="server" class="col-md-4 text-end">
                    <label id="lblExpectedDrawer" runat="server">0.00</label>
                </div>
        
                <div id="divClosing" runat="server" class="col-md-8 label">
                    <medium>Closing Amount</medium>
                </div>
                <div id="divClosingAmount" runat="server" class="col-md-4 text-end">
                    <label id="lblClosingAmount" runat="server">0.00</label>
                </div>

                <div id="divOverShort" runat="server" class="col-md-8 label">
                    <medium><label id="lblOverShort" runat="server"></label></medium>
                </div>
                <div id="divOverShortAmount" runat="server" class="col-md-4 text-end">
                    <label id="lblOverShortAmount" runat="server">0.00</label>
                </div>
            </div>

            <hr />

            <%--Order Status--%>

            <div class="row mb-1">
                <div class="col-md-8 label">
                    <medium>Order Status</medium>
                </div>
                <div class="col-md-4 text-end">
                </div>
            </div>

            <div class="row mb-1">
                <div class="col-md-8 label"><small class="text-muted">New</small></div>
                <div class="col-md-4 text-end">
                    <small>
                        <label id="lblNewOrders" runat="server">0.00</label>
                    </small>
                </div>
            </div>

            <div class="row mb-1">
                <div class="col-md-8 label"><small class="text-muted">Delivered</small></div>
                <div class="col-md-4 text-end">
                    <small>
                        <label id="lblDelivered" runat="server">0.00</label>
                    </small>
                </div>
            </div>

            <div class="row mb-1">
                <div class="col-md-8 label"><small class="text-muted">Paid</small></div>
                <div class="col-md-4 text-end">
                    <small>
                        <label id="lblPaid" runat="server">0.00</label>
                    </small>
                </div>
            </div>

            <div class="row mb-1">
                <div class="col-md-8 label"><small class="text-muted">Voided</small></div>
                <div class="col-md-4 text-end">
                    <small>
                        <label id="lblVoided" runat="server">0.00</label>
                    </small>
                </div>
            </div>

            <div class="row mb-1">
                <div class="col-md-8 label"><small class="text-muted">Cancelled</small></div>
                <div class="col-md-4 text-end">
                    <small>
                        <label id="lblCancelled" runat="server">0.00</label>
                    </small>
                </div>
            </div>

            <hr />

            <%--Payment Type--%>

            <div class="row mb-1">
                <div class="col-md-8 label"><medium>Payment Types</medium></div>
                <div class="col-md-4 text-end">
                </div>
            </div>

            <div class="row mb-1">
                <div class="col-md-8 label"><small class="text-muted">Cash Sales</small></div>
                <div class="col-md-4 text-end">
                    <small>
                        <label id="lblCashSales" runat="server">0.00</label>
                    </small>
                </div>
            </div>

            <div class="row mb-1">
                <div class="col-md-8 label"><small class="text-muted">Credit Card</small></div>
                <div class="col-md-4 text-end">
                    <small>
                        <label id="lblCreditCard" runat="server">0.00</label>
                    </small>
                </div>
            </div>

            <div class="row mb-1">
                <div class="col-md-8 label"><small class="text-muted">GCash</small></div>
                <div class="col-md-4 text-end">
                    <small>
                        <label id="lblGCash" runat="server">0.00</label>
                    </small>
                </div>
            </div>

            <hr />
                
        </div>

        <div class="container my-3">
            <p>
                <%--<asp:Button ID="btnPrintXZReport" runat="server" CssClass="btn btn-primary w-100"/>--%>
                <button id="btnPrintReceipt" class="btn btn-primary w-100" onclick="return PrintXZReport()">Print Report</button>
            </p>
            
        </div>

    </div>
      
    <%--Show OffCanvas--%>
    <script type="text/javascript">
        function showXZReport() {
            var myOffCanvas = new bootstrap.Offcanvas(document.getElementById('offcanvasXZReport'), { keyboard: false });
            myOffCanvas.show();
        }
    </script>

    <%--Print XZReport--%>
    <script type="text/javascript">
        function PrintXZReport() {
            var panel = document.getElementById("<%=divXZReport.ClientID %>");
            var printWindow = window.open('', '', 'height=800,width=700,toolbar=no,resizable=no');
            printWindow.document.write('<html><head><title>Print Receipt</title>');
            printWindow.document.write('<html><head>');
            printWindow.document.write('<link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">');
            printWindow.document.write('<link href="assets/css/style.css" rel="stylesheet">');
            printWindow.document.write('</head><body>');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
    </script>

</asp:Content>