using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJ.VModel
{
    public class AppClassAndApp : ResultJson
    {
        /// <summary>
        /// 
        /// </summary>
        public int AppClassId { get; set; }
        /// <summary>
        /// 空气质量
        /// </summary>
        public string AppClass_Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AppClass_Image { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<AppBase> AppList { get; set; }
    }
}
