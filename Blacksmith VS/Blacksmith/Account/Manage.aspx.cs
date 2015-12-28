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
        private static Label DisplayedListView;

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

            // Bind the displayed ListView
            if (!Page.IsPostBack)
                DisplayedListView = debuglabel;

            DisplayedListView.Text = "aaa";

            // Ajax calls
            // TODO can't these be done more elegantly? Separate functions with annotations?
            string action = Request.QueryString["Action"];
            if (action == null) return;

            var db = ApplicationDbContext.Instance;
            
            try
            {
                   
                var id = Convert.ToInt32(Request.QueryString["id"]);
                var link = db.Links.Find(id);

                if (action == "Insert")
                    db.Links.Add(new Models.Link()
                    {
                        Title = Request.QueryString["title"],
                        Address = Request.QueryString["address"],
                        Description = Request.QueryString["description"],
                        Date = DateTime.Now,
                        Submitter = CurrentUser()
                    });

                if (action == "Update")
                {
                    DisplayedListView.Text = "updatin";
                    string field = Request.QueryString["field"].ToLower();
                    string value = Request.QueryString["value"];
                        
                    if (field == "title")
                        link.Title = value;
                    if (field == "address")
                        link.Address = value;
                    if (field == "description")
                        link.Description = value;

                    link.Date = DateTime.Now;
                    link.Submitter = CurrentUser();
                        
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
                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                    DebugLogger.Log(" The validation errors are: " + string.Join("; ", errorMessages));
                }

            }
        }

        public IQueryable<Models.Link> SubmittedLinks()
        {
            string userId = User.Identity.GetUserId();

            var links = ApplicationDbContext.Instance.Links
                .Where(l => l.Submitter.Id == userId)
                .OrderByDescending(l => l.Date);
//                as List<Models.Link> 
//                ?? new List<Models.Link>();
//
//            // Display a dummy link entry that will be used on the client side when inserting.
//            // Initially the dummy link is hidden, but on insert it's cloned, 
//            // has its values changed and displayed as a new link.
//            links.Add(new Models.Link()
//            {
//                Title = "Dummy Title",
//                Address = "dummy.com",
//                Description = "Dummy link used for client side insertion visualisation",
//                Submitter = CurrentUser(),
//                Date = DateTime.Now
//            });

            return links;
        }

        public User CurrentUser()
        {
            var id = User.Identity.GetUserId();
            return ApplicationDbContext.Instance.Users.Find(id);
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}