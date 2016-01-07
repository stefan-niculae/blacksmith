using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web.ModelBinding;
using System.Web.UI;
using Blacksmith.Models;
using Blacksmith.Utilities;
using Microsoft.AspNet.Identity;

namespace Blacksmith
{
    public partial class Link : Page
    {
        protected Models.Link CurrentLink;
        protected User CurrentUser;
        private ApplicationDbContext _db;

        protected bool canEdit { get; private set; }
        protected bool canDelete { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            FillInfo();
            HandleRights();
            BindData();
            HandleAjax();
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

            bool isLoggedUser = (CurrentLink.Submitter.UserName == CurrentUser.UserName);
            bool isModerator = false;
            
            canEdit = isLoggedUser;
            canDelete = isLoggedUser || isModerator;
        }

        void BindData()
        {
            if (CurrentLink != null)
            {
                CommentsList.DataSource = CurrentLink.Comments;
                CommentsList.DataBind();

                CategoriesList.DataSource = _db.Favorites
                    .Where(f => f.Link.Id == CurrentLink.Id)
                    .ToList(); // this conversion to list is needed because we can't bind queries through EF
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

        // TODO change this to return to details view and use the member current link
        public IQueryable<Models.Link> GetLink(
            [QueryString("addr")] string addr
            //, [RouteData] string address
            )
        {
//            loglabel.Text = $"id = {id}, address = {address}, is null = {string.IsNullOrEmpty(address)}; ceva value = {RouteData.Values["address"]}, ";
//            foreach (var kv in RouteData.Values)
//                loglabel.Text += kv.Key + " = " + kv.Value + ", ";

            var db = ApplicationDbContext.Create();
            
            if (!string.IsNullOrEmpty(addr))
                return db.Links.Where(l => l.Address == addr);

//            if (string.IsNullOrEmpty(address))
//                address = RouteData.Values["address"] as string;
//
//            if (!string.IsNullOrEmpty(address))
//                return db.Links.Where(l => l.Address == address);

//            DebugLogger.Log("found no suitable link by title " + address + "!");

            // TODO redirect to homepage if no links where found (invalid addr)
            //            Response.Redirect("Default");
            return null;
        }
    }
}