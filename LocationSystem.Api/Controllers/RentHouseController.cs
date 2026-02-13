using LocationSystem.Api.Filters;
using LocationSystem.Application.Features.RentHousies.Command.CreateRentHose;
using LocationSystem.Application.Features.RentHousies.Queries.GetRentHouseDetail;
using LocationSystem.Application.Features.RentHousies.Queries.GetRentHouseList;
using LocationSystem.Application.Features.RentHousies.Queries.QueryRentHouseList;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocationSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentHouseController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        private readonly IMediator _mediator;
        public RentHouseController(IMediator mediator, IWebHostEnvironment environment)
        {
            _mediator = mediator;
            _environment = environment;
        }
        [HttpGet("test")]
        public IActionResult GetTest()
        {
            return Ok("测试成功");
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]GetRentHouseListFilter request)
        {
            try
            {
                var query = new GetRentHouseListQuery()
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    keyWord = request.keyWord
                };
                var data = await _mediator.Send(query);
                return Ok(data);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if(id==Guid.Empty)
                return BadRequest("Id不能为默认值");
            var query = new GetRentHouseDetailQuery { Id= id };
            var model = await _mediator.Send(query);
            if (model == null)
                return BadRequest("该信息不存在");
            return Ok(model);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateRentHouseDto model)
        {
            var command = new CreateRentHouseCommand() { Model=model };
            await _mediator.Send(command);
            return Ok();
        }
        [HttpPost("upload-multiple")]
        [RequestSizeLimit(5 * 1024 * 1024)]  // 限制整个请求最大5MB
        [RequestFormLimits(MultipartBodyLengthLimit = 5 * 1024 * 1024)]  // 限制multipart表单数据最大5MB
        [FileCountLimit(5)]
        public async Task<IActionResult> UploadMultipleFiles([FromForm]List<IFormFile> files)
        {
            // 处理文件上传和描述
            if (files == null || files.Count == 0)
                return BadRequest("没有上传文件");
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uploadedFiles = new List<object>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    // 计算文件哈希值用于重复检测
                    var fileHash = await CalculateFileHash(file.OpenReadStream());
                    var extension = Path.GetExtension(file.FileName);
                    
                    // 检查是否已存在相同哈希值的文件
                    var existingFile = FindExistingFile(uploadsFolder, fileHash);
                    if (existingFile != null)
                    {
                        // 文件已存在，直接返回已存在的文件信息
                        uploadedFiles.Add(new
                        {
                            OriginalName = file.FileName,
                            FileSize = file.Length,
                            FileUrl = Path.GetFileName(existingFile),
                            IsDuplicate = true
                        });
                        continue;
                    }

                    // 文件不存在，正常上传
                    string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    var fileName = $"{fileHash.Substring(0, 8)}_{timestamp}{extension}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    //var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
                    var fileUrl = $"{fileName}";
                    uploadedFiles.Add(new
                    {
                        OriginalName = file.FileName,
                        FileSize = file.Length,
                        FileUrl = fileUrl,
                        IsDuplicate = false
                    });
                }
            }
            return Ok(new
            {
                message = "文件上传成功",
                count = uploadedFiles.Count,
                files = uploadedFiles
            });

        }

        // 计算文件哈希值
        private async Task<string> CalculateFileHash(Stream stream)
        {
            using (var sha1 = System.Security.Cryptography.SHA1.Create())
            {
                stream.Position = 0;
                var hash = await sha1.ComputeHashAsync(stream);
                stream.Position = 0;
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        // 查找已存在的文件
        private string FindExistingFile(string directory, string fileHash)
        {
            var files = Directory.GetFiles(directory);
            foreach (var filePath in files)
            {
                var fileName = Path.GetFileName(filePath);
                // 检查文件名是否包含哈希值前缀
                if (fileName.StartsWith(fileHash.Substring(0, 8)))
                {
                    return filePath;
                }
            }
            return null;
        }

        public class FileUploadViewModel
        {
            public string Description { get; set; }
            public IFormFile File { get; set; }
        }

        [HttpGet("preview/{fileName}")]
        public IActionResult PreviewImage(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return BadRequest("文件名不能为空");

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploadsFolder, fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("文件不存在");

            var extension = Path.GetExtension(fileName).ToLower();
            var contentType = GetContentType(extension);

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, contentType);
        }

        private string GetContentType(string extension)
        {
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".bmp":
                    return "image/bmp";
                case ".webp":
                    return "image/webp";
                default:
                    return "application/octet-stream";
            }
        }
    }
}
