using PayRoleSystem.Data;
using PayRoleSystem.Repositories;
using PayRoleSystem.Repositories.Interfaces;
using PayRoleSystem.Repository;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore; 
namespace PayRoleSystem.UOW
{
    public class UnitOfWork : IUnitOfWork
    {


        #region Private / ReadOnly Property Declare
        private readonly ApplicationDbContext _context;
        private readonly ICustomerRepository _customerRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly ILookUpNameRepository _lookUpNameRepository;
        private readonly IItemRateRepository _itemRateRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IItemCategoryRepository _itemCategoryRepository;
        private readonly IItemSubCategoryRepository _itemSubCategoryRepository;
        private readonly ISalePersonRepository _salePersonRepository;
        private readonly ISalePersonLoaderRepository _salePersonLoaderRepository;
        private readonly ISupervisorAssignSectorRepository _supervisorAssignSectorRepository;
        private readonly ISectorAssignSaleMenRepository _sectorAssignSaleMenRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly ISaleParameterRepository _saleParameterRepository;
        private readonly ILoaderParameterRepository _loaderParameterRepository;
        private readonly ISaleBillTypeParameterRepository _saleBillTypeRepository;
        private readonly ISaleReturnRepository _saleReturnRepository;
        private readonly ISaleReturnParameterRepository _saleReturnParameterRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IPurchaseParameterRepository _purchaseParameterRepository;
        private readonly IFunctionRepository _functionRepository;
        private readonly IPageRepository _pageRepository;
        private readonly IPageControlRepository _pageControlRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleControlPermissionRepository _roleControlPermissionRepository;
        private readonly IRolePagePermissionRepository _rolePagePermissionRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IProvinceRepository _provinceRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IPurchaseReturnRepository _purchaseReturnRepository;
        private readonly IPurchaseReturnParameterRepository _purchaseReturnParameterRepository;
        private readonly ISaleDashboardRepository _saleDashboardRepository;
        private readonly ICompanySetupRepository _companySetupRepository;
        private readonly IGatePassNumberRepository _gatePassNumberRepository;
        private readonly IAccountDetailRepository _accountDetailRepository;
        private readonly IAccountGroupRepository _accountGroupRepository;
        private readonly IAccountSubGroupRepository _accountSubGroupRepository;
        private readonly IAccountDetailTypeParameterRepository _accountDetailTypeParameterRepository;
        private readonly IGeneralLedgerRepository  _generalLedgerRepository;
        private readonly IGeneralLedgerParametersRepository  _generalLedgerParametersRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IBookParameterRepository _bookParameterRepository;
        private readonly IBookTypeRepository _bookTypeRepository;
        private readonly IBankRepository _bankRepository;
        private readonly IGlobalAccountRepository _globalAccountRepository;
        private readonly IChequeRepository _chequeRepository;
        private readonly ILicenseRepository _licenseRepository;
        private readonly IStockConfigurationRepository _stockConfigurationRepository;
        private readonly IStockConfigMasterRepository _stockConfigMasterRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IStockAdjustmentRepository _stockAdjustmentRepository;
        private readonly IStockAdjustmentParameterRepository _stockAdjustmentParameterRepository;

        #endregion

        #region Public Getter Properties  
        public ICustomerRepository CustomerRepository => _customerRepository;
        public IApplicationUserRepository ApplicationUserRepository => _applicationUserRepository;
        public ILookUpNameRepository LookUpNameRepository => _lookUpNameRepository;
        public IItemRateRepository ItemRateRepository => _itemRateRepository;
        public IItemRepository ItemRepository => _itemRepository;
        public IItemCategoryRepository ItemCategoryRepository => _itemCategoryRepository;
        public IItemSubCategoryRepository ItemSubCategoryRepository => _itemSubCategoryRepository;
        public ISalePersonRepository SalePersonRepository => _salePersonRepository;
        public ISalePersonLoaderRepository SalePersonLoaderRepository => _salePersonLoaderRepository;
        public ISupervisorAssignSectorRepository SupervisorAssignSectorRepository => _supervisorAssignSectorRepository;
        public ISectorAssignSaleMenRepository SectorAssignSaleMenRepository => _sectorAssignSaleMenRepository;
        public ISaleRepository SaleRepository => _saleRepository;
        public ISaleParameterRepository SaleParameterRepository => _saleParameterRepository;
        public ILoaderParameterRepository LoaderParameterRepository => _loaderParameterRepository;
        public ISaleBillTypeParameterRepository SaleBillTypeParameterRepository => _saleBillTypeRepository;
        public ISaleReturnRepository SaleReturnRepository => _saleReturnRepository;
        public ISaleReturnParameterRepository SaleReturnParameterRepository => _saleReturnParameterRepository;
        public ISupplierRepository SupplierRepository => _supplierRepository;
        public IPurchaseRepository PurchaseRepository => _purchaseRepository;
        public IPurchaseParameterRepository PurchaseParameterRepository => _purchaseParameterRepository;
        public IFunctionRepository FunctionRepository => _functionRepository;
        public IPageRepository PageRepository => _pageRepository;
        public IPageControlRepository PageControlRepository => _pageControlRepository;
        public IPurchaseReturnRepository PurchaseReturnRepository => _purchaseReturnRepository;
        public IPurchaseReturnParameterRepository PurchaseReturnParameterRepository => _purchaseReturnParameterRepository;
        public ICountryRepository CountryRepository => _countryRepository;
        public IProvinceRepository ProvinceRepository => _provinceRepository;
        public IDistrictRepository DistrictRepository => _districtRepository;
        public ICityRepository CityRepository => _cityRepository;
        public IRoleRepository RoleRepository => _roleRepository;
        public IRolePagePermissionRepository RolePagePermissionRepository => _rolePagePermissionRepository;
        public IRoleControlPermissionRepository RoleControlPermissionRepository => _roleControlPermissionRepository;
        public ISaleDashboardRepository SaleDashboardRepository => _saleDashboardRepository;
        public ICompanySetupRepository CompanySetupRepository => _companySetupRepository;
        public IGatePassNumberRepository GatePassNumberRepository => _gatePassNumberRepository;
        public IAccountDetailRepository AccountDetailRepository => _accountDetailRepository;

        public IAccountGroupRepository AccountGroupRepository => _accountGroupRepository;
        public IAccountSubGroupRepository AccountSubGroupRepository => _accountSubGroupRepository;
        public IAccountDetailTypeParameterRepository AccountDetailTypeParameterRepository=> _accountDetailTypeParameterRepository;
        public IGeneralLedgerRepository GeneralLedgerRepository => _generalLedgerRepository;
        public IGeneralLedgerParametersRepository GeneralLedgerParametersRepository => _generalLedgerParametersRepository;
        public IBookRepository BookRepository => _bookRepository;
        public IBookParameterRepository BookParameterRepository => _bookParameterRepository;
        public IBookTypeRepository BookTypeRepository=> _bookTypeRepository;
        public IBankRepository BankRepository=> _bankRepository;

        public IGlobalAccountRepository GlobalAccountRepository => _globalAccountRepository;
        public IChequeRepository ChequeRepository => _chequeRepository;
        public ILicenseRepository LicenseRepository => _licenseRepository;

        public IStockConfigurationRepository stockConfigurationRepository => _stockConfigurationRepository;
        public IStockConfigMasterRepository stockConfigMasterRepository => _stockConfigMasterRepository;

        public IStockRepository stockRepository => _stockRepository;
        public IStockAdjustmentRepository stockAdjustmentRepository => _stockAdjustmentRepository;
        public IStockAdjustmentParameterRepository stockAdjustmentParameterRepository => _stockAdjustmentParameterRepository;



        #endregion

        #region Contrutor pass parameter and initial object using dependency injection
        public UnitOfWork(
            ApplicationDbContext context,
        ICustomerRepository customerRepository,
        IApplicationUserRepository applicationUserRepository,
        ILookUpNameRepository lookUpNameRepository,
        IItemRateRepository itemRateRepository,
        IItemRepository itemRepository,
        IItemCategoryRepository itemCategoryRepository,
        IItemSubCategoryRepository itemSubCategoryRepository,
        ISalePersonRepository salePersonRepository,
        ISalePersonLoaderRepository salePersonLoaderRepository,
        ISupervisorAssignSectorRepository supervisorAssignSectorRepository,
        ISectorAssignSaleMenRepository sectorAssignSaleMenRepository,
        ISaleRepository saleRepository,
        ISaleParameterRepository saleParameterRepository,
        ILoaderParameterRepository loaderParameterRepository,
        ISaleBillTypeParameterRepository saleBillTypeRepository,
        ISaleReturnRepository saleReturnRepository,
        ISaleReturnParameterRepository saleReturnParameterRepository,
        ISupplierRepository supplierRepository,
        IPurchaseRepository purchaseRepository,
        IPurchaseParameterRepository purchaseParameterRepository,
        IFunctionRepository functionRepository,
        IPageRepository pageRepository,
        IPageControlRepository pageControlRepository,
        IRoleRepository roleRepository,
        IRolePagePermissionRepository rolePagePermissionRepository,
        IRoleControlPermissionRepository roleControlPermissionRepository,
        ICountryRepository countryRepository,
        IProvinceRepository provinceRepository,
        IDistrictRepository districtRepository,
        ICityRepository cityRepository,
        IPurchaseReturnRepository purchaseReturnRepository,
        IPurchaseReturnParameterRepository purchaseReturnParameterRepository,
        ISaleDashboardRepository saleDashboardRepository,
        ICompanySetupRepository companySetupRepository,
        IGatePassNumberRepository gatePassNumberRepository,
        IAccountDetailRepository accountDetailRepository,
        IAccountGroupRepository accountGroupRepository,
        IAccountSubGroupRepository accountSubGroupRepository,
        IAccountDetailTypeParameterRepository accountDetailTypeParameterRepository,
        IBookRepository bookRepository,
        IBookParameterRepository bookParameterRepository,
        IGeneralLedgerRepository generalLedgerRepository,
        IGeneralLedgerParametersRepository generalLedgerParametersRepository,
        IBookTypeRepository bookTypeRepository,
        IBankRepository bankRepository,
        IGlobalAccountRepository globalAccountRepository,
        IChequeRepository chequeRepository,
        ILicenseRepository licenseRepository,
        IStockConfigurationRepository stockConfigurationRepository,
        IStockConfigMasterRepository stockConfigMasterRepository,
        IStockRepository stockRepository,
        IStockAdjustmentRepository stockAdjustmentRepository,
        IStockAdjustmentParameterRepository stockAdjustmentParameterRepository




            )
        {
            _context = context;
            _customerRepository = customerRepository;
            _applicationUserRepository = applicationUserRepository;
            _lookUpNameRepository = lookUpNameRepository;
            _itemRateRepository = itemRateRepository;
            _itemRepository = itemRepository;
            _itemCategoryRepository = itemCategoryRepository;
            _itemSubCategoryRepository = itemSubCategoryRepository;
            _salePersonRepository = salePersonRepository;
            _salePersonLoaderRepository = salePersonLoaderRepository;
            _supervisorAssignSectorRepository = supervisorAssignSectorRepository;
            _sectorAssignSaleMenRepository = sectorAssignSaleMenRepository;
            _saleRepository = saleRepository;
            _saleParameterRepository = saleParameterRepository;
            _loaderParameterRepository = loaderParameterRepository;
            _saleBillTypeRepository = saleBillTypeRepository;
            _saleReturnRepository = saleReturnRepository;
            _saleReturnParameterRepository = saleReturnParameterRepository;
            _supplierRepository = supplierRepository;
            _purchaseRepository = purchaseRepository;
            _purchaseParameterRepository = purchaseParameterRepository;
            _functionRepository = functionRepository;
            _pageRepository = pageRepository;
            _pageControlRepository = pageControlRepository;
            _roleRepository = roleRepository;
            _rolePagePermissionRepository = rolePagePermissionRepository;
            _roleControlPermissionRepository = roleControlPermissionRepository;
            _countryRepository = countryRepository;
            _provinceRepository = provinceRepository;
            _districtRepository = districtRepository;
            _cityRepository = cityRepository;
            _purchaseReturnRepository = purchaseReturnRepository;
            _purchaseReturnParameterRepository = purchaseReturnParameterRepository;
            _saleDashboardRepository = saleDashboardRepository;
            _companySetupRepository = companySetupRepository;
            _gatePassNumberRepository = gatePassNumberRepository;
            _accountDetailRepository = accountDetailRepository;
            _accountGroupRepository = accountGroupRepository;
            _accountSubGroupRepository = accountSubGroupRepository;
            _accountDetailTypeParameterRepository = accountDetailTypeParameterRepository;
            _generalLedgerRepository = generalLedgerRepository;
            _generalLedgerParametersRepository = generalLedgerParametersRepository;
            _bookRepository = bookRepository;
            _bookParameterRepository = bookParameterRepository;
            _bookTypeRepository = bookTypeRepository;
            _bankRepository = bankRepository;
            _globalAccountRepository = globalAccountRepository;
            _chequeRepository = chequeRepository;
            _licenseRepository = licenseRepository;
            _stockConfigurationRepository = stockConfigurationRepository;
            _stockConfigMasterRepository = stockConfigMasterRepository;
            _stockConfigMasterRepository = stockConfigMasterRepository;
            _stockRepository = stockRepository;
            _stockAdjustmentRepository = stockAdjustmentRepository;
            _stockAdjustmentParameterRepository = stockAdjustmentParameterRepository;







            _repositories = new Dictionary<string, object>();
        }
        #endregion


        // Generic repositories cache
        private readonly Dictionary<string, object> _repositories;

        // Method to get the repository for a specific entity
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            var entityType = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(entityType))
            {
                var repositoryType = typeof(GenericRepository<>).MakeGenericType(typeof(TEntity));
                var repositoryInstance = Activator.CreateInstance(repositoryType, _context);
                _repositories.Add(entityType, repositoryInstance);
            }

            return (IGenericRepository<TEntity>)_repositories[entityType];
        }


        public DbContext Context => _context; 
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Database update failed. See inner exception for details.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving changes.", ex);
            }
        }


        public void Dispose()
        {
            _context.Dispose();
        }


        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }




    }
}
