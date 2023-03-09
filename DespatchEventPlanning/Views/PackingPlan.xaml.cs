using DespatchEventPlanning.Database;
using DespatchEventPlanning.Helpers;
using DespatchEventPlanning.ObjectClasses;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;

namespace DespatchEventPlanning.Views
{
	/// <summary>
	/// Interaction logic for PackingPlan.xaml
	/// </summary>
	public partial class PackingPlan : UserControl
	{
		
		DatabaseClass db = new DatabaseClass();

		public PackingPlan()
		{
			InitializeComponent();

			excelDataGrid.ItemsSource = db.getInformationInList();
			//db.getInformationInList();
		}


		private void PackingDateCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			//excelDataGrid.ItemsSource = packingPlanList.Where(item => item.packingDate == PackingDateCalendar.SelectedDate.Value.ToShortDateString());
		}

		private void ClearPackingDateButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			//excelDataGridItemSourceReset();
		}

		private void excelDataGridItemSourceReset()
		{
			//excelDataGrid.ItemsSource = packingPlanList.OrderBy(item => item.packingDate);
		}
	}
}