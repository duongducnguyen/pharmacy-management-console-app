using System;

using PharmacyManagement.Data;
using PharmacyManagement.Models;

namespace PharmacyManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {

            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var user = new User
                    {
                        Username = "admin",
                        Email = "test@example.com",
                        Password = "123456",
                        CreatedAt = DateTime.Now
                    };

                    context.Users.Add(user);
                    context.SaveChanges();

                    Console.WriteLine("User added successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();

        }
    }
}
