using AbpSolution1.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace AbpSolution1.Permissions;

public class AbpSolution1PermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AbpSolution1Permissions.GroupName, L("Permission:AbpSolution1"));

        // ✅ Departments
        var departmentPermission = myGroup.AddPermission(
            AbpSolution1Permissions.Departments.Default,
            L("Permission:Departments")
        );
        departmentPermission.AddChild(AbpSolution1Permissions.Departments.Create, L("Permission:Departments.Create"));
        departmentPermission.AddChild(AbpSolution1Permissions.Departments.Edit, L("Permission:Departments.Edit"));
        departmentPermission.AddChild(AbpSolution1Permissions.Departments.Delete, L("Permission:Departments.Delete"));

        var employeePermission = myGroup.AddPermission(AbpSolution1Permissions.Employees.Default, L("Permission:Employees"));
        employeePermission.AddChild(AbpSolution1Permissions.Employees.Create, L("Permission:Employees.Create"));
        employeePermission.AddChild(AbpSolution1Permissions.Employees.Edit, L("Permission:Employees.Edit"));
        employeePermission.AddChild(AbpSolution1Permissions.Employees.Delete, L("Permission:Employees.Delete"));

        var booksPermission = myGroup.AddPermission(AbpSolution1Permissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(AbpSolution1Permissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(AbpSolution1Permissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(AbpSolution1Permissions.Books.Delete, L("Permission:Books.Delete"));

        // ✅ Customers
        var customersPermission = myGroup.AddPermission(
            AbpSolution1Permissions.Customers.Default,
            L("Permission:Customers")
        );
        customersPermission.AddChild(AbpSolution1Permissions.Customers.Create, L("Permission:Customers.Create"));
        customersPermission.AddChild(AbpSolution1Permissions.Customers.Edit, L("Permission:Customers.Edit"));
        customersPermission.AddChild(AbpSolution1Permissions.Customers.Delete, L("Permission:Customers.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpSolution1Resource>(name);
    }
}
