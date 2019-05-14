using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace WR.Utils
{
    public class NpoiHelper
    {
        /// <summary>
        /// Yield
        /// </summary>
        /// <param name="xlstpmname"></param>
        /// <param name="xlsname"></param>
        /// <param name="dtSrc"></param>
        /// <param name="dtSum"></param>
        public static void GridToExcelYield(string xlstpmname, string xlsname, DataTable dtSrc, DataTable dtSum)
        {
            FileStream file = new FileStream(xlstpmname, FileMode.Open, FileAccess.Read);
            HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);

            //create a entry of DocumentSummaryInformation
            NPOI.HPSF.DocumentSummaryInformation dsi = NPOI.HPSF.PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "SMEE";
            hssfworkbook.DocumentSummaryInformation = dsi;

            //create a entry of SummaryInformation
            NPOI.HPSF.SummaryInformation si = NPOI.HPSF.PropertySetFactory.CreateSummaryInformation();
            si.Subject = "Wafer Review System";
            hssfworkbook.SummaryInformation = si;

            ISheet sheet1 = hssfworkbook.GetSheet("yield");
            for (int i = 0; i < dtSrc.Columns.Count; i++)
            {
                //ICell cell = sheet1.GetRow(0).CreateCell(i);
                ICell cell = sheet1.GetRow(0).GetCell(i);
                cell.SetCellType(CellType.String);
                ICellStyle style = hssfworkbook.CreateCellStyle();
                style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Blue.Index2;
                style.FillPattern = FillPattern.NoFill;
                style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                IFont font = hssfworkbook.CreateFont();
                font.Boldweight = (short)FontBoldWeight.Bold;
                font.Color = (short)FontColor.Red;
                style.SetFont(font);
                cell.CellStyle = style;

                cell.SetCellValue(dtSrc.Columns[i].ColumnName);
                //sheet.AutoSizeColumn(i);
            }
            for (int i = 0, cnt = dtSrc.Rows.Count; i < cnt; i++)
            {
                if (dtSrc.Rows[i][0].ToString() == "")
                    continue;

                for (int j = 0; j < dtSrc.Columns.Count; j++)
                {
                    ICell cell = sheet1.GetRow(i + 1).GetCell(j);
                    object val = dtSrc.Rows[i][j];
                    if (val is string)
                        cell.SetCellValue(val.ToString());
                    else
                        cell.SetCellValue(double.Parse(val.ToString()));


                    if ((i + 1) == cnt)
                    {
                        ICellStyle style = hssfworkbook.CreateCellStyle();
                        style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Blue.Index2;
                        style.FillPattern = FillPattern.NoFill;
                        style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Right;
                        IFont font = hssfworkbook.CreateFont();
                        font.Boldweight = (short)FontBoldWeight.Bold;
                        font.FontHeightInPoints = 11;
                        font.FontName = cell.CellStyle.GetFont(hssfworkbook).FontName;
                        style.SetFont(font);
                        cell.CellStyle = style;
                    }
                }
            }

            ISheet sheet2 = hssfworkbook.GetSheet("report");
            //写入classify名称
            for (int j = 1; j <= dtSum.Rows.Count; j++)
            {
                object val = dtSum.Rows[j - 1][0];
                if (val.ToString() == "Inspected Die_Y2")
                    continue;

                ICell cell = sheet2.GetRow(j).GetCell(1);
                cell.SetCellValue(val.ToString());
            }
            //缺陷个数
            for (int j = 1; j <= dtSum.Rows.Count; j++)
            {
                object valv = dtSum.Rows[j - 1][0];
                if (valv.ToString() == "Inspected Die_Y2")
                    continue;

                ICell cell = sheet2.GetRow(j).GetCell(2);
                object val = dtSum.Rows[j - 1][1];
                cell.SetCellValue(double.Parse(val.ToString()));
            }
            //被测的die总数
            for (int j = 1; j <= dtSum.Rows.Count; j++)
            {
                object valv = dtSum.Rows[j - 1][0];
                if (valv.ToString() == "Inspected Die_Y2")
                    continue;

                ICell cell = sheet2.GetRow(j).GetCell(6);
                object val = dtSum.Rows[j - 1][2];
                cell.SetCellValue(double.Parse(val.ToString()));
            }
            //所有缺陷的总和
            for (int j = 1; j <= dtSum.Rows.Count; j++)
            {
                object valv = dtSum.Rows[j - 1][0];
                if (valv.ToString() == "Inspected Die_Y2")
                    continue;

                ICell cell = sheet2.GetRow(j).GetCell(7);
                object val = dtSum.Rows[j - 1][3];
                cell.SetCellValue(double.Parse(val.ToString()));
            }

            if (dtSum.Rows.Count < 70)
            {
                for (int i = dtSum.Rows.Count; i < 70 - dtSum.Rows.Count; i++)
                {
                    ICell cell2 = sheet2.GetRow(i).GetCell(3);
                    if (cell2 != null)
                        sheet2.GetRow(i).RemoveCell(cell2);
                    ICell cell3 = sheet2.GetRow(i).GetCell(4);
                    if (cell3 != null)
                        sheet2.GetRow(i).RemoveCell(cell3);
                    ICell cell4 = sheet2.GetRow(i).GetCell(5);
                    if (cell4 != null)
                        sheet2.GetRow(i).RemoveCell(cell4);
                }
            }

            sheet2.ForceFormulaRecalculation = true;

            //写入数据
            FileStream file2 = new FileStream(xlsname, FileMode.Create);
            hssfworkbook.Write(file2);
            file2.Close();

            //删除模板
            try
            {
                file.Dispose();
                File.Delete(xlstpmname);
            }
            catch { }
        }

        public static void GridToExcelPolat(string xlstpmname, string xlsname, DataTable dtSrc, string img)
        {
            FileStream file = new FileStream(xlstpmname, FileMode.Open, FileAccess.Read);
            HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);

            //create a entry of DocumentSummaryInformation
            NPOI.HPSF.DocumentSummaryInformation dsi = NPOI.HPSF.PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "SMEE";
            hssfworkbook.DocumentSummaryInformation = dsi;

            //create a entry of SummaryInformation
            NPOI.HPSF.SummaryInformation si = NPOI.HPSF.PropertySetFactory.CreateSummaryInformation();
            si.Subject = "Wafer Review System";
            hssfworkbook.SummaryInformation = si;

            ISheet sheet1 = hssfworkbook.GetSheet("Sheet1");

            HSSFPatriarch patriarch = (HSSFPatriarch)sheet1.CreateDrawingPatriarch();

            HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 255, 0, 15, 8, 50);
            anchor.AnchorType = AnchorType.MoveAndResize;

            FileStream fileI = new FileStream(img, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fileI.Length];
            fileI.Read(buffer, 0, (int)fileI.Length);

            int hs = hssfworkbook.AddPicture(buffer, PictureType.JPEG);
            HSSFPicture picture = (HSSFPicture)patriarch.CreatePicture(anchor, hs);

            picture.Resize();
            picture.LineStyle = LineStyle.DotSys;

            for (int i = 0; i < dtSrc.Columns.Count; i++)
            {
                //ICell cell = sheet1.GetRow(0).CreateCell(i);
                ICell cell = sheet1.GetRow(0).GetCell(i);
                cell.SetCellType(CellType.String);
                ICellStyle style = hssfworkbook.CreateCellStyle();
                style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Blue.Index2;
                style.FillPattern = FillPattern.NoFill;
                style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                IFont font = hssfworkbook.CreateFont();
                font.Boldweight = (short)FontBoldWeight.Bold;
                font.Color = (short)FontColor.Red;
                style.SetFont(font);
                cell.CellStyle = style;

                cell.SetCellValue(dtSrc.Columns[i].ColumnName);
                //sheet.AutoSizeColumn(i);
            }
            for (int i = 0, cnt = dtSrc.Rows.Count; i < cnt; i++)
            {
                if (dtSrc.Rows[i][0].ToString() == "")
                    continue;

                for (int j = 0; j < dtSrc.Columns.Count; j++)
                {
                    ICell cell = sheet1.GetRow(i + 1).GetCell(j);
                    object val = dtSrc.Rows[i][j];
                    if (val is string)
                        cell.SetCellValue(val.ToString());
                    else
                        cell.SetCellValue(double.Parse(val.ToString()));
                }
            }
            sheet1.ForceFormulaRecalculation = true;

            //写入数据
            FileStream file2 = new FileStream(xlsname, FileMode.Create);
            hssfworkbook.Write(file2);
            file2.Close();

            //删除模板
            try
            {
                file.Dispose();
                File.Delete(xlstpmname);
                File.Delete(img);
            }
            catch { }
        }

        public static void GridToExcelLotYield(string sheetname, string lotid, string date,
            string[] summ1, string[] summ2, string[] summ3, DataGridView gv, string strExcelFileName, bool summflag, string[] summ4 = null, DataGridView gv1 = null)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            try
            {
                var rIndex = 0;

                ISheet sheet = workbook.CreateSheet(sheetname);

                //汇总栏
                ICellStyle SummStyle = workbook.CreateCellStyle();
                SummStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                SummStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                //字体
                NPOI.SS.UserModel.IFont summfont = workbook.CreateFont();
                summfont.Boldweight = (short)FontBoldWeight.Bold;
                summfont.Color = NPOI.HSSF.Util.HSSFColor.DarkBlue.Index;
                SummStyle.SetFont(summfont);
                SummStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.DarkBlue.Index;
                SummStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Lavender.Index;
                //Lot ID
                IRow summRow_1 = sheet.CreateRow(0);
                summRow_1.Height = 20 * 20;

                if (summ4 == null)
                {
                    ICell cell_1 = summRow_1.CreateCell(0);
                    cell_1.SetCellValue("Lot ID:");
                    cell_1.CellStyle = SummStyle;
                    ICell cell_2 = summRow_1.CreateCell(1);
                    cell_2.SetCellValue(lotid);
                    cell_2.CellStyle = SummStyle;
                    ICell cell_3 = summRow_1.CreateCell(2);
                    cell_3.SetCellValue("Date:");
                    cell_3.CellStyle = SummStyle;
                    ICell cell_4 = summRow_1.CreateCell(3);
                    cell_4.SetCellValue(date);
                    cell_4.CellStyle = SummStyle;
                }

                if (summflag)
                {
                    //汇总
                    IRow summRow_2 = sheet.CreateRow(1);
                    summRow_2.Height = 20 * 20;
                    ICell cell_21 = summRow_2.CreateCell(0);
                    cell_21.SetCellValue(summ1[0]);
                    cell_21.CellStyle = SummStyle;
                    ICell cell_22 = summRow_2.CreateCell(1);
                    cell_22.SetCellValue(summ1[1]);
                    cell_22.CellStyle = SummStyle;
                    ICell cell_23 = summRow_2.CreateCell(2);
                    cell_23.SetCellValue(summ1[2]);
                    cell_23.CellStyle = SummStyle;
                    ICell cell_24 = summRow_2.CreateCell(3);
                    cell_24.SetCellValue(summ1[3]);
                    cell_24.CellStyle = SummStyle;

                    ICell cell_25 = summRow_2.CreateCell(4);
                    cell_25.SetCellValue("Create Date:");
                    cell_25.CellStyle = SummStyle;
                    ICell cell_26 = summRow_2.CreateCell(5);
                    cell_26.SetCellValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cell_26.CellStyle = SummStyle;


                    IRow summRow_3 = sheet.CreateRow(2);
                    summRow_3.Height = 20 * 20;
                    ICell cell_31 = summRow_3.CreateCell(0);
                    cell_31.SetCellValue(summ2[0]);
                    cell_31.CellStyle = SummStyle;
                    ICell cell_32 = summRow_3.CreateCell(1);
                    cell_32.SetCellValue(summ2[1]);
                    cell_32.CellStyle = SummStyle;
                    ICell cell_33 = summRow_3.CreateCell(2);
                    cell_33.SetCellValue(summ2[2]);
                    cell_33.CellStyle = SummStyle;
                    ICell cell_34 = summRow_3.CreateCell(3);
                    cell_34.SetCellValue(summ2[3]);
                    cell_34.CellStyle = SummStyle;

                    IRow summRow_4 = sheet.CreateRow(3);
                    summRow_4.Height = 20 * 20;
                    ICell cell_41 = summRow_4.CreateCell(0);
                    cell_41.SetCellValue(summ3[0]);
                    cell_41.CellStyle = SummStyle;
                    ICell cell_42 = summRow_4.CreateCell(1);
                    cell_42.SetCellValue(summ3[1]);
                    cell_42.CellStyle = SummStyle;
                    if (summ3.Length > 2)
                    {
                        ICell cell_43 = summRow_4.CreateCell(2);
                        cell_43.SetCellValue(summ3[2]);
                        cell_43.CellStyle = SummStyle;
                        ICell cell_44 = summRow_4.CreateCell(3);
                        cell_44.SetCellValue(summ3[3]);
                        cell_44.CellStyle = SummStyle;
                    }

                    if (summ4 != null)
                    {
                        IRow r = sheet.CreateRow(4);
                        r.Height = 20 * 20;

                        IRow r4 = null;

                        for (int i = 0; i < summ4.Length; i++)
                        {
                            var cIndex = 1;

                            if (i % 2 == 0)
                            {
                                r4 = sheet.CreateRow(5 + rIndex);
                                r4.Height = 20 * 20;

                                cIndex = 0;

                                rIndex++;
                            }

                            ICell c = r4.CreateCell(cIndex);
                            c.SetCellValue(summ4[i]);
                            c.CellStyle = SummStyle;
                        }
                        rIndex++;
                    }
                }

                //表格头
                ICellStyle HeadercellStyle = workbook.CreateCellStyle();
                HeadercellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                HeadercellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                HeadercellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                HeadercellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                HeadercellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                HeadercellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                //字体
                NPOI.SS.UserModel.IFont headerfont = workbook.CreateFont();
                headerfont.Boldweight = (short)FontBoldWeight.Bold;
                HeadercellStyle.SetFont(headerfont);
                //HeadercellStyle.FillBackgroundColorColor
                HeadercellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                HeadercellStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Lavender.Index;

                var waferId = string.Empty;
                int tDefects = 0, badDie = 0;
                var yieldLoss = 0.00;

                if (gv1 != null)
                {
                    int icolIndex1 = 0;
                    int ridx1 = (summflag ? 4 + rIndex : 1 + rIndex);

                    IRow r = sheet.CreateRow(ridx1);
                    r.Height = 20 * 20; rIndex++; ridx1++;

                    IRow headerRow1 = sheet.CreateRow(ridx1); //rIndex++;
                    headerRow1.Height = 20 * 20;
                    foreach (DataGridViewColumn item in gv1.Columns)
                    {
                        if (!item.Visible)
                            continue;

                        ICell cell = headerRow1.CreateCell(icolIndex1);
                        sheet.SetColumnWidth(icolIndex1, item.Width * 50);
                        cell.SetCellValue(item.HeaderText);
                        cell.CellStyle = HeadercellStyle;
                        icolIndex1++;
                    }

                    ICellStyle cellStyle1 = workbook.CreateCellStyle();

                    //为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
                    cellStyle1.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
                    cellStyle1.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    cellStyle1.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    cellStyle1.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    cellStyle1.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                    NPOI.SS.UserModel.IFont cellfont1 = workbook.CreateFont();
                    cellfont1.Boldweight = (short)FontBoldWeight.Normal;
                    cellStyle1.SetFont(cellfont1);

                    //建立内容行
                    int iRowIndex1 = (summflag ? 5 + rIndex : 2 + rIndex);
                    int iCellIndex1 = 0;
                    if (gv1.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow Rowitem in gv1.Rows)
                        {
                            IRow DataRow = sheet.CreateRow(iRowIndex1);
                            foreach (DataGridViewColumn Colitem in gv1.Columns)
                            {
                                if (Colitem.Visible)
                                {
                                    ICell cell = DataRow.CreateCell(iCellIndex1);
                                    cell.SetCellValue(Rowitem.Cells[Colitem.Name].FormattedValue.ToString());
                                    cell.CellStyle = cellStyle1;
                                    iCellIndex1++;

                                    if (Colitem.Name == "SumYieldLoss")
                                        yieldLoss += Convert.ToDouble(Rowitem.Cells[Colitem.Name].Value);
                                }
                            }
                            iCellIndex1 = 0;
                            iRowIndex1++;
                            rIndex++;
                        }

                        IRow DataRows = sheet.CreateRow(iRowIndex1);
                        foreach (DataGridViewColumn Colitem in gv1.Columns)
                        {
                            if (Colitem.Visible)
                            {
                                ICell cell = DataRows.CreateCell(iCellIndex1);

                                if (Colitem.Name == "SumYieldLoss")
                                    cell.SetCellValue(string.Format("{0}/{1}", yieldLoss, 100 - yieldLoss));

                                cell.CellStyle = cellStyle1;
                                iCellIndex1++;
                            }
                        }

                        iCellIndex1 = 0;
                        iRowIndex1++;
                        rIndex++;
                    }
                    rIndex++;
                }

                //用column name 作为列名
                int icolIndex = 0;
                int ridx = (summflag ? 4 + rIndex : 1 + rIndex);

                IRow r2 = sheet.CreateRow(ridx);
                r2.Height = 20 * 20; rIndex++; ridx++;

                IRow headerRow = sheet.CreateRow(ridx); //rIndex++;
                headerRow.Height = 20 * 20;
                foreach (DataGridViewColumn item in gv.Columns)
                {
                    if (!item.Visible)
                        continue;

                    ICell cell = headerRow.CreateCell(icolIndex);
                    sheet.SetColumnWidth(icolIndex, item.Width * 50);
                    cell.SetCellValue(item.HeaderText);
                    cell.CellStyle = HeadercellStyle;
                    icolIndex++;
                }

                ICellStyle cellStyle = workbook.CreateCellStyle();

                //为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
                cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
                cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                NPOI.SS.UserModel.IFont cellfont = workbook.CreateFont();
                cellfont.Boldweight = (short)FontBoldWeight.Normal;
                cellStyle.SetFont(cellfont);

                ICellStyle sumStyle = workbook.CreateCellStyle();

                //为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
                sumStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
                sumStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                sumStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                sumStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                sumStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                NPOI.SS.UserModel.IFont sumfont = workbook.CreateFont();
                sumfont.Boldweight = (short)FontBoldWeight.Bold;
                sumStyle.SetFont(sumfont);


                //建立内容行
                int iRowIndex = (summflag ? 5 + rIndex : 2 + rIndex);
                int iCellIndex = 0;
                int rcnt = 0;

                yieldLoss = 0;

                if (gv.Rows.Count > 0)
                {
                    foreach (DataGridViewRow Rowitem in gv.Rows)
                    {
                        if (!string.IsNullOrEmpty(waferId) && waferId != Rowitem.Cells["WaferId"].FormattedValue.ToString())
                        {
                            IRow DataRows = sheet.CreateRow(iRowIndex);
                            foreach (DataGridViewColumn Colitem in gv.Columns)
                            {
                                if (Colitem.Visible)
                                {
                                    ICell cell = DataRows.CreateCell(iCellIndex);

                                    if (Colitem.Name != "Code" && Colitem.Name != "Category" && Colitem.Name != "Pareto")
                                    {
                                        if (Colitem.Name == "TotalDefects")
                                            cell.SetCellValue(tDefects);
                                        else if (Colitem.Name == "DieQuantity")
                                            cell.SetCellValue(badDie);
                                        else if (Colitem.Name == "YieldLoss")
                                            cell.SetCellValue(string.Format("{0}/{1}", yieldLoss, 100 - yieldLoss));
                                        else if (Colitem.Name == "WaferId")
                                            cell.SetCellValue(waferId);
                                        else
                                            cell.SetCellValue(Rowitem.Cells[Colitem.Name].FormattedValue.ToString());
                                    }

                                    cell.CellStyle = sumStyle;
                                    iCellIndex++;
                                }
                            }

                            tDefects = 0;
                            badDie = 0;
                            yieldLoss = 0;

                            iCellIndex = 0;
                            iRowIndex++;
                            rIndex++;

                            sheet.CreateRow(iRowIndex);

                            iRowIndex++;
                            rIndex++;
                        }


                        IRow DataRow = sheet.CreateRow(iRowIndex);
                        foreach (DataGridViewColumn Colitem in gv.Columns)
                        {
                            if (Colitem.Visible)
                            {
                                ICell cell = DataRow.CreateCell(iCellIndex);
                                cell.SetCellValue(Rowitem.Cells[Colitem.Name].FormattedValue.ToString());
                                cell.CellStyle = cellStyle;
                                iCellIndex++;

                                if (Colitem.Name == "TotalDefects")
                                    tDefects += Convert.ToInt16(Rowitem.Cells[Colitem.Name].Value);
                                else if (Colitem.Name == "DieQuantity")
                                    badDie += Convert.ToInt16(Rowitem.Cells[Colitem.Name].Value);
                                else if (Colitem.Name == "YieldLoss")
                                    yieldLoss += Convert.ToDouble(Rowitem.Cells[Colitem.Name].Value);
                            }
                        }

                        iCellIndex = 0;
                        iRowIndex++;
                        rIndex++;
                        rcnt++;

                        waferId = Rowitem.Cells["WaferId"].FormattedValue.ToString();

                        if (!string.IsNullOrEmpty(waferId) && rcnt == gv.Rows.Count)
                        {
                            IRow DataRows = sheet.CreateRow(iRowIndex);
                            foreach (DataGridViewColumn Colitem in gv.Columns)
                            {
                                if (Colitem.Visible)
                                {
                                    ICell cell = DataRows.CreateCell(iCellIndex);

                                    if (Colitem.Name != "Code" && Colitem.Name != "Category" && Colitem.Name != "Pareto")
                                    {
                                        if (Colitem.Name == "TotalDefects")
                                            cell.SetCellValue(tDefects);
                                        else if (Colitem.Name == "DieQuantity")
                                            cell.SetCellValue(badDie);
                                        else if (Colitem.Name == "YieldLoss")
                                            cell.SetCellValue(string.Format("{0}/{1}", yieldLoss, 100 - yieldLoss));
                                        else if (Colitem.Name == "WaferId")
                                            cell.SetCellValue(waferId);
                                        else
                                            cell.SetCellValue(Rowitem.Cells[Colitem.Name].FormattedValue.ToString());
                                    }

                                    cell.CellStyle = sumStyle;
                                    iCellIndex++;
                                }
                            }

                            tDefects = 0;
                            badDie = 0;
                            yieldLoss = 0;

                            iCellIndex = 0;
                            iRowIndex++;
                            rIndex++;

                            IRow drSum = sheet.CreateRow(iRowIndex);
                            drSum.Height = 20 * 20;

                            iRowIndex++;
                            rIndex++;
                        }
                    }

                    ////自适应列宽度
                    //for (int i = 0; i < icolIndex; i++)
                    //{
                    //    sheet.AutoSizeColumn(i);
                    //}
                }

                //写Excel
                FileStream file = new FileStream(strExcelFileName, FileMode.OpenOrCreate);
                workbook.Write(file);
                file.Flush();
                file.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { workbook = null; }
        }

        public static void GridToExcelByNPOI(string sheetname, string lotid, string date,
           string[] summ1, string[] summ2, string[] summ3, DataGridView gv, string strExcelFileName, bool summflag)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            try
            {
                ISheet sheet = workbook.CreateSheet(sheetname);

                //汇总栏
                ICellStyle SummStyle = workbook.CreateCellStyle();
                SummStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                SummStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                //字体
                NPOI.SS.UserModel.IFont summfont = workbook.CreateFont();
                summfont.Boldweight = (short)FontBoldWeight.Bold;
                summfont.Color = NPOI.HSSF.Util.HSSFColor.DarkBlue.Index;
                SummStyle.SetFont(summfont);
                SummStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.DarkBlue.Index;
                SummStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Lavender.Index;
                //Lot ID
                IRow summRow_1 = sheet.CreateRow(0);
                summRow_1.Height = 20 * 20;
                ICell cell_1 = summRow_1.CreateCell(0);
                cell_1.SetCellValue("Lot ID:");
                cell_1.CellStyle = SummStyle;
                ICell cell_2 = summRow_1.CreateCell(1);
                cell_2.SetCellValue(lotid);
                cell_2.CellStyle = SummStyle;
                ICell cell_3 = summRow_1.CreateCell(2);
                cell_3.SetCellValue("Date:");
                cell_3.CellStyle = SummStyle;
                ICell cell_4 = summRow_1.CreateCell(3);
                cell_4.SetCellValue(date);
                cell_4.CellStyle = SummStyle;

                ICell cell_5 = summRow_1.CreateCell(4);
                cell_5.SetCellValue("Create Date:");
                cell_5.CellStyle = SummStyle;
                ICell cell_6 = summRow_1.CreateCell(5);
                cell_6.SetCellValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cell_6.CellStyle = SummStyle;

                if (summflag)
                {
                    //汇总
                    IRow summRow_2 = sheet.CreateRow(1);
                    summRow_2.Height = 20 * 20;
                    ICell cell_21 = summRow_2.CreateCell(0);
                    cell_21.SetCellValue(summ1[0]);
                    cell_21.CellStyle = SummStyle;
                    ICell cell_22 = summRow_2.CreateCell(1);
                    cell_22.SetCellValue(summ1[1]);
                    cell_22.CellStyle = SummStyle;
                    ICell cell_23 = summRow_2.CreateCell(2);
                    cell_23.SetCellValue(summ1[2]);
                    cell_23.CellStyle = SummStyle;
                    ICell cell_24 = summRow_2.CreateCell(3);
                    cell_24.SetCellValue(summ1[3]);
                    cell_24.CellStyle = SummStyle;

                    IRow summRow_3 = sheet.CreateRow(2);
                    summRow_3.Height = 20 * 20;
                    ICell cell_31 = summRow_3.CreateCell(0);
                    cell_31.SetCellValue(summ2[0]);
                    cell_31.CellStyle = SummStyle;
                    ICell cell_32 = summRow_3.CreateCell(1);
                    cell_32.SetCellValue(summ2[1]);
                    cell_32.CellStyle = SummStyle;
                    ICell cell_33 = summRow_3.CreateCell(2);
                    cell_33.SetCellValue(summ2[2]);
                    cell_33.CellStyle = SummStyle;
                    ICell cell_34 = summRow_3.CreateCell(3);
                    cell_34.SetCellValue(summ2[3]);
                    cell_34.CellStyle = SummStyle;

                    IRow summRow_4 = sheet.CreateRow(3);
                    summRow_4.Height = 20 * 20;
                    ICell cell_41 = summRow_4.CreateCell(0);
                    cell_41.SetCellValue(summ3[0]);
                    cell_41.CellStyle = SummStyle;
                    ICell cell_42 = summRow_4.CreateCell(1);
                    cell_42.SetCellValue(summ3[1]);
                    cell_42.CellStyle = SummStyle;
                    if (summ3.Length > 2)
                    {
                        ICell cell_43 = summRow_4.CreateCell(2);
                        cell_43.SetCellValue(summ3[2]);
                        cell_43.CellStyle = SummStyle;
                        ICell cell_44 = summRow_4.CreateCell(3);
                        cell_44.SetCellValue(summ3[3]);
                        cell_44.CellStyle = SummStyle;
                    }
                }

                //表格头
                ICellStyle HeadercellStyle = workbook.CreateCellStyle();
                HeadercellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                HeadercellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                HeadercellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                HeadercellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                HeadercellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                HeadercellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                //字体
                NPOI.SS.UserModel.IFont headerfont = workbook.CreateFont();
                headerfont.Boldweight = (short)FontBoldWeight.Bold;
                HeadercellStyle.SetFont(headerfont);
                //HeadercellStyle.FillBackgroundColorColor
                HeadercellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                HeadercellStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Lavender.Index;

                //用column name 作为列名
                int icolIndex = 0;
                int ridx = (summflag ? 4 : 1);
                IRow headerRow = sheet.CreateRow(ridx);
                headerRow.Height = 20 * 20;
                foreach (DataGridViewColumn item in gv.Columns)
                {
                    if (!item.Visible)
                        continue;

                    ICell cell = headerRow.CreateCell(icolIndex);
                    sheet.SetColumnWidth(icolIndex, item.Width * 50);
                    cell.SetCellValue(item.HeaderText);
                    cell.CellStyle = HeadercellStyle;
                    icolIndex++;
                }

                ICellStyle cellStyle = workbook.CreateCellStyle();

                //为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
                cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
                cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                NPOI.SS.UserModel.IFont cellfont = workbook.CreateFont();
                cellfont.Boldweight = (short)FontBoldWeight.Normal;
                cellStyle.SetFont(cellfont);

                //建立内容行
                int iRowIndex = (summflag ? 5 : 2);
                int iCellIndex = 0;
                if (gv.Rows.Count > 0)
                {
                    foreach (DataGridViewRow Rowitem in gv.Rows)
                    {
                        IRow DataRow = sheet.CreateRow(iRowIndex);
                        foreach (DataGridViewColumn Colitem in gv.Columns)
                        {
                            if (Colitem.Visible)
                            {
                                ICell cell = DataRow.CreateCell(iCellIndex);
                                cell.SetCellValue(Rowitem.Cells[Colitem.Name].FormattedValue.ToString());
                                cell.CellStyle = cellStyle;
                                iCellIndex++;
                            }
                        }
                        iCellIndex = 0;
                        iRowIndex++;
                    }

                    ////自适应列宽度
                    //for (int i = 0; i < icolIndex; i++)
                    //{
                    //    sheet.AutoSizeColumn(i);
                    //}
                }

                //写Excel
                FileStream file = new FileStream(strExcelFileName, FileMode.OpenOrCreate);
                workbook.Write(file);
                file.Flush();
                file.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { workbook = null; }
        }

        /// <summary>
        /// Excel文件导成Datatable
        /// </summary>
        /// <param name="strFilePath">Excel文件目录地址</param>
        /// <param name="strTableName">Datatable表名</param>
        /// <param name="iSheetIndex">Excel sheet index</param>
        /// <returns></returns>
        public static DataTable XlSToDataTable(string strFilePath, string strTableName, int iSheetIndex)
        {

            string strExtName = Path.GetExtension(strFilePath);

            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(strTableName))
            {
                dt.TableName = strTableName;
            }

            if (strExtName.Equals(".xls") || strExtName.Equals(".xlsx"))
            {
                using (FileStream file = new FileStream(strFilePath, FileMode.Open, FileAccess.Read))
                {
                    HSSFWorkbook workbook = new HSSFWorkbook(file);
                    ISheet sheet = workbook.GetSheetAt(iSheetIndex);

                    //列头
                    foreach (ICell item in sheet.GetRow(sheet.FirstRowNum).Cells)
                    {
                        dt.Columns.Add(item.ToString(), typeof(string));
                    }

                    //写入内容
                    System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                    while (rows.MoveNext())
                    {
                        IRow row = (HSSFRow)rows.Current;
                        if (row.RowNum == sheet.FirstRowNum)
                        {
                            continue;
                        }

                        DataRow dr = dt.NewRow();
                        foreach (ICell item in row.Cells)
                        {
                            switch (item.CellType)
                            {
                                case CellType.Boolean:
                                    dr[item.ColumnIndex] = item.BooleanCellValue;
                                    break;
                                case CellType.Error:
                                    //dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                    break;
                                case CellType.Formula:
                                    switch (item.CachedFormulaResultType)
                                    {
                                        case CellType.Boolean:
                                            dr[item.ColumnIndex] = item.BooleanCellValue;
                                            break;
                                        case CellType.Error:
                                            //dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                            break;
                                        case CellType.Numeric:
                                            if (DateUtil.IsCellDateFormatted(item))
                                            {
                                                dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                            }
                                            else
                                            {
                                                dr[item.ColumnIndex] = item.NumericCellValue;
                                            }
                                            break;
                                        case CellType.String:
                                            string str = item.StringCellValue;
                                            if (!string.IsNullOrEmpty(str))
                                            {
                                                dr[item.ColumnIndex] = str.ToString();
                                            }
                                            else
                                            {
                                                dr[item.ColumnIndex] = null;
                                            }
                                            break;
                                        case CellType.Unknown:
                                        case CellType.Blank:
                                        default:
                                            dr[item.ColumnIndex] = string.Empty;
                                            break;
                                    }
                                    break;
                                case CellType.Numeric:
                                    if (DateUtil.IsCellDateFormatted(item))
                                    {
                                        dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                    }
                                    else
                                    {
                                        dr[item.ColumnIndex] = item.NumericCellValue;
                                    }
                                    break;
                                case CellType.String:
                                    string strValue = item.StringCellValue;
                                    if (string.IsNullOrEmpty(strValue))
                                    {
                                        dr[item.ColumnIndex] = strValue.ToString();
                                    }
                                    else
                                    {
                                        dr[item.ColumnIndex] = null;
                                    }
                                    break;
                                case CellType.Unknown:
                                case CellType.Blank:
                                default:
                                    dr[item.ColumnIndex] = string.Empty;
                                    break;
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }

            return dt;
        }
    }
}
