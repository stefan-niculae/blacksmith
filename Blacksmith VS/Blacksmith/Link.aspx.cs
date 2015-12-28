using System;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using Blacksmith.Models;
using Blacksmith.Utilities;
using System.Web.Routing;

namespace Blacksmith
{
    public partial class Link : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            loglabel.Text = "aaa";
        }

        public IQueryable<Models.Link> GetLink(
            [QueryString("id")] int? id,
            [RouteData] string address)
        {
            loglabel.Text = $"id = {id}, address = {address}, is null = {string.IsNullOrEmpty(address)}; ceva value = {RouteData.Values["address"]}, ";
            foreach (var kv in RouteData.Values)
                loglabel.Text += kv.Key + " = " + kv.Value + ", ";


            var db = new ApplicationDbContext();
            
            if (id.HasValue && id > 0)
                return db.Links.Where(l => l.Id == id);

            if (string.IsNullOrEmpty(address))
                address = RouteData.Values["address"] as string;

            if (!string.IsNullOrEmpty(address))
                return db.Links.Where(l => l.Address == address);

            DebugLogger.Log("found no suitable link by title " + address + "!");

            // TODO redirect to homepage if no links where found (invalid id)
            //            Response.Redirect("Default");
            return null;
        }
    }
}