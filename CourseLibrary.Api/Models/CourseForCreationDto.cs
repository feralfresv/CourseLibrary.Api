using CourseLibrary.Api.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLibrary.Api.Models
{
    [CourseTittleMustBeDiferrentFromDescriptionAttribute(
        ErrorMessage = "Error en [CourseTitle.etc]")]
    public class CourseForCreationDto //: IValidatableObject
    {
        [Required( ErrorMessage = "Requerido")]
        [MaxLength(100, ErrorMessage = "maximo 100")]
        public string Title { get; set; }

        [MaxLength(1500, ErrorMessage = "maximo 1500")]
        public string Description { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if(Title == Description)
        //    {
        //        yield return new ValidationResult(
        //            "To be different Titlle and descripcion ", new[] { "CreadoPOrMi" });
        //    }
        //}
    }
}
