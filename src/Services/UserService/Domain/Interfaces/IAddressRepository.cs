using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces;

public interface IAddressRepository
{
    Task<Address> AddAsync(Address address);
    Task<Address> UpdateAsync(Address address);
    Task<Address?> GetByIdAsync(Guid id);
    Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId);
}
