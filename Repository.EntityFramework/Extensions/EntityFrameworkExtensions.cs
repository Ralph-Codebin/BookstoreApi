using Application.Factories.Abstractions;
using Domain.Model.ValueObjects.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Repository.EntityFramework.Extensions
{
    public static class EntityFrameworkExtensions
    {
        public static PropertyBuilder<TProperty> ValueObject<TEntity, TProperty>(
            this EntityTypeBuilder<TEntity> builder,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            Func<IValueObjectFactory> valueObjectFactory)
            where TEntity : class
            where TProperty : class, IValueObject
        {
            var valueConverter = new ValueConverter<TProperty, string>(
                v => v.Code.Value,
                v => valueObjectFactory().List<TProperty>().First(f => f.Code.Value.Equals(v, StringComparison.InvariantCultureIgnoreCase)));

            return builder.Property(propertyExpression).HasConversion(valueConverter);
        }

    }

}
