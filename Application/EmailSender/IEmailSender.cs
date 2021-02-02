using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using Domain.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Application.EmailSender
{
    public interface IEmailSender
    {
        Task SendEmailNotification(emailMsgTypes msg, string recipientEmail, int retryCount, string recipientFullname, string fullSeviceName);
        
    }
}
