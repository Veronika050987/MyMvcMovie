using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;

namespace MyMvcMovie
{
	public class PaginatedList<T> : List<T>
	{
		public int PageIndex { get; private set; }
		public int TotalPages { get; private set; }
		public PaginatedList(List<T> items, int count, int PageIndex, int PageSize)
		{
			this.PageIndex = PageIndex;
			this.TotalPages = (int)Math.Ceiling(count / (double)PageSize);

			this.AddRange(items);
		}

		public bool HasPrevPage => PageIndex > 1;
		public bool HasNextPage => PageIndex < TotalPages;
		public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
		{
			int count = await source.CountAsync();
			List<T> items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
			return new PaginatedList<T>(items, count, pageIndex, pageSize);
		}
	}
}
