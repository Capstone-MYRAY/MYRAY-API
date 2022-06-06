using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Business.Constants;

namespace MYRAY.Business.DTOs;

public class SortingDto<TKey> where TKey : Enum
{
    /// <summary>
    /// Gets or sets ordering column.
    /// </summary>
    [FromQuery(Name = "sort-column")]
    [Description("Parameter use for sorting column. Value: {propertyName}")]
    public TKey? SortColumn { get; set; } = default(TKey?);
    
    /// <summary>
    /// Gets or sets ordering criteria.
    /// </summary>
    [FromQuery(Name = "order-by")]
    [DefaultValue(PagingConstant.OrderCriteria.ASC)]
    [JsonConverter(typeof(PagingConstant.OrderCriteria))]
    [EnumDataType(typeof(PagingConstant.OrderCriteria))]
    [Description("Parameter user for sorting order by. Value {propertyName}")]
    public PagingConstant.OrderCriteria OrderBy { get; set; }
}