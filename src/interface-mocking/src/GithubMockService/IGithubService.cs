using System.Threading.Tasks;

namespace GithubMockService
{
    public interface IGithubService
    {
        /// <summary>
        /// Returns a user by the name provided
        /// </summary>
        /// <param name="username">case sensitive username</param>
        /// <exception cref="GithubUserNotFoundException">Thrown when user doesn't exist for username</exception>
        /// <returns>Returns <see cref="GithubUser"/> object for corresponding username</returns>
        ValueTask<GithubUser> GetUserAsync(string username);
    }
}