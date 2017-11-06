# GitPulseAnalytics
## Description
Get all pull requests of an organization on GitHub, using the GitHub REST APIs.
This project is built as an ASP.NET Web API project in Visual Studio.
The homepage of this application has a link to the API help documentation.
## Testing the Web APIs
You can use any HttpClient, such as Postman to test the APIs.

To test, send HTTP GET request to:
```http://localhost:<port>/GitHubApi/<org_name>/PullRequests```
## GitHub Authentication
When you test, you will need to replace the Bearer token in the GitHubApiController.cs file with a valid one. Otherwise, you will get authentication error.
## C# Unit Tests
The GitPulseAnalytics.Tests project folder has a ApiControllerTest class with very simple test methods. The data were only valid at the time of development.

