public class AuthService
{
    private readonly UserRepository _user;
    public AuthService(UserRepository user)
    {
        _user = user;
    }

    public async Task<UserDTO?> Login(UserDTO u)
    {
        if (string.IsNullOrEmpty(u.UserName) || string.IsNullOrEmpty(u.Password))
        {
            return null;
        }

        var user = await _user.GetUserByUsername(u.UserName);
        if(user == null) { return null; }

        if(user.Password!=u.Password) { return null; }

        return user;
    }


    public async Task<bool> Register(UserDTO u, string confirmPass)
    {
        if (string.IsNullOrEmpty(u.UserName) || string.IsNullOrEmpty(u.Password) || string.IsNullOrEmpty(confirmPass))
        {
            return false;
        }

        if(u.Password!=confirmPass) { return false; } 
        if(! await _user.Add(u)) {  return false; }
        return true;
    }
}