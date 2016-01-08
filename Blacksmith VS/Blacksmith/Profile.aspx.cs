using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Blacksmith.Models;
using Blacksmith.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.BuilderProperties;

namespace Blacksmith
{
    public partial class Profile : System.Web.UI.Page
    {
        protected string SuccessMessage { get; private set; }
        protected string Username { get; private set; }

        protected bool canAdd { get; private set; }
        protected bool canEdit { get; private set; }
        protected bool canDelete { get; private set; }
        protected bool isModerator { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            FillInfo();
            HandleRights();
            ShowMessage();

            if (User.Identity.IsAuthenticated)
            {
                FavoritesList.DataSource = ApplicationDbContext.Create()
                    .Favorites.Where(f => f.User.UserName == Username)
                    .ToList();
                FavoritesList.DataBind();
            }

            HandleAjax();
        }

        void ShowMessage()
        {
            // Built-in stuff
            if (IsPostBack) return;
            
            // Render success message
            var message = Request.QueryString["m"];
            if (message == null) return;

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

        void HandleRights()
        {
            // No point in contiuning when noone is logged in
            if (!User.Identity.IsAuthenticated)
                return;

            var db = ApplicationDbContext.Create();
            var signedUser = db.Users.Find(User.Identity.GetUserId());

            var userManager = new UserManager<User>(new UserStore<User>(db));

            bool isLoggedUser = (Username == signedUser.UserName);
            isModerator = userManager.IsInRole(signedUser.Id, "admin"); ;

            canAdd = isLoggedUser;
            canEdit = isLoggedUser;
            canDelete = isLoggedUser || isModerator;

            administration.Visible = isLoggedUser;
            creation.Visible = canAdd;
        }

        void FillInfo()
        {
            Username = Request.QueryString["user"];

            // If no username parameter was given
            // Use the current logged in one
            if (Username == null && User.Identity.IsAuthenticated)
                Username = User.Identity.GetUserName();
            // Unless there is noone logged in
        }

        void HandleAjax()
        {
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

                        if (field == "title")
                            link.Title = value;
                        if (field == "address")
                            link.Address = value;
                        if (field == "description")
                            link.Description = value;

                        link.Date = DateTime.Now;
                        link.Submitter = currentUser;
                    }

                    if (action == "Delete")
                        db.Links.Remove(link);

                    db.SaveChanges();
                }

                // TODO display this exception to the user
                catch (Exception ex)
                {
                    DebugLogger.Log($"Exception on {action} of Links: {ex.Message}");
                    DebugLogger.Log($"Username = {Username}, title = {Request.QueryString["title"]}, addr = {Request.QueryString["address"]}, desc = {Request.QueryString["description"]}");
                    var validation = ex as DbEntityValidationException;

                    if (validation != null)
                    {
                        // Retrieve the error messages as a list of strings.
                        var errorMessages = string.Join("; ", 
                            validation.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage));

                        // Combine the original exception message with the new one.
                        DebugLogger.Log(" The validation errors are: " + errorMessages);
                    }
                }
            }
        }

        public IQueryable<Models.Link> SubmittedLinks()
        {
            // No parameter in URL given and noone logged in
            if (Username == null)
                return null;

            var db = ApplicationDbContext.Create();
            var user = db.Users.Single(u => u.UserName == Username);
            return db.Links
                .Where(l => l.Submitter.Id == user.Id)
                .OrderByDescending(l => l.Date);
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}