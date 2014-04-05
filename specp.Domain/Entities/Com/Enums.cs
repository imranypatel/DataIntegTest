using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace specp.Domain.Entities.Com
{
    public class Status
    {
        public static int NEW { get { return 35; } }
        public static int CLOSED { get { return 36; } }
    }

    public class SysUsers
    {
       public static  int SYSTEM { get { return 1;}  }
       public static int SERVICE { get { return 2; } }
    }
}
