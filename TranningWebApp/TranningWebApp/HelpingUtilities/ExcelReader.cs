using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Excel; 

namespace TmsWebApp.HelpingUtilities
{
    public class ExcelReader
    {
        public void ReadFile(string path)
        {
            try
            {
                FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = null;

                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                if (Path.GetExtension(path).Equals(".xls"))
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                else
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                
                //4. DataSet - Create column names from first row
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet();
                DataTable dt = result.Tables[0];

                var foundRows = dt.AsEnumerable().ToList();

                //foreach (var item in mergeFilePaths)
                //{
                //    string fileName = foundRows[item.Name].ToString();
                //    pdfPath.Add(Path.Combine(item.Folderpath, fileName + ".pdf"));

                //}
                excelReader.Close();
             }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }
         
    }
}