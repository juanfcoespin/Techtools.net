using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;

namespace TechTools.Excel
{
    public class ExcelUtils
    {
        private List<string> columnNames;
        private SLDocument excelDocument;
        private string fileName;
        public ExcelUtils(string fileName) {
            this.columnNames = new List<string>();
            this.fileName = fileName;
            excelDocument = new SLDocument();
        }

        public void EndEditAndSave() {

            excelDocument.SaveAs(fileName);
        }

        public void AddData(int row, int column, object data)
        {
            // en esta libreria los arreglos empiezan desde 1 no desde cero
            row++;
            column++;
            if(data.GetType()==typeof(string))
                excelDocument.SetCellValue(row, column, (string)data);
            if (data.GetType() == typeof(int))
                excelDocument.SetCellValue(row, column, (int)data);
            if (data.GetType() == typeof(double))
                excelDocument.SetCellValue(row, column, (double)data);
            if (data.GetType() == typeof(DateTime))
                excelDocument.SetCellValue(row, column, (DateTime)data);
            if (data.GetType() == typeof(bool))
                excelDocument.SetCellValue(row, column, (bool)data);
            if (data.GetType() == typeof(decimal))
                excelDocument.SetCellValue(row, column, (decimal)data);
            if (data.GetType() == typeof(float))
                excelDocument.SetCellValue(row, column, (float)data);
            if (data.GetType() == typeof(long))
                excelDocument.SetCellValue(row, column, (long)data);
            if (data.GetType() == typeof(short))
                excelDocument.SetCellValue(row, column, (short)data);
            if (data.GetType() == typeof(byte))
                excelDocument.SetCellValue(row, column, (byte)data);
        }

        public void AddColumn(string columnName) {
            columnNames.Add(columnName);
            excelDocument.SetCellValue(1, columnNames.Count, columnName);
        }
    }
}
