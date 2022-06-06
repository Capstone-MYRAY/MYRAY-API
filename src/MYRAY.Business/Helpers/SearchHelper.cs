using System.Linq.Expressions;

namespace MYRAY.Business.Helpers;

public static class SearchHelper
{
    public static IQueryable<TEntity> GetWithSearch<TEntity>(this IQueryable<TEntity> query, object searchModel) where TEntity : class
    {
       
        foreach (var prop in searchModel.GetType().GetProperties())
        {
            if((string)prop.GetValue(searchModel, null)! is {Length: > 0})
            {
                // Build expression tree
                //--entity
                var param = Expression.Parameter(typeof(TEntity), "entity");
                //--entity.{PropertyName}
                var entityProp = Expression.Property(param, prop.Name);
                //--searchValue
                var searchValue = Expression.Constant(prop.GetValue(searchModel, null));
                //--entity.{PropertyName}.Contains(searchValue)
                var body = Expression.Call(entityProp, "Contains",null, searchValue);
                //--entity => entity.{PropertyName}.Contains(searchValue)
                var exp = Expression.Lambda<Func<TEntity, bool>>(body, param);
                //entity.{PropertyName}.Contains(searchValue)
                query = query.Where(exp);
            }
        }
        
        return query;
    }
    
}