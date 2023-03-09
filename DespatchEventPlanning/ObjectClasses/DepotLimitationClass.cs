using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DespatchEventPlanning.ObjectClasses
{
	internal class DepotLimitationClass
	{

		List<DepotLimitationClass> depotLimitationRuleSet;

		public string depotName { get; set; }
		public int depotNumber { get; set; }

		public int palletsPerLoads { get; private set; } = 26;

		public int maxPalletsPerDepotDate { get; set; }

		public int maxLoadsPerDepotDate { get { return maxPalletsPerDepotDate / palletsPerLoads; } }

		public int maxPalletsPerOutboundDate { get; set; }

		public int maxLoadsPerOutboundDate { get { return maxPalletsPerOutboundDate / palletsPerLoads; } }





		public List<DepotLimitationClass> GetDepotLimitationList()
		{

			depotLimitationRuleSet = new List<DepotLimitationClass>() { 
			
			
			new DepotLimitationClass()
			{
				depotName="ERITH",
				depotNumber=7461,
				maxPalletsPerDepotDate=208,
				maxPalletsPerOutboundDate=130
			},
			new DepotLimitationClass()
			{
				depotName="LUTTERWORTH",
				depotNumber=7445,
				maxPalletsPerDepotDate=312,
				maxPalletsPerOutboundDate=182
			},
			new DepotLimitationClass()
			{
				depotName="ROCHDALE",
				depotNumber=7479,
				maxPalletsPerDepotDate=156,
				maxPalletsPerOutboundDate=78
			},
			new DepotLimitationClass()
			{
				depotName="SKELMERSDALE",
				depotNumber=7452,
				maxPalletsPerDepotDate=338,
				maxPalletsPerOutboundDate=182
			},
			new DepotLimitationClass()
			{
				depotName="WAKEFIELD",
				depotNumber=7447,
				maxPalletsPerDepotDate=234,
				maxPalletsPerOutboundDate=130
			},
			new DepotLimitationClass()
			{
				depotName="WASHINGTON",
				depotNumber=7457,
				maxPalletsPerDepotDate=156,
				maxPalletsPerOutboundDate=78
			},
			new DepotLimitationClass()
			{
				depotName="FALKIRK",
				depotNumber=7460,
				maxPalletsPerDepotDate=338,
				maxPalletsPerOutboundDate=182
			},
			new DepotLimitationClass()
			{
				depotName="BRISTOL",
				depotNumber=7498,
				maxPalletsPerDepotDate=364,
				maxPalletsPerOutboundDate=208
			},
			new DepotLimitationClass()
			{
				depotName="BEDFORD",
				depotNumber=7439,
				maxPalletsPerDepotDate=234,
				maxPalletsPerOutboundDate=130
			},
			new DepotLimitationClass()
			{
				depotName="LARNE",
				depotNumber=7430,
				maxPalletsPerDepotDate=0,
				maxPalletsPerOutboundDate=0
			},





			};





			return depotLimitationRuleSet;


		}


	}
}
