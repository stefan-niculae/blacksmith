using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Blacksmith.Models;
using Blacksmith.Utilities;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace Blacksmith.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        protected string SuccessMessage
        {
            get;
            private set;
        }

        private ApplicationDbContext db;
        
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
                Session["displayed"] = debuglabel;
            var displayedListView = Session["displayed"] as Label;

            displayedListView.Text = "aaa";

            // Ajax calls
            // TODO can't these be done more elegantly? Separate functions with annotations?
            string action = Request.QueryString["Action"];
            if (action == null) return;

            if (db == null) db = ApplicationDbContext.Create();
            //using (var db = ApplicationDbContext.Create())

                try
                {
                    var id = Convert.ToInt32(Request.QueryString["id"]);

                    if (action == "Insert")
                        AddLink(
                            Request.QueryString["title"],
                            Request.QueryString["address"],
                            Request.QueryString["description"]);

                    if (action == "Update")
                        
//                    {
//                        db.Links.Remove(db.Links.Find(id));
//
//                        displayedListView.Text = "updatin";
//                        string field = Request.QueryString["field"].ToLower();
//                        string value = Request.QueryString["value"];
//
//                        if (field == "title")
//                            link.Title = value;
//                        if (field == "address")
//                            link.Address = value;
//                        if (field == "description")
//                            link.Description = value;
//
//                        link.Date = DateTime.Now;
//                        link.Submitter = CurrentUser();
//                        //                    db.Links.Add(link);
//                    }

                    if (action == "Delete")
                        DeleteLink(id);
                    
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

        void AddLink(string title, string address, string description, int? id = null)
        {
            var link = new Models.Link()
            {
                Title = title,
                Address = address,
                Description = description,
                Date = DateTime.Now,
                Submitter = CurrentUser()
            };
            if (id != null)
                link.Id = (int) id;

            db.Links.Add(link);

            db.SaveChanges();
        }

        int DeleteLink(int id)
        {
            var link = db.Links.Find(id);
            db.Links.Remove(link);
            db.SaveChanges();

            return link.Id;
        }
        
        public IQueryable<Models.Link> SubmittedLinks()
        {
            //            using (var db = ApplicationDbContext.Create())
            //            {
            if (db == null) db = ApplicationDbContext.Create();
                string userId = User.Identity.GetUserId();

                return db.Links
                    .Where(l => l.Submitter.Id == userId)
                    .OrderByDescending(l => l.Date);
//            }


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

        }

        public User CurrentUser()
        {
            var id = User.Identity.GetUserId();
            return ApplicationDbContext.Create().Users.Find(id);
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}