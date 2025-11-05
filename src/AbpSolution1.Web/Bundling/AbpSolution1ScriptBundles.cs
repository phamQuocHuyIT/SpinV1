using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.Modularity;

namespace AbpSolution1.Web.Bundling;

[DependsOn(typeof(JQueryScriptContributor))]
public class AbpSolution1ScriptBundles : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/Pages/Customers/index.js");
        context.Files.AddIfNotContains("/Pages/Departments/index.js");
    }
}