using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.Api.Helpers
{
    public static class DateTimeOffsetExtensions
    {
        public static int GetCurrentAge(this DateTimeOffset dateTimeOffSet)
        {
            var currentData = DateTime.UtcNow;
            int age = currentData.Year - dateTimeOffSet.Year;

            if (currentData < dateTimeOffSet.AddYears(age))
            {
                age--;
            }

            return age;
        }
    }
}
