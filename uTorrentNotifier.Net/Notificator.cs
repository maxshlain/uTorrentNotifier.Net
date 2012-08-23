using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uTorrentNotifier.Net
{
    class Notificator
    {
        internal static void SendNotification(string i_moduleName, string i_message, SimpleMailConfig i_mailConfig)
        {
            SimpleMail sMsg = new SimpleMail(i_mailConfig);

            //sMsg.To = "max@shlain.net";

            //sMsg.Subject = String.Format("Notification from {0}. {1}", i_moduleName, i_message);
            sMsg.Subject = String.Format(i_mailConfig.SubjectPattern, i_moduleName, i_message);

            switch (i_moduleName)
            {
                case "uTorrent":
                    //sMsg.Body = String.Format("Notification from {0}. {1} has finished downloading", i_moduleName, i_message);
                    sMsg.Body = String.Format(i_mailConfig.BodyPattern, i_moduleName, i_message);
                    break;

                default:
                    //sMsg.Body = String.Format("Notification from {0}. {1}", i_moduleName, i_message);
                    sMsg.Body = String.Format(i_mailConfig.BodyPattern, i_moduleName, i_message);
                    break;
            }

            sMsg.Send();            
        }
    }
}
