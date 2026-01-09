using System.Reflection;
using MudBlazor;

namespace Odasoft.XBOL.AdminPortal.Extensions;

public static class MudGridExtensions
{
    public static GridData<T> ToClientSideGridData<T>(
        this IEnumerable<T> items,
        GridState<T> state,
        Func<T, bool>? filter = null
    )
    {
        return items.ApplyFilter(filter).ApplySort(state).ToPaginatedGridData(state);
    }

    private static IEnumerable<T> ApplyFilter<T>(this IEnumerable<T> data, Func<T, bool>? filter)
    {
        return filter != null ? data.Where(filter) : data;
    }

    private static IEnumerable<T> ApplySort<T>(this IEnumerable<T> data, GridState<T> state)
    {
        var sortDefinition = state.SortDefinitions.FirstOrDefault();

        if (sortDefinition == null || string.IsNullOrEmpty(sortDefinition.SortBy))
        {
            return data;
        }

        var propInfo = typeof(T).GetProperty(
            sortDefinition.SortBy,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
        );

        if (propInfo == null)
        {
            return data;
        }

        return sortDefinition.Descending
            ? data.OrderByDescending(x => propInfo.GetValue(x))
            : data.OrderBy(x => propInfo.GetValue(x));
    }

    private static GridData<T> ToPaginatedGridData<T>(this IEnumerable<T> data, GridState<T> state)
    {
        var materialized = data.ToList();

        var pagedData = materialized
            .Skip(state.Page * state.PageSize)
            .Take(state.PageSize)
            .ToList();

        return new GridData<T> { TotalItems = materialized.Count, Items = pagedData };
    }
}
