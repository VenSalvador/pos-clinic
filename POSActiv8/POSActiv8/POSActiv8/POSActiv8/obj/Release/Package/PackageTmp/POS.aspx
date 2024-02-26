<%@ Page Title="POS" Language="C#" MasterPageFile="~/POSMaster.Master" AutoEventWireup="true" CodeBehind="POS.aspx.cs" Inherits="POSActiv8.POS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container">

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

        <div id="divButtonControls" runat="server" class="col-md-1">

            <div class="row">

                <p>
                    <asp:LinkButton ID="btnNewSale" runat="server" CssClass="btn btn-primary w-100" OnClick="btnNewSale_Click" style="word-wrap:break-word;width:50px;height:60px;"><i class="bi bi-plus-lg"></i>&nbsp; <br /> New</asp:LinkButton>
                </p>

                <p>
                    <asp:LinkButton ID="btnBillOut" runat="server" CssClass="btn btn-success w-100" OnClick="btnBillOut_Click" style="word-wrap:break-word;width:50px;height:60px;"><i class="bi bi-check2-all"></i>&nbsp; <br /> Bill Out</asp:LinkButton>
                </p>

                <p>
                    <asp:LinkButton ID="btnVoid" runat="server" CssClass="btn btn-dark w-100" OnClick="btnVoid_Click" style="word-wrap:break-word;width:50px;height:60px;"><i class="bi bi-dash-lg"></i>&nbsp; <br /> Void</asp:LinkButton>
                </p>

                <p>
                    <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-danger w-100" OnClick="btnCancel_Click" style="word-wrap:break-word;width:50px;height:60px;"><i class="bi bi-x-lg"></i>&nbsp; <br /> Cancel</asp:LinkButton>
                </p>
                
                <p>
                    <asp:LinkButton ID="lbtnXReport" runat="server" CssClass="btn btn-secondary w-100" OnClick="lbtnXReport_Click" style="word-wrap:break-word;width:50px;height:60px;"><i class="bi bi-card-checklist"></i>&nbsp; XRep</asp:LinkButton>
                </p>

                <p>
                    <asp:LinkButton ID="lbtnZReport" runat="server" CssClass="btn btn-secondary w-100" OnClick="lbtnZReport_Click" style="word-wrap:break-word;width:50px;height:60px;"><i class="bi bi-card-checklist"></i>&nbsp; ZRep</asp:LinkButton>
                </p>

            </div>

        </div>

        <div id="divItems" runat="server" class="pagetitle col-md-7">

           <%-- <div class="card" style="height:100%;">

                <div class="card-body my-3">--%>

                    <p>
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" placeholder="Search menu items" ToolTip="Type in to find specific equipment"></asp:TextBox>
                    </p>

                    <div class="table-responsive mb-3">

                        <asp:DataList ID="dtItemCategory" runat="server" CssClass="d-flex justify-content-left" RepeatDirection="Horizontal" RepeatColumns="4" CellPadding="3" OnItemCommand="dtItemCategory_ItemCommand">
                            <ItemTemplate>
                        
                                <asp:Button ID="btnItem" runat="server" CssClass="btn text-light" Text='<%#  Eval("CategoryName") %>' CommandArgument='<%# Eval("CategoryCode") + "|" + Eval("CategoryName") %>' BackColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("CategoryColor").ToString()) %>' Style="word-wrap:break-word;width:auto;height:40px;" />
                    
                            </ItemTemplate>
                        </asp:DataList>

                    </div>

                    <div class="table-responsive">  <%--style="height:350px;"--%>

                        <asp:DataList ID="dtItemMaster" runat="server" CssClass="d-flex justify-content-left" RepeatDirection="Horizontal" RepeatColumns="4" CellPadding="5" OnItemCommand="dtItemMaster_ItemCommand">
                            <ItemTemplate>

                                <asp:LinkButton ID="lbtnTableNames" runat="server" CssClass="btn text-light" BackColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("CategoryColor").ToString()) %>' CommandArgument='<%# Eval("ItemCode") + "|" + Eval("ItemName") + "|" + Eval("ItemCategory") + "|" + Eval("ItemPrice") %>'>
                                
                                    <div class="my-2" style="min-height:70px;width:120px;"> <%--style="height:80px;width:155px;"--%>
                            
                                        <div class="text-center">

                                            <h6 class="my-2">
                                                <medium class="text-dark">
                                                    <b><%# Eval("Itemname")%></b>
                                                </medium>                               
                                            </h6>
                                
                                            <p class="card-text">
                                                <small class="text-light">&#8369; <%# Eval("ItemPrice")%></small>
                                            </p>

                                        </div>

                                    </div>

                                </asp:LinkButton>

                                <%--<div class="card m-0" style="height:auto;width:210px;">
                            
                                    <div class="card-body text-center">

                                        <h5 class="my-3">
                                            <strong>
                                                <%# Eval("ItemName")%>
                                            </strong>                               
                                        </h5>
                                
                                        <p class="card-text">
                                            <medium class="text-muted">&#8369; <%# Eval("ItemPrice")%></medium>
                                        </p>

                                        <%--<div class="qty my-3 text-center">
                                            <span class="minus btn-light" style="border:1px solid #d9d9d9;">-</span>
                                            <%-- <input id="txtItemQuantity" runat="server" type="number" class="count text-muted" name="qty" value="1">
                                            <asp:TextBox ID="txtItemQuantity" runat="server" CssClass="count text-muted" Text="1"></asp:TextBox>
                                            <span class="plus btn-light" style="border:1px solid #d9d9d9;">+</span>
                                        </div>-
                                   
                                        <p class="card-text">
                                            <asp:LinkButton ID="lbtnAddCart" runat="server" CssClass="btn btn-light w-100" CommandArgument='<%# Eval("ItemCode") + "|" + Eval("ItemName") + "|" + Eval("ItemCategory") + "|" + Eval("ItemPrice") %>' style="border:1px solid #d9d9d9;"><span class="bi bi-cart4"></span>&nbsp; Add</asp:LinkButton>
                                        </p>
                            
                                    </div>

                                </div>--%>

                                <%--<asp:Button ID="btnItem" runat="server" Text='<%#  Eval("ItemName") %>' Style="word-wrap: break-word; width: 168px; height: 50px;" />--%>
                        
                            </ItemTemplate>
                        </asp:DataList>

                        <p>
                            <label id="lblItemMaster" runat="server"></label>
                        </p>

                    </div>
             
                <%--</div>

            </div>--%>

        </div>

        <div id="divOrderItems" runat="server" class="pagetitle col-md-4">

            <div class="card" style="height:auto;width:auto;">

                <div class="card-body">

                    <h1 class="my-3">
                        <label id="lblTableName" runat="server"></label>
                    </h1>
                    <asp:HiddenField ID="hfControlNumber" runat="server" />
                    <asp:HiddenField ID="hfTransactionCode" runat="server" />
                    <asp:HiddenField ID="hfTransactionDate" runat="server" />
                    <asp:HiddenField ID="hfTableCode" runat="server" />

                    <div class="table-responsive">

                        <asp:GridView ID="gvOrderItems" runat="server" CssClass="table table-borderless table-hover align-middle" AutoGenerateColumns="false" ShowHeader="false" ShowFooter="false" GridLines="None" Width="100%" OnRowCommand="gvOrderItems_RowCommand" OnRowDataBound="gvOrderItems_RowDataBound">
                        <Columns>

                            <asp:TemplateField>
                                <HeaderStyle/>
                                <ItemTemplate>

                                    <div>
                                      <input class="form-check-input" type="checkbox" value="" id="cbOrderItems" runat="server">
                                    </div>
                                    <%--<asp:CheckBox ID="cbOrderItems" runat="server"/>--%>
                                   
                                </ItemTemplate>
                                <ItemStyle CssClass="text-center" Width="1px"/>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderStyle/>
                                <ItemTemplate>
                                   
                                   <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                   
                                </ItemTemplate>
                                <ItemStyle CssClass="text-center" Width="1px"/>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderStyle />
                                <ItemTemplate>

                                    <small class="text-muted"> 
                                        <%# Eval("ItemQuantity") %> &nbsp;
                                    </small>

                                    <b>
                                        <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>'></asp:Label>
                                        <br />
                                        <small class="text-muted">
                                           <i><%# Eval("Discount") %> <%# Eval("Void") %></i>
                                        </small>
                                    </b>

                                    <%--<p>
                                        <small class="text-muted">
                                            &#8369; <%# Eval("ItemPrice") %> <small class="text-muted">x</small> 
                                        </small>
                                        <br />
                                        <small class="text-muted">
                                           <i><%# Eval("Discount") %> <%# Eval("Void") %></i>
                                       </small>
                                    </p>--%>

                                </ItemTemplate>
                                <ItemStyle Width="350px"/>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderStyle CssClass="text-end"/>
                                <ItemTemplate>
                                   
                                   <medium>
                                       &#8369; <asp:Label id="lblItemAmount" runat="server" Text='<%# Eval("ItemAmount") %>'></asp:Label>
                                   </medium>
                                   
                                </ItemTemplate>
                                <ItemStyle CssClass="text-end" Width="150px"/>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderStyle/>
                                <ItemTemplate>
                                   
                                   <asp:Label ID="lblOrderStatus" runat="server" Text='<%# Eval("OrderStatus") %>'></asp:Label>
                                   
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="left"/>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderStyle/>
                                <ItemTemplate>

                                    <asp:LinkButton ID="link" runat="server" ToolTip="Click to remove item" CommandArgument='<%# Eval("RecordID") + "|" + Eval("ItemName") + "|" + "Remove" %>'><span class="bi bi-x-circle-fill text-muted"></span></asp:LinkButton>
                                
                                </ItemTemplate>
                                <ItemStyle CssClass="text-end" Width="1px"/>
                            </asp:TemplateField>

                        </Columns>
                        </asp:GridView>

                    </div>

                    <div id="divOrderTotals" class="my-1">

                        <hr />

                        <div class="row mb-1">
                            <div class="col-md-8 label"><small class="text-muted">Sub Total</small></div>
                            <div class="col-md-4 text-end">
                                &#8369; <label id="lblSubTotal" runat="server">0.00</label>
                            </div>
                        </div>

                        <div class="row mb-1" hidden>
                            <div class="col-md-8 label text-muted"><small class="text-muted">Tax (VAT)</small></div>
                            <div class="col-md-4 text-end">
                                <label id="lblTaxAmount" runat="server">0.00</label>
                            </div>
                        </div>

                        <div class="row mb-1">
                            <div class="col-md-8 label text-muted">
                                <small class="text-muted">
                                    <label id="lblDiscountName" runat="server">Discount</label>
                                </small>
                            </div>
                            <div class="col-md-4 text-end">
                                <label id="lblDiscountAmount" runat="server">0.00</label>
                            </div>
                        </div>

                        <div class="row mb-1">
                            <div class="col-md-8 label text-muted">
                                <small class="text-muted">
                                    <label id="lblServiceChargeName" runat="server">Service Charge</label>
                                </small>
                            </div>
                            <div class="col-md-4 text-end">
                                <label id="lblServiceChargeAmount" runat="server">0.00</label>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-8 label">
                                <b>Grand Total</b>
                            </div>
                            <div class="col-md-4 text-end">
                                <b>&#8369; <label id="lblGrandTotal" runat="server">0.00</label></b>
                            </div>
                        </div>

                        <%--Discounts--%>

                        <div id="divDiscounts" runat="server" class="my-2">

                            <hr />

                            Discount Type
                            <br />
                            <asp:DropDownList ID="ddlDiscountType" runat="server" CssClass="form-select" TabIndex="1" AutoPostBack="true" OnTextChanged="ddlDiscountType_TextChanged"></asp:DropDownList>
                            <br />

                            <div id="divDiscountAmount" runat="server">
                                Discount Amount
                                <br />
                                <input type="number" id="txtDiscountAmount" runat="server" class="form-control text-end" tabindex="2"/>
                                <br />
                            </div>
                        
                        </div>

                        <%--Quantity--%>

                        <div id="divQuantity" runat="server" class="my-2" visible="false">

                            <hr />


                        </div>

                        <%--Cancel Order--%>

                        <div id="divCancelOrder" runat="server" class="my-2">

                            <hr />

                            Reason for Cancellation
                            <asp:DropDownList ID="ddlCancellationTypes" runat="server" CssClass="form-select"></asp:DropDownList>
                            <br />

                        </div>

                        <%--Service Charge--%>

                        <div id="divServiceCharge" runat="server" class="my-2">

                            <hr />

                            Service Charge
                            <asp:DropDownList ID="ddlServiceCharge" runat="server" CssClass="form-select" TabIndex="1"></asp:DropDownList>
                            <br />

                        </div>

                     
                        <p class="my-3">
                            <asp:LinkButton ID="lbtnDiscount" runat="server" CssClass="btn btn-light" style="border:1px solid #d9d9d9;" OnClick="lbtnDiscount_Click"><i class="bi bi-clipboard2-check"></i>&nbsp; DISC</asp:LinkButton>               
                            <asp:LinkButton ID="lbtnQuantity" runat="server" CssClass="btn btn-light" style="border:1px solid #d9d9d9;" OnClick="lbtnQuantity_Click"><i class="bi bi-plus-slash-minus"></i>&nbsp; QTY</asp:LinkButton>         
                            <asp:LinkButton ID="lbtnServiceCharge" runat="server" CssClass="btn btn-light" style="border:1px solid #d9d9d9;" OnClick="lbtnServiceCharge_Click"><i class="bi bi-lightning-charge"></i>&nbsp; SC</asp:LinkButton>           
                        </p>

                        <p class="my-2">
                            <asp:Button ID="btnSaveTransaction" runat="server" CssClass="btn btn-primary w-100" OnClick="btnSaveTransaction_Click"/>
                        </p>

                        <p class="my-2">
                            <asp:Button ID="btnCancelTransaction" runat="server" CssClass="btn btn-Default w-100" Text="Cancel" OnClick="btnCancelTransaction_Click"/>
                        </p>

                    </div>

                </div>

                


                
                

            </div>

        </div>

         
     



    </div>

</div>

    <!--Table Names-->
    <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasTableNames" aria-labelledby="offcanvasRightLabel">
        <div class="offcanvas-header">
            <h5 class="offcanvas-title" id="offcanvasRightLabel">
                <label id="lblTableNamesTitle" runat="server"></label>
            </h5>
            <button type="button" class="btn-close" data-bs-dismiss="offcanvas" aria-label="Close"></button>
        </div>

        <div class="offcanvas-body">

            <p>
                <label id="lblTableNames" runat="server"></label>
            </p>

            <asp:DataList ID="dtTableNames" runat="server" CssClass="d-flex justify-content-left" RepeatDirection="Vertical" RepeatColumns="1" CellPadding="3" OnItemCommand="dtTableNames_ItemCommand">
                <ItemTemplate>
                        
                    <asp:LinkButton ID="lbtnTableNames" runat="server" CssClass="btn btn-light" BackColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("FloorColor").ToString()) %>' CommandArgument='<%# Eval("FloorLocation") + "|" + Eval("FloorName") + "|" + Eval("TableCode") + "|" + Eval("TableName") + "|" + Eval("TableStatus") + "|" + Eval("ControlNumber") %>'>
                                
                        <div style="height:auto;width:330px;">
                            
                            <div class="text-center">

                                <h5 class="my-3">
                                    <strong class="text-dark">
                                        <%# Eval("TableName")%>
                                    </strong>                               
                                </h5>

                                <p class="card-text">
                                    <small class="text-light"><%# Eval("FloorName")%></small>
                                </p>
                                
                            </div>

                        </div>
                    </asp:LinkButton>

                </ItemTemplate>
            </asp:DataList>

        </div>

    </div>

    <%--Show OffCanvas--%>
    <script type="text/javascript">
        function showTableNames() {
            var myOffCanvas = new bootstrap.Offcanvas(document.getElementById('offcanvasTableNames'), { keyboard: false });
            myOffCanvas.show();
        }
    </script>

    <%--<!--Discounts-->
    <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasDiscounts" aria-labelledby="offcanvasDiscountTitle" data-bs-backdrop="static">
        <div class="offcanvas-header">
            <h5 class="offcanvas-title" id="offcanvasDiscountTitle">
                <label id="lblDiscountsTitle" runat="server"></label>
            </h5>
            <button type="button" class="btn-close" data-bs-dismiss="offcanvas" aria-label="Close"></button>
        </div>

        <div class="offcanvas-body">

            
            
            <div id="divDiscountItems" runat="server">
                Order Items
                <br />
                <asp:DropDownList ID="ddlDiscountItems" runat="server" CssClass="form-select" TabIndex="3"></asp:DropDownList>
                <br />
            </div>

            Remarks <i class="text-muted">(optional)</i>
            <br />
            <textarea id="txtDiscountRemarks" runat="server" class="form-control" tabindex="4" rows="3" maxlength="200" placeholder="Input your remarks here"></textarea>

        </div>

        <div class="container">

            <p>
                <label id="lblValidationDiscounts" runat="server"></label>
            </p>

            <p>
                <asp:Button ID="btnSaveDiscount" runat="server" CssClass="btn btn-primary w-100" Text="Save Discount" OnClick="btnSaveDiscount_Click"/>
            </p>

        </div>

    </div>

    <%--Show OffCanvas
    <script type="text/javascript">
        function showDiscounts() {
            var myOffCanvas = new bootstrap.Offcanvas(document.getElementById('offcanvasDiscounts'), { keyboard: false });
            myOffCanvas.show();
        }
    </script>--%>

    <!--Quantity-->
    <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasQuantity" aria-labelledby="offcanvasQuantityTitle">
        <div class="offcanvas-header">
            <h5 class="offcanvas-title" id="offcanvasQuantityTitle">
                <label id="lblQuantityTitle" runat="server"></label>
            </h5>
            <button type="button" class="btn-close" data-bs-dismiss="offcanvas" aria-label="Close"></button>
        </div>

        <div class="offcanvas-body">

            Order Items
            <br />
            <asp:DropDownList ID="ddlOrderItemsQuantity" runat="server" CssClass="form-select" TabIndex="1" AutoPostBack="true" OnTextChanged="ddlOrderItemsQuantity_TextChanged"></asp:DropDownList>
            <br />

            Quantity
            <br />
            <input type="number" id="txtQuantity" runat="server" class="form-control text-end" tabindex="2" min="1"/>
            <%--<asp:TextBox id="txtQuantity" runat="server" CssClass="form-control text-end" TabIndex="2"></asp:TextBox>--%>
            <br />
            

            <%--Item Price
            <br />
            <input type="number" id="txtItemAmountQuantity" runat="server" class="form-control text-end" tabindex="3" disabled/>
            <br />

            Total Amount
            <br />
            <input type="number" id="txtTotalAmount" runat="server" class="form-control text-end" tabindex="4" disabled/>
            <br />--%>

            <%--<div class="qty my-3 text-center">
           <span class="minus btn-light" style="border:1px solid #d9d9d9;">-</span>
           <input id="txtItemQuantity" runat="server" type="number" class="count text-muted" name="qty" value="1">
           <span class="plus btn-light" style="border:1px solid #d9d9d9;">+</span>
                </div>
            <br />--%>
            
        </div>

        <div class="container">

            <p>
                <label id="lblValidationQuantity" runat="server"></label>
            </p>

            <p>
                <asp:Button ID="btnSaveQuantity" runat="server" CssClass="btn btn-primary w-100" Text="Save Quantity" OnClick="btnSaveQuantity_Click"/>
            </p>

        </div>

    </div>

    <%--Show OffCanvas--%>
    <script type="text/javascript">
        function showQuantity() {
            var myOffCanvas = new bootstrap.Offcanvas(document.getElementById('offcanvasQuantity'), { keyboard: false });
            myOffCanvas.show();
        }
    </script>

    <!--Payment-->
    <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasPayment" aria-labelledby="offcanvasPaymentTitle" data-bs-backdrop="static">
        <div class="offcanvas-header">
            <h5 class="offcanvas-title" id="offcanvasPaymentTitle">
                <label id="lblPaymentTitle" runat="server"></label>
            </h5>
            <button type="button" class="btn-close" data-bs-dismiss="offcanvas" aria-label="Close"></button>

        </div>

        <div class="row text-center">
            <div class="col-md-12 label text-muted">
                <small class="text-muted">Total Amount Due</small>
            </div>
            <div class="col-md-12">
                <h3>&#8369; <label id="lblAmountDue" runat="server" class="bold">0.00</label></h3>
            </div>
        </div>

        <div class="offcanvas-body">

            Payment Type
            <br />
            <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-select" TabIndex="1" AutoPostBack="true" OnTextChanged="ddlPaymentType_TextChanged"></asp:DropDownList>
            <br />

            <div id="divCash" runat="server">

                Amount Tendered
                <br />
                <asp:TextBox ID="txtCashAmount" runat="server" CssClass="form-control text-end" TabIndex="2" AutoPostBack="true" OnTextChanged="txtCashAmount_TextChanged"></asp:TextBox>
                <br />

                Change Due
                <br />
                <asp:TextBox ID="txtChangeDue" runat="server" CssClass="form-control text-end" TabIndex="3"></asp:TextBox>

            </div>

            <div id="divCreditCard" runat="server">

                Amount Tendered
                <br />
                <asp:TextBox ID="txtCreditCardAmount" runat="server" CssClass="form-control text-end" TabIndex="4"></asp:TextBox>
                <br />

                Card Type
                <br />
                <input type="text" id="txtCardType" runat="server" class="form-control" tabindex="5" placeholder="e.g. VISA, MasterCard, etc."/>
                <br />

                Card Number
                <br />
                <input type="text" id="txtCardNumber" runat="server" class="form-control" tabindex="6" maxlength="9" placeholder="xxxxxxxxx"/>
                <br />

                Account Name
                <br />
                <input type="text" id="txtAccountName" runat="server" class="form-control" tabindex="7" placeholder="Name on card"/>
                <br />

                Expiry Date
                <br />
                <input type="month" id="txtExpiryDate" runat="server" class="form-control" tabindex="8" placeholder="MM/YY"/>

            </div>

            <div id="divGCash" runat="server">
                
                Amount Tendered
                <br />
                <asp:TextBox ID="txtGCashAmount" runat="server" CssClass="form-control text-end" TabIndex="9"></asp:TextBox>
                <br />

                Reference Number
                <br />
                <input type="text" id="txtGCashReferenceNumber" runat="server" class="form-control" tabindex="10"/>

            </div>

        </div>

        <div class="container">
            <p>
                <label id="lblValidationPayment" runat="server"></label>
            </p>

            <p>
                <asp:Button ID="btnPostPayment" runat="server" CssClass="btn btn-primary w-100" Text="Save Payment" OnClick="btnPostPayment_Click"/>
            </p>
        </div>

    </div>

    <%--Show OffCanvas--%>
    <script type="text/javascript">
        function showPayment() {
            var myOffCanvas = new bootstrap.Offcanvas(document.getElementById('offcanvasPayment'), { keyboard: false });
            myOffCanvas.show();
        }
    </script>

    <!--Order Receipt-->
    <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasOrderReceipt" aria-labelledby="offcanvasOrderReceiptLabel" data-bs-backdrop="static">
        <div class="offcanvas-header">
            <h5 class="offcanvas-title" id="offcanvasOrderReceiptLabel">
                Order Receipt
            </h5>
            <asp:Button ID="btnCloseReceipt" runat="server" CssClass="btn-close" data-bs-dismiss="offcanvas" aria-label="Close" OnClick="btnCloseReceipt_Click" />
        </div>

        <div id="divOrderReceipt" runat="server" class="offcanvas-body">

            <div class="d-flex justify-content-center mb-3">
                <img src="Images/activ8-black.jpg" width="75" />
            </div>

            <div class="text-center mb-3">
                <small>
                    8th Floor Victoria Sports Tower, EDSA Southbound, Quezon City
                </small>
            </div>

            <div class="d-flex justify-content-center mb-3">
                <b>SALES ORDER</b>
            </div>

            <div class="row mb-1">
                <div class="col-md-2 label">
                    <small class="text-muted">
                        Invoice:
                    </small>
                </div>
                <div class="col-md-10 text-end">
                    <small class="text-muted">
                        <label id="lblPOSCode" runat="server"></label>
                    </small>
                </div>
            </div>

            <div class="row mb-1">
                <div class="col-md-2 label">
                    <small class="text-muted">
                        Table:
                    </small>
                </div>
                <div class="col-md-10 text-end">
                    <small class="text-muted">
                        <label id="lblTableNameReceipt" runat="server">Table 1 - Main Bar</label>
                    </small>
                </div>
            </div>

            <div class="row mb-1">
                <div class="col-md-2 label text-muted">
                    <small class="text-muted">
                        Date/Time:
                    </small>
                </div>
                <div class="col-md-10 text-end">
                    <small class="text-muted text-end">
                        <label id="lblPrintDateTimeReceipt" runat="server"></label>
                    </small>
                </div>
            </div>

            <div class="row mb-1">
                <div class="col-md-2 label text-muted">
                    <small class="text-muted">
                        Cashier:
                    </small>
                </div>
                <div class="col-md-10 text-end">
                    <small class="text-muted text-end">
                        <label id="lblCashier" runat="server"></label>
                    </small>
                </div>
            </div>

            <hr />

            <div class="row mb-3">

                <div class="table-responsive">

                    <asp:GridView ID="gvOrdersReceipt" runat="server" CssClass="align-middle" AutoGenerateColumns="false" ShowHeader="false" ShowFooter="false" GridLines="None" Width="100%">
                    <Columns>

                        <asp:TemplateField>
                            <HeaderStyle />
                            <ItemTemplate>

                                <b>
                                    <%# Eval("ItemName") %>
                                </b>

                                <p class="my-1">
                                    <small class="text-muted">
                                        &#8369; <%# Eval("ItemPrice") %> <small class="text-muted">x</small> <%# Eval("ItemQuantity") %>
                                    </small>
                                    <br />
                                    <small class="text-muted">
                                        <i><%# Eval("Discount") %></i>
                                    </small>
                                </p>

                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderStyle/>
                            <ItemTemplate>
                                   
                                <small>
                                    &#8369; <%# Eval("ItemAmount") %>
                                </small>
                                   
                            </ItemTemplate>
                            <ItemStyle CssClass="text-end"/>
                        </asp:TemplateField>

                    </Columns>
                    </asp:GridView>

                </div>

            </div>

            <hr />
            
            <div class="row mb-1">
                <div class="col-md-4 label">
                    <medium>
                        <b>Total:</b>
                    </medium>
                </div>
                <div class="col-md-8 text-end">
                    <medium>
                        &#8369; <b><label id="lblTotalAmountReceipt" runat="server">0.00</label></b>
                    </medium>
                </div>
            </div>

            <div class="row mb-1">
                <div class="col-md-2 label">
                    <small class="text-muted">
                        Payment:
                    </small>
                </div>
                <div class="col-md-10 text-end">
                    <small>
                        &#8369; <label id="lblPaymentReceipt" runat="server">0.00</label>
                    </small>
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-2 label">
                    <small class="text-muted">
                        Change:
                    </small>
                </div>
                <div class="col-md-10 text-end">
                    <small>
                        &#8369; <label id="lblChangeReceipt" runat="server">0.00</label>
                    </small>
                </div>
            </div>

            <div class="d-flex justify-content-center mb-3">
                <small><i>THIS IS NOT AN OFFICIAL RECEIPT</i></small>
            </div>

            <div class="d-flex justify-content-center">
                <small><i>THANK YOU FOR DINING WITH US!</i></small>
            </div>

            <div class="d-flex justify-content-center">
                <small><i>PLEASE COME BACK AGAIN</i></small>
            </div>
           
        </div>

        <div class="container my-3">
            <p>
                <%--<button id="btnPrintReceipt" class="btn btn-primary w-100">Print Receipt</button>--%>  <%--onclick="return OrderReceipt()"--%>
                <asp:Button ID="btnPrintReceipt" runat="server" CssClass="btn btn-primary w-100" Text="Generate Receipt" OnClick="btnPrintReceipt_Click" />
            </p>
        </div>

    </div>

    <%--Show OffCanvas--%>
    <script type="text/javascript">
        function showOrderReceipt() {
            var myOffCanvas = new bootstrap.Offcanvas(document.getElementById('offcanvasOrderReceipt'), { keyboard: false });
            myOffCanvas.show();
        }
    </script>

    <%--Print Receipt--%>
    <script type="text/javascript">
        function OrderReceipt() {
            var panel = document.getElementById("<%=divOrderReceipt.ClientID %>");
            var printWindow = window.open('', '', 'height=700,width=500,toolbar=no,resizable=no');
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

    <!--XZ Report-->
    <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasXZReport" aria-labelledby="offcanvasXZReportLabel">
        <div class="offcanvas-header">
            <h5 class="offcanvas-title" id="offcanvasXZReportLabel">
                
            </h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>

        <div id="divXZReport" runat="server" class="offcanvas-body">

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

            <br />

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
                <button id="btnPrintXZReport" class="btn btn-primary w-100" onclick="return PrintXZReport()">Print Report</button>
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

    <!--Authorization-->
    <div id="modalAuthorization" class="modal fade" role="dialog" data-bs-backdrop="static">
        <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable">
    
          <!-- Modal content-->
          <div class="modal-content">

            <div class="modal-header">
                <h5 class="modal-title">
                    <label id="lblAuthorizationTitle" runat="server"></label>
                </h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>

            <div class="modal-body">
                
                Immediate Supervisor
                <br />
                <asp:DropDownList ID="ddlImmediateSupervisor" runat="server" CssClass="form-select"></asp:DropDownList>
                <br />

                Immediate Supervisor Password
                <br />
                <input id="txtImmediateSupervisorPassword" runat="server" type="password" class="form-control">
                <br />

                <label id="lblValidationAuthorization" runat="server"></label>
                
            </div>

            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-bs-dismiss="modal">Cancel</button>
                <asp:Button ID="btnPostAuthorization" runat="server" CssClass="btn btn-primary" Text="Post" OnClick="btnPostAuthorization_Click" />
            </div>

          </div>
      
        </div>
    </div>

    <%--Show Modal--%>
    <script type="text/javascript">
        function showAuthorization() {
            var myModal = new bootstrap.Modal(document.getElementById('modalAuthorization'), { keyboard: false });
            myModal.show();
        }
    </script>


    <%--<script type="text/javascript">
        $(document).ready(function () {
            $('.count').prop('disabled', true);
            $(document).on('click', '.plus', function () {
                $('.count').val(parseInt($('.count').val()) + 1);
            });
            $(document).on('click', '.minus', function () {
                $('.count').val(parseInt($('.count').val()) - 1);
                if ($('.count').val() == 0) {
                    $('.count').val(1);
                }
            });
        });
    </script>--%>
    

</asp:Content>
