using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using AbpSolution1.Shared.Extensions; // Extension method GetStringValue
using static AbpSolution1.DomainEnums; // Enum Gender
using AbpSolution1.Localization; // AbpSolution1Resource

namespace AbpSolution1
{
    public static class DomainHelps
    {
        // Lấy text localization từ enum int
        public static string GenderText(int status, IStringLocalizer<AbpSolution1Resource> L)
        {
            if (!Enum.IsDefined(typeof(Gender), status))
                return L["MissingData"];

            var genderEnum = (Gender)status;
            return L[genderEnum.GetStringValue()];
        }

        // Tạo danh sách SelectListItem cho dropdown
        public static List<SelectListItem> ListGender(int? currentValue, IStringLocalizer<AbpSolution1Resource> L)
        {
            return Enum.GetValues(typeof(Gender))
                .Cast<Gender>()
                .Select(g => new SelectListItem(
                    text: L[g.GetStringValue()],
                    value: ((int)g).ToString(),
                    selected: currentValue.HasValue && currentValue.Value == (int)g
                )).ToList();
        }

        public static string RankedText(int status, IStringLocalizer<AbpSolution1Resource> L)
        {
            if (!Enum.IsDefined(typeof(Ranked), status))
                return L["MissingData"];

            var rankedEnum = (Ranked)status;
            return L[rankedEnum.GetStringValue()];
        }

        // Tạo danh sách SelectListItem cho dropdown
        public static List<SelectListItem> ListRanked(int? currentValue, IStringLocalizer<AbpSolution1Resource> L)
        {
            return Enum.GetValues(typeof(Ranked))
                .Cast<Ranked>()
                .Select(g => new SelectListItem(
                    text: L[g.GetStringValue()],
                    value: ((int)g).ToString(),
                    selected: currentValue.HasValue && currentValue.Value == (int)g
                )).ToList();
        }
    }
}
