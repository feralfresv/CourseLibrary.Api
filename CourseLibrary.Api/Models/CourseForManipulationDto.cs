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
    public abstract class CourseForManipulationDto
    {
        [Required(ErrorMessage = "Requerido")]
        [MaxLength(100, ErrorMessage = "maximo 100")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Requerido")]
        [MaxLength(1500, ErrorMessage = "maximo 1500")]
        public virtual string Description { get; set; }
    }
}
