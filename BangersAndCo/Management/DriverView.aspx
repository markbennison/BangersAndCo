<%@ Page Title="Driver View" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DriverView.aspx.cs" Inherits="BangersAndCo.Management.DriverView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h2><%: Title %>.</h2>

	<section class="container">
		<asp:FormView 
			ID="FvDriverView" 
			runat="server" 
			CssClass="col-md-12"
			AllowPaging="false" 
			DataKeyNames="UserID" 
			OnItemCommand="FvDriverView_ItemCommand" 
			OnModeChanging="FvDriverView_ModeChanging"
			OnItemInserting="FvDriverView_ItemInserting"
			OnItemUpdating="FvDriverView_ItemUpdating"
			OnDataBound="FvDriverView_DataBound">

			<ItemTemplate>
				<section class="row">
					<section class="col-xs-8">
						<h3>Form View (Read-Only)</h3>
					</section>
					<section class="col-xs-4 text-right">
						<h3>
							<asp:linkbutton id="btnInsertView" CommandName="New" runat="server" CssClass="glyphicon glyphicon-plus btn"/>
							<asp:linkbutton id="btnEditView" CommandName="Edit" runat="server" CssClass="glyphicon glyphicon-pencil btn"/>
							<asp:linkbutton id="btnListView" CommandName="ListView" runat="server" CssClass="glyphicon glyphicon-th-list btn"/>
						</h3>
					</section>
				</section>
				<section class="row">
					<section class="col-xs-12">
						<table class="table table-borderless table-condensed">
							<tr><td>User:</td><td><asp:DropDownList ID="ddlUser" runat="server" Enabled="False" AutoPostBack="True" ></asp:DropDownList></td></tr>
							<tr><td>Forename:</td><td><asp:TextBox ID="txtForename" runat="server" Enabled="False" Text='<%#Eval("Forename") %>'></asp:TextBox></td></tr>
							<tr><td>Surname:</td><td><asp:TextBox ID="txtSurname" runat="server" Enabled="False" Text='<%#Eval("Surname") %>'></asp:TextBox></td></tr>
							<tr><td>Date of Birth:</td><td><asp:TextBox ID="txtDateOfBirth" runat="server" Enabled="False" Text='<%#Eval("DateOfBirth") %>' TextMode="Date" ></asp:TextBox></td></tr>
							<tr><td>Address Line 1:</td><td><asp:TextBox ID="txtAddress1" runat="server" Enabled="False" Text='<%#Eval("Address1") %>'></asp:TextBox></td></tr>
							<tr><td>Address Line 2:</td><td><asp:TextBox ID="txtAddress2" runat="server" Enabled="False" Text='<%#Eval("Address2") %>'></asp:TextBox></td></tr>
							<tr><td>City:</td><td><asp:TextBox ID="txtCity" runat="server" Enabled="False" Text='<%#Eval("City") %>'></asp:TextBox></td></tr>
							<tr><td>County:</td><td><asp:TextBox ID="txtCounty" runat="server" Enabled="False" Text='<%#Eval("County") %>'></asp:TextBox></td></tr>
							<tr><td>Post Code:</td><td><asp:TextBox ID="txtPostCode" runat="server" Enabled="False" Text='<%#Eval("PostCode") %>'></asp:TextBox></td></tr>
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
							<tr><td>User:</td><td><asp:DropDownList ID="ddlUser" runat="server" Enabled="True" AutoPostBack="True" ></asp:DropDownList></td></tr>
							<tr><td>Forename:</td><td><asp:TextBox ID="txtForename" runat="server" Enabled="True" Text='<%#Bind("Forename") %>'></asp:TextBox></td></tr>
							<tr><td>Surname:</td><td><asp:TextBox ID="txtSurname" runat="server" Enabled="True" Text='<%#Bind("Surname") %>'></asp:TextBox></td></tr>
							<tr><td>Date of Birth:</td><td><asp:TextBox ID="txtDateOfBirth" runat="server" Enabled="True" Text='<%#Bind("DateOfBirth") %>' TextMode="Date" ></asp:TextBox></td></tr>
							<tr><td>Address Line 1:</td><td><asp:TextBox ID="txtAddress1" runat="server" Enabled="True" Text='<%#Bind("Address1") %>'></asp:TextBox></td></tr>
							<tr><td>Address Line 2:</td><td><asp:TextBox ID="txtAddress2" runat="server" Enabled="True" Text='<%#Bind("Address2") %>'></asp:TextBox></td></tr>
							<tr><td>City:</td><td><asp:TextBox ID="txtCity" runat="server" Enabled="True" Text='<%#Bind("City") %>'></asp:TextBox></td></tr>
							<tr><td>County:</td><td><asp:TextBox ID="txtCounty" runat="server" Enabled="True" Text='<%#Bind("County") %>'></asp:TextBox></td></tr>
							<tr><td>Post Code:</td><td><asp:TextBox ID="txtPostCode" runat="server" Enabled="True" Text='<%#Bind("PostCode") %>'></asp:TextBox></td></tr>

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
							<tr><td>User:</td><td><asp:DropDownList ID="ddlUser" runat="server" Enabled="True" AutoPostBack="True" ></asp:DropDownList></td></tr>
							<tr><td>Forename:</td><td><asp:TextBox ID="txtForename" runat="server" Enabled="True" Text='<%#Bind("Forename") %>'></asp:TextBox></td></tr>
							<tr><td>Surname:</td><td><asp:TextBox ID="txtSurname" runat="server" Enabled="True" Text='<%#Bind("Surname") %>'></asp:TextBox></td></tr>
							<tr><td>Date of Birth:</td><td><asp:TextBox ID="txtDateOfBirth" runat="server" Enabled="True" Text='<%#Bind("DateOfBirth") %>' TextMode="Date" ></asp:TextBox></td></tr>
							<tr><td>Address Line 1:</td><td><asp:TextBox ID="txtAddress1" runat="server" Enabled="True" Text='<%#Bind("Address1") %>'></asp:TextBox></td></tr>
							<tr><td>Address Line 2:</td><td><asp:TextBox ID="txtAddress2" runat="server" Enabled="True" Text='<%#Bind("Address2") %>'></asp:TextBox></td></tr>
							<tr><td>City:</td><td><asp:TextBox ID="txtCity" runat="server" Enabled="True" Text='<%#Bind("City") %>'></asp:TextBox></td></tr>
							<tr><td>County:</td><td><asp:TextBox ID="txtCounty" runat="server" Enabled="True" Text='<%#Bind("County") %>'></asp:TextBox></td></tr>
							<tr><td>Post Code:</td><td><asp:TextBox ID="txtPostCode" runat="server" Enabled="True" Text='<%#Bind("PostCode") %>'></asp:TextBox></td></tr>

							<tr><td></td><td><asp:LinkButton ID="btnInsert" runat="server" CommandName="Insert" Text="Submit" CssClass="btn btn-primary"/></td></tr>
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

	<script src="../Scripts/myScripts.js"></script>
</asp:Content>
