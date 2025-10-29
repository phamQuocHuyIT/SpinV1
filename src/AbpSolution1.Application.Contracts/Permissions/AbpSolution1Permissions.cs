using System.Diagnostics.Contracts;
using Volo.Abp.Uow;

namespace AbpSolution1.Permissions;

public static class AbpSolution1Permissions
{
    public const string GroupName = "AbpSolution1";

    public static class Books
    {
        public const string Default = GroupName + ".Books";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class Departments
    {
        public const string Default = GroupName + ".Administration.Departments";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
