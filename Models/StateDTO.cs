namespace CountryApi.Models
{
    public class StateDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }

        public long countryId { get; set; }

    }
}