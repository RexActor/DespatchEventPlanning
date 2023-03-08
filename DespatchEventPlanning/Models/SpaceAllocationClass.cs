using DocumentFormat.OpenXml.Office2010.ExcelAc;

using System.Collections.Generic;

namespace DespatchEventPlanning.Models
{
	internal class SpaceAllocationClass
	{
		public double winNumber { get; set; }
		public string productDescription { get; set; }
		public string packingDate { get; set; }
		public string depotDate { get; set; }
		public double packingQuantity { get; set; }

		public List<StorageClass> storageInformation { get; set; }

		public double palletSpacesProduced { get; set; }
		public double packsPerPallet { get; set; }
	}
}