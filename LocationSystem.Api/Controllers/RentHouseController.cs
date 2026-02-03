using LocationSystem.Api.Filters;
using LocationSystem.Application.Features.Appointments.Queries.GetAppointmentDetail;
using LocationSystem.Application.Features.RentHousies.Command.CreateRentHose;
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
            var query = new GetAppointmentDetailQuery { Id= id };
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
                    var extension = Path.GetExtension(file.FileName);
                    string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    var fileName = $"{timestamp}{extension}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

                    uploadedFiles.Add(new
                    {
                        OriginalName = file.FileName,
                        FileSize = file.Length,
                        FileUrl = fileUrl
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

        public class FileUploadViewModel
        {
            public string Description { get; set; }
            public IFormFile File { get; set; }
        }
    }
}
