using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Blacksmith.Models
{
    public class LinksDatabaseInitializer : DropCreateDatabaseAlways<LinkContext>
    {
        protected override void Seed(LinkContext context)
        {
//            var stefan = new User
//            {
//                Id = "idstefan",
//                Email = "stefa@email.com",
//                UserName = "stefan",
//            };
//            var ionut = new User
//            {
//                Id = "idionut",
//                Email = "ionut@email.com",
//                UserName = "ionut",
//            };
//            var manager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
//            manager.Create(stefan, "parolastefan1");
//            manager.Create(ionut, "parolaionut2");


            //            var users = new List<User>
            //            {
            //                new User
            //                {
            //                    Id = "idstefan",
            //                    Email = "stefan@email.com",
            //                    UserName = "Stefan",
            //                },
            //                new User
            //                {
            //                    Id = "idionut",
            //                    Email = "ionut@email.com",
            //                    UserName = "Ionut",
            //                },
            //            };

            var links = new List<Link>
            {
                new Link
                {
                    Id = 1,
                    Title = "Google",
                    Address = "google.dk",
                    Description = "Google Denmark",
                    Date = DateTime.Now,
//                    SubmitterId = "idstefan",
                },
                new Link
                {
                    Id = 2,
                    Title = "Amazon",
                    Address = "amazon.it",
                    Description = "Amazon Italy",
                    Date = DateTime.Now.AddDays(-5),
//                    SubmitterId = "idionut",
                },
                new Link
                {
                    Id = 3,
                    Title = "Ebay",
                    Address = "ebay.ca",
                    Description = "Ebay Canada",
                    Date = DateTime.Now.AddDays(-10),
//                    SubmitterId = "idstefan",
                },
            };

            var comments = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
//                    LinkId = 1,
                    Link = links[0],
                    Content = "Best search engine!",
//                    SubmitterId = "idstefan",
                },
                new Comment
                {
                    Id = 2,
//                    LinkId = 1,
                    Link = links[0],
                    Content = "Why denmark link though?",
//                    SubmitterId = "idionut",
                },
                new Comment
                {
                    Id = 3,
//                    LinkId = 2,
                    Link = links[1],
                    Content = "Amazon rocks!",
//                    SubmitterId = "idstefan",
                },
            };

//            context.Users.AddRange(users);
            context.Links.AddRange(links);
            context.Comments.AddRange(comments);
        }
        
    }
}