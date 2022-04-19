using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class StatisticsTask
	{
		public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
		{
			var groups = visits.GroupBy(v => v.UserId);

			var groupsOfOffsets = groups.Select(g => g.OrderBy(v => v.DateTime)
				.Bigrams()
				.Select(t => new {
					Offset = (t.Item2.DateTime - t.Item1.DateTime).TotalMinutes,
					Type = t.Item1.SlideType
				}));

			var allOffsets = groupsOfOffsets.SelectMany(o => o)
				.Where(d => d.Offset >= 1 && d.Offset <= 120)
				.Where(d => d.Type == slideType)
				.Select(d => d.Offset);

			if (allOffsets.Count() <= 0)
				return 0;

			return allOffsets.Median();
		}
	}
}
