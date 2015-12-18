using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;

namespace Blacksmith.Models
{
    public class Link
    { 
        public int Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string Submitter { get; set; }
        public DateTime Date { get; set; }
       
        // Use services to provide favicon and thumbnail
        public string Favicon => $@"http://www.google.com/s2/favicons?domain_url={Address}";
        public string Thumbnail => $@"http://images.shrinktheweb.com/xino.php?stwembed=1&stwxmax=320&stwymax=240&stwaccesskeyid=c7fc29b382dca49&stwurl=http://{Address}";

        public Link(int id, string title, string address, string description, string submitter, DateTime date)
        {
            Id = id;
            Title = title;
            Address = address;
            Description = description;
            Submitter = submitter;
            Date = date;
        }

        public Link(string title, string address, string description, string submitter)
        {
            Title = title;
            Address = address;
            Description = description;
            Submitter = submitter;

            Date = DateTime.Now;
        }

        public Link(SqlDataReader reader)
        {
            Id = (int) reader["Id"];
            Title = (string) reader["Title"];
            Description = (string)reader["Description"];
            Address = (string) reader["Address"];
            Submitter = (string) reader["Submitter"];
            Date = (DateTime) reader["Date"];
        }
        
    }

    public class LinkManager
    {
        // Create
        public static int Insert(Link link)
        {
            var connection = DatabaseHelper.NewConnection();

            string query = "Insert Into links(Title, Description, Address, Submitter, Date) " +
                           $"Values('{link.Title}', '{link.Description}', '{link.Address}', '{link.Submitter}', '{link.Date}'); " +
                           "Select @@Identity";
            var command = new SqlCommand(query, connection);

            int newLinkId = Convert.ToInt32(command.ExecuteScalar());

            DatabaseHelper.Cleanup(connection, command);
            return newLinkId;
        }

        // Read
        public static List<Link> ExecuteSelect(string query)
        {
            var connection = DatabaseHelper.NewConnection();

            var command = new SqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var links = new List<Link>();
            if (reader.HasRows)
                while (reader.Read())
                    links.Add(new Link(reader));

            DatabaseHelper.Cleanup(connection, command);
            return links;
        }

        // Update
        public static int Update(string field, string value, int id)
        {
            var connection = DatabaseHelper.NewConnection();

            string query =  "Update links " +
                           $"Set    {field} = '{value}', " +
                           $"       date    = '{DateTime.Now}'" +
                           $"Where  id = {id}";
            var command = new SqlCommand(query, connection);
            int rowsUpdated = command.ExecuteNonQuery();

            DatabaseHelper.Cleanup(connection, command);
            return rowsUpdated;
        }

        // Delete
        public static bool Delete(int id)
        {
            var connection = DatabaseHelper.NewConnection();
            var command = new SqlCommand($"Delete From Links Where id = {id}", connection);

            int rowsDeleted = command.ExecuteNonQuery();

            DatabaseHelper.Cleanup(connection, command);
            return rowsDeleted != 0;
        }
    }

    public class LinkDbContext : DbContext
    {
        public DbSet<Link> Links { get; set; }
    }
}