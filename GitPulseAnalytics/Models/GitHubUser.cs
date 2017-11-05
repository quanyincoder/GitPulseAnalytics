namespace GitPulseAnalytics.Models
{
	/// <summary>
	/// GitHub user account.
	/// </summary>
	public class GitHubUser
	{
		public long Id { get; set; }
		public string Login { get; set; }
		public string Url { get; set; }
		public string Type { get; set; }
	}
}