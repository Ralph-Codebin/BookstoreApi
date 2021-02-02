using Domain.Model.ValueObjects.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Reflection;

namespace Application.ContractResolvers
{
    public class ValueObjectInternalSetResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty prop = base.CreateProperty(member, memberSerialization);

            if (member.DeclaringType.GetInterfaces().Contains(typeof(IValueObject)))
            {
                prop.Writable = true;
            }

            return prop;
        }
    }
}
