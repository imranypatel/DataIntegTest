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

    public class State
    {
        public static int TO_BE_SENT { get { return 100; } }
        //public static int TO_BE_SENT_PENDING { get { return 105; } }

        public static int TO_BE_SENT_STAGED { get { return 200; } }
        public static int STAGED_PENDING { get { return 205; } }
        public static int STAGED_SENT { get { return 210; } }
        //public static int STAGED_MISSING { get { return 215; } }

        public static int SENT_PENDING { get { return 300; } }
        public static int SENT_REJECTED { get { return 305; } }
        public static int SENT_SUCCESS { get { return 310; } }
        public static int SENT_MISSING { get { return 315; } }
    }

    public class SysUsers
    {
       public static  int SYSTEM { get { return 1;}  }
       public static int SERVICE { get { return 2; } }
    }

    public class Service
    {
        public static int TENCIA { get { return 3; } }
       
    }
}
