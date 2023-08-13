using AutoMapper;

namespace Service.Infrastructure.CustomMapping
{
    public interface IHaveCustomMapping
    {
        void CreateMappings(Profile profile);
    }
}
