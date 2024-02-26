<%@ Page Title="Item Master" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ItemMaster.aspx.cs" Inherits="POSActiv8.ItemMaster" %>
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

    <h1>Item Master</h1>

</div>

<div id="divButtonControls" runat="server" class="section">

    <div class="row">

        <div class="col-md-4 mb-3">
             <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" placeholder="Search for item name" ToolTip="Type in to find specific equipment"></asp:TextBox>
        </div>

       <div class="col-md-8 mb-3 text-end">
            <asp:Button ID="btnExport" runat="server" CssClass="btn btn-secondary" Text="Export" OnClick="btnExport_Click" />
            <asp:Button ID="btnCreate" runat="server" CssClass="btn btn-primary" Text="Create New" OnClick="btnCreate_Click" ToolTip="Click to create new item" />
       </div>

    </div>

</div>

<div id="divItemMaster" runat="server" class="section">

    <p>
        <label id="lblItemMaster" runat="server"></label>
    </p>

    <div class="table-responsive">
   
        <asp:GridView ID="gvItemMaster" runat="server" CssClass="table table-bordered table-hover align-middle" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No records to display" EmptyDataRowStyle-HorizontalAlign="center" ShowHeader="true" ShowFooter="false" GridLines="None" onrowcommand="gvItemMaster_RowCommand">
        <Columns>

            <asp:TemplateField HeaderText="">
                <HeaderStyle />
                <ItemTemplate>
                    <medium>
                        <asp:LinkButton ID="link" runat="server" Text="View" CommandArgument='<%# Eval("ItemCode")%>' ToolTip="Click to view the details"></asp:LinkButton>
                    </medium>
                </ItemTemplate>
                <ItemStyle Width="1px" CssClass="text-centers"/>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Item Name">
                <HeaderStyle />
                <ItemTemplate>
                    <div>
                        <%# Eval("ItemName")%>
                    </div>
                    <small class="text-muted"><%# Eval("ItemDescription") %></small>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="left" Width="350px"/>
            </asp:TemplateField>

            <asp:BoundField DataField="CategoryName" HeaderText="Category">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left"/>
            </asp:BoundField>

            <asp:BoundField DataField="SubCategoryName" HeaderText="SubCategory">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left"/>
            </asp:BoundField>

            <asp:BoundField DataField="ItemPrice" HeaderText="Price">
                <HeaderStyle CssClass="text-end"/>
                <ItemStyle CssClass="text-end"/>
            </asp:BoundField>

            <asp:BoundField DataField="ItemStatus" HeaderText="Status">
                <HeaderStyle CssClass="text-center"/>
                <ItemStyle CssClass="text-center"/>
            </asp:BoundField>
            

            <%--<asp:TemplateField HeaderText="Equipment Description">
                <HeaderStyle />
                <ItemTemplate>

                    <div class="float-end">
                        <%# Eval("RecordStatus") %>
                    </div>

                    <div class="row g-0 d-flex position-relative mb-3">
                    
                        <h6>
                            <b><asp:LinkButton ID="link" runat="server" CssClass="stretched-link text-dark" Text='<%# Eval("EquipmentName")%>' CommandArgument='<%# Eval("RecordID")%>' ToolTip="Click to view the details"></asp:LinkButton></b>
                        </h6>

                        <p class="card-text">
                            By <medium class="text-muted"><%# Eval("CreatedBy") %></medium> on <medium class="text-muted"><%# Eval("DateTimeCreated") %>
                        </p>
                   
                    </div>

                </ItemTemplate>
                <ItemStyle HorizontalAlign="left"/>
            </asp:TemplateField>--%>

        </Columns>
        </asp:GridView>

    </div>

    <br />

</div>

    <!--Item Master-->
    <div id="modalItemDetails" class="modal fade" role="dialog">
        <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable">
    
          <!-- Modal content-->
          <div class="modal-content">

            <div class="modal-header">
                <h5 class="modal-title">
                    <asp:Label ID="lblItemMasterDetails" runat="server"></asp:Label>
                </h5>
                <asp:label id="lblItemCode" runat="server" visible="false"></asp:label>
            </div>

            <div class="modal-body">
                Item Code
                <br />
                <input type="text" id="txtItemCode" runat="server" class="form-control" tabindex="1"/>
                <br />

                Item Name
                <br />
                <input type="text" id="txtItemName" runat="server" class="form-control" tabindex="2" placeholder="Name of the item" />
                <br />

                Item Description
                <br />
                <textarea id="txtItemDescription" runat="server" class="form-control" tabindex="3" rows="3" maxlength="200" placeholder="Quick description of the item"></textarea>
                <br />

                Category
                <br />
                <asp:DropDownList ID="ddlItemCategory" runat="server" CssClass="form-select" TabIndex="4" AutoPostBack="true" OnTextChanged="ddlItemCategory_TextChanged"></asp:DropDownList>
                <br />

                Sub Category
                <br />
                <asp:DropDownList ID="ddlItemSubCategory" runat="server" CssClass="form-select" TabIndex="5"></asp:DropDownList>
                <br />

                Price
                <br />
                <input type="number" id="txtItemPrice" runat="server" class="form-control text-end" tabindex="6" />
                <br />

                Status
                <div class="form-check">
                  <input id="rbStatusInactive" runat="server" class="form-check-input" type="radio" tabindex="7">
                  <label class="form-check-label" for="rbStatusInactive">
                    Inactive
                  </label>
                </div>

                <div class="form-check">
                  <input id="rbStatusActive" runat="server" class="form-check-input" type="radio" tabindex="7" checked>
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
            var myModal = new bootstrap.Modal(document.getElementById('modalItemDetails'), { keyboard: false });
            myModal.show();
        }
    </script>


</asp:Content>
