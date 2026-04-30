using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PayRoleSystem.Repositories.Interfaces;
using PayRoleSystem.Repository;

namespace PayRoleSystem.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; } // ✅ Add this line
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        ICustomerRepository CustomerRepository { get; }
        IApplicationUserRepository ApplicationUserRepository { get; }
        ILookUpNameRepository LookUpNameRepository { get; }
        IItemRateRepository ItemRateRepository { get; }
        IItemRepository ItemRepository { get; }
        IItemCategoryRepository ItemCategoryRepository { get; }
        IItemSubCategoryRepository ItemSubCategoryRepository { get; }
        ISalePersonRepository SalePersonRepository { get; }
        ISalePersonLoaderRepository SalePersonLoaderRepository { get; }
        ISupervisorAssignSectorRepository SupervisorAssignSectorRepository { get; }
        ISectorAssignSaleMenRepository SectorAssignSaleMenRepository { get; }
        ISaleRepository SaleRepository { get; }
        ISaleParameterRepository SaleParameterRepository { get; }
        ILoaderParameterRepository LoaderParameterRepository { get; }
        ISaleBillTypeParameterRepository SaleBillTypeParameterRepository { get; }
        ISaleReturnRepository SaleReturnRepository { get; }
        ISaleReturnParameterRepository SaleReturnParameterRepository { get; }
        IFunctionRepository FunctionRepository { get; }
        IPageRepository PageRepository { get; }
        IPageControlRepository PageControlRepository { get; }
        ISupplierRepository SupplierRepository { get; }
        IPurchaseRepository PurchaseRepository { get; }
        IPurchaseParameterRepository PurchaseParameterRepository { get; }
        IRoleRepository RoleRepository { get; }
        IRolePagePermissionRepository RolePagePermissionRepository { get; }
        IRoleControlPermissionRepository RoleControlPermissionRepository { get; }
        ICountryRepository CountryRepository { get; }
        IProvinceRepository ProvinceRepository { get; }
        IDistrictRepository DistrictRepository { get; }
        ICityRepository CityRepository { get; }
        IPurchaseReturnRepository PurchaseReturnRepository { get; }
        IPurchaseReturnParameterRepository PurchaseReturnParameterRepository { get; }
        ISaleDashboardRepository SaleDashboardRepository { get; }
        ICompanySetupRepository CompanySetupRepository { get; }
        IGatePassNumberRepository GatePassNumberRepository { get; }
        IAccountGroupRepository AccountGroupRepository { get; }
        IAccountSubGroupRepository AccountSubGroupRepository { get; }
        IAccountDetailRepository AccountDetailRepository { get; }
        IAccountDetailTypeParameterRepository AccountDetailTypeParameterRepository { get; }
        IGeneralLedgerRepository GeneralLedgerRepository { get; }
        IGeneralLedgerParametersRepository GeneralLedgerParametersRepository { get; }
        IBookRepository BookRepository { get; }
        IBookParameterRepository BookParameterRepository { get; }
        IBookTypeRepository BookTypeRepository { get; }
        IBankRepository BankRepository { get; }
        IGlobalAccountRepository GlobalAccountRepository { get; }
        IChequeRepository ChequeRepository { get; }
        ILicenseRepository LicenseRepository { get; }
        IStockConfigurationRepository stockConfigurationRepository { get; }
        IStockConfigMasterRepository stockConfigMasterRepository { get; }
        IStockRepository stockRepository { get; }
        IStockAdjustmentRepository stockAdjustmentRepository { get; }
        IStockAdjustmentParameterRepository stockAdjustmentParameterRepository { get; }
    }
}
