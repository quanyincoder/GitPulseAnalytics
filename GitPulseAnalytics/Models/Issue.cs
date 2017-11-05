using System;
using System.Collections.Generic;

namespace GitPulseAnalytics.Models
{
	public class Issue
	{
		/// <summary>
		/// Issue ID.
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Url of the issue.
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// Sequence number of the issue.
		/// </summary>
		public int Number { get; set; }

		/// <summary>
		/// Current state of the issue.
		/// </summary>
		public string State { get; set; }

		/// <summary>
		/// Title of the issue.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Title of the issue.
		/// </summary>
		public string Body { get; set; }

		/// <summary>
		/// Owner of the issue.
		/// </summary>
		public GitHubUser User { get; set; }

		/// <summary>
		/// Time the issue was created.
		/// </summary>
		public DateTime CreatedAt { get; set; }

		/// <summary>
		/// Time the issue was updated.
		/// </summary>
		public DateTime? UpdatedAt { get; set; }

		/// <summary>
		/// Time the issue was closed.
		/// </summary>
		public DateTime? ClosedAt { get; set; }
	}
}