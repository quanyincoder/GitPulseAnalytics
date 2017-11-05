using System.Collections.Generic;

namespace GitPulseAnalytics.Models
{
	/// <summary>
	/// Container class for GitHub search API return results.
	/// </summary>
	public class SearchResultContainer
	{
		/// <summary>
		/// Total number of results expected from the search.
		/// This is not the actual number of returned results.
		/// </summary>
		public int TotalCount { get; set; }

		/// <summary>
		/// Are the results complete?
		/// </summary>
		public bool IncompleteResults { get; set; }

		/// <summary>
		/// List of result objects returned from the search API.
		/// </summary>
		public List<Issue> Items { get; set; }
	}
}