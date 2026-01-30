public class UpdateAuthorDto
{
    public int Id { get; set; } 
    public required string Fullname { get; set; }
    public DateTime BirthDate { get; set; }
    public required string Country { get; set; }
}