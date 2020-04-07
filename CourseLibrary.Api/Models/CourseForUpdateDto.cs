using CourseLibrary.Api.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLibrary.Api.Models
{
    public class CourseForUpdateDto : CourseForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out a descrioption__")]
        public override string Description { get => base.Description; set => base.Description = value; }
    }
}
