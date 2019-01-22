using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BangersAndCo.DataAccessLayerTableAdapters;

namespace BangersAndCo.Management
{
	public partial class DriverList : System.Web.UI.Page
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
			string dID = txtSearchUserID.Text.Trim();
			string dForename = txtSearchForename.Text.Trim();
			string dSurname = txtSearchSurname.Text.Trim();
			string dAddress1 = txtSearchAddress1.Text.Trim();
			string dAddress2 = txtSearchAddress2.Text.Trim();
			string dCity = txtSearchCity.Text.Trim();
			string dCounty = txtSearchCounty.Text.Trim();
			string dPostCode = txtSearchPostCode.Text.Trim();
			// Force empty strings to nulls?

			// Validity checks
			bool valid_dDobStart = DateTime.TryParse(txtSearchDateOfBirth_Start.Text.Trim(), out DateTime dDobStart);
			bool valid_dDobEnd = DateTime.TryParse(txtSearchDateOfBirth_End.Text.Trim(), out DateTime dDobEnd);
			if (!valid_dDobStart)
			{
				dDobStart = Convert.ToDateTime("1/1/1753 12:00:00 AM");
			}
			if (!valid_dDobEnd)
			{
				dDobEnd = DateTime.MaxValue;
			}

			DriversTableAdapter driverAdapter = new DriversTableAdapter();
			LvDriverList.DataSource = driverAdapter.GetDataByParameters(dID, dForename, dSurname, dDobStart, dDobEnd, dAddress1, dAddress2, dCity, dCounty, dPostCode);
			LvDriverList.DataBind();
		}

		protected void LvDriverList_ItemCommand(object sender, ListViewCommandEventArgs e)
		{
			Response.Redirect("~/Management/DriverView.aspx?id=" + e.CommandArgument.ToString());
		}

		protected void DataPager_LvDriverList_PreRender(object sender, EventArgs e)
		{
			PageDataRefresh();
		}
	}
}