using System.Text.RegularExpressions;

namespace GitPulseAnalytics.Models
{
	/// <summary>
	/// This class represents the starndard web link header per RFC5988 specicifcation.
	/// </summary>
	public class LinkHeader
	{
		public string First { get; set; }
		public string Last { get; set; }
		public string Next { get; set; }
		public string Prev { get; set; }

		/// <summary>
		/// This constructor accepts a link header string, and parses it to create a LinkHeader object.
		/// </summary>
		/// <param name="linkStr"></param>
		public LinkHeader(string linkStr)
		{
			if (!string.IsNullOrWhiteSpace(linkStr))
			{
				// the link sections are delimited by a comma, so we split them by ','
				var linkSections = linkStr.Split(',');

				foreach (var section in linkSections)
				{
					var relationMatch = Regex.Match(section, "(?<=rel=\").+?(?=\")", RegexOptions.IgnoreCase);
					var linkMatch = Regex.Match(section, "(?<=<).+?(?=>)", RegexOptions.IgnoreCase);

					if (relationMatch.Success && linkMatch.Success)
					{
						var relation = relationMatch.Value.ToLower();
						var link = linkMatch.Value.ToLower();

						switch (relation)
						{
							case "first":
								First = link;
								break;

							case "last":
								Last = link;
								break;

							case "next":
								Next = link;
								break;

							case "prev":
								Prev = link;
								break;
						}
					}
				}
			}
		}
	}
}