using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Helpers;
using WebApplication1.Models;
using WebApplication1.ResourceParameters;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository,IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentException(nameof(courseLibraryRepository));
            _mapper = mapper ??
                throw new ArgumentException(nameof(mapper));

        }

        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<AuthorModel>> GetAuthors([FromQuery] AuthorsResourceParameters authorsResourceParameters)
        {
            var authorsFromRepo = _courseLibraryRepository.GetAuthors(authorsResourceParameters);
            return Ok(_mapper.Map<IEnumerable<AuthorModel>>(authorsFromRepo));
        }

        [HttpGet("{authorId:guid}", Name = "GetAuthor")]
        public ActionResult<AuthorModel> GetAuthor(Guid authorId)
        {
            var authorsFromRepo = _courseLibraryRepository.GetAuthor(authorId);

            if (authorsFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AuthorModel>(authorsFromRepo));
        }

        [HttpPost]
        public ActionResult<AuthorModel> CreateAuthor(AuthorForCreationVM author)
        {
            var authorEntity = _mapper.Map<Author>(author);
            _courseLibraryRepository.AddAuthor(authorEntity);
            _courseLibraryRepository.Save();

            var authorToReturn = _mapper.Map<AuthorModel>(authorEntity);
            return CreatedAtRoute("GetAuthor", new { authorId = authorToReturn.Id }, authorToReturn);
        }

        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

    }
}
