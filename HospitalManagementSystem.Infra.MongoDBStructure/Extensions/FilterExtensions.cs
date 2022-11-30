using MongoDB.Driver;
using System.Linq.Expressions;

namespace HospitalManagementSystem.Infra.MongoDBStructure.Extensions
{
    public static class FilterExtensions
    {
        public static void AddFilter<T>(this List<FilterDefinition<T>> filters, Expression<Func<T, string>> field, string value) where T : class
        {
            if (!String.IsNullOrWhiteSpace(value))
                filters.Add(Builders<T>.Filter.Eq(field, value));
        }

        public static void AddFilter<T, Tfield>(this List<FilterDefinition<T>> filters, Expression<Func<T, Tfield>> field, List<Tfield> values) where T : class
        {
            if (values?.Any() ?? false)
                filters.Add(Builders<T>.Filter.In(field, values));
        }

        public static void AddFilter<T, Tfield>(this List<FilterDefinition<T>> filters, Expression<Func<T, Tfield>> field, bool? flag) where T : class
        {
            if (flag.HasValue)
                if (flag.Value)
                    filters.Add(Builders<T>.Filter.Ne(field, default));
                else
                    filters.Add(Builders<T>.Filter.Eq(field, default));
        }

        public static FilterDefinition<T> Consolidate<T>(this List<FilterDefinition<T>> filters)
            => filters.Count == 0
                    ? Builders<T>.Filter.Empty
                    : filters.Count == 1
                        ? filters[0]
                        : Builders<T>.Filter.And(filters);
    }
}
