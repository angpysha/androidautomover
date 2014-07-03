using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using AutoMove;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        private readonly AutoMove.AutoMove _mover;
        public Service1()
        {
            InitializeComponent();
            var configpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "conf.cfg");
            
            _mover = new AutoMove.AutoMove(configpath);
      //      mmmm = new AutoMove.AutoMove(configpath);
        }

        protected override void OnStart(string[] args)
        {
            _mover.Start();
        }

        protected override void OnStop()
        {
            _mover.Stop();
        }
    }
}
