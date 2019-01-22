using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BangersAndCo.DataAccessLayerTableAdapters;

namespace BangersAndCo.Management
{
	public partial class DriverView : System.Web.UI.Page
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
			string id = Request.QueryString["id"];
			if (id == null || id == "0")
			{
				id = "0";
				FvDriverView.ChangeMode(FormViewMode.Insert);
			}

			/* *********** Configure DAL *********** */
			DriversTableAdapter driverAdapter = new DriversTableAdapter();
			FvDriverView.DataSource = driverAdapter.GetDataById(id);
			FvDriverView.DataBind();


			/* ****** Refresh Drop Down Lists ****** */

			if (FvDriverView.CurrentMode == FormViewMode.Insert)
			{
				DdlUserRefresh();
			}
			else if (FvDriverView.CurrentMode == FormViewMode.Edit || FvDriverView.CurrentMode == FormViewMode.ReadOnly)
			{
				DdlUserRefresh(id);
			}
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
				case "New":
					Response.Redirect("~/Management/DriverView.aspx?id=0");
					break;
				case "Cancel":
					FvDriverView.ChangeMode(FormViewMode.ReadOnly);
					PageDataRefresh();
					break;
				case "ListView":
					Response.Redirect("~/Management/DriverList.aspx");
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
			// Code versions of all controls
			DropDownList ddlUser = (DropDownList)FvDriverView.FindControl("ddlUser");
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
			string currentUserID = ddlUser.SelectedValue;
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
					// Perform Update
					DriverAdapter.UpdateRecord(forename, surname, dateofbirth, address1, address2, city, county, postCode, currentUserID);

					//Response.Write("<script LANGUAGE='JavaScript' >alert('Record Edited')</script>");
					ClientScript.RegisterStartupScript(GetType(), "text", "AlertTimeout();", true);

					// Return to Read Only mode
					FvDriverView.ChangeMode(FormViewMode.ReadOnly);
					PageDataRefresh();
				}
				else if (CallCommand == "Insert")
				{
					//DriverAdapter.InsertRecord(currentUserID, forename, surname, dateofbirth, address1, address2, city, county, postCode);
					//Response.Write("<script LANGUAGE='JavaScript' >alert('Record Added')</script>");
					//FvDriverView.ChangeMode(FormViewMode.ReadOnly);

					// Perform INSERT
					string newID = DriverAdapter.InsertAndReturnID(currentUserID, forename, surname, dateofbirth, address1, address2, city, county, postCode).ToString();
					// Display message to user
					ClientScript.RegisterStartupScript(GetType(), "text", "AlertTimeout();", true);
					// Redirect User
					Response.Redirect("~/Management/DriverView.aspx?id=" + newID);
				}
			}
			catch (System.Exception ex)
			{
				Response.Write("<script LANGUAGE='JavaScript' >alert('An ERROR occurred connecting to the database. " + ex.Message + "')</script>");
			}
		}

		/* *************************************
		* ****** DDL Refresh Subroutines *******
		* ************************************* */

		private void DdlUserRefresh(string UserValue = "", int UserIndex = 0)
		{
			UsersAndRolesTableAdapter userAdapter = new UsersAndRolesTableAdapter();

			DropDownList ddlUser = (DropDownList)FvDriverView.FindControl("ddlUser");
			ddlUser.DataSource = userAdapter.GetUsersNotDrivers(UserValue);
			ddlUser.DataValueField = "UserID";
			ddlUser.DataTextField = "UserName";

			if (UserValue == "" || UserValue == null)
			{
				ddlUser.SelectedIndex = UserIndex;
			}
			else
			{
				ddlUser.SelectedValue = UserValue;
			}

			ddlUser.DataBind();
		}

	}
}