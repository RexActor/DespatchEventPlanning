using DespatchEventPlanning.Database;

using DocumentFormat.OpenXml.Office.CustomUI;

using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftAntimalwareEngine;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;

namespace DespatchEventPlanning.ObjectClasses
{
	internal class StorageAllocationClass
	{
		private DatabaseClass db = new DatabaseClass();
		private List<Storage> allocatedLoads = new List<Storage>();

		private List<Storage>productsAllocatedForStorageTrucks = new List<Storage>();

		private List<StorageSummary> allocatedLoadSummary;
		private string selectedProductionPlanVersion = string.Empty;

		public void AllocateStorage(string allocationDate,string selectedProductinPlan)
		{
			List<DepotLimitationClass> depotLimits = new List<DepotLimitationClass>();
			List<PackingProductInformationClass> packingProduct = db.getInformationInList(selectedProductinPlan);

			selectedProductionPlanVersion = selectedProductinPlan;

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
									inStorage = db.productExistsInStorage(item.winNumber,item.depotDate,item.packingDate,"BEDFORD", selectedProductionPlanVersion),
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
									inStorage = db.productExistsInStorage(item.winNumber, item.depotDate, item.packingDate, "ERITH", selectedProductionPlanVersion),

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
									inStorage = db.productExistsInStorage(item.winNumber, item.depotDate, item.packingDate, "LUTTERWORTH", selectedProductionPlanVersion),
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
									inStorage = db.productExistsInStorage(item.winNumber, item.depotDate, item.packingDate, "ROCHDALE", selectedProductionPlanVersion),
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
									inStorage = db.productExistsInStorage(item.winNumber, item.depotDate, item.packingDate, "SKELMERSDALE", selectedProductionPlanVersion),
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
									inStorage = db.productExistsInStorage(item.winNumber, item.depotDate, item.packingDate, "WAKEFIELD", selectedProductionPlanVersion),
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
									inStorage = db.productExistsInStorage(item.winNumber, item.depotDate, item.packingDate, "WASHINGTON", selectedProductionPlanVersion),
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
									inStorage = db.productExistsInStorage(item.winNumber, item.depotDate, item.packingDate, "FALKIRK", selectedProductionPlanVersion),
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
									inStorage = db.productExistsInStorage(item.winNumber, item.depotDate, item.packingDate, "LARNE", selectedProductionPlanVersion),
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
									inStorage = db.productExistsInStorage(item.winNumber, item.depotDate, item.packingDate, "BRISTOL", selectedProductionPlanVersion),
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

		public List<StorageSummary> GetAllocatedLoadsSummary(string productionPlanVersion)
		{



			allocatedLoadSummary = new List<StorageSummary>();

			productsAllocatedForStorageTrucks = db.getStorageInformationInList(selectedProductionPlanVersion);

			db.getStorageInformationInList(productionPlanVersion).AsEnumerable().Select(item=>item.loadReference).Distinct().ToList().ForEach(subItem=> {


				allocatedLoadSummary.Add(new StorageSummary()
				{
					loadReference = subItem
				});


			});


			//productsAllocatedForStorageTrucks.AsEnumerable().ToList().Select(item => item.loadReference).Distinct().ToList().ForEach(subItem =>
			//{
			//	allocatedLoadSummary.Add(new StorageSummary()
			//	{
			//		loadReference = subItem
			//	});
			//});

			allocatedLoadSummary.AsEnumerable().ToList().ForEach(item =>
			{
				
				item.palletSummary = productsAllocatedForStorageTrucks.Where(x => x.loadReference == item.loadReference).Sum(x => x.quantityPalletsAllocated);
				item.casesSummary = productsAllocatedForStorageTrucks.Where(x => x.loadReference == item.loadReference).Sum(x => x.quantityCases);
				item.depotName = productsAllocatedForStorageTrucks.Where(x => x.loadReference == item.loadReference).Distinct().Select(x => x.depotName).FirstOrDefault().ToString();
				item.loadReference = productsAllocatedForStorageTrucks.Where(x => x.loadReference == item.loadReference).Distinct().Select(x => x.loadReference).FirstOrDefault().ToString();
				item.storageDate = productsAllocatedForStorageTrucks.Where(x => x.loadReference == item.loadReference).Distinct().Select(x => x.storageDate).FirstOrDefault().ToString();
				item.depotDate = productsAllocatedForStorageTrucks.Where(x => x.loadReference == item.loadReference).Distinct().Select(x => x.depotDate).FirstOrDefault().ToString();
			});

			return allocatedLoadSummary;
		}

		public int GetTotalPalletsInLoad(string loadReference)
		{
			return productsAllocatedForStorageTrucks.Where(item => item.loadReference.Contains(loadReference)).Sum(item => item.quantityPalletsAllocated);
		}


		public int GetTotalLoadsWithReference(string loadReference,string productionVersion)
		{
			return GetAllocatedLoadsSummary(selectedProductionPlanVersion).Where(item => item.loadReference.Contains(loadReference)).Distinct().Count();
		}

		public string GetLastLoadReferenceWithDepotDate(string loadReference,string depotDate,string storageDate)
		{
			
				return productsAllocatedForStorageTrucks.Where(item => item.depotDate == depotDate).Where(item=>item.storageDate==storageDate).Where(item => item.loadReference.Contains(loadReference)).Distinct().Select(item => item.loadReference).LastOrDefault().ToString();
			
		}

		public string GetLastLoadReference(string loadReference)
		{

			return productsAllocatedForStorageTrucks.Where(item => item.loadReference.Contains(loadReference)).Distinct().Select(item => item.loadReference).LastOrDefault().ToString();

		}

		public int GetAmountOfLoadsWithDepotDate(string loadReference,string depotDate,string storageDate)
		{
			return productsAllocatedForStorageTrucks.Where(item => item.depotDate == depotDate).Where(item=>item.storageDate ==storageDate).Count(item=>item.loadReference.Contains(loadReference));
		}

		public int GetAmountOfLoads(string loadReference)
		{
			return productsAllocatedForStorageTrucks.Where(item => item.loadReference.Contains(loadReference)).Count();
		}


		public void AddProductToStorageTruck(Storage storage)
		{

			productsAllocatedForStorageTrucks.Add(new Storage() {

				winNumber = storage.winNumber,
				productDescription = storage.productDescription,
				storageDate = storage.storageDate,
				depotDate = storage.depotDate,
				quantityPalletsAllocated = storage.quantityPalletsAllocated,
				depotName = storage.depotName,
				loadReference = storage.loadReference,
				quantityCases = storage.quantityCases,
				productionPlanVersion = selectedProductionPlanVersion


				
			});

			/*
			MessageBox.Show($"WIN: \t{storage.winNumber}\n" +
				$"Description: \t{storage.productDescription}\n" +
				$"Storage Date: \t{storage.storageDate}\n" +
				$"Depot Date: \t{storage.depotDate}\n" +
				$"Allocated Pallets: \t{storage.quantityPalletsAllocated}\n" +
				$"Depot Name: \t{storage.depotName}\n" +
				$"Load Reference: \t{storage.loadReference}\n" +
				$"Quantity Cases: \t{storage.quantityCases}\n");
			*/

			db.saveProductIntoStorageLoad("StorageAllocation", storage.winNumber, storage.productDescription, storage.storageDate, storage.depotDate, storage.depotName, storage.quantityCases, storage.loadReference, storage.quantityPalletsAllocated,selectedProductionPlanVersion);




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

		public string inStorage { get; set; }

		public int quantityPalletsToAllocate { get; set; }
		public int quantityPalletsAllocated { get; set; }

		public int siteCapacityTarget { get; set; }
		public string productionPlanVersion { get; set; }
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