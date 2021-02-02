using Domain.Services;
using System;

namespace Integration.LocalServer
{
    public class LocalDateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}
