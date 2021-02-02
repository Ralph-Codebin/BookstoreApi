using Application.Factories.Abstractions;
using AutoMapper;
using Domain.Model.ValueObjects.Abstractions;
using System;

namespace Application.Mappers
{
    public class ValueObjectConverter<T> : ITypeConverter<string, T> where T : class, IValueObject
    {
        private readonly Func<IValueObjectFactory> _vo;

        public ValueObjectConverter(Func<IValueObjectFactory> vo)
        {
            _vo = vo;
        }

        public T Convert(string source, T destination, ResolutionContext context)
        {
            return _vo().Get<T>(source);
        }
    }
}
