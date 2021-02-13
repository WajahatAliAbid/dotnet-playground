using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System;

namespace GithubMockService
{
    public interface IGithubService
    {
        /// <summary>
        /// Returns a user by the name provided
        /// </summary>
        /// <param name="username">case sensitive username</param>
        /// <exception cref="GithubUserNotFoundException">Thrown when user doesn't exist for username</exception>
        /// <exception cref="ArgumentNullException">Thrown when username is null or empty</exception>
        /// <returns>Returns <see cref="GithubUser"/> object for corresponding username</returns>
        ValueTask<GithubUser> GetUserAsync([NotNull] string username);
    }
}