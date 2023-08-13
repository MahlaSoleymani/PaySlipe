using AutoMapper;
using Common.Exceptions;
using Database.Repositories.RepositoryWrapper;
using Service.CompanyServices.Dto;
using Service.CompanyServices.Paginating;

namespace Service.CompanyServices
{
    public class CompanyService : ICompanyService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public CompanyService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public void Add(CompanyDto dto)
        {
            var company = dto.ToEntity(_mapper);
            _repository.Company.Add(company , true);
        }

        public void Edit(CompanyDto dto)
        {
            var company = _repository.Company.Find(dto.Id)
                          ?? throw new NotFoundException();
            company = dto.ToEntity(_mapper, company);
            _repository.Company.Add(company , true);
        }

        public void Delete(CompanyDto dto)
        {
           
        }

        public (IList<CompanyListDto>, int) GetAll(CompanyPaginate paginate)
        {
            var model = _repository.Company.TableNoTracking;
            (model, var count) = CompanyPaginate.GetPaginatedList(paginate, model);

            return (CompanyListDto.FromEntities(_mapper, model.ToList()), count);
        }

        public CompanyDto GetById(long id)
        {
            var company = _repository.Company.Find(id)
                ?? throw new NotFoundException();
            return CompanyDto.FromEntity(_mapper, company);
        }

    }
}
