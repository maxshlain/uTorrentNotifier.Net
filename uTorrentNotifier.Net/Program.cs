using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace uTorrentNotifier.Net
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				if (args.Length != 2)
				{
					Console.WriteLine("Invalid number of parameters!");
					Console.WriteLine("Usage: notifier.exe module_name message");
				}
				else
				{
					string moduleName = args[0];
					string message = args[1];

					moduleName = string.IsNullOrEmpty(moduleName) ? "" : moduleName;
					message = string.IsNullOrEmpty(message) ? "" : message;

					if (string.IsNullOrEmpty(moduleName))
					{
						Console.WriteLine("Invalid parameter: module_name");
						Console.WriteLine("Usage: notifier.exe module_name message");
					}
					else 
					{
						SimpleMailConfig config = BuildSmptpConfig();
						Notificator.SendNotification(moduleName, message, config);
					}
					
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

			return;
		}

		static SimpleMailConfig BuildSmptpConfig()
		{
			SimpleMailConfig resultConfig = new SimpleMailConfig();

			try 
			{
				string tempValue = "";
				resultConfig.EmailTo = ConfigurationManager.AppSettings["EmailTo"];
				resultConfig.EmailFromUsername = ConfigurationManager.AppSettings["EmailFromUsername"];
				resultConfig.EmailFrom = ConfigurationManager.AppSettings["EmailFrom"];
				resultConfig.EmailFromPassword = ConfigurationManager.AppSettings["EmailFromPassword"];
				resultConfig.SmtpHost = ConfigurationManager.AppSettings["SmtpHost"];
				tempValue = ConfigurationManager.AppSettings["SmtpPort"];
				resultConfig.SmtpPort = ((!string.IsNullOrEmpty(tempValue)) && (int.TryParse(tempValue, out resultConfig.SmtpPort))) ? resultConfig.SmtpPort : 587;

				resultConfig.SubjectPattern = ConfigurationManager.AppSettings["SubjectPattern"];
				resultConfig.BodyPattern = ConfigurationManager.AppSettings["BodyPattern"];
			}
			catch (Exception)
			{
				var Nop = "nop";
			}

			return resultConfig;
		}

	}
}
