using System;
using System.Linq;
using System.Web.UI;
using Blacksmith.Models;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNet.Identity;

namespace Blacksmith
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           LogoutResidualUser();
        }

        private void LogoutResidualUser()
        {
            // Log out the signed user if their id is not found in the database
            // protection for residual logged users from previous databases
            string loggedId = User.Identity.GetUserId();
            var user = ApplicationDbContext.Create()
                .Users.Find(loggedId);
            if (user == null)
                HttpContext.Current.Session.Abandon();
        }

        public IQueryable<Models.Link> RecentLinks()
        {
            return ApplicationDbContext.Create()
                .Links.OrderByDescending(l => l.Date);
        }

        public IQueryable<Models.Link> PopularLinks()
        {
            return ApplicationDbContext.Create()
                .Links.OrderByDescending(l => l.Favorites.Count);
        }

        // TODO move this to a logic class and use it everywhere a link appears
        private HashSet<int> _favoriteIds;
        public bool HasFavorited(int linkId)
        {
            LogoutResidualUser();

            // All links are not-favorited by unauthenticated users
            if (!User.Identity.IsAuthenticated)
                return false;

            // Cache results for faster querying
            if (_favoriteIds == null)
            {
                var userId = User.Identity.GetUserId();
                var favorites = ApplicationDbContext.Create()
                    .Users.Find(userId).Favorites;

                _favoriteIds = new HashSet<int>();
                foreach (var favorite in favorites)
                    _favoriteIds.Add(favorite.Link.Id);
            }
            
            return _favoriteIds.Contains(linkId);
        }
    }
}