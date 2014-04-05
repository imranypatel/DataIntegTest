using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace specp.DataIntegration
{
    class Globals
    {
        private static string _connectionString;
        static Globals()
        {
            try
            {

                _connectionString = ConfigurationManager.ConnectionStrings["TraxConnectionString"].ConnectionString;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string ConnectionString
        {
            get { return _connectionString; }


        }

        public static void Trace(string msg, string category)
        {

            System.Diagnostics.Trace.WriteLine(msg, category);
        }
    }
}
