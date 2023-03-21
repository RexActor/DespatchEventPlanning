using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DespatchEventPlanning.ObjectClasses
{
	internal class ProductInformationClass
	{

		public int winNumber { get; set; }
		public int productNumber { get; set; }	
		public string productDescription { get; set; }
		public int packsPerPallet { get; set; }
		public string productGroup { get; set; }	
		public int weightOfOuter { get; set; }
	}
}
