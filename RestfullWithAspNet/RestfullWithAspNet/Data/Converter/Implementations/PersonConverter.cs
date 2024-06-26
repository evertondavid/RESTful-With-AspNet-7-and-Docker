using RestfullWithAspNet.Data.Converter.Contract;
using RestfullWithAspNet.Data.VO;
using RestfullWithAspNet.Model;

namespace RestfullWithAspNet.Data.Converter.Implementations
{
    /// <summary>
    /// Converts between PersonVO and Person entities.
    /// </summary>
    public class PersonConverter : IParser<PersonVO, Person>, IParser<Person, PersonVO>
    {
        /// <summary>
        /// Converts a PersonVO object to a Person entity.
        /// </summary>
        /// <param name="origin">The PersonVO object to be converted.</param>
        /// <returns>The converted Person entity.</returns>
        public Person Parse(PersonVO origin) =>
        origin == null
        ? throw new ArgumentNullException(nameof(origin), "The origin cannot be null.")
        : new Person
        {
            Id = origin.Id,
            FirstName = origin.FirstName,
            LastName = origin.LastName,
            Address = origin.Address,
            Gender = origin.Gender
        };


        /// <summary>
        /// Converts a Person entity to a PersonVO object.
        /// </summary>
        /// <param name="origin">The Person entity to be converted.</param>
        /// <returns>The converted PersonVO object.</returns>
        public PersonVO Parse(Person origin) =>
        origin == null
        ? throw new ArgumentNullException(nameof(origin), "The origin cannot be null.")
        : new PersonVO
        {
            Id = origin.Id,
            FirstName = origin.FirstName,
            LastName = origin.LastName,
            Address = origin.Address,
            Gender = origin.Gender,
            Enabled = origin.Enabled
        };

        /// <summary>
        /// Converts a list of PersonVO objects to a list of Person entities.
        /// </summary>
        /// <param name="origin">The list of PersonVO objects to be converted.</param>
        /// <returns>The converted list of Person entities.</returns>
        public List<Person> Parse(List<PersonVO> origin)
        {
            if (origin == null) throw new ArgumentNullException(nameof(origin), "The origin list cannot be null.");
            return origin.Select(item => Parse(item)).ToList();
        }

        /// <summary>
        /// Converts a list of Person entities to a list of PersonVO objects.
        /// </summary>
        /// <param name="origin">The list of Person entities to be converted.</param>
        /// <returns>The converted list of PersonVO objects.</returns>
        public List<PersonVO> Parse(List<Person> origin)
        {
            if (origin == null) throw new ArgumentNullException(nameof(origin), "The origin list cannot be null.");
            return origin.Select(item => Parse(item)).ToList();
        }
    }
}
