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
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/authorcollection")]
    public class AuthorCollectionController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public AuthorCollectionController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentException(nameof(courseLibraryRepository));

            _mapper = mapper ??
                throw new ArgumentException(nameof(mapper));
        }

        //array key : key1,key2,key3
        //composite key : key1=value1,key2=value2
        [HttpGet("({ids})", Name = "GetAuthorCollection")]
        public IActionResult GetAuthorCollection(
            [FromRoute]
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var authorEntities = _courseLibraryRepository.GetAuthors(ids);

            if (ids.Count() != authorEntities.Count())
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<AuthorModel>>(authorEntities));
        }

        [HttpPost]
        public ActionResult<IEnumerable<AuthorModel>> CreateAuthorCollection(IEnumerable<AuthorForCreationVM> authorcollection)
        {
            var authorEntity = _mapper.Map<IEnumerable<Author>>(authorcollection);

            foreach (var author in authorEntity)
            {
                _courseLibraryRepository.AddAuthor(author);
            }

            _courseLibraryRepository.Save();

            var authorCollectionToReturn = _mapper.Map<IEnumerable<AuthorModel>>(authorEntity);
            var idsAsString = string.Join(",", authorCollectionToReturn.Select(x => x.Id));

            return CreatedAtRoute("GetAuthorCollection",
                new { ids = idsAsString },
                authorCollectionToReturn);
        }
    }
}
