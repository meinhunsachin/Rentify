public class InterestedProperty
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public required Property Property { get; set; }
    public int BuyerId { get; set; }
    public required User Buyer { get; set; }
}