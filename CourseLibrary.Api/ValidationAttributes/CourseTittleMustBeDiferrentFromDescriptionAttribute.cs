using CourseLibrary.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLibrary.Api.ValidationAttributes
{
    public class CourseTittleMustBeDiferrentFromDescriptionAttribute :  ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var course = (CourseForManipulationDto)validationContext.ObjectInstance;

            if (course.Title == course.Description)
            {
                return new ValidationResult(ErrorMessage,
                    new[] { nameof(CourseForManipulationDto) });
            }

            return ValidationResult.Success;
        }
    }
}





