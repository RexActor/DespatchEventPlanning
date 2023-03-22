using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DespatchEventPlanning.ObjectClasses
{
	internal class DefaultDepotSplitsClass
	{

		public string productGroup { get; set; }
		public int winNumber { get; set; }
		public string productDescription { get; set; }
		public string depotName { get; set; }
		public float qty { get; set; }
	}
}
