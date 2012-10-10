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
						//SimpleMailConfig config = BuildSmptpConfig();
						//Notificator.SendNotification_viaEmail(moduleName, message, config);
                        
                        SimpleTwitterConfig config = BuildSimpleTwitterConfig();
                        
                        foreach (string splitStr in config.SkipStringList)
                        {
                            if (moduleName.ToLower().Contains(splitStr.ToLower()))
                            {
                                return;
                            }
                        }

                        
                        Notificator.SendNotification_viaTwitter(moduleName, message, config);
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

        static SimpleTwitterConfig BuildSimpleTwitterConfig() 
        {
            SimpleTwitterConfig resConfig = new SimpleTwitterConfig();
            resConfig.BodyPattern = ConfigurationManager.AppSettings["BodyPattern"];
            resConfig.Autorized = false;

            resConfig.AppConsumerKey = ConfigurationManager.AppSettings["tw_AppConsumerKey"];
            resConfig.AppConsumerSecret = ConfigurationManager.AppSettings["tw_AppConsumerSecret"];

            resConfig.AccessToken = ConfigurationManager.AppSettings["tw_AccessToken"];
            resConfig.AccessTokenSecret = ConfigurationManager.AppSettings["tw_AccessTokenSecret"];
            resConfig.AccessPin = ConfigurationManager.AppSettings["tw_AccessPin"];

            resConfig.Autorized = (!(string.IsNullOrEmpty(resConfig.AppConsumerKey)
                                    || string.IsNullOrEmpty(resConfig.AppConsumerKey)
                                    || string.IsNullOrEmpty(resConfig.AppConsumerSecret)
                                    || string.IsNullOrEmpty(resConfig.AccessToken)
                                    || string.IsNullOrEmpty(resConfig.AccessTokenSecret)
                                    /*|| string.IsNullOrEmpty(resConfig.AccessPin)*/));

            string valStr = ConfigurationManager.AppSettings["SkipStrings"];
            valStr = string.IsNullOrEmpty(valStr) ? "" : valStr;
            string[] split = valStr.Split(new Char[] { '|' });

            if ((null != split) && (split.Length > 0))
            {
                resConfig.SkipStringList.AddRange(split);
            }

            return resConfig;
        }

	}
}
