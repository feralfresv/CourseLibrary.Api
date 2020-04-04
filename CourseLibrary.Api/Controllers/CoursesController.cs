using AutoMapper;
using CourseLibrary.Api.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/authors/{authorId}/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public CoursesController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> GetCoursesForAuthor(Guid authorId)
        {
            try
            {
                //Validate {authorId}
                if (!_courseLibraryRepository.AuthorExists(authorId)) return NotFound();

                var GetCourses = _courseLibraryRepository.GetCourses(authorId);

                return Ok(_mapper.Map<IEnumerable<CourseDto>>(GetCourses));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure __");
            }
        }

        [HttpGet("{courseId}")]
        public ActionResult<CourseDto> GetCourseForAuthor(Guid authorId, Guid courseId)
        {
            try
            {
                //Validate {authorId}
                if (!_courseLibraryRepository.AuthorExists(authorId)) return NotFound();
                //Validate {courseId}
                var resultCourseAuthor = _courseLibraryRepository.GetCourse(authorId, courseId);
                if (resultCourseAuthor == null) return NotFound();

                return Ok(_mapper.Map<CourseDto>(resultCourseAuthor));
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure __");
            }
        }
    }
}
