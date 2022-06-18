using System.Linq.Expressions;

namespace MYRAY.Business.Helpers;

public static class SearchHelper
{
    public static IQueryable<TEntity> GetWithSearch<TEntity>(this IQueryable<TEntity> query, object searchModel) where TEntity : class
    {
       
        foreach (var prop in searchModel.GetType().GetProperties())
        {
            var value = prop.GetValue(searchModel, null);
            if(value is string)
            {
                if((string)value! is {Length: > 0})
                {
                    // Build expression tree
                    //--entity
                    var param = Expression.Parameter(typeof(TEntity), "entity");
                    //--entity.{PropertyName}
                    var entityProp = Expression.Property(param, prop.Name);
                    //--searchValue
                    var searchValue = Expression.Constant(value);
                    //--entity.{PropertyName}.Contains(searchValue)
                    var body = Expression.Call(entityProp, "Contains",null, searchValue);
                    //--entity => entity.{PropertyName}.Contains(searchValue)
                    var exp = Expression.Lambda<Func<TEntity, bool>>(body, param);
                    //entity.{PropertyName}.Contains(searchValue)
                    query = query.Where(exp);
                }
            }

            if (value != null && value is int?)
            {
                // Build expression tree
                //--entity
                var param = Expression.Parameter(typeof(TEntity), "entity");
                //--entity.{PropertyName}
                var entityProp = Expression.Property(param, prop.Name);
                //--searchValue
                var searchValue = Expression.Constant((int?)value);
                var searchValueE = Expression.Convert(searchValue, typeof(int?));
                //--entity.{PropertyName}.Contains(searchValue)
                var body = Expression.Equal(entityProp, searchValueE);
                //--entity => entity.{PropertyName}.Contains(searchValue)
                var exp = Expression.Lambda<Func<TEntity, bool>>(body, param);
                //entity.{PropertyName}.Contains(searchValue)
                query = query.Where(exp);
            }
        }
        
        return query;
    }
    
}