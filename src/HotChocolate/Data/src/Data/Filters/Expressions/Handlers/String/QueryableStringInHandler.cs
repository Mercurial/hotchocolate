using System.Linq.Expressions;
using HotChocolate.Language;

namespace HotChocolate.Data.Filters.Expressions
{
    public class QueryableStringInHandler : QueryableStringOperationHandler
    {
        protected override int Operation => DefaultOperations.In;

        public override Expression HandleOperation(
            QueryableFilterContext context,
            IFilterOperationField field,
            IValueNode value,
            object parsedValue)
        {
            Expression property = context.GetInstance();

            return FilterExpressionBuilder.In(
                property,
                context.RuntimeTypes.Peek().Source,
                parsedValue);
        }
    }
}
