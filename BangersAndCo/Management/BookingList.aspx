<%@ Page Title="Bookings List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BookingList.aspx.cs" Inherits="BangersAndCo.Management.BookingList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h2><%: Title %>.</h2>

	<section class="container">
		<section class="row">
			<section class="col-md-12">
				<!-- Title Controls -->
				<section class="row ">
					<section class="col-xs-8">
						<h3>Bookings</h3>
					</section>
					<section class="col-xs-4 text-right">
						<h3>
							<a data-toggle="collapse" href="#searchBar" aria-expanded="false" aria-controls="searchBar"><asp:Label ID="lblSearch" runat="server" CssClass="glyphicon glyphicon-search btn" /></a>
							<asp:linkbutton id="btnAddNew" PostBackUrl="~/Management/BookingView.aspx" CommandName="New" runat="server" CssClass="glyphicon glyphicon-plus btn"/>
						</h3>
					</section>
				</section>

				<!-- Search Boxes -->
				<section class="row collapse" id="searchBar">
					<section class="col-md-12">
						<p><asp:TextBox ID="txtSearchBookingID" runat="server" placeholder="Booking ID" TextMode="Number"></asp:TextBox></p>
						<!-- <p><asp:TextBox ID="txtSearchUsername" runat="server" placeholder="Username"></asp:TextBox></p> -->
						<p><asp:TextBox ID="txtSearchVehiclePlate" runat="server" placeholder="Vehicle Plate"></asp:TextBox>
							<asp:TextBox ID="txtSearchVehicleType" runat="server" placeholder="Vehicle Type"></asp:TextBox></p>
						<p>Available From: <asp:TextBox ID="txtSearchDateFromStart" runat="server" placeholder="Available: After" TextMode="Date"></asp:TextBox>
							<asp:TextBox ID="txtSearchTimeFromStart" runat="server" placeholder="Available: After" TextMode="Time"></asp:TextBox>
							To: <asp:TextBox ID="txtSetxtDateFromEnd" runat="server" placeholder="Available: Before" TextMode="Date"></asp:TextBox>
							<asp:TextBox ID="txtSetxtTimeFromEnd" runat="server" placeholder="Available: Before" TextMode="Time"></asp:TextBox>
						</p>
						<p><asp:TextBox ID="txtSearchPickupStart" runat="server" placeholder="Picked Up: After" TextMode="DateTime"></asp:TextBox>
							<asp:TextBox ID="txtSearchPickupEnd" runat="server" placeholder="Picked Up: Before" TextMode="DateTime"></asp:TextBox></p>
						<p><asp:TextBox ID="txtSearchDueStart" runat="server" placeholder="Due: After" TextMode="DateTime"></asp:TextBox>
							<asp:TextBox ID="txtSearchDueEnd" runat="server" placeholder="Due: Before" TextMode="DateTime"></asp:TextBox></p>
						<p><asp:TextBox ID="txtSearchReturnStart" runat="server" placeholder="Returned: After" TextMode="DateTime"></asp:TextBox>
							<asp:TextBox ID="txtSearchReturnEnd" runat="server" placeholder="Returned: Before" TextMode="DateTime"></asp:TextBox></p>
						<p><asp:TextBox ID="txtSearchStatus" runat="server" placeholder="Status"></asp:TextBox></p>
						<p><asp:TextBox ID="txtSearchDailyRateChargedStart" runat="server" placeholder="Hourly Rate  (£): From" TextMode="Number"></asp:TextBox>
							<asp:TextBox ID="txtSearchDailyRateChargedEnd" runat="server" placeholder="Hourly Rate (£): To" TextMode="Number"></asp:TextBox></p>
						<p><asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary"/></p>
					</section>
				</section>
			</section>
		</section>

		<%------------------------------------------------------------
			Booking List
		  ------------------------------------------------------------%>

        <section class="row">
            <section class="col-md-12">

                <asp:ListView ID="LvBookingList"  runat="server" OnItemCommand="LvBookingList_ItemCommand">

                    <LayoutTemplate>
						<asp:DataPager ID="DataPager_LvBookingList" PageSize="10" PagedControlID="LvBookingList" runat="server" OnPreRender="DataPager_LvBookingList_PreRender">
							<Fields>
								<asp:NumericPagerField ButtonCount="10" CurrentPageLabelCssClass="btn btn-primary btn-sm" NumericButtonCssClass="btn btn-default btn-sm" ButtonType="Button" />
							</Fields>
						</asp:DataPager>
                        <table runat="server" id="tblBookings" class="table table-hover">
							<tr runat="server" >
								<th>Booking ID</th>
                                <th>Username</th>
                                <th>Vehicle Plate</th>
                                <th>Vehicle Type</th>
                                <th>Date/Time From</th>
                                <th>Date/Time Due</th>
                                <th>Date/Time Pickup</th>
								<th>Date/Time Return</th>
								<th>Status</th>
                                <th>Hourly Rate</th>
                                <th>View</th>
							</tr>
                            <tr runat="server" id="itemPlaceholder" ></tr>
                        </table>
                    </LayoutTemplate>

                    <ItemTemplate>
                        <tr runat="server">
							<td><asp:Label ID="lblBookingID" runat="server" Text='<%#Eval("[ID]") %>' /></td>
							<td><asp:Label ID="lblUsername" runat="server" Text='<%#Eval("[UserName]") %>' /></td>
                            <td><asp:Label ID="lblVehiclePlate" runat="server" Text='<%#Eval("[RegistrationPlate]") %>' /></td>
							<td><asp:Label ID="lblVehicleType" runat="server" Text='<%#Eval("[Description]") %>' /></td>
							<td><asp:Label ID="lblFrom" runat="server" Text='<%#Eval("[DateTimeFrom]", "{0:dd/MM/yyyy  HH:mm}") %>' /></td>
							<td><asp:Label ID="lblPickup" runat="server" Text='<%#Eval("[DateTimeDue]", "{0:dd/MM/yyyy  HH:mm}") %>' /></td>
							<td><asp:Label ID="lblDue" runat="server" Text='<%#Eval("[DateTimePickup]", "{0:dd/MM/yyyy  HH:mm}") %>' /></td>
							<td><asp:Label ID="lblReturn" runat="server" Text='<%#Eval("[DateTimeReturn]", "{0:dd/MM/yyyy  HH:mm}") %>' /></td>
							<td><asp:Label ID="lblStatus" runat="server" Text='<%#Eval("[Status]") %>' /></td>
							<td><asp:Label ID="lblDailyRateCharged" runat="server" Text='<%#Eval("[DailyRateCharged]", "£ {0:#,##0.00}") %>' /></td>

							<td><asp:LinkButton ID="btnView" runat="server" CssClass="glyphicon glyphicon-eye-open btn" CommandName="View" CommandArgument='<%#Eval("ID") %>'/></td>
						</tr>
                    </ItemTemplate>

                </asp:ListView>
            </section>
        </section>
	</section>
</asp:Content>
