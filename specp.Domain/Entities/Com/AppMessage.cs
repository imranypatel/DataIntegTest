using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace specp.Domain.Entities.Com
{
    public class AppMessage
    {
        public string Status { get; set; }  // Error, Warning , Info ,OK
        public string MessageId { get; set; }   //
        public string Message1 { get; set; }  // 
        public string Message2 { get; set; }  // 
        public string Message3 { get; set; }  // 
        public String SourceMessage { get; set; }

    }
}
