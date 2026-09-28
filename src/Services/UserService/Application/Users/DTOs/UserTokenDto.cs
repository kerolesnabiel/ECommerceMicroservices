namespace UserService.Application.Users.DTOs;

public class UserTokenDto
{
    public string Token = string.Empty;
    public int TokenExpiryMinutes;
    public string RefreshToken = string.Empty;
    public int RefreshTokenExpiryDays;

    public object ToObject()
    {
        return new { 
            Token, 
            TokenExpiryMinutes, 
            RefreshToken, 
            RefreshTokenExpiryDays};
    }
}
