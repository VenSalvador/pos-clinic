﻿<%@ Page Title="Discounts" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Discounts.aspx.cs" Inherits="POSActiv8.Discounts" %>
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

    <h1>Discounts</h1>

</div>

<div id="divButtonControls" runat="server" class="section">

    <div class="row">

        <div class="col-md-4 mb-3">
             <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" placeholder="Search for discount name" ToolTip="Type in to find specific equipment"></asp:TextBox>
        </div>

       <div class="col-md-8 mb-3" style="text-align:right;">
            <asp:Button ID="btnExport" runat="server" CssClass="btn btn-secondary" Text="Export" OnClick="btnExport_Click" />
            <asp:Button ID="btnCreate" runat="server" CssClass="btn btn-primary" Text="Create New" OnClick="btnCreate_Click" ToolTip="Click to create new discount" />
       </div>

    </div>

</div>

<div id="divDiscounts" runat="server" class="section">

    <p>
        <label id="lblDiscounts" runat="server"></label>
    </p>

    <div class="table-responsive">
   
        <asp:GridView ID="gvDiscounts" runat="server" CssClass="table table-bordered table-hover align-middle" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false" GridLines="None" onrowcommand="gvDiscounts_RowCommand">
        <Columns>

            <asp:TemplateField HeaderText="">
                <HeaderStyle />
                <ItemTemplate>
                    <medium>
                        <asp:LinkButton ID="link" runat="server" Text="View" CommandArgument='<%# Eval("RecordID")%>' ToolTip="Click to view the details"></asp:LinkButton>
                    </medium>
                </ItemTemplate>
                <ItemStyle Width="1px" CssClass="text-center align-middle"/>
            </asp:TemplateField>

            <asp:BoundField DataField="DiscountName" HeaderText="Discount Name">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left"/>
            </asp:BoundField>

            <asp:BoundField DataField="DiscountAmount" HeaderText="Discount Amount">
                <HeaderStyle CssClass="text-end"/>
                <ItemStyle CssClass="text-end"/>
            </asp:BoundField>

            <asp:BoundField DataField="RecordStatus" HeaderText="Status">
                <HeaderStyle CssClass="text-center"/>
                <ItemStyle CssClass="text-center"/>
            </asp:BoundField>
            
        </Columns>
        </asp:GridView>

    </div>

    <br />

</div>

    <!--Discount Details-->
    <div id="modalDiscountDetails" class="modal fade" role="dialog">
        <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable">
    
          <!-- Modal content-->
          <div class="modal-content">

            <div class="modal-header">
                <h5 class="modal-title">
                    <asp:Label ID="lblDiscountDetailsTitle" runat="server"></asp:Label>
                </h5>
                <asp:label id="lblRecordID" runat="server" visible="false"></asp:label>
            </div>

            <div class="modal-body">
                Discount Name
                <br />
                <input type="text" id="txtDiscountName" runat="server" class="form-control" tabindex="1"/>
                <br />

                Discount Amount
                <br />
                <input type="number" id="txtDiscountAmount" runat="server" class="form-control text-end" tabindex="2" />
                <br />

                Status
                <div class="form-check">
                  <input id="rbStatusInactive" runat="server" class="form-check-input" type="radio" tabindex="3">
                  <label class="form-check-label" for="rbStatusInactive">
                    Inactive
                  </label>
                </div>

                <div class="form-check">
                  <input id="rbStatusActive" runat="server" class="form-check-input" type="radio" tabindex="4" checked>
                  <label class="form-check-label" for="rbStatusActive">
                    Active
                  </label>
                </div>

                <p style="text-align:left;">
                    <asp:label id="lblValidationMessage" runat="server"></asp:label>
                </p>
                
            </div>

            <div class="modal-footer">
                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-default" data-bs-dismiss="modal" Text="Cancel"/>
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click"/>
                <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-primary" Text="Update" OnClick="btnUpdate_Click" Visible="false" />
            </div>

          </div>
      
        </div>
    </div>

    <%--Show Modal--%>
    <script type="text/javascript">
        function showModal() {
            var myModal = new bootstrap.Modal(document.getElementById('modalDiscountDetails'), { keyboard: false });
            myModal.show();
        }
    </script>


</asp:Content>
