namespace CItyInfo.API.Model
{
    /// <summary>
    /// A DTO for a city without Point of Interest
    /// </summary>
    public class CityWithoutPointOfInterestDto
    {
        /// <summary>
        /// The Id of the City
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The Name of the City
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The Description of the City
        /// </summary>
        public string? Description { get; set; }
    }
}
