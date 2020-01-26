using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace ChatTool {
	public class AppSettings {

		static AppSettings() {
			ResourceDirectoryPath = ConfigurationManager.AppSettings["resourceDirectoryPath"];
		}

		public static string ResourceDirectoryPath { get; }
	}
}
