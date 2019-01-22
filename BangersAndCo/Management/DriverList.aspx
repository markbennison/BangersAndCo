<%@ Page Title="Drivers List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DriverList.aspx.cs" Inherits="BangersAndCo.Management.DriverList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<h2><%: Title %>.</h2>

	<section class="container">
		<section class="row">
			<section class="col-md-12">
				<!-- Title Controls -->
				<section class="row ">
					<section class="col-xs-8">
						<h3>Drivers</h3>
					</section>
					<section class="col-xs-4 text-right">
						<h3>
							<a data-toggle="collapse" href="#searchBar" aria-expanded="false" aria-controls="searchBar"><asp:Label ID="lblSearch" runat="server" CssClass="glyphicon glyphicon-search btn" /></a>
							<asp:linkbutton id="btnAddNew" PostBackUrl="~/Management/DriverView.aspx" CommandName="New" runat="server" CssClass="glyphicon glyphicon-plus btn"/>
						</h3>
					</section>
				</section>

				<!-- Search Boxes -->
				<section class="row collapse" id="searchBar">
					<section class="col-md-12">
						<p><asp:TextBox ID="txtSearchUserID" runat="server" placeholder="User ID"></asp:TextBox>
							<asp:TextBox ID="txtSearchForename" runat="server" placeholder="Forename"></asp:TextBox>
							<asp:TextBox ID="txtSearchSurname" runat="server" placeholder="Surname"></asp:TextBox></p>
						<p><asp:TextBox ID="txtSearchDateOfBirth_Start" runat="server" placeholder="DoB: After" TextMode="Date"></asp:TextBox>
							<asp:TextBox ID="txtSearchDateOfBirth_End" runat="server" placeholder="DoB: Before" TextMode="Date"></asp:TextBox></p>
						<p><asp:TextBox ID="txtSearchAddress1" runat="server" placeholder="Address Line 1"></asp:TextBox></p>
						<p><asp:TextBox ID="txtSearchAddress2" runat="server" placeholder="Address Line 2"></asp:TextBox></p>
						<p><asp:TextBox ID="txtSearchCity" runat="server" placeholder="City"></asp:TextBox></p>
						<p><asp:TextBox ID="txtSearchCounty" runat="server" placeholder="County"></asp:TextBox></p>
						<p><asp:TextBox ID="txtSearchPostCode" runat="server" placeholder="Post Code"></asp:TextBox></p>
						<p><asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary"/></p>
					</section>
				</section>
			</section>
		</section>

		<%------------------------------------------------------------
			Driver List
		  ------------------------------------------------------------%>

        <section class="row">
            <section class="col-md-12">

                <asp:ListView ID="LvDriverList"  runat="server" OnItemCommand="LvDriverList_ItemCommand">

                    <LayoutTemplate>
						<asp:DataPager ID="DataPager_LvDriverList" PageSize="10" PagedControlID="LvDriverList" runat="server" OnPreRender="DataPager_LvDriverList_PreRender">
							<Fields>
								<asp:NumericPagerField ButtonCount="10" CurrentPageLabelCssClass="btn btn-primary btn-sm" NumericButtonCssClass="btn btn-default btn-sm" ButtonType="Button" />
							</Fields>
						</asp:DataPager>
                        <table runat="server" id="tblDrivers" class="table table-hover">
							<tr runat="server" >
                                <th>User ID</th>
                                <th>Forename</th>
                                <th>Surname</th>
                                <th>Date of Birth</th>
                                <th>Address Line 1</th>
                                <th>Address Line 2</th>
								<th>City</th>
                                <th>County</th>
								<th>Post Code</th>
                                <th>View</th>
							</tr>
                            <tr runat="server" id="itemPlaceholder" ></tr>
                        </table>
                    </LayoutTemplate>

                    <ItemTemplate>
                        <tr runat="server">
							<td><asp:Label ID="lblUserID" runat="server" Text='<%#Eval("[UserID]") %>' /></td>
							<td><asp:Label ID="lblForename" runat="server" Text='<%#Eval("[Forename]") %>' /></td>
                            <td><asp:Label ID="lblSurname" runat="server" Text='<%#Eval("[Surname]") %>' /></td>
							<td><asp:Label ID="lblDateOfBirth" runat="server" Text='<%#Eval("[DateOfBirth]", "{0:dd/MM/yyyy}") %>' /></td>
							<td><asp:Label ID="lblAddress1" runat="server" Text='<%#Eval("[Address1]") %>' /></td>
                            <td><asp:Label ID="lblAddress2" runat="server" Text='<%#Eval("[Address2]") %>' /></td>
							<td><asp:Label ID="lblCity" runat="server" Text='<%#Eval("[City]") %>' /></td>
							<td><asp:Label ID="lblCounty" runat="server" Text='<%#Eval("[County]") %>' /></td>
							<td><asp:Label ID="lblPostCode" runat="server" Text='<%#Eval("[PostCode]") %>' /></td>

							<td><asp:LinkButton ID="btnView" runat="server" CssClass="glyphicon glyphicon-eye-open btn" CommandName="View" CommandArgument='<%#Eval("UserID") %>'/></td>
						</tr>
                    </ItemTemplate>

                </asp:ListView>
            </section>
        </section>
	</section>
</asp:Content>
