using DespatchEventPlanning.Database;

using DocumentFormat.OpenXml.Office.CustomUI;

using System;
using System.Collections.Generic;
using System.Linq;

namespace DespatchEventPlanning.ObjectClasses
{
	internal class StorageAllocationClass
	{
		private List<Storage> storages = new List<Storage>();

		private DatabaseClass db = new DatabaseClass();
		private List<Storage> allocatedLoads;
		private List<StorageSummary> allocatedLoadSummary;

		public void AllocateStorage()
		{
			allocatedLoads = new List<Storage>();
			List<DepotLimitationClass> depotLimits = new List<DepotLimitationClass>();
			List<PackingProductInformationClass> packingProduct = db.getInformationInList();

			for (int i = 0; i < 20; i++)
			{
				for (int x = 0; x < i; x++)
				{
					allocatedLoads.Add(new Storage()
					{
						winNumber = i,
						productDescription = $"Product {i}",
						storageDate = DateTime.Now.AddDays(x-i).ToShortDateString(),
						depotDate = DateTime.Now.AddDays(x+i).ToShortDateString(),
						depotName = "BEDFORD",
						quantityPallets = i *x,
						quantityCases = i*x*30,
						loadReference = $"BE{i}",
						availableFullPallets = x
					});
				}
			}
		}

		public List<Storage> GetAllocatedLoads()
		{
			return allocatedLoads.ToList();
		}

		public List<StorageSummary> GetAllocatedLoadsSummary(List<Storage> list)
		{
			allocatedLoadSummary = new List<StorageSummary>();



			list.AsEnumerable().ToList().Select(item => item.loadReference).Distinct().ToList().ForEach(subItem =>
			{
				allocatedLoadSummary.Add(new StorageSummary()
				{
					loadReference = subItem
				});
				

			});


			allocatedLoadSummary.AsEnumerable().ToList().ForEach(item =>
			{
				item.palletSummary = list.Where(x => x.loadReference == item.loadReference).Sum(x=>x.quantityPallets);
				item.casesSummary = list.Where(x => x.loadReference == item.loadReference).Sum(x => x.quantityCases);
				item.depotName = list.Where(x => x.loadReference == item.loadReference).Distinct().Select(x => x.depotName).FirstOrDefault().ToString();
				item.storageDate = list.Where(x => x.loadReference == item.loadReference).Distinct().Select(x => x.storageDate).FirstOrDefault().ToString();
				item.depotDate = list.Where(x => x.loadReference == item.loadReference).Distinct().Select(x => x.depotDate).FirstOrDefault().ToString();
			});



			return allocatedLoadSummary;
		}
	}

	internal class Storage
	{
		public int winNumber { get; set; }
		public string productDescription { get; set; }
		public string storageDate { get; set; }
		public string depotDate { get; set; }
		public string depotName { get; set; }
		public int quantityPallets { get; set; }
		public int quantityCases { get; set; }
		public string loadReference { get; set; }
		public int availableFullPallets { get; set; }
	}

	internal class StorageSummary
	{
		public string loadReference { get; set; }
		public int palletSummary { get; set; }
		public int casesSummary { get; set; }
		public string depotDate { get; set; }
		public string storageDate { get; set; }
		public string depotName { get; set; }
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