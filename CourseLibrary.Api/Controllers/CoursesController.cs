using AutoMapper;
using CourseLibrary.Api.Models;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

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

        [HttpGet("{courseId}", Name ="GetCourseFOrAuthor")]
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

        [HttpPost()]
        public ActionResult<CourseForCreationDto> CreateCOurseForAuthor(Guid authorId, CourseForCreationDto course)
             {
            //Validate {authorId}
            if (!_courseLibraryRepository.AuthorExists(authorId)) return NotFound();
            if (course == null) return BadRequest();

            var courseEntity = _mapper.Map<Course>(course);
            _courseLibraryRepository.AddCourse(authorId, courseEntity);
            _courseLibraryRepository.Save();

            var courseReturn = _mapper.Map<CourseDto>(courseEntity);
            return CreatedAtRoute("GetCourseFOrAuthor",
                new { authorId = authorId, courseId = courseReturn.Id }, courseReturn);
        }

        [HttpPut("{courseId}")]
        public ActionResult UpdateCourseAuthor(Guid authorId, Guid courseId, CourseForUpdateDto course)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId)) return NotFound();

            var courseForAuthorFromRepo = _courseLibraryRepository.GetCourse(authorId, courseId);

            if (courseForAuthorFromRepo == null)
            {
                var courseAdd = _mapper.Map<Course>(course);
                courseAdd.Id = courseId;

                _courseLibraryRepository.AddCourse(authorId,courseAdd);
                _courseLibraryRepository.Save();

                var courseToReturn = _mapper.Map<CourseDto>(courseAdd);

                return CreatedAtRoute("GetCourseFOrAuthor",
                    new {authorId, courseId = courseToReturn.Id }, courseToReturn);

            }

            // map the entity to a CourseForUpdateDto
            // apply the updated field values to that dto
            // map the CourseForUpdateDto back to an entity
            _mapper.Map(course, courseForAuthorFromRepo);
            _courseLibraryRepository.UpdateCourse(courseForAuthorFromRepo);
            _courseLibraryRepository.Save();

            return Ok(courseForAuthorFromRepo);

        }

        [HttpPatch("{courseId}")]

        public ActionResult PartiallyUpdateCourseForAuthor
            (Guid authorId, Guid courseId, JsonPatchDocument<CourseForUpdateDto> patchDocument)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId)) return NotFound();
            var courseForAuthorFromRepo = _courseLibraryRepository.GetCourse(authorId, courseId);
            if (courseForAuthorFromRepo == null)
            {
                var courseDto = new CourseForUpdateDto();
                patchDocument.ApplyTo(courseDto, ModelState);

                if (!TryValidateModel(courseDto))
                {
                    return ValidationProblem(ModelState);
                }

                var courseToAdd = _mapper.Map<Course>(courseDto);
                courseToAdd.Id = courseId;

                _courseLibraryRepository.AddCourse(authorId, courseToAdd);
                _courseLibraryRepository.Save();

                var courseToReturn = _mapper.Map<CourseDto>(courseToAdd);

                return CreatedAtRoute("GetCourseForAuthor",
                    new { authorId, courseId = courseToReturn.Id },
                    courseToReturn);
            }

            var courseToPatch = _mapper.Map<CourseForUpdateDto>(courseForAuthorFromRepo);
            patchDocument.ApplyTo(courseToPatch, ModelState);

           if(!TryValidateModel(courseToPatch)) return ValidationProblem(ModelState);

            _mapper.Map(courseToPatch, courseForAuthorFromRepo);
            _courseLibraryRepository.UpdateCourse(courseForAuthorFromRepo);
            _courseLibraryRepository.Save();

            return NoContent();
        }

        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
