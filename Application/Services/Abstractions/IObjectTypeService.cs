using System;
using System.Collections.Generic;

namespace Application.Services.Abstractions
{
    public interface IObjectTypeService
    {
        IEnumerable<Type> ResolveImplementersOfInterface<TInterface>();
    }
}
