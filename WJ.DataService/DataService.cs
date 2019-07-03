using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WJ.DataService
{
    public partial class DataService : ServiceBase
    {
        public DataService()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            TimedDataService.Instance.Start();
        }

        /// <summary>
        /// 停止
        /// </summary>
        protected override void OnStop()
        {
            TimedDataService.Instance.Stop();
        }

        /// <summary>
        /// 系统关闭
        /// </summary>
        protected override void OnShutdown()
        {
            TimedDataService.Instance.Stop();
        }
    }
}
