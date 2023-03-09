using DespatchEventPlanning.Database;

using System.Linq;
using System.Windows.Controls;

namespace DespatchEventPlanning.Views
{
	/// <summary>
	/// Interaction logic for PackingPlan.xaml
	/// </summary>
	public partial class PackingPlan : UserControl
	{
		private DatabaseClass db = new DatabaseClass();

		public PackingPlan()
		{
			InitializeComponent();

			excelDataGrid.ItemsSource = db.getInformationInList();
		}

		private void PackingDateCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			excelDataGrid.ItemsSource = db.getInformationInList().Where(item => item.packingDate == PackingDateCalendar.SelectedDate.Value.ToShortDateString());
		}

		private void ClearPackingDateButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			excelDataGridItemSourceReset();
		}

		private void excelDataGridItemSourceReset()
		{
			excelDataGrid.ItemsSource = db.getInformationInList().OrderBy(item => item.packingDate);
		}
	}
}