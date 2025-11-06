using AbpSolution1.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpSolution1
{
    public class DomainEnums
    {
        public enum Gender
        {
            [StringValue("Gender_Male")]
            Male = 0,

            [StringValue("Gender_Female")]
            Female = 1
        }

        public enum Ranked
        {
            [StringValue("Ranked_Normal")]
            Normal = 0,
            [StringValue("Ranked_Bronze")]
            Bronze = 1,
            [StringValue("Ranked_Silver")]
            Silver = 2,
            [StringValue("Ranked_Gold")]
            Gold = 3,
            [StringValue("Ranked_Diamond")]
            Diamond = 4,
            [StringValue("Ranked_Platinum")]
            Platinum = 5
        }
    }
}
