using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace svchost;

class Program
{
    private const int MaxDepth = 7;
    private static readonly string Link = "https://github.com/CNMengHan";
    
    private static async Task Main(string[] args)
    {
        if (args.Length != 1 || args[0] != "1")
        {
            return;
        }
        try
        {
            var art = ReadEmbeddedResource("svchost.LynCandy.txt");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(art);
            Console.ResetColor();
            Console.WriteLine($"                 {Link}\n");
        }
        catch (Exception)
        {
            
        }
        Console.WriteLine(" 开始扫描...\n");
        var drives = DriveInfo.GetDrives()
            .Where(drive => drive.IsReady && drive.DriveType == DriveType.Fixed)
            .Select(drive => drive.Name)
            .ToArray();

        if (drives.Length == 0)
        {
            Console.WriteLine(" 没有找到可用的磁盘驱动器!");
            return;
        }
        var folderSizes = new ConcurrentBag<FolderInfo>();
        var tasks = new List<Task>();
        foreach (var drive in drives)
        {
            tasks.Add(ProcessDirectoryAsync(drive, 1, folderSizes));
        }
        await Task.WhenAll(tasks);
        var sortedFolderSizes = folderSizes.OrderByDescending(f => f.SizeInBytes).ToList();

        Console.ForegroundColor = ConsoleColor.Cyan;
        foreach (var folder in sortedFolderSizes)
        {
            if (folder.SizeInGB > 0.01m)
            {
                Console.WriteLine($" {folder.SizeInGB:N2} GB M:{folder.LastModifiedDate:yyyy/MM/dd} {folder.FullPath}");
            }
        }
        Console.WriteLine("\n 扫描完成啦!");
        Console.ReadLine();
        Console.ReadLine();
        Console.ResetColor();
    }
    private static string ReadEmbeddedResource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
                throw new FileNotFoundException(" 可恶...要被逆向了吗(", resourceName);
            
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
    private static async Task ProcessDirectoryAsync(string path, int currentDepth, ConcurrentBag<FolderInfo> folderSizes)
    {
        if (currentDepth > MaxDepth)
            return;
        try
        {
            var directoryInfo = new DirectoryInfo(path);
            long size = await GetFolderSizeAsync(directoryInfo);

            folderSizes.Add(new FolderInfo
            {
                FullPath = path,
                SizeInBytes = size,
                CreationDate = directoryInfo.CreationTime,
                LastModifiedDate = directoryInfo.LastWriteTime,
                SizeInGB = (decimal)(size / (1024.0 * 1024.0 * 1024.0))
            });
            var subDirectoryTasks = directoryInfo.GetDirectories().Select(subDirectory =>
                ProcessDirectoryAsync(subDirectory.FullName, currentDepth + 1, folderSizes));
            
            await Task.WhenAll(subDirectoryTasks);
        }
        catch (Exception)
        {
            return;
        }
    }
    private static async Task<long> GetFolderSizeAsync(DirectoryInfo directoryInfo)
    {
        long totalSize = 0;
        try
        {
            var files = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            var fileTasks = files.Select(file =>
                Task.Run(() =>
                {
                    Interlocked.Add(ref totalSize, file.Length);
                }));
            await Task.WhenAll(fileTasks);
        }
        catch (Exception)
        {
            return totalSize;
        }
        return totalSize;
    }
}

class FolderInfo
{
    public required string FullPath { get; set; }
    public long SizeInBytes { get; set; }
    public decimal SizeInGB { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
}
