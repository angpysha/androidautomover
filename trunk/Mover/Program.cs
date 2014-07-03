using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMove;

namespace Mover
{
    class Program
    {
        public string zz;
        static void Main(string[] args)
        {
            var confpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "conf.cfg");
            var mover = new AutoMove.AutoMove(confpath);
            mover.Start();


            System.Console.WriteLine("Press any key");
            System.Console.ReadKey();
        }
    }
}
