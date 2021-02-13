using System;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace GithubMockService.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task GetsUserFromApi()
        {
            var mock = new Mock<IGithubService>();
            mock.Setup(a=>a.GetUserAsync("xyz"))
                .Returns(ValueTask.FromResult(new GithubUser
                {
                    FirstName = "Github",
                    LastName = "User",
                    UserName = "xyz"
                }));
            var service = mock.Object;
            var result = await service.GetUserAsync("xyz");
            Assert.Equal("xyz", result.UserName);
        }

        [Fact]
        public async Task ThrowsExceptionIfUserDoesntExist()
        {
            var mock = new Mock<IGithubService>();
            mock.Setup(a=>a.GetUserAsync("abc"))
                .Throws<GithubUserNotFoundException>();
            var service = mock.Object;
            await Assert.ThrowsAsync<GithubUserNotFoundException>(async () => await service.GetUserAsync("abc"));
        }
    }
}
