using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace TestTaskParse
{
    public class ExcelHelper : IDisposable
    {
        static List<string> NameCollum = new List<string>() { "Title", "Brand", "Id", "Feedbacks", "Price" };
        private Application _excel;
        private Workbook _workbook;
        private string _filePath;

        public ExcelHelper()
        {
            _excel = new Excel.Application();
        }

        public void Dispose()
        {
            try
            {
                _workbook.Close();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }
        }

        public bool Open(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    _workbook = _excel.Workbooks.Open(filePath);
                }
                else
                {
                    _workbook = _excel.Workbooks.Add();
                    _filePath = filePath;
                }

                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }
            return false;
        }

        public bool Set(Data productsList, Excel._Worksheet worksheet)
        {
            try
            {
                for (int x = 1; x < NameCollum.Count + 1; x++)
                {
                    worksheet.Cells[1, x] = NameCollum[x - 1];
                }

                for (int y = 1; y < productsList.products.Count + 1; y++)
                {

                    for (int i = 1; i < typeof(Product).GetProperties().Length + 1; i++)
                    {
                        var prop = typeof(Product).GetProperties()[i - 1].GetValue(productsList.products[y - 1]).ToString();
                        worksheet.Cells[y + 1, i] = prop;
                    }
                }

                Excel.Range usedrange = worksheet.UsedRange;
                usedrange.Columns.AutoFit();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        internal void Save()
        {
            if (!string.IsNullOrEmpty(_filePath))
            {
                _workbook.SaveAs(_filePath);
                _filePath=null;
            }
            else
            {
                _workbook.Save();
            }
        }

        internal void CreateWorkSheets(List<string> searchWords)
        {
            try
            {
                Excel._Worksheet _workSheet = null;
                foreach (Excel.Worksheet worksheet in _workbook.Worksheets)
                {
                    if (_workbook.Worksheets.Count == 1)
                    {
                        break;
                    }
                    worksheet.Delete();
                }

                for (int i = 1; i < searchWords.Count + 1; i++)
                {
                    if (i == 1)
                    {
                        _workSheet = (Excel.Worksheet)_workbook.ActiveSheet;
                        _workSheet.Name = searchWords[i - 1];
                        continue;
                    }
                    _workbook.Worksheets.Add();
                    _workSheet = (Excel.Worksheet)_workbook.ActiveSheet;
                    _workSheet.Name = searchWords[i - 1];

                }
                for (int q = 1; q < NameCollum.Count + 1; q++)
                {
                    _workSheet.Cells[1, q] = NameCollum[q - 1];
                }
                _workbook.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public  Excel._Worksheet GetWorksheetByName( string metadataName)
        {

            var name = char.ToUpper(metadataName[0]) + metadataName.Substring(1);
            Excel.Worksheet worksheet = (Excel.Worksheet)_workbook.Worksheets[$"{name}"];
            return worksheet;
        }
    }
}