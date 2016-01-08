using System.Linq;
using Blacksmith.Models;

namespace Blacksmith
{
    public partial class Search : System.Web.UI.Page
    {
        public IQueryable<Models.Link> GetLinks()
        {
            return ApplicationDbContext.Create().Links;
        } 
    }
}