using System;
using System.Linq;
using System.Web.UI;
using Blacksmith.Models;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace Blacksmith
{
    public partial class _Default : Page
    {
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

        private List<int> _favoriteIds;
        public bool HasFavorited(int linkId)
        {
            if (_favoriteIds == null)
            {
                var userId = User.Identity.GetUserId();
                var favorites = ApplicationDbContext.Create()
                    .Users.Find(userId).Favorites;

                _favoriteIds = new List<int>();
                foreach (var favorite in favorites)
                    _favoriteIds.Add(favorite.Link.Id);
            }
            
            return _favoriteIds.Contains(linkId);
        }
    }
}