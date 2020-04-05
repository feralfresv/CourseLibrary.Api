using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLibrary.Api.Models
{
    public class AuthorForCreationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateofBirht { get; set; }
        public string MainCategory { get; set; }
        public ICollection<CourseForCreationDto> Course { get; set; } 
            = new List<CourseForCreationDto>();

    }
}
