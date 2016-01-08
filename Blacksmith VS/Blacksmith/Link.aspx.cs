using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.UI;
using Blacksmith.Models;
using Blacksmith.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blacksmith
{
    public partial class Link : Page
    {
        protected Models.Link CurrentLink;
        protected User CurrentUser;
        private ApplicationDbContext _db;

        private Favorite _currentFavorite;
        protected Favorite CurrentFavorite
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                    return null;

                // Cache it
                if (_currentFavorite == null)
                    _currentFavorite = _db.Favorites
                        .SingleOrDefault(f => f.Link.Id == CurrentLink.Id && f.User.Id == CurrentUser.Id);

                return _currentFavorite;
            }
        }

        protected bool canEdit { get; private set; }
        protected bool canDelete { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            FillInfo();
            HandleRights();
            HandleAjax();
            BindData();
        }

        void FillInfo()
        {
            string addr = Request.QueryString["addr"];

           _db = ApplicationDbContext.Create();
            
            if (!string.IsNullOrEmpty(addr))
                CurrentLink = _db.Links.Single(l => l.Address == addr);

            if (User.Identity.IsAuthenticated)
                CurrentUser = _db.Users.Find(User.Identity.GetUserId());
        }

        void HandleRights()
        {
            // No point in contiuning when noone is logged in
            if (!User.Identity.IsAuthenticated || CurrentLink == null)
                return;

            var userManager = new UserManager<User>(new UserStore<User>(_db));

            bool isLoggedUser = (CurrentLink.Submitter.UserName == CurrentUser.UserName);
            bool isModerator = userManager.IsInRole(CurrentUser.Id, "admin");
            
            canEdit = isLoggedUser;
            canDelete = isLoggedUser || isModerator;
        }

        void BindData()
        {
            if (CurrentLink != null)
            {
                CommentsList.DataSource = CurrentLink.Comments
                    .OrderBy(c => c.Date)
                    .ToList(); // this conversion to list is needed because we can't bind queries through EF
                CommentsList.DataBind();

                CategoriesList.DataSource = _db.Favorites
                    .Where(f => f.Link.Id == CurrentLink.Id)
                    .Where(f => f.Category != null)
                    .ToList(); 
                CategoriesList.DataBind();

                // TODO
                SimilarsList.DataSource = new List<Link>();
                SimilarsList.DataBind();
            }
        }

        void HandleAjax()
        {
            var noValueParams = Request.QueryString[null];

            if (noValueParams != null && noValueParams.Contains("toggle-fav"))
                ToggleFav();

            string newComment = Request.QueryString["new-comm"];
            if (newComment != null)
                PostComment(newComment);

            string deleteCommentId = Request.QueryString["del-comm"];
            if (deleteCommentId != null)
                DeleteComment(Convert.ToInt32(deleteCommentId));

            string action = Request.QueryString["Action"];
            if (action != null && action == "Update")
            {
                string field = Request.QueryString["field"];
                if (field == "category")
                    UpdateCategory(Request.QueryString["value"]);
            }
        }

        void UpdateCategory(string value)
        {
            var favorite = _db.Favorites.Single(
                 f => f.User.Id == CurrentUser.Id && f.Link.Id == CurrentLink.Id);
            favorite.Category = value;
            _db.SaveChanges();
        }

        void ToggleFav()
        {
            var favorite = _db.Favorites.SingleOrDefault(
                f => f.User.Id == CurrentUser.Id && f.Link.Id == CurrentLink.Id);
                
            // If it is already favorited, un-favorite it
            if (favorite != null)
                _db.Favorites.Remove(favorite);
            // If not, favorite it
            else
                _db.Favorites.Add(new Favorite
                {
                    Link = CurrentLink,
                    User = CurrentUser,
                    Date = DateTime.Now,
                    Category = null
                });

            try
            {
                _db.SaveChanges();
            }
            catch (DbEntityValidationException validation)
            {
                string action = favorite == null ? "adding" : "removing";
                string errorMessages = string.Join("; ",
                        validation.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage));

                DebugLogger.Log($"Error when {action}, " +
                                $"User = {CurrentUser.UserName}, Link = {CurrentLink.Address} by {CurrentLink.Submitter.UserName}, " +
                                $"Validation errors = {errorMessages}");
            }
            
        }

        void PostComment(string content)
        {
            _db.Comments.Add(new Comment
            {
                Content = content,
                Date = DateTime.Now,
                Submitter = CurrentUser,
                Link = CurrentLink
            });
            _db.SaveChanges();
        }

        void DeleteComment(int id)
        {
            var comment = _db.Comments.Find(id);
            _db.Comments.Remove(comment);
            _db.SaveChanges();
        }

        public bool CanDeleteComment(int commentId)
        {
            if (!User.Identity.IsAuthenticated)
                return false;
            
            var comment = _db.Comments.Find(commentId);
            return canDelete || comment.Submitter.Id == CurrentUser.Id;
        }

//        public string FavoriteCategory()
//        {
//            DebugLogger.Log("inside user favorite");
//            if (!User.Identity.IsAuthenticated)
//                return null;
//
//            var fav = _db.Favorites
//                .SingleOrDefault(f => f.Link.Id == CurrentLink.Id && f.User.Id == CurrentUser.Id);
//
//            DebugLogger.Log($"inside link get user favorite, it is null = {fav == null}");
//
//            return fav?.Category;
//        }

//        // TODO change this to return to details view and use the member current link
//        public IQueryable<Models.Link> GetLink(
//            [QueryString("addr")] string addr
//            //, [RouteData] string address
//            )
//        {
////            loglabel.Text = $"id = {id}, address = {address}, is null = {string.IsNullOrEmpty(address)}; ceva value = {RouteData.Values["address"]}, ";
////            foreach (var kv in RouteData.Values)
////                loglabel.Text += kv.Key + " = " + kv.Value + ", ";
//
//            var db = ApplicationDbContext.Create();
//            
//            if (!string.IsNullOrEmpty(addr))
//                return db.Links.Where(l => l.Address == addr);
//
////            if (string.IsNullOrEmpty(address))
////                address = RouteData.Values["address"] as string;
////
////            if (!string.IsNullOrEmpty(address))
////                return db.Links.Where(l => l.Address == address);
//
////            DebugLogger.Log("found no suitable link by title " + address + "!");
//
//            // TODO redirect to homepage if no links where found (invalid addr)
//            //            Response.Redirect("Default");
//            return null;
//        }
    }
}