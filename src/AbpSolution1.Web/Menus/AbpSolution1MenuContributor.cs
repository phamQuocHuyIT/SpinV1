using AbpSolution1.Localization;
using AbpSolution1.MultiTenancy;
using AbpSolution1.Permissions;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace AbpSolution1.Web.Menus;

public class AbpSolution1MenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private static Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<AbpSolution1Resource>();

        // 🏠 Home
        context.Menu.AddItem(
            new ApplicationMenuItem(
                AbpSolution1Menus.Home,
                l["Menu:Home"],
                "~/",
                icon: "fa fa-home",
                order: 1
            )
        );

        // ⚙️ Administration group
        var administration = context.Menu.GetAdministration();
        administration.Order = 6;

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 1);

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 2);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);

        // 📚 Books group
        context.Menu.AddItem(
            new ApplicationMenuItem(
                "BooksStore",
                l["Menu:AbpSolution1"],
                icon: "fa fa-book"
            ).AddItem(
                new ApplicationMenuItem(
                    "BooksStore.Books",
                    l["Menu:Books"],
                    url: "/Books"
                ).RequirePermissions(AbpSolution1Permissions.Books.Default)
            )
        );
        // Organizational group
        context.Menu.AddItem(
            new ApplicationMenuItem(
                "Organizational",
                l["Menu:Organizational"],
                icon: "fa fa-building"
            ).AddItem(
                new ApplicationMenuItem(
                    "Organizational.Departments",
                    l["Menu:Departments"],
                    url: "/Departments"
                ).RequirePermissions(AbpSolution1Permissions.Departments.Default)
            ).AddItem(
                new ApplicationMenuItem(
                    "Organizational.Employees",
                    l["Menu:Employees"],
                    url: "/Employees"
                ).RequirePermissions(AbpSolution1Permissions.Employees.Default)
            ).AddItem(
                new ApplicationMenuItem(
                    "Organizational.Customers",
                    l["Menu:Customers"],
                    url: "/Customers"
                ).RequirePermissions(AbpSolution1Permissions.Customers.Default)
            )
        );
        // Config group
        context.Menu.AddItem(
            new ApplicationMenuItem(
                "Config",
                l["Menu:Config"],
                icon: "fa fa-cog"
            ).AddItem(
                new ApplicationMenuItem(
                    "Organizational.Products",
                    l["Menu:Products"],
                    url: "/Products"
                ).RequirePermissions(AbpSolution1Permissions.Products.Default)
            ).AddItem(
                new ApplicationMenuItem(
                    "Organizational.Spins",
                    l["Menu:Spins"],
                    url: "/Spins"
                ).RequirePermissions(AbpSolution1Permissions.Spins.Default)
            )
        );

        return Task.CompletedTask;
    }
}
