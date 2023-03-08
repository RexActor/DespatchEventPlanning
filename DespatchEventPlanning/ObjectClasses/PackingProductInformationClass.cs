using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DespatchEventPlanning.ObjectClasses
{
	internal class PackingProductInformationClass
	{

		public int winNumber { get; set; }
		public string productDescription { get; set; } = string.Empty;
		public string productGroup { get; set; } = string.Empty;
		public string packingDate { get; set; } = string.Empty;
		public string depotDate { get; set; } = string.Empty;
		public double packingQty { get; set; } = 0f;

		public double forecastQty { get; set; } = 0f;

		public double difference { get { return forecastQty - packingQty; } }

		public int packsPerPallet { get; set; } = 1;

		public double palletsGenerated { get { return Math.Ceiling(packingQty/packsPerPallet); } }

		public List<DepotInformationClass> depotSplitInfotmation { get; set; } = new List<DepotInformationClass>();
	}




	

	

}
