using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace CouchBaseHelloWorldGUI
{
    class stuff
    {
        public DateTime DateAdded { get; set; }
        public proc[] Processes { get; set; }

        public stuff()
        {
            DateAdded = DateTime.Now;
            Processes = Process.GetProcesses().Where(x => x.ProcessName == "chrome").Select(x => new proc() { procname = x.ProcessName, timestarted = x.StartTime }).ToArray();
        }
    }

    class proc
    {
        public string procname { get; set; }
        public DateTime timestarted { get; set; }
    }
}
