using System.Diagnostics;
using System.Linq;
using System.Windows;
using XGuideSQLiteDB;
using XGuideSQLiteDB.Models;

namespace DbTester
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IRepository repositoryOperation = new Repository();

        public App()
        {
            var i = repositoryOperation.GetAll<User>();
            Debug.WriteLine(i.First());
        }
    }
}