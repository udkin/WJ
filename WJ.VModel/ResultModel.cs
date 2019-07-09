using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJ.VModel
{
    public class ResultModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Success { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ResultJson ResultData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ErrorMsg { get; set; }
    }
}
