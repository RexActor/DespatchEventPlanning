using DespatchEventPlanning.Database;



using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
		private string productInformationText = string.Empty;
		private string depotSplitText = string.Empty;
		private string defaultSplitText = string.Empty;
		private string forecastText = string.Empty;
		private string packingPlanText = string.Empty;

		private bool depotSplitsExists = false;
		private bool productInformationExists = false;
		private bool defaultDepotSplitsExists = false;
		private bool forecastExists = false;
		private bool packingPlanExists = false;


		private ToolTip depotSplitsToolTip;
		private ToolTip productInformationToolTip;
		private ToolTip defaultDepotSplitsToolTip;
		private ToolTip forecastToolTip;
		private ToolTip packingPlanTooltip;

		BackgroundWorker importProductInformationBackgroundWorker = new BackgroundWorker();
		BackgroundWorker importDepotSplitsBackgroundWorker = new BackgroundWorker();
		BackgroundWorker defaultDepotSplitBackgroundWorker = new BackgroundWorker();
		BackgroundWorker forecastBackgroundWorker = new BackgroundWorker();
		BackgroundWorker packingPlanBackgroundWorker = new BackgroundWorker();

		public ManageDatabaseUserControl()
		{
			InitializeComponent();

			handler = new HandleExcelFiles();

			productInformationProgressBar.Maximum = handler.GenerateProductInformation().Count;
			depotSplitProgressBar.Maximum = handler.GenerateDepotSplits().Count;
			defaultDepotSplitProgressBar.Maximum = handler.GenerateDefaultDepotSplits().Count;
			forecastProgressBar.Maximum = handler.GenerateForecast().Count;
			packingPlanProgressBar.Maximum = handler.GeneratePackingPlan().Count;
		}

		public bool IsSomethingBeingUpdated()
		{

			if (importProductInformationBackgroundWorker.IsBusy || importDepotSplitsBackgroundWorker.IsBusy || defaultDepotSplitBackgroundWorker.IsBusy || forecastBackgroundWorker.IsBusy || packingPlanBackgroundWorker.IsBusy)
			{
				return true;
			}
			else { return false; }
		}


		#region productInformation import section

		private void importProductInformationButton_Click(object sender, RoutedEventArgs e)
		{
			if (IsSomethingBeingUpdated()==true) { MessageBox.Show("Already Uploading items...! Please be patient!");return; }
			
			importProductInformationBackgroundWorker.WorkerReportsProgress = true;
			importProductInformationBackgroundWorker.DoWork += importProductInformationBackgroundWorker_DoWork;
			importProductInformationBackgroundWorker.ProgressChanged += importProductInformationBackgroundWorker_ProgressChanged;
			importProductInformationBackgroundWorker.RunWorkerCompleted += ImportProductInformationBackgroundWorker_RunWorkerCompleted;

			

			importProductInformationBackgroundWorker.RunWorkerAsync();
			productInformationToolTip = new ToolTip();
			productInformationProgressBar.ToolTip = productInformationToolTip;
		}

		private void importProductInformationBackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
		{
			float progressValue = e.ProgressPercentage;

			productInformationProgressBar.Value = progressValue;
			productInformationProgressBarProgressText.Text = $"{Math.Round((progressValue / productInformationProgressBar.Maximum) * 100)}%";
			productInformationTextBlock.Text = productInformationText;
			productInformationToolTip.Content = $"Uploading {progressValue} from {productInformationProgressBar.Maximum}";
		}

		private void importProductInformationBackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
		{
			int increase = 0;
			handler.GenerateProductInformation().AsEnumerable().ToList().ForEach(item =>
			{
				if (db.productExistsInProductInformationTable(item.winNumber, item.productNumber) == true)
				{
					increase++;
					productInformationExists = true;


				
				}
				else
				{
					productInformationExists = false;

					db.saveProductInformation("ProductInformation", item.winNumber, item.productNumber, item.productDescription, item.packsPerPallet, item.productGroup, item.weightOfOuter);
					productInformationText = $"Uploading: WIN {item.winNumber} " +
					$"has {item.productDescription} as description " +
					$"with product number {item.productNumber} " +

					$"have {item.packsPerPallet} cases per pallet " +
					$"belongs to {item.productGroup} group " +
					$"and it have {item.weightOfOuter} as weight of outer.";

					increase++;
					(sender as BackgroundWorker).ReportProgress(increase);
				}
			});
		}

		private void ImportProductInformationBackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
		{
			if (productInformationExists == true)
			{
				productInformationTextBlock.Text = "Nothing uploaded! Products already exist in database";
				productInformationToolTip.Content = "Nothing uploaded! Products already exist in database";
				productInformationProgressBarProgressText.Text = "Failed!";
			}
			else
			{
				productInformationTextBlock.Text = string.Empty;

				productInformationProgressBarProgressText.Text = "Completed!";
				productInformationToolTip.Content = $"Complted! In Total uploaded {productInformationProgressBar.Maximum} entries!";
			}
		}

		#endregion productInformation import section

		#region importDepotsplits section

		private void importDepotSplitsButton_Click(object sender, RoutedEventArgs e)
		{

			if (IsSomethingBeingUpdated() == true) { MessageBox.Show("Already Uploading items...! Please be patient!"); return; }
			
			importDepotSplitsBackgroundWorker.WorkerReportsProgress = true;
			importDepotSplitsBackgroundWorker.DoWork += ImportDepotSplitsBackgroundWorker_DoWork;
			importDepotSplitsBackgroundWorker.ProgressChanged += ImportDepotSplitsBackgroundWorker_ProgressChanged;
			importDepotSplitsBackgroundWorker.RunWorkerCompleted += ImportDepotSplitsBackgroundWorker_RunWorkerCompleted;

			importDepotSplitsBackgroundWorker.RunWorkerAsync();
			depotSplitsToolTip = new ToolTip();

			depotSplitProgressBar.ToolTip = depotSplitsToolTip;
		}

		private void ImportDepotSplitsBackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
		{
			if (depotSplitsExists == true)
			{
				depotSplitsTextBlock.Text = "Nothing uploaded! Products already exist in database";
				depotSplitsToolTip.Content = "Nothing uploaded! Products already exist in database";
				depotSplitProgressBarProgressText.Text = "Failed!";
			}
			else
			{
				depotSplitsTextBlock.Text = string.Empty;
				depotSplitProgressBarProgressText.Text= "Completed!";
				depotSplitsToolTip.Content = $"Complted! In Total uploaded  {depotSplitProgressBar.Maximum} entries!";
			}
		}

		private void ImportDepotSplitsBackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
		{
			float progressValue = e.ProgressPercentage;

			depotSplitProgressBar.Value = progressValue;

			depotSplitProgressBarProgressText.Text = $"{Math.Round((progressValue / depotSplitProgressBar.Maximum) * 100)}%";
			depotSplitsTextBlock.Text = depotSplitText;
			depotSplitsToolTip.Content = $"Loading {progressValue} from {depotSplitProgressBar.Maximum}";
		}

		private void ImportDepotSplitsBackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
		{
			int increase = 0;
			handler.GenerateDepotSplits().AsEnumerable().ToList().ForEach(item =>
			{
				if (db.productExistsIndepotSplitTable(item.winNumber, item.depotNumber, item.depotDate) == true)
				{
					increase++;
					depotSplitsExists = true;

					
				}
				else
				{
					depotSplitsExists = false;
					db.saveDepotSplits("DepotSplits", item.winNumber, item.productDescription, item.depotNumber, item.depotName, item.depotDate, item.qty);

					depotSplitText = $"Uploading: WIN {item.winNumber} " +
					$"has {item.productDescription} as description " +
					$"for depot number {item.depotNumber} " +

					$"which one is called {item.depotName} " +
					$"for {item.depotDate} depot date " +
					$"and have {item.qty} cases as split.";

					increase++;
					(sender as BackgroundWorker).ReportProgress(increase);

				}
			});
		}

		#endregion importDepotsplits section

		#region default Depot Split import

		private void importDefaultDepotSplitsButton_Click(object sender, RoutedEventArgs e)
		{
			if (IsSomethingBeingUpdated() == true) { MessageBox.Show("Already Uploading items...! Please be patient!"); return; }
			
			defaultDepotSplitBackgroundWorker.WorkerReportsProgress = true;
			defaultDepotSplitBackgroundWorker.DoWork += DefaultDepotSplitBackgroundWorker_DoWork;
			defaultDepotSplitBackgroundWorker.ProgressChanged += DefaultDepotSplitBackgroundWorker_ProgressChanged;
			defaultDepotSplitBackgroundWorker.RunWorkerCompleted += DefaultDepotSplitBackgroundWorker_RunWorkerCompleted;
			defaultDepotSplitBackgroundWorker.RunWorkerAsync();
			defaultDepotSplitsToolTip = new ToolTip();
			defaultDepotSplitProgressBar.ToolTip = defaultDepotSplitsToolTip;
		}

		private void DefaultDepotSplitBackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
		{
			if (defaultDepotSplitsExists == true)
			{
				defaultDepotSplitsTextBlock.Text = "Nothing uploaded! Products already exist in database";
				defaultDepotSplitsToolTip.Content = "Nothing uploaded! Products already exist in database";
				defaultDepotSplitProgressBarProgressText.Text = "Failed!";
			}
			else
			{
				defaultDepotSplitsTextBlock.Text = string.Empty;
				defaultDepotSplitProgressBarProgressText.Text = "Completed!";
				defaultDepotSplitsToolTip.Content = $"Complted! In Total uploaded {defaultDepotSplitProgressBar.Maximum} entries!";
			}
		}

		private void DefaultDepotSplitBackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
		{
			float progressValue = e.ProgressPercentage;

			defaultDepotSplitProgressBar.Value = progressValue;

			defaultDepotSplitProgressBarProgressText.Text = $"{Math.Round((progressValue / defaultDepotSplitProgressBar.Maximum) * 100)}%";
			defaultDepotSplitsTextBlock.Text = defaultSplitText;
			defaultDepotSplitsToolTip.Content = $"Loading {progressValue} from {defaultDepotSplitProgressBar.Maximum}";
		}

		private void DefaultDepotSplitBackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
		{
			int increase = 0;
			handler.GenerateDefaultDepotSplits().AsEnumerable().ToList().ForEach(item =>
			{
				if (db.productExistsInDefaultDepotSplitTable(item.winNumber, item.depotName) == true)
				{
					increase++;
					defaultDepotSplitsExists = true;


					
				}
				else
				{
					defaultDepotSplitsExists = false;
					db.saveDefaultDepotSplits("DefaultDepotSplits", item.productGroup, item.winNumber, item.productDescription, item.depotName, item.qty);

					defaultSplitText = $"Uploading: WIN {item.winNumber} " +
					$"has {item.productDescription} as description " +
					$"and belongs to  {item.productGroup} group " +

					$"for {item.depotName} depot " +
									$"and have {item.qty} cases as split.";

					increase++;
					(sender as BackgroundWorker).ReportProgress(increase);
				}
			});
		}

		#endregion default Depot Split import

		#region forecast Import

		private void importForecastButton_Click(object sender, RoutedEventArgs e)
		{
			if (IsSomethingBeingUpdated() == true) { MessageBox.Show("Already Uploading items...! Please be patient!"); return; }
		
			forecastBackgroundWorker.WorkerReportsProgress = true;
			forecastBackgroundWorker.DoWork += ForecastBackgroundWorker_DoWork;
			forecastBackgroundWorker.ProgressChanged += ForecastBackgroundWorker_ProgressChanged;
			forecastBackgroundWorker.RunWorkerCompleted += ForecastBackgroundWorker_RunWorkerCompleted;
			forecastBackgroundWorker.RunWorkerAsync();
			forecastToolTip = new ToolTip();
			forecastProgressBar.ToolTip = forecastToolTip;
		}

		private void ForecastBackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
		{
			if (forecastExists == true) {
				forecastTextBlock.Text = "Nothing uploaded! Products already exist in database";
				forecastToolTip.Content = "Nothing uploaded! Products already exist in database";
				forecastBarProgressText.Text = "Failed!";
			}
			else
			{
				forecastTextBlock.Text = string.Empty;
				forecastBarProgressText.Text = "Completed!";
				forecastToolTip.Content = $"Complted! In Total uploaded {forecastProgressBar.Maximum} entries!";
			}
		}

		private void ForecastBackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
		{
			float progressValue = e.ProgressPercentage;

			forecastProgressBar.Value = progressValue;

			forecastBarProgressText.Text = $"{Math.Round((progressValue / forecastProgressBar.Maximum) * 100)}%";
			forecastTextBlock.Text = forecastText;
			forecastToolTip.Content = $"Loading {progressValue} from {forecastProgressBar.Maximum}";
		}

		private void ForecastBackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
		{
			int increase = 0;
			handler.GenerateForecast().AsEnumerable().ToList().ForEach(item =>
			{
				if (db.productExistsInForecastTable(item.winNumber, item.depotDate) == true)
				{

					increase++;
					forecastExists = true;
				
				}
				else
				{
					forecastExists = false;

					db.saveForecast("Forecast", item.winNumber, item.productDescription, item.depotDate, item.qty);

					forecastText = $"Uploading: WIN {item.winNumber} " +
					$"has {item.productDescription} as description " +
					$"and have {item.depotDate} as depot date " +

					$"and have {item.qty} as forecast.";

					increase++;
					(sender as BackgroundWorker).ReportProgress(increase);
				}
			});
		}

		#endregion forecast Import

		#region packing Plan Import

		private void importPackingPlanButton_Click(object sender, RoutedEventArgs e)
		{
			if (IsSomethingBeingUpdated() == true) { MessageBox.Show("Already Uploading items...! Please be patient!"); return; }
			

			packingPlanBackgroundWorker.WorkerReportsProgress = true;
			packingPlanBackgroundWorker.DoWork += PackingPlanBackgroundWorker_DoWork;
			packingPlanBackgroundWorker.ProgressChanged += PackingPlanBackgroundWorker_ProgressChanged;
			packingPlanBackgroundWorker.RunWorkerCompleted += PackingPlanBackgroundWorker_RunWorkerCompleted;
			packingPlanBackgroundWorker.RunWorkerAsync();
			
			packingPlanTooltip = new ToolTip();
			packingPlanProgressBar.ToolTip = packingPlanTooltip;
		}

		private void PackingPlanBackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
		{
			if (packingPlanExists == true)
			{
				packingPlanTextBlock.Text = "Nothing uploaded! Products already exist in database";
				packingPlanTooltip.Content = "Nothing uploaded! Products already exist in database";
				packingPlanProgressText.Text = "Failed!";
				
				
			}
			else
			{
				
				packingPlanProgressText.Text = "Completed!";
				packingPlanTextBlock.Text = string.Empty;
				packingPlanTooltip.Content = $"Complted! In Total uploaded {packingPlanProgressBar.Maximum} entries!";
			}
		}

		private void PackingPlanBackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
		{
			float progressValue = e.ProgressPercentage;

			packingPlanProgressBar.Value = progressValue;

			packingPlanProgressText.Text = $"{Math.Round((progressValue / packingPlanProgressBar.Maximum) * 100)}%";
			packingPlanTextBlock.Text = packingPlanText;
			packingPlanTooltip.Content = $"Loading {progressValue} from {packingPlanProgressBar.Maximum}";
		}

		private void PackingPlanBackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
		{
			int increase = 0;

			handler.GeneratePackingPlan().AsEnumerable().ToList().ForEach(item =>
			{
				if (db.productExistsInPackingPlanTable(item.winNumber, item.packingDate, item.depotDate) == true)
				{
					increase++;
					packingPlanExists = true;

					
				}
				else
				{
					packingPlanExists = false;


					db.savePackingPlan("PackingPlan", item.winNumber, item.productDescription, item.productGroup, item.packingDate, item.depotDate, item.packingQuantity);

					packingPlanText = $"Uploading: WIN {item.winNumber} " +
					$"has {item.productDescription} as description " +
					$"and have {item.depotDate} as depot date " +
					$"which belongs to {item.productGroup} product group " +

					$"and {item.packingQuantity} is " +
					$"being packed on {item.packingDate} ";

					increase++;
					(sender as BackgroundWorker).ReportProgress(increase);
				}
			});
		}

		#endregion packing Plan Import
	}
}