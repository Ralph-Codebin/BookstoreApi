using Domain.Model.Entities;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace Application.ErrorHandlers
{
    public class RepositoryExceptionHandler: IRepositoryExceptionHandler
    {
        private readonly ILogger<RepositoryExceptionHandler> _logger;

        public RepositoryExceptionHandler(ILogger<RepositoryExceptionHandler> logger)
        {
            _logger = logger;
        }

        public DReturnObject HandleException(DReturnObject bdr, Exception exception)
        {
            ///**** Remember to remove this in production, end users should never see system errors.
            bdr.message = exception.Message;
            bdr.status = status.error.ToString();
            LogException(exception, "HandleException");
            return bdr;
        }
        public DReturnObject HandleException(DReturnObject bdr, string customMessage)
        {
            ///**** Remember to remove this in production, end users should never see system errors.
            bdr.message = customMessage;
            bdr.status = status.error.ToString();
            return bdr;
        }
        public DBusinessObject HandleException(DBusinessObject bdr, Exception exception)
        {
            ///**** Remember to remove this in production, end users should never see system errors.
            bdr.error = exception.Message;
            LogException(exception, "HandleException");
            return bdr;
        }

        public void LogException(Exception e, string sender)
        {
            Serilog.Log.Error(e, sender);
        }

        public void LogInformation(string information)
        {
            _logger.LogInformation(information);
        }
    }
}
