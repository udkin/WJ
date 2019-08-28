using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WJ.API.Models;
using WJ.Common;

namespace WJ.API.Controllers
{
    [ApiAuthorize]
    public class ImageController : ApiBaseController
    {
        /// <summary>
        /// 接受上传图片文件
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult UploadFile()
        {
            var resultObj = new ResultModel { Success = 0, Code = 1, ErrorMsg = "图片上传失败" };

            var request = System.Web.HttpContext.Current.Request;// 这句是关键.不能用WEBAPI的Request
            // 到这里就一样的
            if (request.Files.Count > 0)
            {
                try
                {
                    string ext = System.IO.Path.GetExtension(request.Files[0].FileName);
                    string fileName = "t_" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ext;
                    string directory = HttpContext.Current.Server.MapPath("~/Store/Image");//指定要将文件存入的服务器物理位置
                    request.Files[0].SaveAs(directory + "\\" + fileName);

                    resultObj.Success = 1;
                    resultObj.ResultData = fileName;
                    resultObj.ErrorMsg = "";
                }
                catch { }
                return Json<dynamic>(resultObj);
            }
            else
            {
                return Json<dynamic>(resultObj);
            }
        }
    }
}
