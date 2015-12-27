using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Blacksmith.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blacksmith.Models
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
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
                    Submitter = stefan
                },
                new Link
                {
                    Id = 2,
                    Title = "Amazon",
                    Address = "amazon.it",
                    Description = "Amazon Italy",
                    Date = DateTime.Now.AddDays(-5),
                    Submitter = ionut
                },
                new Link
                {
                    Id = 3,
                    Title = "Ebay",
                    Address = "ebay.ca",
                    Description = "Ebay Canada",
                    Date = DateTime.Now.AddDays(-10),
                    Submitter = stefan
                },
            };
            for (int i = 0; i < links.Count; i++)
                links[i] = context.Links.Add(links[i]);
            
            var comments = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    Link = links[0],
                    Content = "Best search engine!",
                    Submitter = stefan
                },
                new Comment
                {
                    Id = 2,
                    Link = links[0],
                    Content = "Why denmark link though?",
                    Submitter = ionut
                },
                new Comment
                {
                    Id = 3,
                    Link = links[1],
                    Content = "Amazon rocks!",
                    Submitter = ionut
                },
            };
            
            context.Comments.AddRange(comments);
        }
        
    }
}