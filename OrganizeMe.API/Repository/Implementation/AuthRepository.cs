using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using OrganizeMe.API.Data;
using OrganizeMe.API.Models;

namespace OrganizeMe.API.Repository.Implementation;

public class AuthRepository(ApplicationDbContext context) : IAuthRepository
{
    public async Task<User> Register(User user, string password)
    {
        using var hmac = new HMACSHA512();

        user.UserId = Guid.NewGuid();
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        user.PasswordSalt = hmac.Key;

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        return user;
    }

    public async Task<User?> Login(string username, string password)
    {
        var user = await context.Users.SingleOrDefaultAsync(x => x.Username == username);

        if (user == null)
            return null;

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        // Compare hashes in a safe manner
        if (!computedHash.SequenceEqual(user.PasswordHash))
            return null;

        return user;
    }

    public async Task<bool> UserExists(string username)
    {
        return await context.Users.AnyAsync(x => x.Username == username);
    }
}