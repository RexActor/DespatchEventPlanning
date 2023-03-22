using DespatchEventPlanning.Database;

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DespatchEventPlanning.Views
{
	/// <summary>
	/// Interaction logic for ManageDatabaseUserControl.xaml
	/// </summary>
	public partial class ManageDatabaseUserControl : UserControl
	{
		private DatabaseClass db = new DatabaseClass();
		private HandleExcelFiles handler;
		private BackgroundWorker backgroundWorker;
		string productInformationText = string.Empty;
		string depotSplitText = string.Empty;

		ToolTip importDepotSplitsToolTip;
		ToolTip importProductInformationToolTip;


		public ManageDatabaseUserControl()
		{
			InitializeComponent();

			handler = new HandleExcelFiles();

			productInformationProgressBar.Maximum = handler.GenerateProductInformation().Count;
			depotSplitProgressBar.Maximum = handler.GenerateDepotSplits().Count;




		}
		#region productInformation import section
		private void importProductInformationButton_Click(object sender, RoutedEventArgs e)
		{
			BackgroundWorker importProductInformationBackgroundWorker = new BackgroundWorker();
			importProductInformationBackgroundWorker.WorkerReportsProgress = true;
			importProductInformationBackgroundWorker.DoWork += importProductInformationBackgroundWorker_DoWork;
			importProductInformationBackgroundWorker.ProgressChanged += importProductInformationBackgroundWorker_ProgressChanged;
			importProductInformationBackgroundWorker.RunWorkerCompleted += ImportProductInformationBackgroundWorker_RunWorkerCompleted;


			importProductInformationBackgroundWorker.RunWorkerAsync();
			importProductInformationToolTip = new ToolTip();
			productInformationProgressBar.ToolTip = importProductInformationToolTip;

		}
		private void importProductInformationBackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
		{
			float progressValue = e.ProgressPercentage;

			productInformationProgressBar.Value = progressValue;
			productInformationProgressBarProgressText.Text = $"{Math.Round((progressValue / productInformationProgressBar.Maximum)*100)}%";
			productInformationTextBlock.Text = productInformationText;
			importProductInformationToolTip.Content = $"Uploading {progressValue} from {productInformationProgressBar.Maximum}";
		}

		private void importProductInformationBackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
		{
			

			int increase = 0;
			handler.GenerateProductInformation().AsEnumerable().ToList().ForEach(item =>
			{
				db.saveProductInformation("ProductInformation", item.winNumber, item.productNumber, item.productDescription, item.packsPerPallet, item.productGroup, item.weightOfOuter);
				productInformationText = $"Uploading: WIN {item.winNumber} " +
				$"has {item.productDescription} as description " +
				$"with product number {item.productNumber} " +
				
				$"have {item.packsPerPallet} cases per pallet " +
				$"belongs to {item.productGroup} group " +
				$"and it have {item.weightOfOuter} as weight of outer.";
			


				increase++;
				(sender as BackgroundWorker).ReportProgress(increase);
				



			});
		}
		private void ImportProductInformationBackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
		{
			productInformationTextBlock.Text = "Product information upload completed!!!";
			importProductInformationToolTip.Content = $"Upload completed. Fully uploaded {productInformationProgressBar.Maximum} entries";
		}
		#endregion

		#region importDepotsplits section

		private void importDepotSplitsButton_Click(object sender, RoutedEventArgs e)
		{
			BackgroundWorker importDepotSplitsBackgroundWorker= new BackgroundWorker();
			importDepotSplitsBackgroundWorker.WorkerReportsProgress= true;
			importDepotSplitsBackgroundWorker.DoWork += ImportDepotSplitsBackgroundWorker_DoWork;
			importDepotSplitsBackgroundWorker.ProgressChanged += ImportDepotSplitsBackgroundWorker_ProgressChanged;
			importDepotSplitsBackgroundWorker.RunWorkerCompleted += ImportDepotSplitsBackgroundWorker_RunWorkerCompleted;

			importDepotSplitsBackgroundWorker.RunWorkerAsync();
			importDepotSplitsToolTip = new ToolTip();

			depotSplitProgressBar.ToolTip = importDepotSplitsToolTip;
		}

		private void ImportDepotSplitsBackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
		{
			depotSplitsTextBlock.Text = "Depot splits uploaded successfully";
			importDepotSplitsToolTip.Content = $"Upload completed. Fully uploaded {depotSplitProgressBar.Maximum} entries";
		}

		private void ImportDepotSplitsBackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
		{
			float progressValue = e.ProgressPercentage;

			
			depotSplitProgressBar.Value = progressValue;

			depotSplitProgressBarProgressText.Text = $"{Math.Round((progressValue / depotSplitProgressBar.Maximum) * 100)}%";
			depotSplitsTextBlock.Text = depotSplitText;
			importDepotSplitsToolTip.Content = $"Loading {progressValue} from {depotSplitProgressBar.Maximum}";
		}

		private void ImportDepotSplitsBackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
		{
		

			int increase = 0;
			handler.GenerateDepotSplits().AsEnumerable().ToList().ForEach(item =>
			{
				db.saveDepotSplits("DepotSplits", item.winNumber, item.productDescription, item.depotNumber, item.depotName, item.depotDate, item.qty);



				depotSplitText = $"Uploading: WIN {item.winNumber} " +
				$"has {item.productDescription} as description " +
				$"for depot number {item.depotNumber} " +

				$"which one is called {item.depotName} " +
				$"for {item.depotDate} depot date " +
				$"and have {item.qty} cases as split.";



				increase++;
				(sender as BackgroundWorker).ReportProgress(increase);




			});
		}
		#endregion
		private void importDefaultDepotSplitsButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void importForecastButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void importPackingPlanButton_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}