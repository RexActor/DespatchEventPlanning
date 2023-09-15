using DocumentFormat.OpenXml.Bibliography;

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

		public int leftOnSitePreviousDay { get; set; }

		public int palletsInBound { get; set; }
		public int palletsToStorage { get; set; }
		public int palletsDirect { get; set; }

		public int palletsOutbound { get; set; }

		public int siteCapacity { get { return palletsGeneratedTotal - palletsOutbound; } }

		public string productGroup { get; set; }
		public int directLoads { get; set; }
		public int storageLoads { get; set; }
		public int totalLoads { get { return directLoads + storageLoads; } }

		public bool hasStorage { get; set; }
		public bool totalLoadLimitReached { get; set; }
		public bool totalLoadsaboveFifty { get; set; }
	}
}
