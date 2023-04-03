using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using OfficeOpenXml;

namespace ExcelFileRenamer;

class Program
{
    static void Main(string[] args)
    {
        var inputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "input");
        var outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "output");

        // 读取Excel文件
        var excelFile = new FileInfo(Path.Combine(inputDirectory, "filelist.xlsx"));
        using var package = new ExcelPackage(excelFile);
        var worksheet = package.Workbook.Worksheets[0];

        // 获取文件列表
        var files = Directory.GetFiles(inputDirectory);

        // 遍历Excel表格中的行
        for (var row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            var fileNameColumn = worksheet.Cells[row, 1].Value?.ToString();
            var newFileNameColumn = worksheet.Cells[row, 2].Value?.ToString();

            if (!string.IsNullOrEmpty(fileNameColumn) && !string.IsNullOrEmpty(newFileNameColumn))
            {
                // 搜索文件名并重命名
                var matchingFile = files.FirstOrDefault(f => Path.GetFileName(f) == fileNameColumn);
                if (matchingFile != null)
                {
                    var newFileName = newFileNameColumn;
                    var newFilePath = Path.Combine(outputDirectory, newFileName);
                    File.Move(matchingFile, newFilePath);
                    Console.WriteLine($"Renamed {matchingFile} to {newFilePath}");
                }
                else
                {
                    Console.WriteLine($"Could not find file {fileNameColumn}");
                }
            }
        }
    }
}
