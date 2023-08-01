using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Filters;

public class ApiLoggerFilter : IActionFilter
{
    private readonly ILogger<ApiLoggerFilter> _logger;

    public ApiLoggerFilter(ILogger<ApiLoggerFilter> logger)
    {
        _logger = logger;
    }
    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("Executando -> OnActionExecuting");
        _logger.LogInformation("################################");
        _logger.LogInformation($"{DateTime.Now.ToLongDateString()}");
        _logger.LogInformation($"ModelState : {context.ModelState.IsValid}");
        _logger.LogInformation("################################");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("Executado -> OnActionExecuting");
        _logger.LogInformation("################################");
        _logger.LogInformation($"{DateTime.Now.ToLongDateString()}");
        _logger.LogInformation($"ModelState : {context.ModelState.IsValid}");
        _logger.LogInformation("################################");
    }
}