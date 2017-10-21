using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Nano.App.Controllers.Criteria.Entities;
using Nano.App.Controllers.Criteria.Enums;
using Nano.App.Controllers.Criteria.Interfaces;

namespace Nano.App.Controllers.Criteria.Binders
{
    /// <inheritdoc />
    public class CriteriaModelBinder<TQuery> : IModelBinder
        where TQuery : IQuery, new()
    {
        /// <inheritdoc />
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            // TODO: MODEL BINDING: Improve Criteria Binding (only querystring is used now!)

            var query = bindingContext.ActionContext.HttpContext.Request.Query;

            var success = int.TryParse(query["number"].FirstOrDefault(), out var number);
            if (!success)
                number = 1;

            success = int.TryParse(query["count"].FirstOrDefault(), out var count);
            if (!success)
                count = 25;

            var orderBy = query["by"].FirstOrDefault() ?? "Id";

            success = Enum.TryParse<Direction>(query["direction"].FirstOrDefault(), true, out var direction);
            if (!success)
                direction = Direction.Asc;

            var model = new Criteria<TQuery>
            {
                Paging = new Pagination
                {
                    Count = count,
                    Number = number
                },
                Order = new Ordering
                {
                    By = orderBy,
                    Direction = direction
                },
                Query = new TQuery()
            };

            var exclude = new[] { "by", "count", "number", "direction" };

            foreach (var parameter in query.Where(x => !exclude.Contains(x.Key)))
            {
                var type = typeof(TQuery);
                var property = type.GetProperty(parameter.Key, BindingFlags.IgnoreCase | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance);

                if (property == null)
                    continue;

                var value = parameter.Value.FirstOrDefault();

                if (property.PropertyType == typeof(DateTimeOffset) || property.PropertyType == typeof(DateTimeOffset?))
                {
                    property.SetValue(model.Query, DateTimeOffset.Parse(value));
                }
                else
                {
                    property.SetValue(model.Query, value);
                }
            }

            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}