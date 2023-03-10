using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DespatchEventPlanning.ObjectClasses
{
	internal class SiteCapacityClass
	{

		public string produceDate { get; set; }
		public int palletsGenerating { get; set; }

		public int palletsGeneratedTotal { get; set; }

		public int palletsOutbound { get; set; }
		public int palletsInBound { get; set; }

		public int siteCapacity { get { return palletsGeneratedTotal+ palletsGenerating - palletsOutbound; } }

		public string productGroup { get; set; }
	}
}
