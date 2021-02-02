
using ClosedXML.Excel;
using Domain.Model.Entities;
using Optional;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface INotificationRepository
    {
        DReturnObject UpdateNotificationToken(string token);
        Hashtable GetNotificationSettings();     
    }
}
