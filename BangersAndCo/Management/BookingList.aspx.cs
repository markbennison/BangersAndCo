using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BangersAndCo.DataAccessLayerTableAdapters;

namespace BangersAndCo.Management
{
	public partial class BookingList : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			PageDataRefresh();
		}

		private void PageDataRefresh()
		{
			string bUserName = txtSearchUsername.Text.Trim();
			string bRegistrationPlate = txtSearchVehiclePlate.Text.Trim();
			string bDescription = txtSearchVehicleType.Text.Trim();
			string bStatus = txtSearchStatus.Text.Trim();

			// Validity checks
			bool valid_bID = int.TryParse(txtSearchBookingID.Text.Trim(), out int bID);
			if (!valid_bID || bID < -1)
			{
				bID = -1;
			}

			bool valid_bDailyRateStart = Decimal.TryParse(txtSearchDailyRateChargedStart.Text.Trim(), out Decimal bDailyRateStart);
			bool valid_bDailyRateEnd = Decimal.TryParse(txtSearchDailyRateChargedEnd.Text.Trim(), out Decimal bDailyRateEnd);
			if (!valid_bDailyRateStart)
			{
				bDailyRateStart = 0;
			}
			if (!valid_bDailyRateEnd)
			{
				bDailyRateEnd = Int32.MaxValue;
			}

			bool valid_bDateTimeFromStart = DateTime.TryParse(txtSearchDateFromStart.Text.Trim(), out DateTime bDateTimeFromStart);
			bool valid_bDateTimeFromEnd = DateTime.TryParse(txtSetxtDateFromEnd.Text.Trim(), out DateTime bDateTimeFromEnd);
			bool valid_bDateTimeDueStart = DateTime.TryParse(txtSearchPickupStart.Text.Trim(), out DateTime bDateTimePickupStart);
			bool valid_bDateTimeDueEnd = DateTime.TryParse(txtSearchPickupEnd.Text.Trim(), out DateTime bDateTimePickupEnd);
			bool valid_bDateTimePickupStart = DateTime.TryParse(txtSearchDueStart.Text.Trim(), out DateTime bDateTimeDueStart);
			bool valid_bDateTimePickupEnd = DateTime.TryParse(txtSearchDueEnd.Text.Trim(), out DateTime bDateTimeDueEnd);
			bool valid_bDateTimeReturnStart = DateTime.TryParse(txtSearchReturnStart.Text.Trim(), out DateTime bDateTimeReturnStart);
			bool valid_bDateTimeReturnEnd = DateTime.TryParse(txtSearchReturnEnd.Text.Trim(), out DateTime bDateTimeReturnEnd);
			if (!valid_bDateTimeFromStart)
			{
				bDateTimeFromStart = Convert.ToDateTime("1/1/1753 12:00:00 AM");
			}
			if (!valid_bDateTimeFromEnd)
			{
				bDateTimeFromEnd = DateTime.MaxValue;
			}
			if (!valid_bDateTimeDueStart)
			{
				bDateTimeDueStart = Convert.ToDateTime("1/1/1753 12:00:00 AM");
			}
			if (!valid_bDateTimeDueEnd)
			{
				bDateTimeDueEnd = DateTime.MaxValue;
			}
			if (!valid_bDateTimePickupStart)
			{
				bDateTimePickupStart = Convert.ToDateTime("1/1/1753 12:00:00 AM");
			}
			if (!valid_bDateTimePickupEnd)
			{
				bDateTimePickupEnd = DateTime.MaxValue;
			}
			if (!valid_bDateTimeReturnStart)
			{
				bDateTimeReturnStart = Convert.ToDateTime("1/1/1753 12:00:00 AM");
			}
			if (!valid_bDateTimeReturnEnd)
			{
				bDateTimeReturnEnd = DateTime.MaxValue;
			}

			BookingsTableAdapter bookingAdapter = new BookingsTableAdapter();
			LvBookingList.DataSource = bookingAdapter.GetDataByParameters(bID, "" , -1, bDateTimeFromStart, bDateTimeFromEnd,
										bDateTimeDueStart, bDateTimeDueEnd, bDateTimePickupStart, bDateTimePickupEnd, 
										bDateTimeReturnStart, bDateTimeReturnEnd, bStatus, bDailyRateStart, bDailyRateEnd);
			LvBookingList.DataBind();
		}

		protected void LvBookingList_ItemCommand(object sender, ListViewCommandEventArgs e)
		{
			Response.Redirect("~/Management/BookingView.aspx?id=" + e.CommandArgument.ToString());
		}

		protected void DataPager_LvBookingList_PreRender(object sender, EventArgs e)
		{
			PageDataRefresh();
		}
	}
}