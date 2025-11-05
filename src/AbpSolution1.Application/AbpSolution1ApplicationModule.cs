using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.Account;
using Volo.Abp.Identity;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;
using AbpSolution1.Interface.Administration.Customer;
using AbpSolution1.Service.Administration.Customer;
using Microsoft.Extensions.DependencyInjection;

namespace AbpSolution1;

[DependsOn(
    typeof(AbpSolution1DomainModule),
    typeof(AbpSolution1ApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
    )]
public class AbpSolution1ApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpSolution1ApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AbpSolution1ApplicationModule>(validate: true);
        });

        // Register CustomerAppService
        context.Services.AddTransient<ICustomerAppService, CustomerAppService>();
    }
}
