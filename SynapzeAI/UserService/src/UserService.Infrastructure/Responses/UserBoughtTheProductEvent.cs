namespace UserService.Infrastructure.Responses;

public class UserBoughtTheProductEvent
{
    public Guid UserId { get; set; }
    public int Pack {  get; set; }
}