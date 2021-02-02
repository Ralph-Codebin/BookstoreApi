using System;
using Domain.Model.Entities;

namespace Application.ErrorHandlers
{
    public interface IRepositoryExceptionHandler
    {        
        public DReturnObject HandleException(DReturnObject response, Exception exception);
        public DReturnObject HandleException(DReturnObject response, string message);
        public DBusinessObject HandleException(DBusinessObject response, Exception exception);
        public void LogException(Exception e, string sender);
        public void LogInformation(string information);
    }
}
