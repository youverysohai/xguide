using System;
using System.Linq;
using XGuideSQLiteDB;
using XGuideSQLiteDB.Models;

namespace CalibrationTesting
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IRepository repositoryOperation = new Repository();
            repositoryOperation.Create<User>(new User
            {
                Username = "Chun",
                Email = "ChunChunMaru@gmail.com"
            });
            User user = repositoryOperation.Find<User>(c => c.Username.Equals("Chun")).FirstOrDefault();
            Console.WriteLine(user);
        }
    }
}