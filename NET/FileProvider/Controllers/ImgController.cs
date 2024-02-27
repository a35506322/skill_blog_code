using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Net.Mime;

namespace FileProvider.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ImgController : ControllerBase
{
    private IFileProvider _fileProvider;
    public ImgController(IFileProvider fileProvider)
    {
        this._fileProvider = fileProvider;
    }

    [HttpGet("{fileName}")]
    public ActionResult GetQunImagesFileInfo([FromRoute] string fileName)
    {
        var fileInfo = _fileProvider.GetFileInfo(fileName);
        var jsonResult = new FileInfoResponse(fileInfo.Exists, fileInfo.Length,fileInfo.PhysicalPath,fileInfo.Name,fileInfo.LastModified,fileInfo.IsDirectory).ToString();

        if (fileInfo is { Exists: true })
            return Ok(jsonResult);
        else
            return NotFound($"{fileName} is not found");
    }

    [HttpGet("{fileName}")]
    public ActionResult GetQunImages([FromRoute] string fileName)
    {
        var fileInfo = _fileProvider.GetFileInfo(fileName);

        if (fileInfo is { Exists: true }) 
            return PhysicalFile(_fileProvider.GetFileInfo(fileName).PhysicalPath, MediaTypeNames.Image.Jpeg);
            // return File(fileInfo.CreateReadStream(), MediaTypeNames.Image.Jpeg);
        else
            return NotFound($"{fileName} is not found");
    }


    [HttpGet("{fileName}")]
    public ActionResult DownloadQunImage([FromRoute] string fileName)
    {
        var fileInfo = _fileProvider.GetFileInfo(fileName);

        if (fileInfo is { Exists: true })
            return File(fileInfo.CreateReadStream(), MediaTypeNames.Image.Jpeg, fileInfo.Name);
        else
            return NotFound($"{fileName} is not found");
    }

    private record FileInfoResponse(bool Exists, long Length, string? PhysicalPath, string Name, DateTimeOffset LastModified, bool IsDirectory);
}
