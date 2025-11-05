using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace AbpSolution1.MultiTenancy
{
    public static class AbpSession
    {
        // Lưu instance CurrentTenant tĩnh (set từ DI lúc khởi tạo ứng dụng)
        public static ICurrentTenant CurrentTenant { get; set; }

        // Property TenantId tiện dụng
        public static int? TenantId
        {
            get
            {
                if (CurrentTenant == null)
                    return null;

                // Nếu TenantId trong entity là int? thì convert từ Guid? của ABP 5.x
                return CurrentTenant.Id.HasValue ? Convert.ToInt32(CurrentTenant.Id.Value) : (int?)null;
            }
        }
    }
}
