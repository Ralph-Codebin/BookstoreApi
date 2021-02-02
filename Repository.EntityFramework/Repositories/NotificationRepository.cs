using Application.ErrorHandlers;
using Application.Repositories;
using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using Domain.Model.Entities;
using Repository.EntityFramework.Entities;
using System;
using System.Collections;

namespace Repository.EntityFramework.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositoryExceptionHandler _exceptionHandler;

        public NotificationRepository(
            IUnitOfWork<DepContext> unitOfWork,
            IRepositoryExceptionHandler exceptionHandler)
        {
            _unitOfWork = unitOfWork;
            _exceptionHandler = exceptionHandler;
        }

        public Hashtable GetNotificationSettings()
        {
            Hashtable settings = new Hashtable();
            try
            {
                var notificationSettingsRepo = _unitOfWork.GetRepository<NotificationSettings>();
                IPagedList<NotificationSettings> pl = notificationSettingsRepo.GetPagedList();

                for (int x = 0; x < pl.TotalCount; x++)
                {
                    if (settings.ContainsKey(pl.Items[x].keyitem.ToLower()))
                    {
                        settings[pl.Items[x].keyitem.ToLower()] = settings[pl.Items[x].keyitem.ToLower()].ToString() + ',' + pl.Items[x].valueitem;
                    }
                    else
                    {
                        settings.Add(pl.Items[x].keyitem.ToLower(), pl.Items[x].valueitem);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return settings;
        }

        public DReturnObject UpdateNotificationToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
