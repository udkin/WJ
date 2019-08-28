using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WJ.API.Models;

namespace WJ.API
{
    /// <summary>
    /// ImageHandler 的摘要说明
    /// </summary>
    public class ImageHandler : IHttpHandler
    {

        #region [ 属性 ]
        public HttpRequest Request
        {
            get
            {
                return HttpContext.Current.Request;
            }
        }

        public HttpServerUtility Server
        {
            get
            {
                return HttpContext.Current.Server;
            }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            var resultObj = new ResultModel { Success = 0, Code = 1, ErrorMsg = "上传文件失败" };

            context.Response.ContentType = "text/plain";
            string json = string.Empty;

            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFile file = Request.Files[0];
                    if (file.ContentLength == 0)
                    {
                        resultObj.ErrorMsg = "文件长度为0";
                    }
                    else
                    {
                        string uploadDir = Server.MapPath("~\\Store\\Image");
                        if (!Directory.Exists(uploadDir))
                        {
                            Directory.CreateDirectory(uploadDir);
                        }

                        string filePath = Path.Combine(uploadDir, Path.GetFileName(file.FileName));
                        if (File.Exists(filePath))
                        {
                            try
                            {
                                File.Delete(filePath);
                            }
                            catch (Exception ex)
                            {
                                System.Threading.Thread.Sleep(1000);
                                filePath = Path.Combine(uploadDir, DateTime.Now.ToString("yyyy-MM-dd_HHmmss_") + Path.GetFileName(file.FileName));
                            }
                        }

                        file.SaveAs(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                //处理异常

            }

            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}