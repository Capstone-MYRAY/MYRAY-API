using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using Google.Api.Gax.ResourceNames;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.DTOs.Files;
using MYRAY.Business.Enums;
using MYRAY.Business.Services.File;

namespace MYRAY.Api.Controllers;
/// <summary>
/// Handle request related to file.
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces(MediaType.ApplicationJson)]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }
    /// <summary>
    /// [Authenticated user] Endpoint for upload file to server
    /// </summary>
    /// <param name="formFiles">Multiple file</param>
    /// <returns>List information of files</returns>
    [HttpPost, DisableRequestSizeLimit]
    [Authorize]
    [Consumes(MediaType.MultiPartFormData)]
    [ProducesResponseType(typeof(FilesUpload),StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadFile(
        [Required] List<IFormFile> formFiles)
    {
        try
        {
            FilesUpload result = await _fileService.UploadFile(formFiles);

          return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// [Authenticated user] Endpoint for delete file upload
    /// </summary>
    /// <param name="listFilename">List of filename</param>
    /// <returns></returns>
    [HttpDelete]
    [Authorize]
    [Consumes(MediaType.ApplicationJson)]
    public async Task<IActionResult> RemoveFile([Required] List<string> listFilename)  
    {
        try  
        {  
            _fileService.DeleteFile(listFilename); 
            return Ok();
        }  
        catch (Exception ex)  
        {  
            return BadRequest(ex.Message);  
        }  
  
    }  
}