using System;
using System.Text;
using System.IO;
using System.Web;

namespace Fast.Untility.Base
{
    public static class BaseFile
    {
        #region 检查是否为合法的上传文件
        /// <summary>
        /// 检查是否为合法的上传文件
        /// </summary>
        /// <param name="_fileExt"></param>
        /// <returns></returns>
        private static bool CheckFileName(string _fileExt)
        {
            string[] allowExt = new string[] { ".gif", ".jpg", ".jpeg", ".png" };
            for (int i = 0; i < allowExt.Length; i++)
            {
                if (allowExt[i] == _fileExt.ToLower()) { return true; }
            }
            return false;
        }
        #endregion

        #region 文件名称
        /// <summary>
        /// 文件名称
        /// </summary>
        /// <returns></returns>
        private static string GetFileName()
        {
            Random rd = new Random();
            StringBuilder serial = new StringBuilder();
            serial.Append(DateTime.Now.ToString("yyyy-MM-dd-HHmmss"));
            serial.Append(rd.Next(0, 999999).ToString());
            return serial.ToString();
        }
        #endregion

        #region 上传图片
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        public static bool UpPic(HttpPostedFileBase imgFile, string path, ref string picUrl)
        {
            try
            {
                var file = new FileInfo(imgFile.FileName);

                var FileSize = imgFile.ContentLength.ToString();

                //图片扩展名
                if (!CheckFileName(file.Extension))
                {
                    picUrl = "上传图片格式不正确！";
                    return false;
                }

                //生成将要保存的随机文件名
                var fileName = GetFileName() + file.Extension;

                path = string.Format("~{0}", path);
                
                //不存在建立
                if (Directory.Exists(HttpContext.Current.Server.MapPath(path)) == false)
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));
                }

                var upfileName = HttpContext.Current.Server.MapPath(string.Format("{0}/{1}", path.Replace("~", ""), fileName));
                
                //保存
                imgFile.SaveAs(upfileName);
                picUrl = string.Format("http://{0}{1}/{2}", HttpContext.Current.Request.Url.Authority, path.Replace("~", ""), fileName);
                
                return true;
            }
            catch (Exception ex)
            {
                BaseLog.SaveLog(ex.ToString(), "UpPic");
                picUrl = ex.Message;
                return false;
            }
        }
        #endregion
        
        #region 上传图片
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        public static bool UpPicAsync(HttpContextBase context,HttpPostedFileBase imgFile, string path, ref string picUrl)
        {
            try
            {
                var file = new FileInfo(imgFile.FileName);

                var FileSize = imgFile.ContentLength.ToString();

                //图片扩展名
                if (!CheckFileName(file.Extension))
                {
                    picUrl = "上传图片格式不正确！";
                    return false;
                }

                //生成将要保存的随机文件名
                var fileName = GetFileName() + file.Extension;

                path = string.Format("~{0}", path);

                //不存在建立
                if (Directory.Exists(context.Server.MapPath(path)) == false)
                {
                    Directory.CreateDirectory(context.Server.MapPath(path));
                }

                var upfileName = context.Server.MapPath(string.Format("{0}/{1}", path.Replace("~", ""), fileName));

                //保存
                imgFile.SaveAs(upfileName);
                picUrl = string.Format("http://{0}{1}/{2}", context.Request.Url.Authority, path.Replace("~", ""), fileName);

                return true;
            }
            catch (Exception ex)
            {
                BaseLog.SaveLog(ex.ToString(), "UpPic");
                picUrl = ex.Message;
                return false;
            }
        }
        #endregion 
    }
}
