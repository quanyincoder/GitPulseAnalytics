namespace GitPulseAnalytics.Models
{
	/// <summary>
	/// GitHub repository.
	/// </summary>
	public class Repository
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string FullName { get; set; }
		public string Url { get; set; }
	}
}