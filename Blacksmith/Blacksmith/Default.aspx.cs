using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using Blacksmith.Models;

namespace Blacksmith
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public List<Link> GetRecentLinks()
        {
            return LinkManager.ExecuteSelect("Select * From links Order By date Desc");
        }
    }
}