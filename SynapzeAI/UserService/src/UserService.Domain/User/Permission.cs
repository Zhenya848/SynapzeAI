namespace UserService.Domain.User;

public class Permission
{
    public Guid Id { get; set; }
    
    public string Code { get; set; }
    public string Description { get; set; }
}