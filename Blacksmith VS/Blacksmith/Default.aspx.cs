using System;
using System.Linq;
using System.Web.UI;
using Blacksmith.Models;

namespace Blacksmith
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

//        public List<Link> GetRecentLinks()
//        {
//            return LinkManager.ExecuteSelect("Select * From links Order By date Desc");
//        }

        public IQueryable<Models.Link> GetRecentLinks2()
        {
            return new ApplicationDbContext().Links;
        }
    }
}