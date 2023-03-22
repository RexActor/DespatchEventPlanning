using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DespatchEventPlanning.ObjectClasses
{
	internal class depotSplitClass
	{

		public int winNumber { get; set; }
		public string productDescription { get; set; }
		public int depotNumber { get; set; }
		public string depotName { get; set; }

		public string depotDate { get; set; }
		public int qty { get; set; }
	}
}
