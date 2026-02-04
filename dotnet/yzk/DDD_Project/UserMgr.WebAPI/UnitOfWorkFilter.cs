using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace UserMgr.WebAPI;

public class UnitOfWorkFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // 执行方法
        var result = await next();

        // action failed
        if (result.Exception != null) 
        {
            return;
        }

        if (context.ActionDescriptor is not ControllerActionDescriptor actionDescriptor)
        {
            return;
        }

        var unitOfWorkAttribute = actionDescriptor.MethodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
        if (unitOfWorkAttribute == null)
        {
            return;
        }

        foreach (var dbContextType in unitOfWorkAttribute.DbContextTypes)
        {
            // 通过DI获取DbContext实例
            var dbContext = context.HttpContext.RequestServices.GetService(dbContextType) as DbContext;
            if (dbContext != null)
            {
                await dbContext.SaveChangesAsync();
            }
        }
    }
}