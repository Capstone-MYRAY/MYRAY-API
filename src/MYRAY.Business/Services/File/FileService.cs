using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace MYRAY.Business.Services.File;

public class FileService :  IFileService
{
    private readonly IHostEnvironment _hostEnvironment;

    public FileService(IHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
    }

    public void UploadFile(List<IFormFile> files, string directory)
    {
        directory = directory ?? string.Empty;  
        var target = Path.Combine(_hostEnvironment.ContentRootPath, directory);
        Console.WriteLine(target);
        Directory.CreateDirectory(target);  
  
        files.ForEach(async file =>  
        {  
            if (file.Length <= 0) return;  
            var filePath = Path.Combine(target, file.FileName);  
            using (var stream = new FileStream(filePath, FileMode.Create))  
            {  
                await file.CopyToAsync(stream);  
            }  
        });  
        
    }

    public (string fileType, byte[] archiveData, string archiveName) DownloadFiles(string directory)
    {
        throw new NotImplementedException();
    }

    public string SizeConverter(long bytes)
    {
        var fileSize = new decimal(bytes);  
        var kilobyte = new decimal(1024);  
        var megabyte = new decimal(1024 * 1024);  
        var gigabyte = new decimal(1024 * 1024 * 1024);  
  
        switch (fileSize)  
        {  
            case var _ when fileSize < kilobyte:  
                return $"Less then 1KB";  
            case var _ when fileSize < megabyte:  
                return $"{Math.Round(fileSize / kilobyte, 0, MidpointRounding.AwayFromZero):##,###.##}KB";  
            case var _ when fileSize < gigabyte:  
                return $"{Math.Round(fileSize / megabyte, 2, MidpointRounding.AwayFromZero):##,###.##}MB";  
            case var _ when fileSize >= gigabyte:  
                return $"{Math.Round(fileSize / gigabyte, 2, MidpointRounding.AwayFromZero):##,###.##}GB";  
            default:  
                return "n/a";  
        } 
    }
}