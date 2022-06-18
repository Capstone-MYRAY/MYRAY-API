using Microsoft.AspNetCore.Http;

namespace MYRAY.Business.Services.File;

public interface IFileService
{
    void UploadFile(List<IFormFile> files, string directory);

    (string fileType, byte[] archiveData, string archiveName) DownloadFiles(string directory);

    string SizeConverter(long bytes);

}