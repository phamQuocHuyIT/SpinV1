using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Data;

namespace AbpSolution1.Administration.Customer
{
    public class Customers : FullAuditedAggregateRoot<int>, IHasExtraProperties, IHasConcurrencyStamp
    {
        public string Code { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string NumberPhone { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string Note { get; set; } = string.Empty;

        // ✅ Constructor bắt buộc để fix lỗi CS9035
        public Customers()
        {
            ExtraProperties = new ExtraPropertyDictionary();
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }
    }
}
