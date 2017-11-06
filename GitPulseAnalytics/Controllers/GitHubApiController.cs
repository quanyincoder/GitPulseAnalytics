using GitPulseAnalytics.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace GitPulseAnalytics.Controllers
{
	/// <summary>
	/// ApiController class to work with GitHub APIs.
	/// </summary>
	[RoutePrefix("GitHubApi")]
	public class GitHubApiController : ApiController
	{
		private readonly RestClient _client;

		/// <summary>
		/// Constructor
		/// </summary>
		public GitHubApiController()
		{
			// initialize an HttpClient with base URL.
			_client = new RestClient("https://api.github.com/");
		}

		/// <summary>
		/// Get all pull requests of a given organization from GitHub,
		/// by iterating through the organization's repositories.
		/// </summary>
		/// <param name="org">Organization</param>
		/// <returns>pull requst data</returns>
		[HttpGet]
		[Route("{org}/PullRequests")]
		public IHttpActionResult PullRequests(string org)
		{
			// create a REST request
			var request = new RestRequest($"/orgs/{org}/repos");
			// per GitHub documentation, we should explictly specify media type.
			request.AddHeader("Accept", "application/vnd.github.v3+json");
			// add read-only access authorization token
			request.AddHeader("Authorization", "Bearer e0a5440fd0a5fc2d7be3fd031ff7ae49707c34c3");
			// specify date time format
			request.DateFormat = "yyyy-MM-ddTHH:mm:ssZ";

			// execute the request to get a list of repositories
			var reposResponse = _client.Execute<List<Repository>>(request);

			if (reposResponse.StatusCode != HttpStatusCode.OK) // if the return status is not OK
			{
				return Content(reposResponse.StatusCode, JsonConvert.DeserializeObject(reposResponse.Content));
			}

			var repos = reposResponse.Data;

			// make a list to collect the results
			var pulls = new List<PullRequest>();

			// iterate over the repositories, and get pulls requests for each repository.
			foreach (var repo in repos)
			{
				// remove the baseUrl from the repository URL string, and add query parameters for pull requests.
				request.Resource = repo.Url.Replace(_client.BaseUrl.ToString(), "") + "/pulls?state=all&per_page=100";

				// execute the request to get the first paginated result
				var response = _client.Execute<List<PullRequest>>(request);

				if (response.StatusCode != HttpStatusCode.OK)
				{
					return Content(response.StatusCode, JsonConvert.DeserializeObject(response.Content));
				}

				// parse the Link Header from the response
				var linkHeader = new LinkHeader(response.Headers.FirstOrDefault(h => h.Name == "Link")?.Value.ToString());

				// collect the first-page results
				pulls.AddRange(response.Data);

				// paginate through the links (if any) and collect the results
				while (linkHeader.Next != null)
				{
					// remove the baseUrl from the link string
					request.Resource = linkHeader.Next.Replace(_client.BaseUrl.ToString(), "");
					// execute the request to get pull requests
					response = _client.Execute<List<PullRequest>>(request);
					// collect the results into the pulls list
					pulls.AddRange(response.Data);
					// update the linkHeader URL to the next one
					linkHeader = new LinkHeader(response.Headers.FirstOrDefault(h => h.Name == "Link")?.Value.ToString());
				}
			}

			return Ok(new
			{
				organization = org,
				repositories = repos.Select(r => r.Name),
				count = pulls.Count,
				open = pulls.Count(p => p.State == "open"),
				closed = pulls.Count(p => p.State == "closed"),
				merged = pulls.Count(p => p.MergedAt != null),
				pulls = pulls
			});
		}

		/// <summary>
		/// Get all pull requests of a given organization from GitHub,
		/// using the issues search API.
		/// </summary>
		/// <param name="org"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("{org}/PullRequestsBySearch")]
		public IHttpActionResult PullRequestsBySearch(string org)
		{
			// create the request for issues search
			// we only want issues that are pull requests (type:pr)
			var queryString = $"search/issues?q=org:{org}+type:pr";
			var request = new RestRequest(queryString, Method.GET);
			var issues = new List<Issue>();

			request.AddHeader("Accept", "application/vnd.github.v3+json");
			request.AddHeader("Authorization", "Bearer e0a5440fd0a5fc2d7be3fd031ff7ae49707c34c3");
			request.AddQueryParameter("per_page", "100");
			request.DateFormat = "yyyy-MM-ddTHH:mm:ssZ";

			var response = _client.Execute<SearchResultContainer>(request);

			if (response.StatusCode != HttpStatusCode.OK)
			{
				return Content(response.StatusCode, JsonConvert.DeserializeObject(response.Content));
			}

			if (response.Data.TotalCount > 1000) // the results exceed GitHub search API's limit of 1000
			{
				// we need to break the search into smaller pieces
				var twoYearsAgo = DateTime.Today.AddYears(-2).ToString("yyyy-MM-dd");
				request.Resource = queryString + "+created:<" + twoYearsAgo;
				Paginate<Issue>(request, issues);
				request.Resource = queryString + "+created:>=" + twoYearsAgo;
				Paginate<Issue>(request, issues);
			}
			else
			{
				Paginate<Issue>(request, issues);
			}

			return Ok(new
			{
				organization = org,
				count = issues.Count,
				open = issues.Count(i => i.State == "open"),
				closed = issues.Count(i => i.State == "closed"),
				issues = issues.OrderByDescending(i => i.CreatedAt)
			});
		}

		/// <summary>
		/// Paginate through the search API
		/// </summary>
		/// <typeparam name="T">Result type.</typeparam>
		/// <param name="request">RestRequest object</param>
		/// <param name="collecterList">Collection to save results.</param>
		private void Paginate<T>(RestRequest request, List<T> collecterList)
		{
			// send the first request
			var response = _client.Execute<SearchResultContainer>(request);

			// collect the results
			collecterList.AddRange(response.Data.Items as List<T>);

			// parse the Link Header from the response
			var linkHeader = new LinkHeader(response.Headers.FirstOrDefault(h => h.Name == "Link")?.Value.ToString());

			// we need to remove the query string parameters, because they are included in the returned link headers.
			request.Parameters.RemoveAll(p => p.Type == ParameterType.QueryString);

			// paginate through the rest of the links (if any) and collect the results
			while (linkHeader.Next != null)
			{
				// remove the baseUrl from the link string
				request.Resource = linkHeader.Next.Replace(_client.BaseUrl.ToString(), "");
				// execute the request to get issues
				response = _client.Execute<SearchResultContainer>(request);
				// collect the results into the issues list
				collecterList.AddRange(response.Data.Items as List<T>);
				// update linkHeader URL to the next one
				linkHeader = new LinkHeader(response.Headers.FirstOrDefault(h => h.Name == "Link")?.Value.ToString());
			}
		}

		/// <summary>
		/// Get a single pull request by ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("GetPullRequest/{id}")]
		public IHttpActionResult PullRequest(int id)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Create a new pull request.
		/// </summary>
		/// <param name="value"></param>
		[HttpPost]
		[Route("PostPullRequest")]
		public async Task<PullRequest> Post([FromBody]string value)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Update a pull request.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="value"></param>
		[HttpDelete]
		[Route("DeletePullRequest/{id}")]
		public async Task<PullRequest> Put(int id, [FromBody]string value)
		{
			throw new NotImplementedException();
		}
	}
}
