using MediatR;

namespace UserService.Application.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
}
