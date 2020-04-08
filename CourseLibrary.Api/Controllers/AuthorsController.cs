using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CourseLibrary.Api.Helpers;
using CourseLibrary.Api.Models;
using CourseLibrary.Api.ResourceParameters;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.Api.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

        }

        [HttpGet()]
        [HttpHead]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery]AuthorsResourceParameters authorsResourceParameters)
        {
            try
            {
                //throw new Exception("TEST__");
                var resultAuthors = _courseLibraryRepository.GetAuthors(authorsResourceParameters);

                return Ok(_mapper.Map<IEnumerable<AuthorDto>>(resultAuthors));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure __");
            } 
            #region Mapper antiguio
            //var authors = new List<AuthorDto>();
            //foreach (var author in resultAuthors)
            //{
            //    authors.Add(new AuthorDto()
            //    {
            //        Id = author.Id,
            //        Name = $"{author.FirstName} {author.LastName}",
            //        MainCategory = author.MainCategory,
            //        Age = author.DateOfBirth.GetCurrentAge()
            //    });                
            //}

            //return Ok(authors);
            #endregion
        }

        [HttpGet("{authorId}", Name ="GetAuthor")]
        public IActionResult GetAuthors(Guid authorId)
        {
            try
            {
                var resultAuthor = _courseLibraryRepository.GetAuthor(authorId);
                if (resultAuthor == null) return NotFound();                        
                
                return Ok(_mapper.Map<AuthorDto>(resultAuthor));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure __");
            }
        }

        [HttpPost]
        public ActionResult<AuthorDto> CreateAuthor(AuthorForCreationDto authorForCreation)
        {
            //Validate JSON
            if (authorForCreation == null) return BadRequest();
            var authorEntity = _mapper.Map<Author>(authorForCreation);
            _courseLibraryRepository.AddAuthor(authorEntity);
            _courseLibraryRepository.Save();

            var authorReturn = _mapper.Map<AuthorDto>(authorEntity);
            return CreatedAtRoute("GetAuthor",
                new { authorId = authorReturn.Id },
                authorReturn);
        }

        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }

        [HttpDelete("{authorId}")]
        public  ActionResult DeleteUthor(Guid authorId)
        {
            var author = _courseLibraryRepository.GetAuthor(authorId);
            if (author == null) return NotFound();

            _courseLibraryRepository.DeleteAuthor(author);
            _courseLibraryRepository.Save();

            return NoContent();
        }
    }
}