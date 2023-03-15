using DespatchEventPlanning.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace DespatchEventPlanning.ObjectClasses
{
	internal class StorageAllocationClass
	{
		private DatabaseClass db = new DatabaseClass();
		private List<Storage> allocatedLoads = new List<Storage>();
		private List<StorageSummary> allocatedLoadSummary;

		public void AllocateStorage(string allocationDate)
		{
			List<DepotLimitationClass> depotLimits = new List<DepotLimitationClass>();
			List<PackingProductInformationClass> packingProduct = db.getInformationInList();

			packingProduct.AsEnumerable().Where(item => item.packingDate.Equals(allocationDate)).ToList().ForEach(item =>
			{
				foreach (var x in item.GetType().GetProperties())
				{
					switch (x.Name)
					{
						case "BEDFORD":
							if ((int)(item.BEDFORD - (item.BEDFORD % item.packsPerPallet)) / item.packsPerPallet > 0)
							{
								allocatedLoads.Add(new Storage
								{
									winNumber = item.winNumber,
									storageDate = item.packingDate,
									depotDate = item.depotDate,
									depotName = x.Name,
									productDescription = item.productDescription,
									packsPerPallet = item.packsPerPallet,
									quantityCases = (int)item.BEDFORD,
									productGroup = item.productGroup,
									quantityPallets = (int)Math.Ceiling(item.BEDFORD / item.packsPerPallet),
									quantityPalletsToAllocate = (int)(item.BEDFORD - (item.BEDFORD % item.packsPerPallet)) / item.packsPerPallet
								});
							}
							break;

						case "ERITH":
							if ((int)(item.ERITH - (item.ERITH % item.packsPerPallet)) / item.packsPerPallet > 0)
							{
								allocatedLoads.Add(new Storage
								{
									winNumber = item.winNumber,
									storageDate = item.packingDate,
									depotDate = item.depotDate,
									depotName = x.Name,
									productDescription = item.productDescription,
									packsPerPallet = item.packsPerPallet,
									quantityCases = (int)item.ERITH,
									productGroup = item.productGroup,

									quantityPallets = (int)Math.Ceiling(item.ERITH / item.packsPerPallet),
									quantityPalletsToAllocate = (int)(item.ERITH - (item.ERITH % item.packsPerPallet)) / item.packsPerPallet
								});
							}
							break;

						case "LUTTERWORTH":
							if ((int)(item.LUTTERWORTH - (item.LUTTERWORTH % item.packsPerPallet)) / item.packsPerPallet > 0)
							{
								allocatedLoads.Add(new Storage
								{
									winNumber = item.winNumber,
									storageDate = item.packingDate,
									depotDate = item.depotDate,
									depotName = x.Name,
									productDescription = item.productDescription,
									packsPerPallet = item.packsPerPallet,
									quantityCases = (int)item.LUTTERWORTH,
									productGroup = item.productGroup,
									quantityPallets = (int)Math.Ceiling(item.LUTTERWORTH / item.packsPerPallet),
									quantityPalletsToAllocate = (int)(item.LUTTERWORTH - (item.LUTTERWORTH % item.packsPerPallet)) / item.packsPerPallet
								});
							}
							break;

						case "ROCHDALE":
							if ((int)(item.ROCHDALE - (item.ROCHDALE % item.packsPerPallet)) / item.packsPerPallet > 0)
							{
								allocatedLoads.Add(new Storage
								{
									winNumber = item.winNumber,
									storageDate = item.packingDate,
									depotDate = item.depotDate,
									depotName = x.Name,
									productDescription = item.productDescription,
									packsPerPallet = item.packsPerPallet,
									quantityCases = (int)item.ROCHDALE,
									productGroup = item.productGroup,
									quantityPallets = (int)Math.Ceiling(item.ROCHDALE / item.packsPerPallet),
									quantityPalletsToAllocate = (int)(item.ROCHDALE - (item.ROCHDALE % item.packsPerPallet)) / item.packsPerPallet
								});
							}
							break;

						case "SKELMERSDALE":
							if ((int)(item.SKELMERSDALE - (item.SKELMERSDALE % item.packsPerPallet)) / item.packsPerPallet > 0)
							{
								allocatedLoads.Add(new Storage
								{
									winNumber = item.winNumber,
									storageDate = item.packingDate,
									depotDate = item.depotDate,
									depotName = x.Name,
									productDescription = item.productDescription,
									packsPerPallet = item.packsPerPallet,
									quantityCases = (int)item.SKELMERSDALE,
									productGroup = item.productGroup,
									quantityPallets = (int)Math.Ceiling(item.SKELMERSDALE / item.packsPerPallet),
									quantityPalletsToAllocate = (int)(item.SKELMERSDALE - (item.SKELMERSDALE % item.packsPerPallet)) / item.packsPerPallet
								});
							}
							break;

						case "WAKEFIELD":
							if ((int)(item.WAKEFIELD - (item.WAKEFIELD % item.packsPerPallet)) / item.packsPerPallet > 0)
							{
								allocatedLoads.Add(new Storage
								{
									winNumber = item.winNumber,
									storageDate = item.packingDate,
									depotDate = item.depotDate,
									depotName = x.Name,
									productDescription = item.productDescription,
									packsPerPallet = item.packsPerPallet,
									quantityCases = (int)item.WAKEFIELD,
									productGroup = item.productGroup,
									quantityPallets = (int)Math.Ceiling(item.WAKEFIELD / item.packsPerPallet),
									quantityPalletsToAllocate = (int)(item.WAKEFIELD - (item.WAKEFIELD % item.packsPerPallet)) / item.packsPerPallet
								});
							}
							break;

						case "WASHINGTON":
							if ((int)(item.WASHINGTON - (item.WASHINGTON % item.packsPerPallet)) / item.packsPerPallet > 0)
							{
								allocatedLoads.Add(new Storage
								{
									winNumber = item.winNumber,
									storageDate = item.packingDate,
									depotDate = item.depotDate,
									depotName = x.Name,
									productDescription = item.productDescription,
									packsPerPallet = item.packsPerPallet,
									quantityCases = (int)item.WASHINGTON,
									productGroup = item.productGroup,
									quantityPallets = (int)Math.Ceiling(item.WASHINGTON / item.packsPerPallet),
									quantityPalletsToAllocate = (int)(item.WASHINGTON - (item.WASHINGTON % item.packsPerPallet)) / item.packsPerPallet
								});
							}
							break;

						case "FALKIRK":
							if ((int)(item.FALKIRK - (item.FALKIRK % item.packsPerPallet)) / item.packsPerPallet > 0)
							{
								allocatedLoads.Add(new Storage
								{
									winNumber = item.winNumber,
									storageDate = item.packingDate,
									depotDate = item.depotDate,
									depotName = x.Name,
									productDescription = item.productDescription,
									packsPerPallet = item.packsPerPallet,
									quantityCases = (int)item.FALKIRK,
									productGroup = item.productGroup,
									quantityPallets = (int)Math.Ceiling(item.FALKIRK / item.packsPerPallet),
									quantityPalletsToAllocate = (int)(item.FALKIRK - (item.FALKIRK % item.packsPerPallet)) / item.packsPerPallet
								});
							}
							break;

						case "LARNE":
							if ((int)(item.LARNE - (item.LARNE % item.packsPerPallet)) / item.packsPerPallet > 0)
							{
								allocatedLoads.Add(new Storage
								{
									winNumber = item.winNumber,
									storageDate = item.packingDate,
									depotDate = item.depotDate,
									depotName = x.Name,
									productDescription = item.productDescription,
									packsPerPallet = item.packsPerPallet,
									quantityCases = (int)item.LARNE,
									productGroup = item.productGroup,
									quantityPallets = (int)Math.Ceiling(item.LARNE / item.packsPerPallet),
									quantityPalletsToAllocate = (int)(item.LARNE - (item.LARNE % item.packsPerPallet)) / item.packsPerPallet
								});
							}
							break;

						case "BRISTOL":
							if ((int)(item.BRISTOL - (item.BRISTOL % item.packsPerPallet)) / item.packsPerPallet > 0)
							{
								allocatedLoads.Add(new Storage
								{
									winNumber = item.winNumber,
									storageDate = item.packingDate,
									depotDate = item.depotDate,
									depotName = x.Name,
									productDescription = item.productDescription,
									packsPerPallet = item.packsPerPallet,
									quantityCases = (int)item.BRISTOL,
									productGroup = item.productGroup,
									quantityPallets = (int)Math.Ceiling(item.BRISTOL / item.packsPerPallet),
									quantityPalletsToAllocate = (int)(item.BRISTOL - (item.BRISTOL % item.packsPerPallet)) / item.packsPerPallet
								});
							}
							break;
					}
				}
			});
		}

		public List<Storage> GetAllocatedLoads()
		{
			return allocatedLoads.ToList();
		}

		public void ClearAllocatedLoads()
		{
			allocatedLoads.Clear();
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
				item.palletSummary = list.Where(x => x.loadReference == item.loadReference).Sum(x => x.quantityPallets);
				item.casesSummary = list.Where(x => x.loadReference == item.loadReference).Sum(x => x.quantityCases);

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
		public int packsPerPallet { get; set; }
		public string productGroup { get; set; }

		public int quantityPalletsToAllocate { get; set; }

		public int siteCapacityTarget { get; set; }
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