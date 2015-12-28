using System;
using System.Linq;
using System.Web.UI;
using Blacksmith.Models;

namespace Blacksmith
{
    public partial class _Default : Page
    {
        public IQueryable<Models.Link> GetRecentLinks()
        {
            return ApplicationDbContext.Create().Links
                .OrderByDescending(l => l.Date);
        }
    }
}