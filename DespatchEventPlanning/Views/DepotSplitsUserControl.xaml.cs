using DespatchEventPlanning.Helpers;
using DespatchEventPlanning.Models;

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Controls;

namespace DespatchEventPlanning.Views
{
	public class DepotInformation
	{
		public int? depotNumber { get; set; }
		public string? DepotName { get; set; }
	}

	/// <summary>
	/// Interaction logic for DepotSplitsUserControl.xaml
	/// </summary>
	public partial class DepotSplitsUserControl : UserControl
	{
		private List<DepotInformation> DepotNames = new List<DepotInformation>() {
			new DepotInformation{ DepotName="ERITH",depotNumber=7461},
			new DepotInformation{ DepotName="LUTTERWORTH", depotNumber=7445 },
			new DepotInformation{DepotName="ROCHDALE",depotNumber=7479 },
			new DepotInformation{DepotName="SKELMERSDALE",depotNumber=7452 },
			new DepotInformation{DepotName="WAKEFIELD",depotNumber=7447 },
			new DepotInformation{DepotName="WASHINGTON",depotNumber=7457 },
			new DepotInformation{DepotName="FALKIRK",depotNumber=7460 },
			new DepotInformation{DepotName="LARNE",depotNumber=7430 },
			new DepotInformation{DepotName="BRISTOL",depotNumber=7498 },
			new DepotInformation{DepotName="BEDFORD",depotNumber=7439 }
		};

		private DataTableModel dataTableModel;
		private DataTable? depotSplitsDataTable;

		public DepotSplitsUserControl()
		{
			InitializeComponent();
			dataTableModel = new DataTableModel();
			CreateDepotSplitGrid();
		}

		public void CreateColumns()
		{
			List<DepotInformation> sortedList = DepotNames.OrderBy(o => o.depotNumber).ToList();

			foreach (var depot in sortedList)
			{
				DataGridTextColumn column = new DataGridTextColumn();
				column.Header = $"{depot.DepotName}";

				column.Width = 100;
				DepotSplitGrid.Columns.Add(column);
			}
		}

		public void CreateDepotSplitGrid()
		{
			depotSplitsDataTable = dataTableModel.GetDataTable("DepotSplits", EnumClass.FILE_NAME.DepotSplits);
			DepotSplitGrid.ItemsSource = depotSplitsDataTable.DefaultView;
		}
	}
}