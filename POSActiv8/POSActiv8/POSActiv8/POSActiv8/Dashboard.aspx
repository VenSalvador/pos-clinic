<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="POSActiv8.Dashboard" %>
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

    <h1 class="mb-2">Dashboard</h1>

    <p>
        <label id="lblReferenceDate" runat="server"></label>
        <asp:HiddenField ID="hfRentalCost" runat="server" />
        <asp:HiddenField ID="hfRepairCost" runat="server" />
        <asp:HiddenField ID="hfMaintenanceCost" runat="server" />
    </p>

</div>


<div id="divDashboard" runat="server" class="section dashboard">
    
    <div class="row">

    <!-- Left side columns -->
    <div class="col-lg-12">

        <div class="row">
         
            <!-- Total Net Sales -->
            <div id="divTotalSales" runat="server" class="col-xxl-4 col-md-4">
              <div class="card info-card sales-card">

                <div class="card-body">
                  <h5 class="card-title">Total Sales</h5>

                  <div class="d-flex align-items-center">
                    <div class="card-icon rounded-circle d-flex align-items-center justify-content-center">
                      <i class="bi bi-cash"></i>
                    </div>
                    <div class="ps-3">
                      
                        <h6>
                           &#8369; <asp:LinkButton ID="lnkTotalSales" runat="server" CssClass="pagetitle" ToolTip="Click here to view all vehicles"></asp:LinkButton>
                        </h6>

                        <%--<span class="text-muted small pt-2 ps-1">units</span>--%>
                      
                    </div>
                  </div>
                </div>

              </div>
            </div>

            <!-- Opening Balance -->
            <div id="divOpeningAmount" runat="server" class="col-xxl-4 col-md-4">
              <div class="card info-card revenue-card">

                <div class="card-body">
                  <h5 class="card-title">Opening Amount</h5>

                  <div class="d-flex align-items-center">
                    <div class="card-icon rounded-circle d-flex align-items-center justify-content-center">
                      <i class="bi bi-cash"></i>
                    </div>
                    <div class="ps-3">
                      
                        <h6>
                            &#8369; <asp:LinkButton ID="lnkOpeningAmount" runat="server" CssClass="pagetitle" ToolTip="Click here to view all operational vehicles" OnClick="lnkOpeningAmount_Click"></asp:LinkButton>
                        </h6>

                    </div>
                  </div>
                </div>

              </div>
            </div>

            <!-- Closing Amount -->
            <div id="divClosingAmount" runat="server" class="col-xxl-4 col-md-4">
              <div class="card info-card customers-card">

                <div class="card-body">
                  <h5 class="card-title">Closing Amount</h5>

                  <div class="d-flex align-items-center">
                    <div class="card-icon rounded-circle d-flex align-items-center justify-content-center">
                      <i class="bi bi-cash"></i>
                    </div>
                    <div class="ps-3">
                      
                        <h6>
                            &#8369; <asp:LinkButton ID="lnkClosingAmount" runat="server" CssClass="pagetitle" ToolTip="Click here to view all breakdown vehicles"></asp:LinkButton>
                        </h6>

                    </div>
                  </div>
                </div>

              </div>
            </div> <!-- End Sales Card -->

            <!-- Equipment Breakdown -->
            <div id="divVehicleInventory" runat="server" class="col-12">

                <div class="card recent-sales overflow-auto">

                <div class="card-body">
                  
                    <h5 class="card-title mb-3">
                        Sales Invoice
                    </h5>

                    <canvas id="myChart" style="max-height:300px;"></canvas>

                </div>

                </div>
            </div>

            


        </div>
    </div><!-- End Left side columns -->

    <!-- Right side columns -->
    <div class="col-lg-4">

    </div><!-- End Right side columns -->

    </div>
</div>


<script src="https://cdn.jsdelivr.net/npm/chart.js@3.9.1/dist/chart.min.js"></script>
  
<script type="text/javascript">

    <%--var rentalcost = [0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00];
    var repaircost = [0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00]; <%--document.getElementById("<%=hfRepairCost.ClientID%>").value;
    var maintenancecost = [0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00]--%>
    
    const ctx = document.getElementById('myChart').getContext('2d');
    const myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: ['JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEPT', 'OCT', 'NOV', 'DEC'],
            datasets: [
            {
                label: 'New',
                data: [0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 1550, 0.00],
                fill: false,
                borderColor: 'rgb(255, 205, 86)',
                backgroundColor: 'rgb(255, 205, 86)',
                tension: 0.1,
                showLine: true,
                spanGaps: true
            },
            {
                label: 'Paid',
                data: [0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00],
                fill: false,
                borderColor: 'rgb(54, 162, 235)',
                backgroundColor: 'rgb(54, 162, 235)',
                tension: 0.1,
                showLine: true,
                spanGaps: true
            },
            {
                label: 'Cancelled',
                data: [0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00],
                fill: false,
                borderColor: 'rgb(75, 192, 192)',
                backgroundColor: 'rgb(75, 192, 192)',
                tension: 0.1,
                showLine: true,
                spanGaps: true
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });


    
    function updateChart() {
        var rentalcost = document.getElementById("<%=hfRentalCost.ClientID%>").value;
        var repaircost = document.getElementById("<%=hfRepairCost.ClientID%>").value;
        var maintenancecost = document.getElementById("<%=hfMaintenanceCost.ClientID%>").value;
        myChart.data.datasets[0].data = rentalcost.split(',');
        myChart.data.datasets[1].data = repaircost.split(',');
        myChart.data.datasets[2].data = maintenancecost.split(',');


        myChart.update();
    };



</script>

</asp:Content>
