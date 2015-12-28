using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Blacksmith.Models;
using Blacksmith.Utilities;
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
                        message == "ChangePwdSuccess"
                            ? "Your password has been changed."
                            : message == "SetPwdSuccess"
                                ? "Your password has been set."
                                : message == "RemoveLoginSuccess"
                                    ? "The account was removed."
                                    : string.Empty;
                    successMessage.Visible = !string.IsNullOrEmpty(SuccessMessage);
                }
            }


            // Ajax calls
            string action = Request.QueryString["Action"];
            if (action == null) return;

            using (var db = ApplicationDbContext.Create())
            {
                try
                {
                    var currentUser = db.Users.Find(User.Identity.GetUserId());

                    var id = Convert.ToInt32(Request.QueryString["id"]);
                    var link = db.Links.Find(id);

                    if (action == "Insert")
                        db.Links.Add(new Models.Link()
                        {
                            Title = Request.QueryString["title"],
                            Address = Request.QueryString["address"],
                            Description = Request.QueryString["description"],
                            Date = DateTime.Now,
                            Submitter = currentUser
                        });

                    if (action == "Update")
                    {
                        string field = Request.QueryString["field"].ToLower();
                        string value = Request.QueryString["value"];

                        DebugLogger.Log($"updating {field} to {value} for {id}");
                        if (field == "title")
                            link.Title = value;
                        if (field == "address")
                            link.Address = value;
                        if (field == "description")
                            link.Description = value;

                        link.Date = DateTime.Now;
                        link.Submitter = currentUser;

                        //TryUpdateModel(link);
                    }

                    if (action == "Delete")
                        db.Links.Remove(link);

                    db.SaveChanges();
                }

                // TODO display this exception to the user
                catch (Exception e)
                {
                    DebugLogger.Log($"Exception on {action} of Links: {e.Message}");
                    var ex = e as DbEntityValidationException;
                    
                    if (ex != null)
                    {        
                        // Retrieve the error messages as a list of strings.
                        var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                        // Combine the original exception message with the new one.
                        DebugLogger.Log(" The validation errors are: " + string.Join("; ", errorMessages));
                    }

                }
            }
        }

        public IQueryable<Models.Link> SubmittedLinks()
        {
            var db = ApplicationDbContext.Create();
            string userId = User.Identity.GetUserId();
            return db.Links
                .Where(l => l.Submitter.Id == userId)
                .OrderByDescending(l => l.Date);
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}