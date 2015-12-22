using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Blacksmith.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blacksmith.Models
{
    public class LinksDatabaseInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            DebugLogger.Log("beginning of seed");
            var stefan = new User
            {
                Email = "stefan@email.com",
                UserName = "stefan",
            };
            var ionut = new User
            {
                Email = "ionut@email.com",
                UserName = "ionut",
            };

            var manager = new UserManager<User>(new UserStore<User>(context));

            // FIXME why can't I log in with this password?
            var result = manager.Create(stefan, password: "Pa$$word1");
            if (!result.Succeeded)
                DebugLogger.Log("stefan creation error: " + result.Errors.Aggregate("", (current, error) => current + error + "\n"));
            
            result = manager.Create(ionut, password: "password");
            if (!result.Succeeded)
                DebugLogger.Log("ionut creation error: " + result.Errors.Aggregate("", (current, error) => current + error + "\n"));


            // Do this to assign the generated id
            stefan = manager.FindByEmail(stefan.Email);
            ionut = manager.FindByEmail(ionut.Email);

            var links = new List<Link>
            {
                new Link
                {
                    Id = 1,
                    Title = "Google",
                    Address = "google.dk",
                    Description = "Google Denmark",
                    Date = DateTime.Now,
                    SubmitterId = stefan.Id,
                },
                new Link
                {
                    Id = 2,
                    Title = "Amazon",
                    Address = "amazon.it",
                    Description = "Amazon Italy",
                    Date = DateTime.Now.AddDays(-5),
                    SubmitterId = ionut.Id,
                },
                new Link
                {
                    Id = 3,
                    Title = "Ebay",
                    Address = "ebay.ca",
                    Description = "Ebay Canada",
                    Date = DateTime.Now.AddDays(-10),
                    SubmitterId = stefan.Id,
                },
            };

            var comments = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    LinkId = 1,
                    Link = links[0],
                    Content = "Best search engine!",
                    SubmitterId = stefan.Id,
                },
                new Comment
                {
                    Id = 2,
                    LinkId = 1,
                    Link = links[0],
                    Content = "Why denmark link though?",
                    SubmitterId = ionut.Id,
                },
                new Comment
                {
                    Id = 3,
                    LinkId = 2,
                    Link = links[1],
                    Content = "Amazon rocks!",
                    SubmitterId = ionut.Id,
                },
            };

//            context.Users.AddRange(users);
            context.Links.AddRange(links);
            context.Comments.AddRange(comments);
        }
        
    }
}