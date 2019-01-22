using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BangersAndCo.DataAccessLayerTableAdapters;

namespace BangersAndCo.Management
{
	public partial class BookingView : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				PageDataRefresh();
			}
		}

		private void PageDataRefresh()
		{
			string queryID = Request.QueryString["id"];
			int id = 0;
			bool idValid = Int32.TryParse(queryID, out id);
			if (id == 0)
			{
				FvBookingView.ChangeMode(FormViewMode.Insert);
			}

			/* *********** Configure DAL *********** */
			BookingsTableAdapter BookingAdapter = new BookingsTableAdapter();
			FvBookingView.DataSource = BookingAdapter.GetDataById(id);
			FvBookingView.DataBind();

			/* ****** Refresh Drop Down Lists ****** */

			if (FvBookingView.CurrentMode == FormViewMode.Insert)
			{
				DdlDriverRefresh();
				DdlVehicleRefresh();

			}
			else if (FvBookingView.CurrentMode == FormViewMode.Edit || FvBookingView.CurrentMode == FormViewMode.ReadOnly)
			{
				string userID = DataBinder.Eval(FvBookingView.DataItem, "UserId").ToString();
				string vehicleID = DataBinder.Eval(FvBookingView.DataItem, "VehicleId").ToString();
				DdlDriverRefresh(userID);
				DdlVehicleRefresh(vehicleID);
			}

			DatePickerRefresh();
		}

		protected void FvBookingView_DataBound(object sender, EventArgs e)
		{
			if (FvBookingView.DataItemCount == 0)
			{
				FvBookingView.ChangeMode(FormViewMode.Insert);
			}
			else
			{
				FvBookingView.ChangeMode(FormViewMode.Edit);
			}
		}

		protected void FvBookingView_ItemCommand(object sender, FormViewCommandEventArgs e)
		{
			switch (e.CommandName)
			{
				case "New":
					Response.Redirect("~/Management/BookingView.aspx?id=0");
					break;
				case "Cancel":
					FvBookingView.ChangeMode(FormViewMode.ReadOnly);
					PageDataRefresh();
					break;
				case "DeleteRecord":
					try
					{
						BookingsTableAdapter BookingAdapter = new BookingsTableAdapter();
						TextBox txtBookingID = (TextBox)FvBookingView.FindControl("txtBookingID");
						int bookingid = int.Parse(txtBookingID.Text);
						BookingAdapter.DeleteRecord(bookingid);
					}
					catch (System.Exception ex)
					{
						Response.Write("<script LANGUAGE='JavaScript' >alert('An ERROR occurred connecting to the database. " + ex.Message + "')</script>");
					}

					Response.Redirect("~/Management/BookingList.aspx");
					break;
				case "ListView":
					Response.Redirect("~/Management/BookingList.aspx");
					break;

			}
		}

		protected void FvBookingView_ModeChanging(object sender, FormViewModeEventArgs e)
		{
			// Enable a FormView mode change (Read-Only, Edit/Update, New/Insert, Empty)
			FvBookingView.ChangeMode((FormViewMode)e.NewMode);
			PageDataRefresh();
		}

		protected void FvBookingView_ItemInserting(object sender, FormViewInsertEventArgs e)
		{
			FvBookingView_CallInsertOrUpdate("Insert");
		}

		protected void FvBookingView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
		{
			FvBookingView_CallInsertOrUpdate("Update");
		}

		protected void FvBookingView_CallInsertOrUpdate(string CallCommand)
		{
			string queryID = Request.QueryString["id"];
			int currentUserID = 0;
			bool idValid = Int32.TryParse(queryID, out currentUserID);
			if (currentUserID == 0)
			{
				FvBookingView.ChangeMode(FormViewMode.Insert);
			}

			// Code versions of all controls
			TextBox txtBookingID = (TextBox)FvBookingView.FindControl("txtBookingID");
			DropDownList ddlDriver = (DropDownList)FvBookingView.FindControl("ddlDriver");
			DropDownList ddlVehicle = (DropDownList)FvBookingView.FindControl("ddlVehicle");
			TextBox txtDateFrom = (TextBox)FvBookingView.FindControl("txtDateFrom");
			TextBox txtTimeFrom = (TextBox)FvBookingView.FindControl("txtTimeFrom");
			TextBox txtDatePickup = (TextBox)FvBookingView.FindControl("txtDatePickup");
			TextBox txtTimePickup = (TextBox)FvBookingView.FindControl("txtTimePickup");
			TextBox txtDateDue = (TextBox)FvBookingView.FindControl("txtDateDue");
			TextBox txtTimeDue = (TextBox)FvBookingView.FindControl("txtTimeDue");
			TextBox txtDateReturn = (TextBox)FvBookingView.FindControl("txtDateReturn");
			TextBox txtTimeReturn = (TextBox)FvBookingView.FindControl("txtTimeReturn");
			DropDownList ddlStatus = (DropDownList)FvBookingView.FindControl("ddlStatus");
			TextBox txtDailyRateCharged = (TextBox)FvBookingView.FindControl("txtDailyRateCharged");

			// Assign all text properties of controls to variables
			int.TryParse(txtBookingID.Text, out int bookingid);
			string bUserID = ddlDriver.SelectedValue;
			bool valid_bVehicleId = int.TryParse(ddlVehicle.SelectedValue, out int bVehicleId);
			string bStatus = ddlStatus.SelectedValue;

			bool valid_bDailyRateCharged = Decimal.TryParse(txtDailyRateCharged.Text.Replace("£", String.Empty).Trim(), out Decimal bDailyRateCharged);
			if (!valid_bDailyRateCharged)
			{
				bDailyRateCharged = 999;
			}

			// Validity checks
			DateTime bDateTimeFrom = Convert.ToDateTime("1/1/1753 12:00:00 AM");
			DateTime bDateTimeDue = bDateTimeFrom;
			DateTime bDateTimePickup = bDateTimeFrom;
			DateTime bDateTimeReturn = bDateTimeFrom;

			bool valid_bDateTimeFrom = DateTime.TryParse(txtDateFrom.Text.Trim(), out bDateTimeFrom);
			bool valid_bDateTimeDue = DateTime.TryParse(txtDateDue.Text.Trim(), out bDateTimeDue);
			bool valid_bDateTimePickup = DateTime.TryParse(txtDatePickup.Text.Trim(), out bDateTimePickup);
			bool valid_bDateTimeReturn = DateTime.TryParse(txtDateReturn.Text.Trim(), out bDateTimeReturn);

			if (valid_bDateTimeFrom && txtTimeFrom.Text != "")
			{
				TimeSpan timeFrom = TimeToTimeSpan(txtTimeFrom.Text);
				bDateTimeFrom += timeFrom;
			}
			else
			{
				valid_bDateTimeFrom = false;
			}

			if (valid_bDateTimeDue && txtTimeDue.Text != "")
			{
				TimeSpan timeDue = TimeToTimeSpan(txtTimeDue.Text);
				bDateTimeDue += timeDue;
			}
			else
			{
				valid_bDateTimeDue = false;
			}

			if (valid_bDateTimePickup && txtTimePickup.Text != "")
			{
				TimeSpan timePickup = TimeToTimeSpan(txtTimePickup.Text);
				bDateTimePickup += timePickup;
			}
			else
			{
				valid_bDateTimePickup = false;
			}

			if (valid_bDateTimeReturn && txtTimeReturn.Text != "")
			{
				TimeSpan timeReturn = TimeToTimeSpan(txtTimeReturn.Text);
				bDateTimeReturn += timeReturn;
			}
			else
			{
				valid_bDateTimeReturn = false;
			}

			BookingsTableAdapter BookingAdapter = new BookingsTableAdapter();

			try
			{
				//Final Validity Check
				if (!(valid_bVehicleId && valid_bDateTimeFrom && valid_bDateTimeDue && valid_bDailyRateCharged))
				{
					throw new System.MissingFieldException("Not all fields are complete");
				}


				if (CallCommand == "Update")
				{
					// Perform Update (only update pickup and return dates if relevant)
					if (valid_bDateTimeFrom && valid_bDateTimeDue && valid_bDateTimePickup && valid_bDateTimeReturn)
					{
						BookingAdapter.UpdateRecord(bUserID, bVehicleId, bDateTimeFrom, bDateTimeDue, bDateTimePickup, bDateTimeReturn, bStatus, bDailyRateCharged, bookingid);
					}
					else if(valid_bDateTimeFrom && valid_bDateTimeDue && valid_bDateTimePickup)
					{
						BookingAdapter.UpdateRecord(bUserID, bVehicleId, bDateTimeFrom, bDateTimeDue, bDateTimePickup, null, bStatus, bDailyRateCharged, bookingid);
					}
					else
					{
						BookingAdapter.UpdateRecord(bUserID, bVehicleId, bDateTimeFrom, bDateTimeDue, null, null, bStatus, bDailyRateCharged, bookingid);
					}


					//Response.Write("<script LANGUAGE='JavaScript' >alert('Record Edited')</script>");
					ClientScript.RegisterStartupScript(GetType(), "text", "AlertTimeout();", true);

					// Return to Read Only mode
					FvBookingView.ChangeMode(FormViewMode.ReadOnly);
					PageDataRefresh();
				}
				else if (CallCommand == "Insert")
				{
					// Perform INSERT
					string newID;
					if (valid_bDateTimeFrom && valid_bDateTimeDue && valid_bDateTimePickup && valid_bDateTimeReturn)
					{
						newID = BookingAdapter.InsertAndReturnID(bUserID, bVehicleId, bDateTimeFrom, bDateTimeDue, bDateTimePickup, bDateTimeReturn, bStatus, bDailyRateCharged).ToString();
					}
					else if (valid_bDateTimeFrom && valid_bDateTimeDue && valid_bDateTimePickup)
					{
						newID = BookingAdapter.InsertAndReturnID(bUserID, bVehicleId, bDateTimeFrom, bDateTimeDue, bDateTimePickup, null, bStatus, bDailyRateCharged).ToString();
					}
					else
					{
						newID = BookingAdapter.InsertAndReturnID(bUserID, bVehicleId, bDateTimeFrom, bDateTimeDue, null, null, bStatus, bDailyRateCharged).ToString();
					}

					// Display message to user
					ClientScript.RegisterStartupScript(GetType(), "text", "AlertTimeout();", true);
					// Redirect User
					Response.Redirect("~/Management/BookingView.aspx?id=" + newID);
				}
			}
			catch (System.MissingFieldException ex)
			{
				Response.Write("<script LANGUAGE='JavaScript' >alert('An ERROR occured. " + ex.Message + "')</script>");
			}
			catch (System.Exception ex)
			{
				Response.Write("<script LANGUAGE='JavaScript' >alert('An ERROR occurred connecting to the database. " + ex.Message + "')</script>");
			}
		}

		/* *************************************
		* ******** FORM-CONTROL EVENTS *********
		* ************************************* */

		protected void DdlDriver_SelectedIndexChanged(object sender, EventArgs e)
		{
			DdlVehicleRefresh();
		}

		protected void DdlVehicle_SelectedIndexChanged(object sender, EventArgs e)
		{
			VehiclesTableAdapter vehicleAdapter = new VehiclesTableAdapter();
			DropDownList ddlVehicle = (DropDownList)FvBookingView.FindControl("ddlVehicle");

			TextBox txtDailyRate = (TextBox)FvBookingView.FindControl("txtDailyRateCharged");
			Decimal rate = Convert.ToDecimal(vehicleAdapter.GetDataById(Convert.ToInt32(ddlVehicle.SelectedValue))[0]["DailyRate"].ToString());
			txtDailyRate.Text = rate.ToString("C");
		}

		protected void DateTimeTextBoxes_TextChanged(object sender, EventArgs e)
		{
			DatePickerRefresh();
		}

		protected void ChkConfirm_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox chkConfirm = (CheckBox)FvBookingView.FindControl("chkConfirm");
			LinkButton btnInsert = (LinkButton)FvBookingView.FindControl("btnInsert");

			if (chkConfirm.Checked)
			{
				btnInsert.Enabled = true;
				btnInsert.CssClass = "btn btn-primary";
			}
			else
			{
				btnInsert.Enabled = false;
				btnInsert.CssClass = "btn btn-default disabled";
			}
		}

		/* *************************************
		* ********** Business Logic ***********
		* ************************************* */

		/* *************************************
		* ****** DDL Refresh Subroutines *******
		* ************************************* */

		private void DdlDriverRefresh(string DriverValue = null, int DriverIndex = 0)
		{
			DriversTableAdapter driverAdapter = new DriversTableAdapter();

			DropDownList ddlDriver = (DropDownList)FvBookingView.FindControl("ddlDriver");
			ddlDriver.DataSource = driverAdapter.GetDataBasics();
			ddlDriver.DataValueField = "UserID";
			ddlDriver.DataTextField = "Driver";

			if (DriverValue == "" || DriverValue == null)
			{
				ddlDriver.SelectedIndex = DriverIndex;
			}
			else
			{
				ddlDriver.SelectedValue = DriverValue;
			}

			ddlDriver.DataBind();

			if (IsBlacklisted())
			{
				Response.Write("<script LANGUAGE='JavaScript' >alert('This Driver has been blacklisted for cancelled appointments. Bookings are now only available in person')</script>");
			}
		}

		private void DdlVehicleRefresh(string VehicleValue = null)
		{
			VehiclesTableAdapter vehicleAdapter = new VehiclesTableAdapter();
			DriversTableAdapter driverAdapter = new DriversTableAdapter();

			// ************************************************
			// Determine age from selected driver date of birth
			DropDownList ddlDriver = (DropDownList)FvBookingView.FindControl("ddlDriver");
			DateTime dob = Convert.ToDateTime(driverAdapter.GetDataById(ddlDriver.SelectedValue)[0]["DateOfBirth"]);
			int age = DateTime.Now.Year - dob.Year;
			if (DateTime.Now.DayOfYear < dob.DayOfYear)
			{
				age = age - 1;
			}

			// ************************************************ 
			// Determine date range for vehicle availability
			TextBox txtDateFrom = (TextBox)FvBookingView.FindControl("txtDateFrom");
			TextBox txtDateDue = (TextBox)FvBookingView.FindControl("txtDateDue");
			bool isDateFrom = DateTime.TryParse(txtDateFrom.Text, out DateTime dateFrom);
			bool isDateDue = DateTime.TryParse(txtDateDue.Text, out DateTime dateDue);

			//if (!isDateFrom)
			//{
			//	dateFrom = DateTime.Today;
			//}
			//if (!isDateDue)
			//{
			//	dateDue = DateTime.Today.AddDays(14);
			//}



			//Set end of dateDue
			dateDue = dateDue.AddMinutes(1439);
			// ************************************************ 


			TextBox txtBookingID = (TextBox)FvBookingView.FindControl("txtBookingID");
			int.TryParse(txtBookingID.Text, out int bookingid);

			DropDownList ddlVehicle = (DropDownList)FvBookingView.FindControl("ddlVehicle");
			if (isDateFrom && isDateDue)
			{
				ddlVehicle.DataSource = vehicleAdapter.GetDataByAgeAndRange(Convert.ToByte(age), dateFrom, dateDue, bookingid);
			}
			else
			{
				ddlVehicle.DataSource = vehicleAdapter.GetDataAboveAge(Convert.ToByte(age));
			}
			ddlVehicle.DataValueField = "ID";
			ddlVehicle.DataTextField = "Details";

			ddlVehicle.ClearSelection();
			ddlVehicle.DataBind();

			if ((VehicleValue == "" || VehicleValue == null) && ddlVehicle.Items.Count > 0)
			{
				VehicleValue = ddlVehicle.SelectedValue;
				ddlVehicle.SelectedValue = VehicleValue;
				ddlVehicle.SelectedIndex = 0;			
			}

			// Revert DDL if previous SelectedValue in new list (must be done after a DataBind)
			if (ddlVehicle.Items.FindByValue(VehicleValue) != null)
			{
				ddlVehicle.SelectedValue = VehicleValue;
			}

			ddlVehicle.DataBind();

			// if DailyRateCharged not set (but a value exists in DDL), set it initially
			TextBox txtDailyRate = (TextBox)FvBookingView.FindControl("txtDailyRateCharged");
			if (txtDailyRate.Text == "" && ddlVehicle.SelectedIndex >= 0)
			{
				Decimal dailyRate = Convert.ToDecimal(vehicleAdapter.GetDataById(Convert.ToInt32(ddlVehicle.SelectedValue))[0]["DailyRate"].ToString());
				txtDailyRate.Text = dailyRate.ToString("C");
			}

		}

		/* *************************************
		* ******* Date/Time Subroutines ********
		* ************************************* */

		private void DatePickerRefresh()
		{
			/* ------------------------------------ *
			*         Date Pickers 
			*  ------------------------------------ */

			TextBox txtDateFrom = (TextBox)FvBookingView.FindControl("txtDateFrom");
			TextBox txtDateDue = (TextBox)FvBookingView.FindControl("txtDateDue");
			TextBox txtDatePickup = (TextBox)FvBookingView.FindControl("txtDatePickup");
			TextBox txtDateReturn = (TextBox)FvBookingView.FindControl("txtDateReturn");

			bool isDateFrom = DateTime.TryParse(txtDateFrom.Text, out DateTime dateFrom);
			bool isDateDue = DateTime.TryParse(txtDateDue.Text, out DateTime dateDue);
			bool isDatePickup = DateTime.TryParse(txtDatePickup.Text, out DateTime datePickup);
			bool isDateReturn = DateTime.TryParse(txtDateReturn.Text, out DateTime dateReturn);


			if (isDateFrom)
			{
				//if dateDue is outside the 14 day window (or doesn't exist), set dateDue = dateFrom
				if (!isDateDue || dateDue.CompareTo(dateFrom) < 0 || dateDue.CompareTo(dateFrom.AddDays(14)) > 0)
				{
					dateDue = dateFrom;
					isDateDue = true;
					ReplaceDateTimeAttributes(ref txtDateDue, dateFrom.ToString("yyyy-MM-dd"), dateFrom.AddDays(14).ToString("yyyy-MM-dd"), dateDue.ToString("yyyy-MM-dd"));
				}
				else
				{
					ReplaceDateTimeAttributes(ref txtDateDue, dateFrom.ToString("yyyy-MM-dd"), dateFrom.AddDays(14).ToString("yyyy-MM-dd"), null);
				}

				//if DateFrom is available, restrict date Pickup:min/max, Due:min/max and Return:min
				ReplaceDateTimeAttributes(ref txtDateDue, dateFrom.ToString("yyyy-MM-dd"), dateFrom.AddDays(14).ToString("yyyy-MM-dd"), dateDue.ToString("yyyy-MM-dd"));
				ReplaceDateTimeAttributes(ref txtDatePickup, dateFrom.ToString("yyyy-MM-dd"), dateDue.ToString("yyyy-MM-dd"));
				ReplaceDateTimeAttributes(ref txtDateReturn, dateFrom.ToString("yyyy-MM-dd"));

			
			}

			/* ------------------------------------ *
			*         Time Pickers 
			*  ------------------------------------ */ 

			TextBox txtTimeFrom = (TextBox)FvBookingView.FindControl("txtTimeFrom");
			TextBox txtTimePickup = (TextBox)FvBookingView.FindControl("txtTimePickup");
			TextBox txtTimeDue = (TextBox)FvBookingView.FindControl("txtTimeDue");
			TextBox txtTimeReturn = (TextBox)FvBookingView.FindControl("txtTimeReturn");

			TimeSpan timeFrom = TimeToTimeSpan(txtTimeFrom.Text);  
			TimeSpan timePickup = TimeToTimeSpan(txtTimePickup.Text);
			TimeSpan timeDue = TimeToTimeSpan(txtTimeDue.Text);
			TimeSpan timeReturn = TimeToTimeSpan(txtTimeReturn.Text);

			TimeSpan openingTime = new TimeSpan(08, 00, 0);
			TimeSpan closingTime = new TimeSpan(18, 00, 0);
			DateTime openingDateTime = DateTime.Today + openingTime;
			DateTime closingDateTime = DateTime.Today + closingTime;
			DateTime minimumTime;

			//DateTime permittedDueTimeStart = DateTime.Today + new TimeSpan(00, 00, 0);
			//DateTime permittedDueTimeEnd = DateTime.Today + new TimeSpan(23, 59, 0);
			DateTime permittedDueTimeStart = DateTime.Today + openingTime;
			DateTime permittedDueTimeEnd = DateTime.Today + closingTime;
			if (IsRepeatCustomer())
			{ 
				permittedDueTimeStart = DateTime.Today + new TimeSpan(00, 00, 0);
				permittedDueTimeEnd = DateTime.Today + new TimeSpan(23, 59, 0);
			}

			if (timeFrom.TotalHours < 8)
			{
				minimumTime = openingDateTime.AddHours(5);
			}
			else
			{
				minimumTime = (DateTime.Today + timeFrom).AddHours(5);
			}

			// Default time ranges (08:00 - 18:00)
			ReplaceDateTimeAttributes(ref txtTimeFrom, openingDateTime.ToString("HH:mm"), closingDateTime.ToString("HH:mm"));
			ReplaceDateTimeAttributes(ref txtTimePickup, openingDateTime.ToString("HH:mm"), closingDateTime.ToString("HH:mm"));
			ReplaceDateTimeAttributes(ref txtTimeDue, permittedDueTimeStart.ToString("HH:mm"), permittedDueTimeEnd.ToString("HH:mm"));
			ReplaceDateTimeAttributes(ref txtTimeReturn, permittedDueTimeStart.ToString("HH:mm"), permittedDueTimeEnd.ToString("HH:mm"));

			// if from and due dates are the same, change the time ranges
			if (isDateFrom && isDateDue && dateFrom.CompareTo(dateDue) == 0)
			{
				
				if (timeFrom.TotalMinutes < 480 || timeFrom.TotalMinutes > 780) // 480 is 8:00 and 780 is 13:00 in minutes from 0:00. 13:00 is 5 hours before closing
				{
					ReplaceDateTimeAttributes(ref txtTimeFrom, openingDateTime.ToString("HH:mm"), closingDateTime.AddHours(-5).ToString("HH:mm"), openingDateTime.ToString("HH:mm"));
				}
				else
				{
					ReplaceDateTimeAttributes(ref txtTimeFrom, openingDateTime.ToString("HH:mm"), closingDateTime.AddHours(-5).ToString("HH:mm"));
				}

				if (timeDue.TotalMinutes - timeFrom.TotalMinutes < 300)
				{
					ReplaceDateTimeAttributes(ref txtTimeDue, minimumTime.ToString("HH:mm"), permittedDueTimeEnd.ToString("HH:mm"), minimumTime.ToString("HH:mm"));
				}
				else
				{
					ReplaceDateTimeAttributes(ref txtTimeDue, minimumTime.ToString("HH:mm"), permittedDueTimeEnd.ToString("HH:mm"));
				}
			}
			else
			{
				ReplaceDateTimeAttributes(ref txtTimeFrom, openingDateTime.ToString("HH:mm"), closingDateTime.ToString("HH:mm"));
				ReplaceDateTimeAttributes(ref txtTimeDue, permittedDueTimeStart.ToString("HH:mm"), permittedDueTimeEnd.ToString("HH:mm"));
			}


			DdlVehicleRefresh();
			CalculateCharge();
		}

		private void ReplaceDateTimeAttributes(ref TextBox textBox, string min = null, string max = null, string value = null)
		{
			if (min != null)
			{
				textBox.Attributes.Remove("min");
				textBox.Attributes.Add("min", min);
			}

			if (max != null)
			{
				textBox.Attributes.Remove("max");
				textBox.Attributes.Add("max", max);
			}

			if (value != null)
			{
				textBox.Attributes.Remove("value");
				textBox.Attributes.Add("value", value);
				textBox.Text = value;
			}
		}

		private TimeSpan TimeToTimeSpan(string time)
		{
			int hours = 0;
			int minutes = 0;
			int seconds = 0;

			if (time != null && time.Length >= 3)
			{
				bool hasHours = int.TryParse(time.Substring(0, 2), out hours);
				bool hasMinutes = int.TryParse(time.Split(':')[1], out minutes);
			}

			return new TimeSpan(hours, minutes, seconds);
		}

		private void CalculateCharge()
		{
			TextBox txtDailyRateCharged = (TextBox)FvBookingView.FindControl("txtDailyRateCharged");
			TextBox txtTotalCharged = (TextBox)FvBookingView.FindControl("txtTotalCharged");
			Label lblTotalCharged = (Label)FvBookingView.FindControl("lblTotalCharged");

			Decimal.TryParse(txtDailyRateCharged.Text.Replace("£", String.Empty), out Decimal dailyRateCharged);
			Decimal totalCharged = 0;
			Decimal TimeDifference = 0;

			TextBox txtDateFrom = (TextBox)FvBookingView.FindControl("txtDateFrom");
			TextBox txtDateDue = (TextBox)FvBookingView.FindControl("txtDateDue");
			TextBox txtDateReturn = (TextBox)FvBookingView.FindControl("txtDateReturn");

			bool isDateFrom = DateTime.TryParse(txtDateFrom.Text, out DateTime dateFrom);
			bool isDateDue = DateTime.TryParse(txtDateDue.Text, out DateTime dateDue);
			bool isDateReturn = DateTime.TryParse(txtDateReturn.Text, out DateTime dateReturn);

			TextBox txtTimeFrom = (TextBox)FvBookingView.FindControl("txtTimeFrom");
			TextBox txtTimeDue = (TextBox)FvBookingView.FindControl("txtTimeDue");
			TextBox txtTimeReturn = (TextBox)FvBookingView.FindControl("txtTimeReturn");

			TimeSpan timeFrom = TimeToTimeSpan(txtTimeFrom.Text);
			TimeSpan timeDue = TimeToTimeSpan(txtTimeDue.Text);
			TimeSpan timeReturn = TimeToTimeSpan(txtTimeReturn.Text);

			dateFrom = dateFrom + timeFrom;
			dateDue = dateDue + timeDue;

			if (isDateFrom && isDateDue)
			{
				if (isDateReturn && (dateDue).CompareTo(dateReturn + timeReturn) > 0)
				{
					dateReturn = dateReturn + timeReturn;
					TimeDifference = Convert.ToDecimal((dateReturn - dateFrom).TotalDays);
				}
				else
				{
					TimeDifference = Convert.ToDecimal((dateDue - dateFrom).TotalDays);
				}

				TimeDifference = Math.Ceiling(TimeDifference * 2) / 2;
				totalCharged = TimeDifference * dailyRateCharged;

				lblTotalCharged.Text = "Charged for " + TimeDifference.ToString() + " days = ";
				txtTotalCharged.Text = totalCharged.ToString("C");
			}

		}

		private bool IsBlacklisted()
		{
			DropDownList ddlDriver = (DropDownList)FvBookingView.FindControl("ddlDriver");
			BookingsTableAdapter BookingAdapter = new BookingsTableAdapter();
			int.TryParse(BookingAdapter.GetCountOfDidNotShow(ddlDriver.SelectedValue).ToString(), out int recordsCancelled);
			if(recordsCancelled > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private bool IsRepeatCustomer()
		{
			DropDownList ddlDriver = (DropDownList)FvBookingView.FindControl("ddlDriver");
			BookingsTableAdapter BookingAdapter = new BookingsTableAdapter();
			int.TryParse(BookingAdapter.GetCountOfComplete(ddlDriver.SelectedValue).ToString(), out int recordsCancelled);
			if (recordsCancelled > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
 