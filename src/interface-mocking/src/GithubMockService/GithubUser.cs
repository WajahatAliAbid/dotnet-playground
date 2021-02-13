namespace GithubMockService
{
    public class GithubUser
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public GithubUser()
        {
            
        }

        public GithubUser(string userName)
        {
            UserName = userName;
        }
    }
}