namespace AddressAPI.DTOs
{
    public class DistanceDTO
    {      
        public string? Db_LocationA { get; set; }        
        public string? Db_LocationB { get; set; }
        public double Distance { get; set; }
        public string? Unit_Of_Measure { get; set; }
    }
}
