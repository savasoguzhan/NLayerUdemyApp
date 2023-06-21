using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Services;

namespace NLayer.API.Filters
{
    public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntitiy
    {
        private readonly IService<T> _service;

        public NotFoundFilter(IService<T> service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var idValue = context.ActionArguments.Values.FirstOrDefault();

            if (idValue == null)
            {
                await next.Invoke();
            }

            var id = (int)idValue;

            var anyEntity = await _service.AnyAsync(x =>x.Id==id);

            if(anyEntity)
            {
                await next.Invoke();
                return;

            }
            context.Result = new NotFoundObjectResult(CustomResponseDto<NoContentDto>.Fail("not found ", 400));






            throw new NotImplementedException();
        }
    }
}
