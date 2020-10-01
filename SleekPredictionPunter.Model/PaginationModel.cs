using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekPredictionPunter.Model
{
    public class PaginationModel<T>
    {
        public IEnumerable<T> TModel { get; set; }
        public int PerPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageNumber()
        {
            return PerPage++;
        }


        public int PageCount()
        {
            return Convert.ToInt32(Math.Ceiling(TModel.Count() / (double)PerPage));
        }
        public IEnumerable<T> Paginated()
        {
            int start = (CurrentPage - 1) * PerPage;
            return TModel.Skip(start).Take(PerPage);
        }
    }
}
