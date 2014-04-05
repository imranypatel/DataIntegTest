using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using specp.Domain.Entities.Com;
namespace specp.Domain.Repository
{
    public class Com
    {
        public static AppMessage GetAppMessage(string appMsg)
        {

            AppMessage msg;
            try
            {
                // DB messaage format: "Reason~Action~Description"
                // Status ~MessageID ~Message1~Message2~Message3
                var Splitted = appMsg.Split(new Char[] { '~' });
                msg = new AppMessage { Status = Splitted[0], MessageId = Splitted[1], Message1 = Splitted[2], Message2 = Splitted[3], Message3 = Splitted[4], SourceMessage = appMsg };
            }
            catch (Exception e)
            {
                //msg = new tDALMessage { Action = "ERR", Reason = "DB Message not formated properly.", Description = "DB API Returned message in incorrect format" }; 
                //throw new Exception("TERR: DB Message not formated properly");
                msg = new AppMessage { Status = "ERR", MessageId = "-1-DAL Message is in incorrect format", Message1 = "", Message2 = "", Message3 = "" };
            }
            return msg;
        }
    }
}
