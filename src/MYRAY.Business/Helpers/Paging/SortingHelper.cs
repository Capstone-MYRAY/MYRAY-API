using System.Linq.Expressions;
using MYRAY.Business.Constants;
using MYRAY.Business.DTOs;

namespace MYRAY.Business.Helpers.Paging;

public static class SortingHelper
{
    public static IQueryable<TEntity> GetWithSorting<TEntity>(this IQueryable<TEntity>? query, string? sortColumn,
        PagingConstant.OrderCriteria sortOrder) where TEntity : class
    {
        if (query == null)
        {
            return Enumerable.Empty<TEntity>().AsQueryable();
        }

        if (sortColumn != null)
        {
            //--Set method to call
            string method = sortOrder == PagingConstant.OrderCriteria.ASC
                ? "OrderBy"
                : "OrderByDescending";
            //-- q
            var param = Expression.Parameter(typeof(TEntity), "q");
            //-- q.{sortColumn}
            var prop = Expression.Property(param, sortColumn);
            //-- q => q.{sortColumn}
            var exp = Expression.Lambda(prop, param);
            //-- Set Type of arguments ( q : Queryable, q.{sortColumn} Type )
            Type[] types = { query.ElementType, exp.Body.Type };
            //--Build Call method 
            var mce = Expression.Call(typeof(Queryable), method, types, query.Expression, exp);
            //-- Invoke Method
            query = query.Provider.CreateQuery<TEntity>(mce);
        }

        return query;
    }
}