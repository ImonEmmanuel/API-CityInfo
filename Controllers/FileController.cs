using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CItyInfo.API.Controllers
{
    [ApiController]
    [Route("api/files")]


    public class FileController : Controller
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FileController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider??
                throw new System.ArgumentNullException(nameof(fileExtensionContentTypeProvider));   
        }

        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            var pathfile = "Aisha.pdf";

            if (!System.IO.File.Exists(pathfile))
            {
                return NotFound();
            }

            if(!_fileExtensionContentTypeProvider.TryGetContentType(pathfile, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(pathfile);
            return File(bytes, contentType, Path.GetFileName(pathfile));
        }
    }
}
