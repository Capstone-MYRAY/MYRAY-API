using Microsoft.AspNetCore.Http;
using MYRAY.Business.DTOs.Files;

namespace MYRAY.Business.Services.File;

public interface IFileService
{
    Task<FilesUpload> UploadFile(List<IFormFile> files);

    void DeleteFile(List<string> listFilename);
    string SizeConverter(long bytes);

}