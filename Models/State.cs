namespace CountryApi.Models
{
    public class State
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }

        public long countryId { get; set; }
    }
}