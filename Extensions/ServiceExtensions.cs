using PayRoleSystem.Core.Aspects.Logging;
using PayRoleSystem.CustomMiddlewares;
using PayRoleSystem.Mapping;
using PayRoleSystem.Repositories;
using PayRoleSystem.Repositories.Interfaces;
using PayRoleSystem.Services;
using PayRoleSystem.Services.Interfaces;
using PayRoleSystem.UOW;

namespace PayRoleSystem.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterAllDependencies(this IServiceCollection services)
        {
            services.RegisterRepositories();
            services.RegisterServices();
            services.RegisterCoreServices();
            return services;
        }

        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        { 
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRolePagePermissionRepository, RolePagePermissionRepository>();
            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped<IRoleService, RoleService>();
            return services;
        }

        public static IServiceCollection RegisterCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(Program));
            services.AddHttpContextAccessor();
            services.AddScoped<AuditLoggingInterceptor>();
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }
         
        public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            return app;
        }
    }
}
