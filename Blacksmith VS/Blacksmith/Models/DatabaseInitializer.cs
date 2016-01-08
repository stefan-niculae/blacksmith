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
            var admin = new User
            {
                Email = "admin@email.com",
                UserName = "administrator"
            };

            var userManager = new UserManager<User>(new UserStore<User>(context));
            
            var result = userManager.Create(stefan, password: "Pa$$word1");
            if (!result.Succeeded)
                DebugLogger.Log("stefan creation error: " + result.Errors.Aggregate("", (current, error) => current + error + "\n"));
            
            result = userManager.Create(ionut, password: "password");
            if (!result.Succeeded)
                DebugLogger.Log("ionut creation error: " + result.Errors.Aggregate("", (current, error) => current + error + "\n"));
            result = userManager.Create(admin, password: "administrator");
            if (!result.Succeeded)
                DebugLogger.Log("admin account creation error: " + result.Errors.Aggregate("", (current, error) => current + error + "\n"));

            // Do this to assign the generated id
            stefan = userManager.FindByEmail(stefan.Email);
            ionut = userManager.FindByEmail(ionut.Email);
            admin = userManager.FindByEmail(admin.Email);

            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            if (!roleManager.RoleExists("admin"))
            {
                var r = roleManager.Create(new IdentityRole("admin"));
                if (!r.Succeeded)
                    DebugLogger.Log("admin role creation error: " + result.Errors.Aggregate("", (current, error) => current + error + "\n"));
            }

            if (!userManager.IsInRole(admin.Id, "admin"))
            {
                var r = userManager.AddToRole(admin.Id, "admin");
                if (!r.Succeeded)
                    DebugLogger.Log("admin role assignment error: " + result.Errors.Aggregate("", (current, error) => current + error + "\n"));
            }


            var links = new List<Link>
            {
                new Link
                {
                    Title = "Google",
                    Address = "google.dk",
                    Description = "Find stuff",
                    Date = DateTime.Now.AddDays(-1),
                    Submitter = stefan
                },
                new Link
                {
                    Title = "Amazon",
                    Address = "amazon.it",
                    Description = "Buy italian stuff",
                    Date = DateTime.Now.AddDays(-2),
                    Submitter = ionut
                },
                new Link
                {
                    Title = "Ebay",
                    Address = "ebay.ca",
                    Description = "Buy canadian stuff",
                    Date = DateTime.Now.AddDays(-3),
                    Submitter = stefan
                },
                new Link
                {
                    Title = "Reddit",
                    Address = "reddit.com",
                    Description = "Waste time in a fun way",
                    Date = DateTime.Now.AddDays(-4),
                    Submitter = stefan
                },
                new Link
                {
                    Title = "Facebook",
                    Address = "facebook.com",
                    Description = "Largest social networking site",
                    Date = DateTime.Now.AddDays(-5),
                    Submitter = ionut
                },
                new Link
                {
                    Title = "FB Messenger",
                    Address = "Messenger.com",
                    Description = "Just the messenger part of facebook",
                    Date = DateTime.Now.AddDays(-6),
                    Submitter = stefan
                },
                new Link
                {
                    Title = "Youtube",
                    Address = "youtube.com",
                    Description = "Most popular video sharing platform",
                    Date = DateTime.Now.AddDays(-7),
                    Submitter = ionut
                },
                new Link
                {
                    Title = "Inbox",
                    Address = "inbox.google.com",
                    Description = "Mail management by the Gmail team",
                    Date = DateTime.Now.AddDays(-8),
                    Submitter = stefan
                },
                new Link
                {
                    Title = "Drive",
                    Address = "drive.google.com",
                    Description = "Cloud storage from Google",
                    Date = DateTime.Now.AddDays(-9),
                    Submitter = stefan
                },
                new Link
                {
                    Title = "Keep",
                    Address = "keep.google.com",
                    Description = "Notes and lists",
                    Date = DateTime.Now.AddDays(-10),
                    Submitter = stefan
                },
                new Link
                {
                    Title = "Univeristy of Bucharest",
                    Address = "fmi.unibuc.ro",
                    Description = "Math & CS, Bucharest Uni",
                    Date = DateTime.Now.AddDays(-11),
                    Submitter = ionut
                },
                new Link
                {
                    Title = "Github",
                    Address = "github.com",
                    Description = "Social coding",
                    Date = DateTime.Now.AddDays(-12),
                    Submitter = stefan
                },
                new Link
                {
                    Title = "Maps",
                    Address = "maps.google.com",
                    Description = "Online maps & directions",
                    Date = DateTime.Now.AddDays(-13),
                    Submitter = ionut
                },
                new Link
                {
                    Title = "Calendar",
                    Address = "calendar.google.com",
                    Description = "Events & reminders",
                    Date = DateTime.Now.AddDays(-14),
                    Submitter = stefan
                },
            };
            for (int i = 0; i < links.Count; i++)
                links[i] = context.Links.Add(links[i]);
            
            var comments = new List<Comment>
            {
                new Comment
                {
                    Link = links[0],
                    Content = "Best search engine!",
                    Date = DateTime.Now.AddHours(-1),
                    Submitter = stefan
                },
                new Comment
                {
                    Link = links[0],
                    Content = "Why denmark link though?",
                    Date = DateTime.Now.AddHours(-2),
                    Submitter = ionut
                },
                new Comment
                {
                    Link = links[0],
                    Content = "Shhh, no-one's gonna notice!",
                    Date = DateTime.Now.AddHours(-2),
                    Submitter = stefan
                },
                new Comment
                {
                    Link = links[1],
                    Content = "Amazon rocks!",
                    Date = DateTime.Now.AddHours(-3),
                    Submitter = ionut
                },
                new Comment
                {
                    Link = links[11],
                    Content = "Highly recommended skill to have!",
                    Date = DateTime.Now.AddHours(-4),
                    Submitter = stefan,
                },
                new Comment
                {
                    Link = links[11],
                    Content = "Essential",
                    Date = DateTime.Now.AddHours(-5),
                    Submitter = ionut,
                },
            };
            context.Comments.AddRange(comments);

            var favorites = new List<Favorite>
            {
                new Favorite
                {
                    Category = "shopping",
                    Date = DateTime.Now.AddHours(-2),
                    Link = links[1],
                    User = stefan
                },
                new Favorite
                {
                    Category = "shopping",
                    Date = DateTime.Now.AddHours(-1),
                    Link = links[2],
                    User = stefan
                },
                new Favorite
                {
                    Category = "deals",
                    Date = DateTime.Now.AddHours(-1),
                    Link = links[1],
                    User = ionut
                },

                new Favorite
                {
                    Category = "Google",
                    Date = DateTime.Now.AddHours(-3),
                    Link = links[0],
                    User = ionut
                },
                new Favorite
                {
                    Category = "Google",
                    Date = DateTime.Now.AddHours(-3),
                    Link = links[7],
                    User = ionut
                },
                new Favorite
                {
                    Category = "Google",
                    Date = DateTime.Now.AddHours(-3),
                    Link = links[8],
                    User = ionut
                },
                new Favorite
                {
                    Category = "Google",
                    Date = DateTime.Now.AddHours(-3),
                    Link = links[9],
                    User = ionut
                },
                new Favorite
                {
                    Category = "Google",
                    Date = DateTime.Now.AddHours(-3),
                    Link = links[12],
                    User = ionut
                },
                new Favorite
                {
                    Category = "Google",
                    Date = DateTime.Now.AddHours(-3),
                    Link = links[13],
                    User = ionut
                },

                 new Favorite
                {
                    Category = "Productivity",
                    Date = DateTime.Now.AddHours(-3),
                    Link = links[7],
                    User = stefan
                },
                new Favorite
                {
                    Category = "Productivity",
                    Date = DateTime.Now.AddHours(-3),
                    Link = links[8],
                    User = stefan
                },
                new Favorite
                {
                    Category = "Productivity",
                    Date = DateTime.Now.AddHours(-3),
                    Link = links[9],
                    User = stefan
                },
                new Favorite
                {
                    Category = "Productivity",
                    Date = DateTime.Now.AddHours(-3),
                    Link = links[12],
                    User = stefan
                },
                new Favorite
                {
                    Category = "Productivity",
                    Date = DateTime.Now.AddHours(-3),
                    Link = links[13],
                    User = stefan
                },
                new Favorite
                {
                    Category = "Productivity",
                    Date = DateTime.Now.AddHours(-3),
                    Link = links[11],
                    User = stefan
                },
            };
            context.Favorites.AddRange(favorites);
        }
        
    }
}