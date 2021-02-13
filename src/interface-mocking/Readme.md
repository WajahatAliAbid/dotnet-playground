# Interface Mocking
This example demonstrates the usage of interface in mock testing using [Moq](https://github.com/Moq/moq4). 

We have a service called [IGithubService](./src/GithubMockService/IGithubService.cs) which defines a contract for getting a user from the Github Api. Normally you cannot test applications using external APIs with actual APIs, so the best solution there is to mock them. We make certain assumptions based on API behavior and test our code against those assumptions. 

This is how we can create mock of github service.
```csharp
var mock = new Mock<IGithubService>();
mock.Setup(a=>a.GetUserAsync("username"))
    .Returns(ValueTask.FromResult(new GithubUser("username")));
```
Whenenver we search for username "username", we will get corresponding user. You can have a look at complete test cases [here](./tests/GithubMockService.Tests/UnitTest1.cs).