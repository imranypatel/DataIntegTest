using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using System.Configuration;
using System.IO;

namespace specp.DataIntegration
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {

            logger.Info("Starting service...");
            DataIntegrator.ServiceController();
            logger.Info("Finished service.");
            //Console.ReadKey();
        }
    }

  
}
