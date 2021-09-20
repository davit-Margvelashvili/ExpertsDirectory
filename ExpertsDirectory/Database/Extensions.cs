using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace ExpertsDirectory.Database
{
    public static class Extensions
    {
        [return: NotNullIfNotNull("self")]
        public static string? ToJson<T>(this T? self) => self is null ? null : JsonSerializer.Serialize(self);

        [return: NotNullIfNotNull("self")]
        public static T? FromJson<T>(this string? self) where T : class => self is null ? null : JsonSerializer.Deserialize<T>(self);

        [return: NotNullIfNotNull("self")]
        public static T? DeepCopy<T>(this T? self) where T : class => self?.ToJson().FromJson<T>();
    }

    public static class ValueConversionExtensions
    {
        public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder) where T : class
        {
            var converter = new ValueConverter<T, string>
            (
                v => v.ToJson(),
                v => v.FromJson<T>()
            );

            var comparer = new ValueComparer<T>
            (
                (l, r) => l.ToJson() == r.ToJson(),
                v => v == null ? 0 : v.ToJson().GetHashCode(),
                v => v.DeepCopy()
            );

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            propertyBuilder.Metadata.SetValueComparer(comparer);
            propertyBuilder.HasColumnType("nvarchar(max)");

            return propertyBuilder;
        }
    }
}