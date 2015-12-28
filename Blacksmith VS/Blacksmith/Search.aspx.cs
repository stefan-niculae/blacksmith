using System;
using System.Linq;
using Blacksmith.Models;

namespace Blacksmith
{
    public partial class Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IQueryable<Models.Link> GetLinks()
        {
            return new ApplicationDbContext().Links;
        }
    }
}