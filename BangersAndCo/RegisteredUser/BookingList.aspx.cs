using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BangersAndCo.DataAccessLayerTableAdapters;

namespace BangersAndCo.RegisteredUser
{
	public partial class BookingList : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			PageDataRefresh();
		}

		private void PageDataRefresh()
		{
			BookingsTableAdapter bookingAdapter = new BookingsTableAdapter();
			LvBookingList.DataSource = bookingAdapter.GetDataByUserID(User.Identity.GetUserId());
			LvBookingList.DataBind();
		}

		protected void LvBookingList_ItemCommand(object sender, ListViewCommandEventArgs e)
		{
			Response.Redirect("~/RegisteredUser/BookingView.aspx?id=" + e.CommandArgument.ToString());
		}

		protected void DataPager_LvBookingList_PreRender(object sender, EventArgs e)
		{
			PageDataRefresh();
		}
	}
}