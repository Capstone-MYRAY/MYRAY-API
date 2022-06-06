using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Business.Constants;

namespace MYRAY.Business.DTOs;

public class PagingDto
{
    private int _page;
    private int _pageIndex;
    /// <summary>
    /// Gets or sets current page number.
    /// </summary>
    [FromQuery(Name = "page")]
    [DefaultValue(PagingConstant.FixPagingConstant.DefaultPage)]
    public int Page 
    { 
        get=> _pageIndex;
        set => _pageIndex = value < 1 ? 1 : value; 
    }

    /// <summary>
    /// Gets or sets size of current page.
    /// </summary>
    [FromQuery(Name = "page-size")]
    [DefaultValue(PagingConstant.FixPagingConstant.DefaultPageSize)]
    public int PageSize
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

}