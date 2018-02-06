using System;
using System.Collections.Generic;
using System.IO;
using NPOI.HPSF;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.UserModel;

namespace Untility.Base
{
    /// <summary>
    /// 导出 excel
    /// </summary>
    public static class BaseExcel
    {
        #region list导出到Excel文件
        /// <summary>
        /// list导出到Excel文件
        /// </summary>
        /// <param name="listContent">内容列表</param>
        /// <param name="Title">标题列表</param>
        /// <param name="headerText">表头文本</param>
        /// <param name="fileName">文件名</param>
        public static byte[] ToExcel<Content, Title>(List<Content> listContent, Title model, string headerText)
        {
            try
            {
                var hssfworkbook = new HSSFWorkbook();

                //InitializeWorkbook
                InitializeWorkbook(hssfworkbook);

                //GenerateData
                var sheet1 = hssfworkbook.CreateSheet(headerText);

                //写入总标题，合并居中
                var row = sheet1.CreateRow(0);
                var cell = row.CreateCell(0);
                cell.SetCellValue(headerText);

                var style = hssfworkbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.Center;
                var font = hssfworkbook.CreateFont();
                font.FontHeight = 20 * 20;
                style.SetFont(font);
                cell.CellStyle = style;
                sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, model.GetType().GetProperties().Length - 1));

                //插入列标题
                row = sheet1.CreateRow(1);
                var pInfo = model.GetType().GetProperties();
                int i = 0;

                if (model != null)
                {
                    foreach (var item in pInfo)
                    {
                        cell = row.CreateCell(i++);

                        cell.Row.Height = 420;
                        if (item.GetValue(model, null) == null)
                            cell.SetCellValue("");
                        else
                            cell.SetCellValue(item.GetValue(model, null).ToString());

                        cell.CellStyle = GetStyle(hssfworkbook, true);
                    }
                }

                //插入查询结果
                i = 0;
                if (listContent != null)
                {
                    foreach (var item in listContent)
                    {
                        pInfo = (item.GetType()).GetProperties();
                        row = sheet1.CreateRow(i + 2);
                        int j = 0;
                        foreach (var itemContent in pInfo)
                        {
                            cell = row.CreateCell(j++);
                            cell.Row.Height = 420;

                            if (itemContent.GetValue(item, null) == null)
                                cell.SetCellValue("");
                            else
                                cell.SetCellValue(itemContent.GetValue(item, null).ToString());

                            cell.CellStyle = GetStyle(hssfworkbook);
                        }

                        i++;
                    }
                }

                //自动列宽
                pInfo = model.GetType().GetProperties();
                i = 0;
                foreach (var item in pInfo)
                {
                    sheet1.AutoSizeColumn(i++, true);
                    sheet1.HorizontallyCenter = true;
                }

                var file = new MemoryStream();
                hssfworkbook.Write(file);

                return file.ToArray();
            }
            catch (Exception ex)
            {
                BaseLog.SaveLog(ex.ToString(), "ToExcel.exp");
                return new byte[1];
            }
        }
        #endregion

        #region excel工作区
        /// <summary>
        /// 初始化工作区
        /// </summary>
        private static void InitializeWorkbook(HSSFWorkbook hssfworkbook)
        {
            //信息
            var dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "";
            hssfworkbook.DocumentSummaryInformation = dsi;

            //工区名称
            var si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "";
            hssfworkbook.SummaryInformation = si;
        }
        #endregion

        #region 样式
        /// <summary>
        /// 样式
        /// </summary>
        /// <returns></returns>
        private static ICellStyle GetStyle(HSSFWorkbook hssfworkbook, bool IsHead = false)
        {
            var style = hssfworkbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;

            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;

            style.Indention = 0;

            if (IsHead)
                style.BorderTop = BorderStyle.Thin;

            return style;
        }
        #endregion
    }
}
