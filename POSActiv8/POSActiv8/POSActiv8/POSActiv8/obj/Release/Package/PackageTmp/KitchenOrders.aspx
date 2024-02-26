    <%@ Page Title="Kitchen Orders" Language="C#" MasterPageFile="~/KitchenMaster.Master" AutoEventWireup="true" CodeBehind="KitchenOrders.aspx.cs" Inherits="POSActiv8.KitchenOrders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container-fluid">

    <asp:Timer ID="tmrKitchenOrders" runat="server" OnTick="tmrKitchenOrders_Tick" Interval="5000"></asp:Timer>

    <div class="row d-flex justify-content-left align-items-left" style="height:auto;margin-top:70px;">

        <div id="divErrorMessage" runat="server" class="pagetitle">
             
            <h1>Ooops! Something went wrong.</h1>

            <p>
                <label id="lblErrorMessage" runat="server"></label>
            </p>

            <p>
                <asp:Button ID="btnReload" runat="server" CssClass="btn btn-primary" Text="Try again" OnClick="btnReload_Click"/>
            </p>

        </div>

        <div id="divKitchenOrders" runat="server" class="pagetitle col-md-12">

            <p>
                <label id="lblKitcherOrders" runat="server"></label>
                <asp:HiddenField ID="hfTransactionDate" runat="server" />
            </p>

            <div class="table-responsive">

                <asp:DataList ID="dtKitchenOrders" runat="server" CssClass="d-flex justify-content-left" RepeatDirection="Horizontal" RepeatColumns="4" CellPadding="5" OnItemCommand="dtKitchenOrders_ItemCommand" OnItemDataBound="dtKitchenOrders_ItemDataBound"> 
                <ItemTemplate>

                    <asp:LinkButton ID="lbtnTableNames" runat="server" CssClass="btn" BackColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("TableColor").ToString()) %>' CommandArgument='<%# Eval("ControlNumber") + "|" + Eval("TableName") %>' ToolTip="Tap to mark the order as completed">
                                
                        <div class="my-2" style="min-height:120px;width:270px;">
                            
                            <h5 class="my-3">
                                <asp:Label ID="lblControlNumber" runat="server" Visible="false" Text='<%# Eval("ControlNumber")%>'></asp:Label>
                                <strong class="text-dark">
                                    <%# Eval("TableName")%> - <%# Eval("FloorName")%> 
                                </strong>                               
                            </h5>

                            <div class="table-responsive my-3">

                                <asp:GridView ID="gvOrderItems" runat="server" CssClass="align-middle" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No records to display" ShowHeader="false" ShowFooter="false" GridLines="None" Width="100%">
                                <Columns>

                                    <asp:TemplateField>
                                        <HeaderStyle />
                                        <ItemTemplate>

                                            <medium class="text-light">
                                                <%# Eval("ItemQuantity") %> <small>x</small>  <%# Eval("ItemName") %>
                                            </medium>

                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                </asp:GridView>

                            </div>

                        </div>

                    </asp:LinkButton>

                </ItemTemplate>
                </asp:DataList>

            </div>
                    
        </div>



    </div>

</div>

   
    

</asp:Content>
