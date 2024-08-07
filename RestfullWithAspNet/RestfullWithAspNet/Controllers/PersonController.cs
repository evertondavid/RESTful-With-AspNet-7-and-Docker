using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestfullWithAspNet.Business;
using RestfullWithAspNet.Data.VO;
using HATEOAS.Hypernedia.Filters;

namespace RestfullWithAspNet.Controllers
{
    /// <summary>
    /// Controller for manipulating Person data.
    /// </summary>
    [ApiVersion("1")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/[controller]/v{version:ApiVersion}")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private IPersonBusiness _personBusiness;

        /// <summary>
        /// Constructor for the Person controller.
        /// </summary>
        /// <param name="logger">Logger to register events or problems.</param>
        /// <param name="personBusiness">Service for manipulating Person data.</param>
        public PersonController(ILogger<PersonController> logger, IPersonBusiness personBusiness)
        {
            _logger = logger;
            _personBusiness = personBusiness;
        }

        /* Disabled after implemented Find With Paged Search
        /// <summary>
        /// Gets all people.
        /// </summary>
        /// <returns>A list of people.</returns>
        /// <remarks>
        /// Maps GET requests to https://localhost:44300/api/person
        /// </remarks>
        [HttpGet]
        [ProducesResponseType((200), Type = typeof(List<PersonVO>))]
        [ProducesResponseType((204))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get()
        {
            return Ok(_personBusiness.FindAll());
        }
        */

        /// <summary>
        /// Gets all people with a paged search.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="page">The page number.</param>
        /// <returns>A paged search result.</returns>
        [HttpGet("{sortDirection}/{pageSize}/{page}")]
        [ProducesResponseType((200), Type = typeof(List<PersonVO>))]
        [ProducesResponseType((204))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(
            [FromQuery] string? name,
            string sortDirection,
            int pageSize,
            int page)
        {
            if (name == null)
            {
                return Ok(_personBusiness.FindWithPagedSearch(sortDirection, pageSize, page));
            }
            return Ok(_personBusiness.FindWithPagedSearch(name, sortDirection, pageSize, page));
        }

        /// <summary>
        /// Gets a person by their ID.
        /// </summary>
        /// <param name="id">The ID of the person.</param>
        /// <returns>The person with the specified ID.</returns>
        /// <remarks>
        /// Maps GET requests to https://localhost:44300/api/person/{id}
        /// </remarks>
        [HttpGet("{id}")]
        [ProducesResponseType((200), Type = typeof(PersonVO))]
        [ProducesResponseType((204))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(long id)
        {
            var person = _personBusiness.FindById(id);
            if (person == null) return NotFound();
            return Ok(person);
        }

        /// <summary>
        /// Gets a person by their first and last name.
        /// </summary>
        /// <param name="firstName">The first name of the person.</param>
        /// <param name="lastName">The last name of the person.</param>
        /// <returns>The person with the specified first and last name.</returns>
        [HttpGet("findPersonByName")]
        [ProducesResponseType((200), Type = typeof(List<PersonVO>))]
        [ProducesResponseType((204))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get([FromQuery] string? firstName, [FromQuery] string? lastName)
        {
            var person = _personBusiness.FindByName(firstName!, lastName!);
            if (person == null) return NotFound();
            return Ok(person);
        }

        /// <summary>
        /// Creates a new person.
        /// </summary>
        /// <param name="person">The person to be created.</param>
        /// <returns>The created person.</returns>
        /// <remarks>
        /// Maps POST requests to https://localhost:44300/api/person
        /// [FromBody] tells the framework to serialize the request body to the person instance.
        /// </remarks>
        [HttpPost]
        [ProducesResponseType((200), Type = typeof(PersonVO))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] PersonVO person)
        {
            if (person == null) return BadRequest();
            return Ok(_personBusiness.Create(person));
        }

        /// <summary>
        /// Updates an existing person.
        /// </summary>
        /// <param name="person">The person to be updated.</param>
        /// <returns>The updated person.</returns>
        /// <remarks>
        /// Maps PUT requests to https://localhost:44300/api/person
        /// </remarks>
        [HttpPut]
        [ProducesResponseType((200), Type = typeof(PersonVO))]
        [ProducesResponseType((204))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put([FromBody] PersonVO person)
        {
            if (person == null) return BadRequest();
            return Ok(_personBusiness.Update(person));
        }

        /// <summary>
        /// Disables a person by their ID.
        /// </summary>
        /// <param name="id">The ID of the person to be disabled.</param>
        /// <returns>The disabled person.</returns>
        /// <remarks>
        /// Maps PATCH requests to https://localhost:44300/api/person/{id}
        [HttpPatch("{id}")]
        [ProducesResponseType((200), Type = typeof(PersonVO))]
        [ProducesResponseType((204))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Patch(long id)
        {
            return Ok(_personBusiness.Disable(id));
        }

        /// <summary>
        /// Deletes a person by their ID.
        /// </summary>
        /// <param name="id">The ID of the person to be deleted.</param>
        /// <returns>Returns a status indicating that there is no content after the deletion.</returns>
        /// <remarks>
        /// Maps DELETE requests to https://localhost:44300/api/person/{id}
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType((204))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        public IActionResult Delete(long id)
        {
            _personBusiness.Delete(id);
            return NoContent();
        }
    }
}
