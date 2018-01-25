using OfficeOpenXml;
using OfficeOpenXml.ConditionalFormatting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace ExcelReport
{
    public class Report
    {
        private List<string> _WorksheetsFiles;

        public Report()
        {
            _WorksheetsFiles = new List<string>();
        }

        public string FileName { get; set; }
        public ReportData Data { get; set; }

        public void AddWorksheet(string XmlForWorksheetsFile)
        {
            _WorksheetsFiles.Add(XmlForWorksheetsFile);
        }

        public bool CreateReport()
        {
            if (string.IsNullOrWhiteSpace(FileName)) return false;
            if (_WorksheetsFiles.Count <= 0) return false;
            using (var p = new ExcelPackage())
            {
                foreach (var wf in _WorksheetsFiles)
                {
                    ReportStructure rs = DeSerializeXML(wf);
                    var ws = p.Workbook.Worksheets.Add(rs.Name);
                    foreach (var header in rs.Header.Rows)
                    {
                        ws.Cells[header.Caption.Cell].Value = header.Caption.Text;
                        if (header.Values != null)
                        {
                            string NextLetter = "";
                            foreach (var val in header.Values.Items)
                            {
                                if (val.IsPrevDays) { NextLetter = BuildPrevRow(ws, val); }
                                else
                                {
                                    string cell = val.Cell;
                                    if (NextLetter != "") { cell = cell.Replace("{#}", NextLetter); }
                                    if (val.IsFormula == true)
                                    {
                                        ws.Cells[cell].Formula = val.Text;
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrWhiteSpace(val.DateFormat))
                                        {
                                            DateTime dtVal;
                                            if (DateTime.TryParse(GetValue(Data, val.Data), out dtVal)) ws.Cells[cell].Value = dtVal;
                                        }
                                        else
                                        {
                                            ws.Cells[cell].Value = GetValue(Data, val.Data); // val.Text;
                                        }
                                    }
                                }
                            }
                        }
                        SetStyle(ws.Cells[header.Cell], header.Style);
                    }
                    ws.OutLineSummaryBelow = false;
                    foreach (var row in rs.Table.Rows)
                    {
                        int level;
                        string NextLetter = "";
                        ws.Cells[row.Caption.Cell].Value = row.Caption.Text;
                        if (row.Values != null)
                        {
                            foreach (var val in row.Values.Items)
                            {
                                if (val.IsPrevDays) { NextLetter = BuildPrevRow(ws, val); }
                                else
                                {
                                    string cell = val.Cell;
                                    if (NextLetter != "") { cell = cell.Replace("{#}", NextLetter); }
                                    if (!string.IsNullOrWhiteSpace(val.DateFormat))
                                    {
                                        ws.Cells[cell].Style.Numberformat.Format = val.DateFormat;
                                    }
                                    else
                                    {
                                        ws.Cells[cell].Style.Numberformat.Format = "0";
                                    }
                                    if (val.IsFormula == true)
                                    {
                                        ws.Cells[cell].Formula = val.Text;
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrWhiteSpace(val.DateFormat))
                                        {
                                            DateTime dtVal;
                                            if (DateTime.TryParse(GetValue(Data, val.Data), out dtVal)) ws.Cells[cell].Value = dtVal;
                                        }
                                        else
                                        {
                                            if (val.IsNotConvert) ws.Cells[cell].Value = val.Text;
                                            else
                                            {
                                                decimal dVal;
                                                if(decimal.TryParse(GetValue(Data, val.Data), out dVal)) ws.Cells[cell].Value = dVal;
                                            }
                                        }
                                        //ws.Cells[val.Cell].Value = Convert.ToDecimal(val.Text);
                                    }
                                }
                            }
                        }
                        level = SetStyle(ws.Cells[row.Cell], row.Style);
                        ws.Row(ws.Cells[row.Cell].Start.Row).OutlineLevel = level;
                        ws.Row(ws.Cells[row.Cell].Start.Row).Collapsed = true;
                    }
                    string tablecell = rs.Table.Cell;
                    tablecell = tablecell.Replace("{#}", Common.GetColumnLetter((Data.PrevDays.Count + 1).ToString()));
                    ws.Cells[tablecell].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    ws.Cells[tablecell].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    ws.Cells[tablecell].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    ws.Cells[tablecell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    //ws.Workbook.CalcMode = ExcelCalcMode.Automatic;
                    //ws.Cells[rs.Table.Cell].Calculate();
                    foreach (var nf in rs.NumberFormats)
                    {
                        ws.Cells[nf.Cell].Style.Numberformat.Format = nf.Text;
                    }
                    foreach (var cf in rs.CoditionalFormats)
                    {
                        SetCoditionalFormat(ws, new ExcelAddress(cf));
                    }
                    foreach (var wt in rs.WrapTexts)
                    {
                        ws.Cells[wt].Style.WrapText = true;
                    }
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    foreach (var merge in rs.Merges)
                    {
                        ws.Cells[merge].Merge = true;
                    }
                    foreach (var col in rs.HideColumns)
                    {
                        ws.Column(Int32.Parse(col)).Hidden = true;
                    }
                    //SetCoditionalFormat(ws);
                }
                p.Workbook.Calculate();
                FileName = Common.GetNextAvailableFilename(FileName);
                //p.Workbook.FullCalcOnLoad = true;
                p.SaveAs(new FileInfo(FileName));
            }
            return true;
        }

        private string BuildPrevRow(ExcelWorksheet ws, Value val)
        {
            int i = 1;
            foreach(var data in Data.PrevDays.OrderBy(d => d.Date))
            {
                string letter = Common.GetColumnLetter((i++).ToString());
                string cell = val.Cell.Replace("{#}", letter);
                if (!string.IsNullOrWhiteSpace(val.DateFormat))
                {
                    ws.Cells[cell].Style.Numberformat.Format = val.DateFormat;
                }
                else
                {
                    ws.Cells[cell].Style.Numberformat.Format = "0";
                }
                if (val.IsFormula == true)
                {
                    ws.Cells[cell].Formula = val.Text.Replace("{#}", letter);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(val.DateFormat))
                    {
                        DateTime dtVal;
                        if (DateTime.TryParse(GetValue(data, val.Data), out dtVal)) ws.Cells[cell].Value = dtVal;
                    }
                    else
                    {
                        Decimal dVal;
                        if (Decimal.TryParse(GetValue(data, val.Data), out dVal)) ws.Cells[cell].Value = dVal;
                    }
                    //ws.Cells[val.Cell].Value = Convert.ToDecimal(val.Text);
                }
            }
            return Common.GetColumnLetter((i).ToString());
        }

        private string GetValue(object Obj, string Name)
        {
            string result = "";
            int DotPosition = Name.IndexOf('.');
            if (DotPosition > 0)
            {
                string str = Name.Substring(DotPosition + 1);
                object objnext = Obj.GetType().GetProperty(Name.Substring(0, DotPosition)).GetValue(Obj);
                result = GetValue(objnext, str);
            }
            else
            {
                try
                {
                    result = (string)Obj.GetType().GetProperty(Name).GetValue(Obj);
                } catch
                {
                    result = "";
                }
            }
            return result;
        }

        private void SetCoditionalFormat(ExcelWorksheet ws, ExcelAddress addr)
        {
            //ExcelAddress addr = new ExcelAddress("Q4:Q76");
            var ic = ws.ConditionalFormatting.AddThreeIconSet(addr, eExcelconditionalFormatting3IconsSetType.Arrows);
            ic.Icon1.Type = eExcelConditionalFormattingValueObjectType.Percent;
            ic.Icon1.Value = 0;
            ic.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
            ic.Icon2.Value = 0;
            ic.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;
            ic.Icon3.Value = 0;
            var node = ic.Node.ChildNodes[0].ChildNodes[2];
            var attr = node.OwnerDocument.CreateAttribute("gte");
            attr.Value = "0";
            node.Attributes.Append(attr);
        }

        private ReportStructure DeSerializeXML(string wf)
        {
            ReportStructure rs = null;
            XmlSerializer serializer = new XmlSerializer(typeof(ReportStructure));
            StreamReader reader = new StreamReader(wf);
            rs = (ReportStructure)serializer.Deserialize(reader);
            reader.Close();
            return rs;
        }

        public void OpenFile()
        {
            Process.Start(FileName);
        }

        private void CreateStructure(ExcelWorksheet ws)
        {
            //ws.OutLineSummaryBelow = false;
            ws.Row(8).OutlineLevel = 1;
            ws.Row(8).Collapsed = true;
            ws.Row(9).OutlineLevel = 1;
            ws.Row(9).Collapsed = true;
            ws.Row(10).OutlineLevel = 1;
            ws.Row(10).Collapsed = true;
            //var tbl = ws.Cells["A5:Q32"].Table;
            //var pivotTable = ws.PivotTables.Add(ws.Cells["A5:Q32"], ws.Cells["A5:Q32"], "test");
        }

        private int SetStyle(ExcelRange er, string StyleName)
        {
            int level = 0;
            switch (StyleName)
            {
                case "Header1":
                    SetHeader1Style(er);
                    break;
                case "Header2":
                    SetHeader2Style(er);
                    break;
                case "Header3":
                    SetHeader3Style(er);
                    level = 1;
                    break;
                case "Header4":
                    SetHeader4Style(er);
                    level = 2;
                    break;
                case "Header5":
                    SetHeader5Style(er);
                    level = 3;
                    break;
                case "Header6":
                    SetHeader6Style(er);
                    level = 4;
                    break;
            }
            return level;
        }

        /// <summary>
        /// Стили для строк
        /// </summary>
        /// <param name="er">Диапозон ячеек</param>
        private void SetHeader1Style(ExcelRange er)
        {
            er.Style.Font.Size = 12;
            er.Style.Font.Bold = true;
            er.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            er.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            if (er.Start.Column != er.End.Column) er.Worksheet.Cells[er.Start.Row, er.Start.Column + 1, er.End.Row, er.End.Column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        }

        private void SetHeader2Style(ExcelRange er)
        {
            er.Style.Font.Size = 12;
            er.Style.Font.Bold = true;
            er.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            er.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
            if (er.Start.Column != er.End.Column) er.Worksheet.Cells[er.Start.Row, er.Start.Column + 1, er.End.Row, er.End.Column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        }

        private void SetHeader3Style(ExcelRange er)
        {
            er.Style.Font.Size = 11;
            er.Style.Font.Bold = true;
            er.Style.Font.Italic = true;
            er.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            er.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
            if (er.Start.Column != er.End.Column) er.Worksheet.Cells[er.Start.Row, er.Start.Column + 1, er.End.Row, er.End.Column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            er[er.Start.Row, er.Start.Column].Style.Indent = 2;
        }

        private void SetHeader4Style(ExcelRange er)
        {
            er.Style.Font.Size = 11;
            er.Style.Font.Bold = true;
            er.Style.Font.Italic = true;
            er.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            er.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
            if (er.Start.Column != er.End.Column) er.Worksheet.Cells[er.Start.Row, er.Start.Column + 1, er.End.Row, er.End.Column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            er[er.Start.Row, er.Start.Column].Style.Indent = 3;
        }

        private void SetHeader5Style(ExcelRange er)
        {
            er.Style.Font.Size = 10;
            er.Style.Font.Italic = true;
            er.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            er.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
            if (er.Start.Column != er.End.Column) er.Worksheet.Cells[er.Start.Row, er.Start.Column + 1, er.End.Row, er.End.Column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            er[er.Start.Row, er.Start.Column].Style.Indent = 5;
        }

        private void SetHeader6Style(ExcelRange er)
        {
            er.Style.Font.Size = 10;
            er.Style.Font.Italic = true;
            er.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            er.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
            if (er.Start.Column != er.End.Column) er.Worksheet.Cells[er.Start.Row, er.Start.Column + 1, er.End.Row, er.End.Column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            er[er.Start.Row, er.Start.Column].Style.Indent = 6;
        }
    }
}
