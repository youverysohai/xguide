using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Vision.Common.Models
{
    public class MenuBar: BindableBase
    {

        /// <summary>
		/// Menu Icons
		/// </summary>
		private string icon;

        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }


        /// <summary>
        /// Menu Name
        /// </summary>
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        /// <summary>
        /// Menu NameSpace
        /// </summary>
        private string nameSpace;

        public string NameSpace
        {
            get { return nameSpace; }
            set { nameSpace = value; }
        }
    }
}
