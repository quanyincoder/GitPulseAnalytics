using System;

namespace GitPulseAnalytics.Models
{
	/// <summary>
	/// Pull request.
	/// </summary>
	public class PullRequest
	{
		/// <summary>
		/// Pull request ID
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Url of the pull request.
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// Sequence number of the pull request.
		/// </summary>
		public long Number { get; set; }

		/// <summary>
		/// Current state of the pull request.
		/// </summary>
		public string State { get; set; }

		/// <summary>
		/// Title of the pull request.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Title of the pull request.
		/// </summary>
		public string Body { get; set; }

		/// <summary>
		/// GitHub user who made the pull request.
		/// </summary>
		public GitHubUser User { get; set; }

		/// <summary>
		/// Time the pull request was created.
		/// </summary>
		public DateTime CreatedAt { get; set; }

		/// <summary>
		/// Time the pull request was updated.
		/// </summary>
		public DateTime? UpdatedAt { get; set; }

		/// <summary>
		/// Time the pull request was closed.
		/// </summary>
		public DateTime? ClosedAt { get; set; }

		/// <summary>
		/// Time the pull request was merged.
		/// </summary>
		public DateTime? MergedAt { get; set; }

		/// <summary>
		/// Head branch.
		/// </summary>
		public Branch Head { get; set; }

		/// <summary>
		/// Base branch.
		/// </summary>
		public Branch Base { get; set; }
	}
}