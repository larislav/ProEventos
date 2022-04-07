using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProEventos.Persistence.Models
{
    public class PageList<T> : List<T>
    {
        public PageList(int currentPage, int totalPages, int pageSize, int totalCount) 
        {
            this.CurrentPage = currentPage;
                this.TotalPages = totalPages;
                this.PageSize = pageSize;
                this.TotalCount = totalCount;
               
        }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; } //total de itens

        public PageList()
        {
            
        }

        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize );
            AddRange(items);
        }

        public static async Task<PageList<T>> CreateAsync(
            IQueryable<T> source, int pageNumber, int pageSize
        )
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber-1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
            return new PageList<T>(items, count, pageNumber, pageSize);
        }
    }
}