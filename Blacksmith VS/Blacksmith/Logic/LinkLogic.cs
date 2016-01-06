using System.Linq;
using System.Web.UI;
using Blacksmith.Models;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace Blacksmith.Logic
{
    public class LinkLogic
    {
//        private List<int> _favoriteIds;
//        public bool HasFavorited(int linkId)
//        {
//            // All links are not-favorited by unauthenticated users
//            if (!User.Identity.IsAuthenticated)
//                return false;
//
//            // Cache results for faster querying
//            if (_favoriteIds == null)
//            {
//                var userId = User.Identity.GetUserId();
//                var favorites = ApplicationDbContext.Create()
//                    .Users.Find(userId).Favorites;
//
//                _favoriteIds = new List<int>();
//                foreach (var favorite in favorites)
//                    _favoriteIds.Add(favorite.Link.Id);
//            }
//
//            return _favoriteIds.Contains(linkId);
//        }
    }
}