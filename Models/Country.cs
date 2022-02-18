namespace CountryApi.Models
{
    public class Country
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }

        public List<State> States { get; set; } 
    }
}