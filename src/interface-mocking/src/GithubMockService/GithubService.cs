using System.Threading.Tasks;

namespace GithubMockService
{
    public class GithubService : IGithubService
    {
        public ValueTask<GithubUser> GetUserAsync(string username)
        {
            throw new System.NotImplementedException();
        }
    }
}