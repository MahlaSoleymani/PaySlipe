
using Common.Utilities.Paging;
using Entities.Companies;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Service.CompanyServices.Paginating
{
    public class CompanyPaginate : Paginate
    {
        public string Search { get; set; }

        public static (IQueryable<Company>, int) GetPaginatedList(CompanyPaginate paginate, IQueryable<Company> models)
        {
            int count ;
            if (paginate == null)
            {
                count = models.Count();
                models = models.OrderByDescending(x => x.CompanyName).Take(20);
            }
            else
            {
                if (!string.IsNullOrEmpty(paginate.Search))
                {
                    models = models.Where(x =>
                        (x.CompanyName)
                        .Contains(paginate.Search)
                    );
                }

                count = models.Count();

                if (string.IsNullOrEmpty(paginate.sort))
                    models = models.OrderByDescending(x => x.CompanyName);
                else
                {
                    var prop = TypeDescriptor.GetProperties(typeof(Company)).Find(paginate.sort, true);
                    models = paginate.order.ToLower() == "asc"
                        ? models.OrderBy(x => EF.Property<Company>(x, prop.Name))
                        : models.OrderByDescending(x => EF.Property<Company>(x, prop.Name));
                }

                if (paginate.count != 0)
                {
                    models = models
                        .Skip(paginate.count * paginate.index)
                        .Take(paginate.count);
                }
                else
                {
                    models = models.Take(20);
                }
            }

            return (models, count);
        }
    }
}
