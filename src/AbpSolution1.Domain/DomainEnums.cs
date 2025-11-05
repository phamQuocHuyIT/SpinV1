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
    }
}
