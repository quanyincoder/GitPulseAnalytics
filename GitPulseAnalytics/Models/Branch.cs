namespace GitPulseAnalytics.Models
{
	/// <summary>
	/// A branch of a repository.
	/// </summary>
	public class Branch
	{
		public string Label { get; set; }
		public string Ref { get; set; }
		public string SHA { get; set; }
		public GitHubUser User { get; set; }
		public Repository Repo { get; set; }
	}
}