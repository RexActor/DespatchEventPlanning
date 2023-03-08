using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DespatchEventPlanning.ObjectClasses
{
	internal class DepotInformationClass
	{

		public string depotName { get; set; } = string.Empty;
		public int depotNumber { get; set; } = 0;
		public double depotSplit { get; set; } = 0f;
		public double allocatedQty { get; set; } = 0f;

		public double depotSplitOverSpill
		{
			get { return depotSplit - allocatedQty; }
		}
	}
}
