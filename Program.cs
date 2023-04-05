using System.Diagnostics;
using ConsoleApp3;
using MiniExcelLibs;

internal class Program
{
    private static void Main(string[] args)
    {
        string? _path;
        string? _inputFolder;
        var _outputFolder = string.Empty;

        if (System.Diagnostics.Debugger.IsAttached)
        {
            // 程序以 debug 模式启动
#if LINQPAD
			var currentDirectory = Path.GetDirectoryName(Util.CurrentQueryPath);
#else
            var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
#endif
            Console.WriteLine("注意：xls/xlsx文件的格式为源文件的列名为“OldFilename”，目标文件的列名为“NewFilename”。");
            Console.WriteLine(
                $"检测到正在以Debug模式运行，将从运行目录:{currentDirectory}下的“filelist.xlsx”文件读入对应关系，从“input”目录读入源文件，从“output”目录输出目标文件。");

            _path = Path.Combine(currentDirectory, "filelist.xlsx");

            _inputFolder = Path.Combine(currentDirectory, "input");
            _outputFolder = Path.Combine(currentDirectory, "output");

            if (!Directory.Exists(_outputFolder)) Directory.CreateDirectory(_outputFolder);

            if (!File.Exists(_path))
            {
                Console.WriteLine("程序目录下不存在“filelist.xlsx”，请检查！");
                return;
            }

            if (!Directory.Exists(_inputFolder))
            {
                Console.WriteLine("程序目录下不存在“input”文件夹，请检查！");
                return;
            }
        }
        else
        {
            // 程序以 release 模式启动
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: <flielist.xlsx> <input folder> <output folder>");
                Console.WriteLine("e.g: OldFilename NewFilename");
                Console.WriteLine("注意：xls/xlsx文件的格式为源文件的列名为“OldFilename”，目标文件的列名为“NewFilename”");
                return;
            }
            _path = args[0];
            _inputFolder = args[1];
            if (args.Length > 2) _outputFolder = args[2];

            if (!File.Exists(_path))
            {
                Console.WriteLine("请确认该xls/xlsx文件是否存在！");
                return;
            }

            if (!Directory.Exists(_inputFolder))
            {
                Console.WriteLine("请确认源文件夹是否存在！");
                return;
            }

            if (!Directory.Exists(_outputFolder))
            {
                if (args.Length == 2)
                {
                    _outputFolder = Path.Combine(Directory.GetParent(_inputFolder)!.FullName, "output");// 别说什么null，真null不可能跑到这里的
                }
                Directory.CreateDirectory(_outputFolder);
            }


        }

        // 计时器
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var rows = MiniExcel.Query<Item>(_path);

        foreach (var VARIABLE in rows)
        {
            var files = Directory.GetFiles(_inputFolder, VARIABLE.OldFilename + ".*");
            switch (files.Length)
            {
                case 1:
                    ChangeFileName(files[0], Path.Combine(_outputFolder, VARIABLE.NewFilename + Path.GetExtension(files[0])));
                    Console.WriteLine($"成功将“{Path.GetFileName(files[0])}”重命名为“{Path.GetFileName(VARIABLE.NewFilename + Path.GetExtension(files[0]))}”。");
                    break;
                case 0:
                    Console.WriteLine($"找不到这个名为“{VARIABLE.OldFilename}”的文件");
                    break;
                default:
                    Console.WriteLine($"似乎有重复的文件名：“{files}”");
                    break;
            }
        }

        stopwatch.Stop();
        Console.WriteLine($"处理时间：{stopwatch.ElapsedMilliseconds} 毫秒");

        if (System.Diagnostics.Debugger.IsAttached)
        {
            Console.ReadKey(); // 卡着看内存用的
        }

        static void ChangeFileName(string oldFile, string newFile)
        {
            try
            {
                File.Copy(oldFile, newFile, true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
