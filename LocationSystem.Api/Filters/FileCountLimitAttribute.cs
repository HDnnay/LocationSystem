using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LocationSystem.Api.Filters
{
    public class FileCountLimitAttribute: ActionFilterAttribute
    {
        private readonly int _limitCount;
        public FileCountLimitAttribute(int maxCount) 
        {
            _limitCount = maxCount;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            if (request.HasFormContentType)
            {
                var form = request.Form;
                var files = form.Files;

                // 检查文件数量
                if (files.Count > _limitCount)
                {
                    context.Result = new BadRequestObjectResult($"上传文件数量不能超过{_limitCount}个");
                    return;
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
