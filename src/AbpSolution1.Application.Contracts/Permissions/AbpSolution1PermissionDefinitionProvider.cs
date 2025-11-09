using AbpSolution1.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace AbpSolution1.Permissions;

public class AbpSolution1PermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AbpSolution1Permissions.GroupName);

        var departmentPermission = myGroup.AddPermission(AbpSolution1Permissions.Departments.Default,L("Permission:Departments"));
        departmentPermission.AddChild(AbpSolution1Permissions.Departments.Create, L("Permission:Departments.Create"));
        departmentPermission.AddChild(AbpSolution1Permissions.Departments.Edit, L("Permission:Departments.Edit"));
        departmentPermission.AddChild(AbpSolution1Permissions.Departments.Delete, L("Permission:Departments.Delete"));

        var employeePermission = myGroup.AddPermission(AbpSolution1Permissions.Employees.Default, L("Permission:Employees"));
        employeePermission.AddChild(AbpSolution1Permissions.Employees.Create, L("Permission:Employees.Create"));
        employeePermission.AddChild(AbpSolution1Permissions.Employees.Edit, L("Permission:Employees.Edit"));
        employeePermission.AddChild(AbpSolution1Permissions.Employees.Delete, L("Permission:Employees.Delete"));

        var customerPermission = myGroup.AddPermission(AbpSolution1Permissions.Customers.Default, L("Permission:Customers"));
        customerPermission.AddChild(AbpSolution1Permissions.Customers.Create, L("Permission:Customers.Create"));
        customerPermission.AddChild(AbpSolution1Permissions.Customers.Edit, L("Permission:Customers.Edit"));
        customerPermission.AddChild(AbpSolution1Permissions.Customers.Delete, L("Permission:Customers.Delete"));

        var productPermission = myGroup.AddPermission(AbpSolution1Permissions.Products.Default, L("Permission:Products"));
        productPermission.AddChild(AbpSolution1Permissions.Products.Create, L("Permission:Products.Create"));
        productPermission.AddChild(AbpSolution1Permissions.Products.Edit, L("Permission:Products.Edit"));
        productPermission.AddChild(AbpSolution1Permissions.Products.Delete, L("Permission:Products.Delete"));

        var spinPermission = myGroup.AddPermission(AbpSolution1Permissions.Spins.Default, L("Permission:Spins"));
        spinPermission.AddChild(AbpSolution1Permissions.Spins.Create, L("Permission:Spins.Create"));
        spinPermission.AddChild(AbpSolution1Permissions.Spins.Edit, L("Permission:Spins.Edit"));
        spinPermission.AddChild(AbpSolution1Permissions.Spins.Delete, L("Permission:Spins.Delete"));

        var booksPermission = myGroup.AddPermission(AbpSolution1Permissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(AbpSolution1Permissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(AbpSolution1Permissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(AbpSolution1Permissions.Books.Delete, L("Permission:Books.Delete"));
        //Define your own permissions here. Example:
        //myGroup.AddPermission(AbpSolution1Permissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpSolution1Resource>(name);
    }
}
