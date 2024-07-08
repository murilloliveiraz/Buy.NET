namespace Buy_NET.API.Contracts.Category;

public class CategoryResponseContract : CategoryRequestContract
{
    public long Id { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime? InactivationDate { get; set; }
}