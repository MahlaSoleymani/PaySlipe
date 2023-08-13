using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace Common.Utilities.Paging
{
    public class Paginate
    {
        public int index { get; set; }
        public int count { get; set; }
        public string order { get; set; }
        public string sort { get; set; }

        public static (IQueryable<T>, int) GetPaginatedList<T>(Paginate paginate, IQueryable<T> models)
        {
            if (paginate == null)
                return (models.Take(20), models.Count());

            if (string.IsNullOrEmpty(paginate.sort))
            {
                var prop = TypeDescriptor.GetProperties(typeof(T))
                    .Find("CreatedDateTime", true);
                models = models.OrderByDescending(x => EF.Property<T>(x, prop.Name));
            }
            else
            {
                var prop = TypeDescriptor.GetProperties(typeof(T)).Find(paginate.sort, true);
                models = paginate.order.ToLower() == "asc"
                    ? models.OrderBy(x => EF.Property<T>(x, prop.Name))
                    : models.OrderByDescending(x => EF.Property<T>(x, prop.Name));
            }

            var count = models.Count();

            if (paginate.count is > 0 and <= 50 && paginate.index >= 0)
            {
                models = models
                    .Skip(paginate.count * paginate.index)
                    .Take(paginate.count);
            }
            else
            {
                models = models.Take(20);
            }

            return (models, count);
        }
    }
}
