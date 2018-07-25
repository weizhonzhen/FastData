using System.Text;

namespace Fast.Untility.Page
{
    /// <summary>
    /// 分页页码
    /// </summary>
    public static class PageList
    {
        /*
        #region 分页
        /// <summary>
        /// 标签：2015.7.13，魏中针
        /// 说明：分页
        /// </summary>
        /// <param name="PageSize">每页条数</param>
        /// <param name="CurrentPage">当前页数</param>
        /// <param name="PageTotal">总页数</param>
        /// <returns></returns>
        public static string GetPageNumbert(int pageSize, int currentPage, int recordTotal, out int pageCount, string UrlValue = "")
        {
            int num4 = 0;
            pageCount = 0;
            string url = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
           
            StringBuilder sb = new StringBuilder();

            if (recordTotal == 0)
                sb.Append("");
            else
            {
                //页数
                if ((recordTotal % pageSize) == 0)
                    num4 = recordTotal / pageSize;
                else
                    num4 = (recordTotal / pageSize) + 1;

                pageCount = num4;

                //当前页码不在范围内为1
                if (currentPage > num4 || currentPage < 1)
                    currentPage = 1;

                int num2 = currentPage - 2;
                int num3 = currentPage + 2;

                sb.Append("<div class=\"pagin\">");
                sb.Append("<div class=\"message\">");
                sb.Append(string.Format("共<i class=\"blue\">&nbsp;{0}&nbsp;</i>条记录，共<i class=\"blue\">&nbsp;{1}&nbsp;</i>页，每页<i class=\"blue\">&nbsp;{2}&nbsp;</i>条"
                                        , recordTotal, num4, pageSize));
                sb.Append("</div>");
                sb.Append("<ul class=\"paginList\">");

                if (num2 <= 0)
                    num2 = 1;

                num3 = 4 + num2;

                if (num3 > num4)
                {
                    num3 = num4;

                    if ((num3 - 4) > 0)
                        num2 = num3 - 4;
                }

                //上页
                if (currentPage != 1)
                    sb.AppendFormat("<li class=\"paginItem\" title=\"上一页\"><a href=\"{0}?page={1}{2}\" title=\"上一页\"><span class=\"pagepre\"></span></a></li>", url, currentPage - 1, UrlValue);
                else
                    sb.AppendFormat("<li class=\"paginItem\"><a href=\"javascript:;\" title=\"上一页\" title=\"上一页\"><span class=\"pagepre\"></span></a></li>");

                //页数字
                for (int num1 = num2; num1 <= num3; num1++)
                {
                    if (num1 == currentPage)
                        sb.AppendFormat("<li class=\"paginItem current\"><a href=\"javascript:;\">{0}</a>", num1);
                    else
                        sb.AppendFormat(" <li class=\"paginItem\"><a href=\"{1}?page={0}{2}\">{0}</a></li>", num1, url,UrlValue);
                }

                //下页
                if (currentPage != num4)
                    sb.AppendFormat("<li class=\"paginItem\"><a href=\"{1}?page={0}{2}\" title=\"下一页\"><span class=\"pagenxt\"></span></a></li>", currentPage + 1, url,UrlValue);
                else
                    sb.AppendFormat("<li class=\"paginItem\"><a href=\"javascript:;\" title=\"下一页\"><span class=\"pagenxt\"></span></a></li>");
                
                sb.Append("</ul>");
                sb.Append("</div>");
            }

            return sb.ToString();
        }
        #endregion
        */
        #region 分页
        /// <summary>
        /// 标签：2015.7.13，魏中针
        /// 说明：分页
        /// </summary>
        /// <param name="PageSize">每页条数</param>
        /// <param name="CurrentPage">当前页数</param>
        /// <param name="PageTotal">总页数</param>
        /// <returns></returns>
        public static string PageNumber(int pageSize, int currentPage, int recordTotal, string url)
        {
            var num4 = 0;

            var sb = new StringBuilder();

            if (recordTotal == 0)
                sb.Append("");
            else
            {
                //页数
                if ((recordTotal % pageSize) == 0)
                    num4 = recordTotal / pageSize;
                else
                    num4 = (recordTotal / pageSize) + 1;
                
                //当前页码不在范围内为1
                if (currentPage > num4 || currentPage < 1)
                    currentPage = 1;

                int num2 = currentPage - 3;
                int num3 = currentPage + 3;

                sb.Append("<ul class=\"pagination\">");

                if (num2 <= 0)
                    num2 = 1;

                num3 = 6 + num2;

                if (num3 > num4)
                {
                    num3 = num4;

                    if ((num3 - 6) > 0)
                        num2 = num3 - 6;
                }

                //上页
                if (currentPage != 1)
                    sb.AppendFormat("<li><a href=\"{0}\" class=\"page-pre\"></a></li>", string.Format(url, currentPage - 1));
                else
                    sb.AppendFormat("<li><a href=\"javascript:;\" class=\"page-pre\"></a></li>");

                //页数字
                for (int num1 = num2; num1 <= num3; num1++)
                {
                    if (num1 == currentPage)
                        sb.AppendFormat("<li class=\"disabled\"><a href=\"javascript:;\">{0}</a>", currentPage);
                    else
                        sb.AppendFormat("<li class=\"active\"><a href=\"{0}\">{1}</a>", string.Format(url, num1), num1);
                }                

                //下页
                if (currentPage != num4)
                    sb.AppendFormat("<li><a href=\"{0}\" class=\"page-next\"></a></li>", string.Format(url, currentPage + 1));
                else
                    sb.AppendFormat("<li><a href=\"javascript:;\" class=\"page-next\"></a></li>");

                sb.Append("</ul>");
            }

            return sb.ToString();
        }
        #endregion
    }
}
