using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using Blacksmith.Models;
using Microsoft.AspNet.Identity;

namespace Blacksmith.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        protected string SuccessMessage
        {
            get;
            private set;
        }

        protected void Page_Load()
        {
            // Built-in stuff
            if (!IsPostBack)
            {
                // Render success message
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // Strip the query string from action
                    Form.Action = ResolveUrl("~/Account/Manage");

                    SuccessMessage =
                        message == "ChangePwdSuccess" ? "Your password has been changed."
                        : message == "SetPwdSuccess" ? "Your password has been set."
                        : message == "RemoveLoginSuccess" ? "The account was removed."
                        : string.Empty;
                    successMessage.Visible = !string.IsNullOrEmpty(SuccessMessage);
                }
            }

            // Ajax calls
            string action = Request.QueryString["Action"];
            if (action == null) return;

            if (action == "Insert")
            {
                LinkManager.Insert(new Link(
                    Request.QueryString["title"],
                    Request.QueryString["address"],
                    Request.QueryString["description"],
                    User.Identity.GetUserId()));
            }
            if (action == "Update")
            {
                LinkManager.Update(
                    Request.QueryString["field"],
                    Request.QueryString["value"],
                    Convert.ToInt32(Request.QueryString["id"]));
            }
            if (action == "Delete")
                LinkManager.Delete(Convert.ToInt32(Request.QueryString["id"]));
        }

        public List<Link> SubmittedLinks()
        {
            string userId = User.Identity.GetUserId();
            string query = "Select * from links " +
                           $"Where submitter = '{userId}'";
            return LinkManager.ExecuteSelect(query);
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}