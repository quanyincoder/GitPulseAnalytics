using GitPulseAnalytics.Controllers;
using GitPulseAnalytics.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace GitPulseAnalytics.Tests.Controllers
{
	[TestClass]
	public class ApiControllerTest
	{
		[TestMethod]
		public void GetPullRequests()
		{
			// Arrange
			GitHubApiController controller = new GitHubApiController();

			// Act
			dynamic result = controller.PullRequests("lodash");

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual("lodash", result.Content.organization);
			Assert.AreEqual(1039, result.Content.count);
			Assert.AreEqual(3, result.Content.open);
			Assert.AreEqual(1036, result.Content.closed);
			Assert.AreEqual(645, result.Content.merged);
		}

		[TestMethod]
		public void GetPullRequestsBySearch()
		{
			// Arrange
			GitHubApiController controller = new GitHubApiController();

			// Act
			dynamic result = controller.PullRequestsBySearch("lodash");

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual("lodash", result.Content.organization);
			Assert.AreEqual(1037, result.Content.count);
			Assert.AreEqual(3, result.Content.open);
			Assert.AreEqual(1034, result.Content.closed);
		}
	}
}