using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MYRAY.Business.DTOs.Files;

namespace MYRAY.Business.Services.File;

public class FileService :  IFileService
{
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IConfiguration _configuration;
    private const string DIRECTORY = "upload";

    public FileService(IHostEnvironment hostEnvironment, IConfiguration configuration)
    {
        _hostEnvironment = hostEnvironment;
        _configuration = configuration;
    }

    public async Task<FilesUpload> UploadFile(List<IFormFile> files)
    {
        string domain = _configuration.GetSection("Domain").Value;
        var target = Path.Combine(_hostEnvironment.ContentRootPath, DIRECTORY);
        Directory.CreateDirectory(target);
        List<LinkFile> listLink = new List<LinkFile>();
        foreach (var formFile in files)
        {
            int indexOf = formFile.FileName.LastIndexOf(".", StringComparison.Ordinal);
            var extension = formFile.FileName.Substring(indexOf);
            var filename = Guid.NewGuid() + extension;
            var filePath = Path.Combine(target, filename);

            using (var stream = System.IO.File.Create(filePath))
            {
                listLink.Add(new LinkFile
                {
                    Link = $"{domain}/{DIRECTORY}/{filename}",
                    Filename = filename,
                    Oldname = formFile.FileName,
                    Size = SizeConverter(formFile.Length),
                    Type = formFile.ContentType
                });
                await formFile.CopyToAsync(stream);
            }
        }
        FilesUpload result = new FilesUpload
        {
            Count = files.Count,
            ListFile = listLink,
        };

        return result;
    }

    public void DeleteFile(List<string> listFilename)
    {
        var target = Path.Combine(_hostEnvironment.ContentRootPath, DIRECTORY);
        foreach (var filename in listFilename)
        {
            var path = target + "/" + filename;
            System.IO.File.Delete(path);
        }
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