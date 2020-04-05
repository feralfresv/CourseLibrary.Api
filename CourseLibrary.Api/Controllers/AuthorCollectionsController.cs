using AutoMapper;
using CourseLibrary.Api.Models;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using CourseLibrary.Api.Helpers;

namespace CourseLibrary.Api.Controllers
{
    [Route("api/authorcollections")]
    [ApiController]
    public class AuthorsCollectionsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public AuthorsCollectionsController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

        }


        [HttpGet("({ids})", Name = "GetAuthorCollection")]
        public IActionResult GetAuthorCollection(
        [FromRoute]
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null) return BadRequest();

            var authoEntities = _courseLibraryRepository.GetAuthors(ids);
            if (ids.Count() != authoEntities.Count()) return NotFound();

            var authoorReturn = _mapper.Map<IEnumerable<AuthorDto>>(authoEntities);

            return Ok(authoorReturn);
        }

        
        [HttpPost()]
        public ActionResult<IEnumerable<AuthorDto>> CreateAuthorCollection(IEnumerable<AuthorForCreationDto> authorForCreationDto)
        {
            if (authorForCreationDto == null) return BadRequest();

            var authorEntity = _mapper.Map<IEnumerable<Author>>(authorForCreationDto);
            foreach (var author in authorEntity)
            {
                _courseLibraryRepository.AddAuthor(author);
            }
            _courseLibraryRepository.Save();

            var authorCollectionToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntity);
            var idsAsString = string.Join(",",authorCollectionToReturn.Select(a => a.Id));
            return CreatedAtRoute("GetAuthorCollection", new { ids = idsAsString }, authorCollectionToReturn);   
        }

    }
}
