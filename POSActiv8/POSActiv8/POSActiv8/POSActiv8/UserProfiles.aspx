<%@ Page Title="User Profiles" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="UserProfiles.aspx.cs" Inherits="POSActiv8.UserProfiles" %>
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

    <h1>User Profiles</h1>

</div>

<div id="divButtonControls" runat="server" class="section">

    <div class="row">

        <div class="col-md-4 mb-3">
             <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" placeholder="Search network id or name" ToolTip="Type in to find specific user profile"></asp:TextBox>
        </div>

        <div class="col-md-8 mb-3" style="text-align:right;">
            <asp:Button ID="btnExport" runat="server" CssClass="btn btn-secondary" Text="Export" OnClick="btnExport_Click" />
            <asp:Button ID="btnCreate" runat="server" CssClass="btn btn-primary" Text="Create New" OnClick="btnCreate_Click" ToolTip="Click to create new user profile" />
        </div>

    </div>

</div>

<div id="divUserProfiles" runat="server" class="section">

    <p>
        <label id="lblUserProfiles" runat="server"></label>
    </p>
    
    <div class="table-responsive">

        <asp:GridView ID="gvUserProfiles" runat="server" CssClass="table table-bordered table-hover align-middle" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No records found" AllowSorting="true" ShowHeader="true" ShowFooter="false" GridLines="None" onrowcommand="gvUserProfiles_RowCommand" ToolTip="Showing all user profiles" Width="100%">
        <Columns>

            <asp:TemplateField HeaderText="Full Name">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemTemplate>
                    <asp:LinkButton ID="lnk" runat="server" Text='<%# Eval("FullName")%>' CommandArgument='<%# Eval("NetworkID")%>'></asp:LinkButton>
                    <div style="margin-bottom:5px;">
                        <%# Eval("EmailAddress")%>
                    </div>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="left"/>
            </asp:TemplateField>

            <asp:BoundField DataField="UserLevel" HeaderText="User Level">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left"/>
            </asp:BoundField>

            <asp:BoundField DataField="UserRole" HeaderText="User Role">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left"/>
            </asp:BoundField>

            <asp:BoundField DataField="UserStatus" HeaderText="User Status">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left"/>
            </asp:BoundField>

            <asp:BoundField DataField="LoginStatus" HeaderText="Login Status">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left"/>
            </asp:BoundField>

            <asp:BoundField DataField="LoginAttempts" HeaderText="Login Attempts">
                <HeaderStyle CssClass="text-center"/>
                <ItemStyle CssClass="text-center"/>
            </asp:BoundField>

            <asp:BoundField DataField="IPAddress" HeaderText="IP Address">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left"/>
            </asp:BoundField>

            <asp:BoundField DataField="DateTimeLogin" HeaderText="Last Login">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left"/>
            </asp:BoundField>

            <asp:BoundField DataField="DateTimeLogout" HeaderText="Last Logout">
                <HeaderStyle HorizontalAlign="left"/>
                <ItemStyle HorizontalAlign="left"/>
            </asp:BoundField>

        </Columns>
        </asp:GridView>

    </div>

    <br />

</div>

    <!--User Profile Details-->
    <div id="modalUserProfiles" class="modal fade" role="dialog">
        <div class="modal-dialog modal-dialog-scrollable">
    
          <!-- Modal content-->
          <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">
                    <asp:Label ID="lblUserProfileDetails" runat="server"></asp:Label>
                </h5>
                <asp:label id="lblRecordID" runat="server" visible="false"></asp:label>
            </div>

            <div class="modal-body">
                Network ID
                <br />
                <asp:TextBox ID="txtUserID" runat="server" CssClass="form-control" TabIndex="1" AutoPostBack="true" OnTextChanged="txtUserID_TextChanged" OnClientClick="LoadingProgress()"></asp:TextBox>
                <br />

                Full Name
                <br />
                <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                <br />

                Email Address
                <br />
                <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                <br />
               
                Department
                <br />
                <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                <br />

                User Level
                <br />
                <asp:DropDownList ID="ddlUserLevel" runat="server" CssClass="form-select" TabIndex="5"></asp:DropDownList>
                <br />

                User Role
                <br />
                <asp:DropDownList ID="ddlUserRole" runat="server" CssClass="form-select" TabIndex="6"></asp:DropDownList>
                <br />

                <div id="divUserStatus" runat="server">
                    User Status
                    <br />
                    <asp:RadioButtonList ID="rdoUserStatus" runat="server" CssClass="form-control" CellPadding="7" TabIndex="8" RepeatDirection="Vertical"></asp:RadioButtonList>
                    <br />
                </div>
        
                <div id="divLoginStatus" runat="server">
                    Login Status
                    <br />
                    <asp:RadioButtonList ID="rdoLoginStatus" runat="server" CssClass="form-control" CellPadding="7" TabIndex="9" RepeatDirection="Vertical"></asp:RadioButtonList>
                    <br />
                </div>

            </div>

            <div class="modal-footer" style="justify-content: space-between;">
                <asp:label id="lblValidationUserProfiles" runat="server"></asp:label>
            </div>

            <div class="modal-footer">
                <asp:Button ID="btnCancelUserProfiles" runat="server" CssClass="btn btn-default" Text="Cancel" data-dismiss="modal" OnClick="btnCancelUserProfiles_Click"/>
                <asp:Button ID="btnSaveUserProfiles" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSaveUserProfiles_Click"/>
                <asp:Button ID="btnUpdateUserProfiles" runat="server" CssClass="btn btn-primary" Text="Update" OnClick="btnUpdateUserProfiles_Click" Visible="false" />
            </div>
          </div>
      
        </div>
    </div>

    <%--Show Modal--%>
    <script type="text/javascript">
        function showModal() {
            var myModal = new bootstrap.Modal(document.getElementById('modalUserProfiles'), { keyboard: false });
            myModal.show();
        }
    </script>

    <%--Hide Modal--%>
    <script type="text/javascript">
        function hideModal() {
            var myModal = new bootstrap.Modal(document.getElementById('modalUserProfiles'), { keyboard: false });
            myModal.hide();
        }
    </script>

</asp:Content>
