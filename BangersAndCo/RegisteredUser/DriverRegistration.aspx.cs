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
	public partial class DriverRegistration : System.Web.UI.Page
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
			string id = id = User.Identity.GetUserId();
			
			if (id == null)
			{
				Response.Redirect("~/Account/Login.aspx");
			}

			/* *********** Configure DAL *********** */
			DriversTableAdapter driverAdapter = new DriversTableAdapter();
			FvDriverView.DataSource = driverAdapter.GetDataById(id);
			FvDriverView.DataBind();
		}

		protected void FvDriverView_DataBound(object sender, EventArgs e)
		{
			if (FvDriverView.DataItemCount == 0)
			{
				FvDriverView.ChangeMode(FormViewMode.Insert);
			}
			else
			{
				FvDriverView.ChangeMode(FormViewMode.Edit);
			}
		}

		protected void FvDriverView_ItemCommand(object sender, FormViewCommandEventArgs e)
		{
			switch (e.CommandName)
			{
				case "Cancel":
					FvDriverView.ChangeMode(FormViewMode.ReadOnly);
					PageDataRefresh();
					break;
			}
		}

		protected void FvDriverView_ModeChanging(object sender, FormViewModeEventArgs e)
		{
			// Enable a FormView mode change (Read-Only, Edit/Update, New/Insert, Empty)
			FvDriverView.ChangeMode((FormViewMode)e.NewMode);
			PageDataRefresh();
		}

		protected void FvDriverView_ItemInserting(object sender, FormViewInsertEventArgs e)
		{
			FvDriverView_CallInsertOrUpdate("Insert");
		}

		protected void FvDriverView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
		{
			FvDriverView_CallInsertOrUpdate("Update");
		}

		protected void FvDriverView_CallInsertOrUpdate(string CallCommand)
		{
			string currentUserID = User.Identity.GetUserId();

			// Code versions of all controls
			TextBox txtForename = (TextBox)FvDriverView.FindControl("txtForename");
			TextBox txtSurname = (TextBox)FvDriverView.FindControl("txtSurname");
			TextBox txtDateOfBirth = (TextBox)FvDriverView.FindControl("txtDateOfBirth");
			TextBox txtAddress1 = (TextBox)FvDriverView.FindControl("txtAddress1");
			TextBox txtAddress2 = (TextBox)FvDriverView.FindControl("txtAddress2");
			TextBox txtCity = (TextBox)FvDriverView.FindControl("txtCity");
			TextBox txtCounty = (TextBox)FvDriverView.FindControl("txtCounty");
			TextBox txtPostCode = (TextBox)FvDriverView.FindControl("txtPostCode");

			// Assign all text properties of controls to variables
			// Or skip this and assign straight into INSERT/UPDATE parameters
			string forename = txtForename.Text;
			string surname = txtSurname.Text;
			string dateofbirth = txtDateOfBirth.Text;
			string address1 = txtAddress1.Text;
			string address2 = txtAddress2.Text;
			string city = txtCity.Text;
			string county = txtCounty.Text;
			string postCode = txtPostCode.Text;

			DriversTableAdapter DriverAdapter = new DriversTableAdapter();

			try
			{
				if (CallCommand == "Update")
				{
					// Conduct Update
					DriverAdapter.UpdateRecord(forename, surname, dateofbirth, address1, address2, city, county, postCode, currentUserID);

					//Response.Write("<script LANGUAGE='JavaScript' >alert('Record Edited')</script>");
					ClientScript.RegisterStartupScript(GetType(), "text", "AlertTimeout();", true);

					// Return to Read Only mode
					FvDriverView.ChangeMode(FormViewMode.ReadOnly);
					PageDataRefresh();
				}
				else if (CallCommand == "Insert")
				{
					// Perform INSERT
					DriverAdapter.InsertRecord(currentUserID, forename, surname, dateofbirth, address1, address2, city, county, postCode);
					// Display message to user
					//Response.Write("<script LANGUAGE='JavaScript' >alert('Record Added')</script>");
					ClientScript.RegisterStartupScript(GetType(), "text", "AlertTimeout();", true);
					// Redirect User
					FvDriverView.ChangeMode(FormViewMode.ReadOnly);
				}
			}
			catch (System.Exception ex)
			{
				Response.Write("<script LANGUAGE='JavaScript' >alert('An ERROR occurred connecting to the database. " + ex.Message + "')</script>");
			}
		}
	}
}