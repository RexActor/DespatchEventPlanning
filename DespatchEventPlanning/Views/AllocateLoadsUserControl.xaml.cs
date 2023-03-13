using DespatchEventPlanning.ObjectClasses;

using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace DespatchEventPlanning.Views
{
	/// <summary>
	/// Interaction logic for AllocateLoadsUserControl.xaml
	/// </summary>
	public partial class AllocateLoadsUserControl : UserControl
	{
		StorageAllocationClass storage = new StorageAllocationClass();
		public AllocateLoadsUserControl()
		{
			InitializeComponent();
			storage.AllocateStorage();

			allocatedLoads.ItemsSource = storage.GetAllocatedLoads();

			storageSummary.ItemsSource = storage.GetAllocatedLoadsSummary(storage.GetAllocatedLoads());

		}





		private void AllocationCheckbox_Checked(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("Adding load to the list?");
		}

		private void AllocationCheckbox_Unchecked(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("Removing product from list?");
		}
	}
}