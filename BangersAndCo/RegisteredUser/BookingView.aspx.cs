using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BangersAndCo.DataAccessLayerTableAdapters;

namespace BangersAndCo.RegisteredUser
{
	public partial class BookingView : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (IsBlacklisted())
				{
					Response.Redirect("~/Sorry.aspx");
				}
				if (!IsDriver())
				{
					Response.Redirect("~/RegisteredUser/DriverRegistration.aspx");
				}
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
				DdlVehicleRefresh();
			}
			else if (FvBookingView.CurrentMode == FormViewMode.Edit || FvBookingView.CurrentMode == FormViewMode.ReadOnly)
			{
				string vehicleID = DataBinder.Eval(FvBookingView.DataItem, "VehicleId").ToString();
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
					Response.Redirect("~/RegisteredUser/BookingView.aspx?id=0");
					break;
				case "Cancel":
					FvBookingView.ChangeMode(FormViewMode.ReadOnly);
					PageDataRefresh();
					break;
				case "ListView":
					Response.Redirect("~/RegisteredUser/BookingList.aspx");
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
			string currentUserID = User.Identity.GetUserId();

			// Code versions of all controls
			TextBox txtBookingID = (TextBox)FvBookingView.FindControl("txtBookingID");
			DropDownList ddlVehicle = (DropDownList)FvBookingView.FindControl("ddlVehicle");
			TextBox txtDateFrom = (TextBox)FvBookingView.FindControl("txtDateFrom");
			TextBox txtTimeFrom = (TextBox)FvBookingView.FindControl("txtTimeFrom");
			TextBox txtDateDue = (TextBox)FvBookingView.FindControl("txtDateDue");
			TextBox txtTimeDue = (TextBox)FvBookingView.FindControl("txtTimeDue");
			DropDownList ddlStatus = (DropDownList)FvBookingView.FindControl("ddlStatus");
			TextBox txtDailyRateCharged = (TextBox)FvBookingView.FindControl("txtDailyRateCharged");

			// Assign all text properties of controls to variables
			int.TryParse(txtBookingID.Text, out int bookingid);
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

			bool valid_bDateTimeFrom = DateTime.TryParse(txtDateFrom.Text.Trim(), out bDateTimeFrom);
			bool valid_bDateTimeDue = DateTime.TryParse(txtDateDue.Text.Trim(), out bDateTimeDue);

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
					BookingAdapter.UpdateRecord(currentUserID, bVehicleId, bDateTimeFrom, bDateTimeDue, null, null, bStatus, bDailyRateCharged, bookingid);

					ClientScript.RegisterStartupScript(GetType(), "text", "AlertTimeout();", true);
					FvBookingView.ChangeMode(FormViewMode.ReadOnly);
					PageDataRefresh();
				}
				else if (CallCommand == "Insert")
				{
					// Perform INSERT
					string newID;
					newID = BookingAdapter.InsertAndReturnID(currentUserID, bVehicleId, bDateTimeFrom, bDateTimeDue, null, null, bStatus, bDailyRateCharged).ToString();

					// Display message to user
					ClientScript.RegisterStartupScript(GetType(), "text", "AlertTimeout();", true);
					// Redirect User
					Response.Redirect("~/RegisteredUser/BookingView.aspx?id=" + newID);
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

		private void DdlVehicleRefresh(string VehicleValue = null)
		{
			VehiclesTableAdapter vehicleAdapter = new VehiclesTableAdapter();
			DriversTableAdapter driverAdapter = new DriversTableAdapter();

			// ************************************************
			// Determine age from selected driver date of birth
			DateTime dob = Convert.ToDateTime(driverAdapter.GetDataById(User.Identity.GetUserId())[0]["DateOfBirth"]);
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
			bool isDateFrom = DateTime.TryParse(txtDateFrom.Text, out DateTime dateFrom);
			bool isDateDue = DateTime.TryParse(txtDateDue.Text, out DateTime dateDue);

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

				//if DateFrom is available, restrict date Due:min/max
				ReplaceDateTimeAttributes(ref txtDateDue, dateFrom.ToString("yyyy-MM-dd"), dateFrom.AddDays(14).ToString("yyyy-MM-dd"), dateDue.ToString("yyyy-MM-dd"));
			
			}

			/* ------------------------------------ *
			*         Time Pickers 
			*  ------------------------------------ */ 

			TextBox txtTimeFrom = (TextBox)FvBookingView.FindControl("txtTimeFrom");
			TextBox txtTimeDue = (TextBox)FvBookingView.FindControl("txtTimeDue");

			TimeSpan timeFrom = TimeToTimeSpan(txtTimeFrom.Text);  
			TimeSpan timeDue = TimeToTimeSpan(txtTimeDue.Text);

			TimeSpan openingTime = new TimeSpan(08, 00, 0);
			TimeSpan closingTime = new TimeSpan(18, 00, 0);
			DateTime openingDateTime = DateTime.Today + openingTime;
			DateTime closingDateTime = DateTime.Today + closingTime;
			DateTime minimumTime;

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
			ReplaceDateTimeAttributes(ref txtTimeDue, permittedDueTimeStart.ToString("HH:mm"), permittedDueTimeEnd.ToString("HH:mm"));

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

			bool isDateFrom = DateTime.TryParse(txtDateFrom.Text, out DateTime dateFrom);
			bool isDateDue = DateTime.TryParse(txtDateDue.Text, out DateTime dateDue);

			TextBox txtTimeFrom = (TextBox)FvBookingView.FindControl("txtTimeFrom");
			TextBox txtTimeDue = (TextBox)FvBookingView.FindControl("txtTimeDue");

			TimeSpan timeFrom = TimeToTimeSpan(txtTimeFrom.Text);
			TimeSpan timeDue = TimeToTimeSpan(txtTimeDue.Text);

			dateFrom = dateFrom + timeFrom;
			dateDue = dateDue + timeDue;

			if (isDateFrom && isDateDue)
			{
				TimeDifference = Convert.ToDecimal((dateDue - dateFrom).TotalDays);

				TimeDifference = Math.Ceiling(TimeDifference * 2) / 2;
				totalCharged = TimeDifference * dailyRateCharged;

				lblTotalCharged.Text = "Charged for " + TimeDifference.ToString() + " days = ";
				txtTotalCharged.Text = totalCharged.ToString("C");
			}

		}

		private bool IsBlacklisted()
		{
			BookingsTableAdapter BookingAdapter = new BookingsTableAdapter();
			int.TryParse(BookingAdapter.GetCountOfDidNotShow(User.Identity.GetUserId()).ToString(), out int recordsCancelled);
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
			BookingsTableAdapter BookingAdapter = new BookingsTableAdapter();
			int.TryParse(BookingAdapter.GetCountOfComplete(User.Identity.GetUserId()).ToString(), out int recordsCancelled);
			if (recordsCancelled > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private bool IsDriver()
		{
			DriversTableAdapter driverAdapter = new DriversTableAdapter();
			DataTable drivers = driverAdapter.GetDataById(User.Identity.GetUserId());
			if (drivers.Rows.Count > 0)
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
 