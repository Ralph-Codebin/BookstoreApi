using Application.Services.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Integration.LocalServer
{
    public class ObjectTypeService : IObjectTypeService
    {
        private readonly ConcurrentDictionary<Type, IEnumerable<Type>> _types = 
            new ConcurrentDictionary<Type, IEnumerable<Type>>();

        public IEnumerable<Type> ResolveImplementersOfInterface<TInterface>()
        {
            var interfaceType = typeof(TInterface);
            return _types.GetOrAdd(interfaceType, i => i.Assembly.GetTypes()
                .Where(w => !w.IsInterface && !w.IsAbstract && i.IsAssignableFrom(w)));
        }
    }
}
