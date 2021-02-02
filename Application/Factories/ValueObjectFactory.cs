using Application.Factories.Abstractions;
using Application.Repositories;
using Application.Services.Abstractions;
using Domain.Model.ValueObjects.Abstractions;
using Domain.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Application.Factories
{
    public class ValueObjectFactory : IValueObjectFactory
    {
        private readonly ValueObjectFactoryConfig _config;
        private readonly IDateTimeService _dateTimeService;
        private readonly IEnumerable<IValueObjectRepository> _repositories;
        private readonly IObjectTypeService _objectTypeService;
        private readonly IMemoryCache _cache;

        private static readonly MethodInfo _listAllMethod = typeof(ValueObjectFactory).GetMethod(nameof(List), new Type[] { });

        public ValueObjectFactory(
            IOptions<ValueObjectFactoryConfig> config,
            IMemoryCache cache,
            IDateTimeService dateTimeService,
            IEnumerable<IValueObjectRepository> repositories,
            IObjectTypeService objectTypeService)
        {
            _config = config.Value;
            _dateTimeService = dateTimeService;
            _cache = cache;
            _repositories = repositories;
            _objectTypeService = objectTypeService;
        }

        public T Get<T>(string code) where T : class, IValueObject
        {
            return List<T>().Single(s => s.Code.Value.Equals(code, StringComparison.InvariantCultureIgnoreCase));
        }

        public IList<T> List<T>() where T : class, IValueObject
        {
            var result = _cache.GetOrCreate(typeof(T), cacheEntry =>
            {
                cacheEntry.AbsoluteExpiration = _dateTimeService.Now.AddMinutes(_config.CacheMinutes);
                return
                _repositories
                .OrderBy(o => o.Priority)
                .Where(f => f.IsRepositoryFor<T>()).Take(1)
                .Select(s => s.List<T>())
                    .DefaultIfEmpty(new List<T>())
                    .Single();
            });
            return result;
        }

        public IList<IValueObject> List(string typeName)
        {
            return
                _objectTypeService.ResolveImplementersOfInterface<IValueObject>()
                .Where(w => w.Name.Equals(typeName, StringComparison.CurrentCultureIgnoreCase))
                .Select(s => (IEnumerable<IValueObject>)_listAllMethod.MakeGenericMethod(s).Invoke(this, null))
                    .DefaultIfEmpty(new List<IValueObject>())
                    .Single().ToList();
        }

        public IEnumerable<string> ListTypes()
        {
            return _objectTypeService.ResolveImplementersOfInterface<IValueObject>().Select(s => s.Name);
        }
    }
}
