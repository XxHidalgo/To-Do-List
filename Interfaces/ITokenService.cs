using Microsoft.AspNetCore.Identity;

namespace ToDoList.Interfaces;

    public interface ITokenService
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
