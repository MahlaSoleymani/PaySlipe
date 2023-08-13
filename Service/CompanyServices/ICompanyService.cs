using Service.CompanyServices.Dto;
using Service.CompanyServices.Paginating;

namespace Service.CompanyServices
{
    public interface ICompanyService
    {
        void Add(CompanyDto dto);
        void Edit(CompanyDto dto);
        void Delete(CompanyDto dto);
        (IList<CompanyListDto> , int ) GetAll(CompanyPaginate paginate);
        CompanyDto GetById(long id);

    }
}
