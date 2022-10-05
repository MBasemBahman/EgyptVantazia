using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace Services
{
    public class ExcelService : IExcelService
    {
        private readonly string _rootPath;

        public ExcelService(string rootPath)
        {
            _rootPath = rootPath;
        }

        public async Task<List<List<string>>> Import(
            IFormFile formFile,
            int rowStart = 1,
            int colStart = 1,
            int rowEnd = 0,
            int colEnd = 0)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return null;
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            List<List<string>> Data = new();

            using (MemoryStream stream = new())
            {
                await formFile.CopyToAsync(stream);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


                using ExcelPackage package = new(stream);
                ExcelWorksheets Worksheets = package.Workbook.Worksheets;
                ExcelWorksheet Worksheet = Worksheets.First();

                int noOfCol = colEnd == 0 ? Worksheet.Dimension.End.Column : colEnd;
                int noOfRow = rowEnd == 0 ? Worksheet.Dimension.End.Row : rowEnd;
                int i = 0;

                for (int rowIterator = rowStart; rowIterator <= noOfRow; rowIterator++)
                {
                    Data.Add(new List<string>());

                    for (int colIterator = colStart; colIterator <= noOfCol; colIterator++)
                    {
                        if (true || Worksheet.Cells[rowIterator, colIterator].Value != null)
                        {
                            Data[i].Add(Worksheet.Cells[rowIterator, colIterator].Value == null ? "" : Worksheet.Cells[rowIterator, colIterator].Value.ToString());
                        }
                    }
                    i++;
                }
            }

            return Data;
        }

        public async Task<string> Export<T>(List<T> Data, string storagePath)
        {
            string fileName = DateTime.UtcNow.ToString("ddMMyyyyhhmmssfffffffK") + ".xlsx";
            string folderName = Path.Combine(_rootPath, storagePath);
            if (!Directory.Exists(folderName))
            {
                _ = Directory.CreateDirectory(folderName);
            }

            string fullPath = Path.Combine(folderName, fileName);

            FileInfo file = new(fullPath);

            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(fullPath);
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage Package = new(file))
            {
                ExcelWorksheet worksheet = Package.Workbook.Worksheets.Add("sheet");

                worksheet.DefaultColWidth = 25;

                _ = worksheet.Cells.LoadFromCollection(Data, true, TableStyles.Medium1);
                await Package.SaveAsync();
            }

            return storagePath + "/" + fileName;
        }
    }
}
