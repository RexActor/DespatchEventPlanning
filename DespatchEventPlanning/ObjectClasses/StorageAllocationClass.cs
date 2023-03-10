using DespatchEventPlanning.Database;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DespatchEventPlanning.ObjectClasses
{
	internal class StorageAllocationClass
	{
		private DatabaseClass db = new DatabaseClass();

		public void AllocateStorage()
		{



			List<DepotLimitationClass> depotLimits = new List<DepotLimitationClass>();
			List<PackingProductInformationClass> packingProduct = db.getInformationInList();




		}
	}

	internal class Storage
	{
		private int winNumber { get; set; }
		private string productDescription { get;set; }
		private string storageDate { get; set; }
		private string depotDate { get; set; }
		private string depotName { get; set; }
		private int quantity { get; set; }
		private string loadReference { get;set; }
		private int availableFullPallets { get;set; }
	}




	/*
	
	CREATE TABLE "StorageAllocation" (
	"Id"	INTEGER,
	"winNumber"	INTEGER DEFAULT 0,
	"productDescription"	TEXT DEFAULT 'N/A',
	"storageDate"	TEXT DEFAULT 'N/A',
	"depotDate"	TEXT DEFAULT 'N/A',
	"depotName"	TEXT DEFAULT 'N/A',
	"quantity"	INTEGER DEFAULT 0,
	"loadReference"	INTEGER DEFAULT 'N/A',
	PRIMARY KEY("Id" AUTOINCREMENT)
);







	 */
}