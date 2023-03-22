using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DespatchEventPlanning.ObjectClasses
{
	internal class PackingPlanClass
	{
		public int winNumber { get; set; }
		public string productDescription { get; set; }
		public string productGroup { get; set; }
		public string packingDate { get; set; }
		public string depotDate { get; set; }
		public int packingQuantity { get; set; }
	}
}
