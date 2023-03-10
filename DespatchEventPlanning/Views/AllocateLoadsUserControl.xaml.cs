using DespatchEventPlanning.ObjectClasses;

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
	}
}