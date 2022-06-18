using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.Services.File;

namespace MYRAY.Api.Controllers;
/// <summary>
/// Handle request related to file.
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.MultiPartFormData)]
[Produces(MediaType.ApplicationJson)]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }
    [HttpPost(), DisableRequestSizeLimit]
    public async Task<IActionResult> UploadFile(
        [Required] List<IFormFile> formFiles,
        [Required] string directory)
    {
        try
        {
          _fileService.UploadFile(formFiles, directory);

          return Ok(new
          {
              formFiles.Count,
              Size = _fileService.SizeConverter(formFiles.Sum(f => f.Length))
          });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}