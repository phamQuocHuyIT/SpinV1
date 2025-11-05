using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using AbpSolution1.Administration.Customer;

namespace AbpSolution1.Data
{
    public class CustomerDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Customers, int> _customerRepository;

        public CustomerDataSeedContributor(IRepository<Customers, int> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            if (await _customerRepository.GetCountAsync() > 0)
            {
                return;
            }

            await _customerRepository.InsertAsync(new Customers
            {
                Code = "CUST001",
                FullName = "John Doe",
                Address = "123 Main St",
                NumberPhone = "0123456789",
                Rank = "A",
                Total = 1000m,
                IsDelete = false,
                Note = "Seeded customer",
                IsActive = true
            }, autoSave: true);

            await _customerRepository.InsertAsync(new Customers
            {
                Code = "CUST002",
                FullName = "Jane Smith",
                Address = "456 Second St",
                NumberPhone = "0987654321",
                Rank = "B",
                Total = 500m,
                IsDelete = false,
                Note = "Seeded customer",
                IsActive = true
            }, autoSave: true);
        }
    }
}