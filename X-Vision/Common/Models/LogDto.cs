﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace X_Vision.Common.Models
{
    public class LogDto:BaseDto
    { 
		private string title;

		public string Title
		{
			get { return title; }
			set { title = value; }
		}

		private string content;

		public string Content
		{
			get { return content; }
			set { content = value; }
		}

		private int status;

		public int Status
		{
			get { return status; }
			set { status = value; }
		}


	}
}
