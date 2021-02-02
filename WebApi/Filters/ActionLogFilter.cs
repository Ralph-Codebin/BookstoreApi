using Domain.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;


namespace bookstore_api.Filters
{
    public class ActionLogFilter : ActionFilterAttribute
    {
        private readonly ILogger<ActionLogFilter> _logger;
        private readonly IDateTimeService _dateTime;

        public ActionLogFilter(
           ILogger<ActionLogFilter> logger,
           IDateTimeService dateTime)
        {
            _logger = logger;
            _dateTime = dateTime;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var actionName = context.ActionDescriptor.DisplayName;
            var arguments = string.Join(",", context.ActionArguments);
            var actionStarted = _dateTime.Now;
            _logger.LogInformation($"Action: {actionName}, Started: {actionStarted:hh.mm.ss.ffffff}, Arguments: {arguments}");
            await next();
            var actionCompleted = _dateTime.Now;
            var duration = actionCompleted - actionStarted;
            _logger.LogInformation($"Action: {actionName}, Ended: {actionCompleted:hh.mm.ss.ffffff}, Duration: {duration}");
        }
    }
}
