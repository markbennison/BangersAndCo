<%@ Page Title="Booking View" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BookingView.aspx.cs" Inherits="BangersAndCo.RegisteredUser.BookingView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h2><%: Title %>.</h2>
	<section class="container">
		<asp:FormView 
			ID="FvBookingView" 
			runat="server" 
			CssClass="col-md-12"
			AllowPaging="false" 
			DataKeyNames="UserID" 
			OnItemCommand="FvBookingView_ItemCommand" 
			OnModeChanging="FvBookingView_ModeChanging"
			OnItemInserting="FvBookingView_ItemInserting"
			OnItemUpdating="FvBookingView_ItemUpdating"
			OnDataBound="FvBookingView_DataBound">

			<ItemTemplate>
				<section class="row">
					<section class="col-xs-8">
						<h3>Form View (Read-Only)</h3>
					</section>
					<section class="col-xs-4 text-right">
						<h3>
							<asp:linkbutton id="btnInsertView" CommandName="New" runat="server" CssClass="glyphicon glyphicon-plus btn"/>
							<asp:linkbutton id="btnEditView" CommandName="Edit" runat="server" CssClass="glyphicon glyphicon-pencil btn" />
							<asp:linkbutton id="btnListView" CommandName="ListView" runat="server" CssClass="glyphicon glyphicon-th-list btn"/>
						</h3>
					</section>
				</section>
				<section class="row">
					<section class="col-xs-12">
						<table class="table table-borderless table-condensed">
							<tr><td>Booking ID:</td><td><asp:TextBox ID="txtBookingID" runat="server" Enabled="False" Text='<%#Eval("[ID]") %>'></asp:TextBox></td>
								<td>Status:</td><td><asp:DropDownList ID="ddlStatus" runat="server" Enabled="False" SelectedValue='<%# Eval("Status") %>'>
									<asp:ListItem Text="Booked" Value="Booked" />
									<asp:ListItem Text="Did Not Show" Value="Did Not Show" />
									<asp:ListItem Text="Active" Value="Active" />
									<asp:ListItem Text="Late" Value="Late" />
									<asp:ListItem Text="Complete" Value="Complete" /></asp:DropDownList></td></tr>
							
							<tr><td>Requested From:</td><td><asp:TextBox ID="txtDateFrom" runat="server" Enabled="False" Text='<%#Eval("[DateTimeFrom]", "{0:yyyy-MM-dd}" ) %>' TextMode="Date"></asp:TextBox>
								<asp:TextBox ID="txtTimeFrom" runat="server" Enabled="False" Text='<%#Eval("[DateTimeFrom]", "{0:HH:mm}") %>' TextMode="Time"></asp:TextBox></td>

							<tr><td>Expected Return:</td><td><asp:TextBox ID="txtDateDue" runat="server" Enabled="False" Text='<%#Eval("[DateTimeDue]", "{0:yyyy-MM-dd}" ) %>' TextMode="Date"></asp:TextBox>
								<asp:TextBox ID="txtTimeDue" runat="server" Enabled="False" Text='<%#Eval("[DateTimeDue]", "{0:HH:mm}") %>' TextMode="Time"></asp:TextBox></td>

							<tr><td>Vehicle:</td><td colspan="4"><asp:DropDownList ID="ddlVehicle" runat="server" Enabled="False" ></asp:DropDownList></td></tr>
							<tr>
								<td>Hourly Rate: </td>
								<td><asp:TextBox ID="txtDailyRateCharged" runat="server" Enabled="False" Text='<%#Bind("[DailyRateCharged]", "£{0:#,##0.00}") %>'></asp:TextBox></td>
								<td><asp:Label ID="lblTotalCharged" runat="server" Text=""></asp:Label></td>
								<td><asp:TextBox ID="txtTotalCharged" runat="server" Enabled="False" ></asp:TextBox></td>
							</tr>
						</table>
					</section>
				</section>

			</ItemTemplate>

			<EditItemTemplate>
				<section class="row">
					<section class="col-xs-8">
						<h3>Form View (Edit)</h3>
					</section>
					<section class="col-xs-4 text-right">
						<h3>
							<asp:linkbutton id="btnInsertView" CommandName="New" runat="server" CssClass="glyphicon glyphicon-plus btn"/>
							<asp:linkbutton id="btnEditView" CommandName="Edit" runat="server" CssClass="glyphicon glyphicon-pencil btn disabled"/>
							<asp:linkbutton id="btnListView" CommandName="ListView" runat="server" CssClass="glyphicon glyphicon-th-list btn"/>
						</h3>
					</section>
				</section>
				<section class="row">
					<section class="col-xs-12">
						<table class="table table-borderless table-condensed">
							<tr><td>Booking ID:</td><td><asp:TextBox ID="txtBookingID" runat="server" Enabled="False" Text='<%#Bind("[ID]") %>'></asp:TextBox></td>
								<td>Status:</td><td><asp:DropDownList ID="ddlStatus" runat="server" Enabled="False" SelectedValue='<%# Bind("Status") %>'>
									<asp:ListItem Text="Booked" Value="Booked" />
									<asp:ListItem Text="Did Not Show" Value="Did Not Show" />
									<asp:ListItem Text="Active" Value="Active" />
									<asp:ListItem Text="Late" Value="Late" />
									<asp:ListItem Text="Complete" Value="Complete" /></asp:DropDownList></td></tr>
												
							<tr><td>Requested From:</td><td><asp:TextBox ID="txtDateFrom" runat="server" Enabled="True" Text='<%#Bind("[DateTimeFrom]", "{0:yyyy-MM-dd}" ) %>' TextMode="Date" 
																		AutoPostBack="True" OnTextChanged="DateTimeTextBoxes_TextChanged"></asp:TextBox>
								<asp:TextBox ID="txtTimeFrom" runat="server" Enabled="True" Text='<%#Bind("[DateTimeFrom]", "{0:HH:mm}") %>' TextMode="Time"
																		AutoPostBack="True" OnTextChanged="DateTimeTextBoxes_TextChanged"></asp:TextBox></td>

							<tr><td>Expected Return:</td><td><asp:TextBox ID="txtDateDue" runat="server" Enabled="True" Text='<%#Bind("[DateTimeDue]", "{0:yyyy-MM-dd}") %>' TextMode="Date"
																		AutoPostBack="True" OnTextChanged="DateTimeTextBoxes_TextChanged"></asp:TextBox>
								<asp:TextBox ID="txtTimeDue" runat="server" Enabled="True" Text='<%#Bind("[DateTimeDue]", "{0:HH:mm}") %>' TextMode="Time"
																		AutoPostBack="True" OnTextChanged="DateTimeTextBoxes_TextChanged"></asp:TextBox></td>

							<tr><td>Vehicle:</td><td colspan="4"><asp:DropDownList ID="ddlVehicle" runat="server" Enabled="True" AutoPostBack="True" OnSelectedIndexChanged="DdlVehicle_SelectedIndexChanged"></asp:DropDownList></td></tr>
							<tr>
								<td>Hourly Rate: </td>
								<td><asp:TextBox ID="txtDailyRateCharged" runat="server" Enabled="False" Text='<%#Bind("[DailyRateCharged]", "£{0:#,##0.00}") %>' ></asp:TextBox></td>
								<td><asp:Label ID="lblTotalCharged" runat="server" Text=""></asp:Label></td>
								<td><asp:TextBox ID="txtTotalCharged" runat="server" Enabled="False"></asp:TextBox></td>
							</tr>

							<tr><td></td><td><asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Save" CssClass="btn btn-primary"/></td></tr>
						</table>
					</section>
				</section>
			</EditItemTemplate>

			<InsertItemTemplate>
				<section class="row">
					<section class="col-xs-8">
						<h3>Form View (Insert)</h3>
					</section>
					<section class="col-xs-4 text-right">
						<h3>
							<asp:linkbutton id="btnInsertView" CommandName="New" runat="server" CssClass="glyphicon glyphicon-plus btn disabled"/>
							<asp:linkbutton id="btnEditView" CommandName="Edit" runat="server" CssClass="glyphicon glyphicon-pencil btn disabled"/>
							<asp:linkbutton id="btnListView" CommandName="ListView" runat="server" CssClass="glyphicon glyphicon-th-list btn"/>
						</h3>
					</section>
				</section>
				<section class="row">
					<section class="col-xs-12">
						<table class="table table-borderless table-condensed">
							<tr><td>Booking ID:</td><td><asp:TextBox ID="txtBookingID" runat="server" Enabled="False" Text='<%#Bind("[ID]") %>'></asp:TextBox></td>
								<td>Status:</td><td><asp:DropDownList ID="ddlStatus" runat="server" Enabled="False" SelectedValue='<%# Bind("Status") %>'>
									<asp:ListItem Text="Booked" Value="Booked" />
									<asp:ListItem Text="Did Not Show" Value="Did Not Show" />
									<asp:ListItem Text="Active" Value="Active" />
									<asp:ListItem Text="Late" Value="Late" />
									<asp:ListItem Text="Complete" Value="Complete" /></asp:DropDownList></td></tr>
													
							<tr><td>Requested From:</td><td><asp:TextBox ID="txtDateFrom" runat="server" Enabled="True" Text='<%#Bind("[DateTimeFrom]", "{0:yyyy-MM-dd}") %>' TextMode="Date" 
																		AutoPostBack="True" OnTextChanged="DateTimeTextBoxes_TextChanged"></asp:TextBox>
								<asp:TextBox ID="txtTimeFrom" runat="server" Enabled="True" Text='<%#Bind("[DateTimeFrom]", "{0:HH:mm}") %>' TextMode="Time"
																		AutoPostBack="True" OnTextChanged="DateTimeTextBoxes_TextChanged"></asp:TextBox></td>

							<tr><td>Expected Return:</td><td><asp:TextBox ID="txtDateDue" runat="server" Enabled="True" Text='<%#Bind("[DateTimeDue]", "{0:yyyy-MM-dd}") %>' TextMode="Date"
																		AutoPostBack="True" OnTextChanged="DateTimeTextBoxes_TextChanged"></asp:TextBox>
								<asp:TextBox ID="txtTimeDue" runat="server" Enabled="True" Text='<%#Bind("[DateTimeDue]", "{0:HH:mm}") %>' TextMode="Time"
																		AutoPostBack="True" OnTextChanged="DateTimeTextBoxes_TextChanged"></asp:TextBox></td>
							
							<tr><td>Vehicle: </td><td colspan="4"><asp:DropDownList ID="ddlVehicle" runat="server" Enabled="True" AutoPostBack="True" OnSelectedIndexChanged="DdlVehicle_SelectedIndexChanged"></asp:DropDownList></td></tr>
							<tr><td>Hourly Rate: </td>
								<td><asp:TextBox ID="txtDailyRateCharged" runat="server" Enabled="False" Text='<%#Bind("[DailyRateCharged]", "£{0:#,##0.00}") %>' ></asp:TextBox></td>
								<td><asp:Label ID="lblTotalCharged" runat="server" Text=""></asp:Label></td>
								<td><asp:TextBox ID="txtTotalCharged" runat="server" Enabled="False"></asp:TextBox></td>
							</tr>
							<tr>
								<td>Identity Check: </td>
								<td><asp:CheckBox ID="chkConfirm" runat="server" AutoPostBack="True" OnCheckedChanged="ChkConfirm_CheckedChanged" /></td>
								<td>Please confirm you will bring the following documents when collecting the vehicle:
									<ul><li>photocard driving licence, <br />and 1 other form of identity from either,</li><ul><li>a recent utility bill (within 3 months), or</li><li>council tax statement</li></ul></ul>
								</td>
							</tr>

							<tr><td></td><td><asp:LinkButton ID="btnInsert" runat="server" Enabled="False" CommandName="Insert" Text="Submit" CssClass="btn btn-default disabled"/></td></tr>
						</table>
					</secti
			</InsertItemTemplate>

			<EmptyDataTemplate>
				<section class="row">
					<section class="col-xs-12">
						<h3>(No record found.)</h3>
					</section>
				</section>
			</EmptyDataTemplate>

		</asp:FormView>
	</section>

	<section class="alert-timeout alert-hidden alert alert-success">
		<a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
		<strong>Success!</strong> The record has been saved.
	</section>


	<script src="../Scripts/moment.js"></script>
	<script src="../Scripts/bootstrap-datetimepicker.min.js"></script>
	<script src="../Scripts/myScripts.js"></script>

</asp:Content>
