using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;


namespace X_Guide.MVVM.ViewModel
{
    internal class RenderControlViewModel
    {

        public IDataSource DataSource { get; set; }
        public RenderControlViewModel()
        {
            LoadSolutionFile();
        }

        private void LoadSolutionFile()
        {
            string solutionFilePath = "path/to/something";
            if (File.Exists(solutionFilePath))
            {
               
     
            }
        }
    }
}
