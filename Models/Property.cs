public class Property
{
    public int Id { get; set; }
    public required string Place { get; set; }
    public required string Area { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public required string NearbyHospitals { get; set; }
    public required string NearbyColleges { get; set; }
    public int SellerId { get; set; }
    public required User Seller { get; set; }
}