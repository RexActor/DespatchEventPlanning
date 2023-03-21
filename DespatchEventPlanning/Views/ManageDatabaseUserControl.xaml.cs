using DespatchEventPlanning.Database;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DespatchEventPlanning.Views
{
	/// <summary>
	/// Interaction logic for ManageDatabaseUserControl.xaml
	/// </summary>
	public partial class ManageDatabaseUserControl : UserControl
	{

		DatabaseClass db = new DatabaseClass();
		public ManageDatabaseUserControl()
		{
			InitializeComponent();



			//this.Status.Message = "Uploading database...";
			//HandleExcelFiles excelFilehandler = new HandleExcelFiles();
			//excelFilehandler.CheckDatabaseTable();
		}

		private void importProductInformationButton_Click(object sender, RoutedEventArgs e)
		{


			HandleExcelFiles handler = new HandleExcelFiles();
			handler.GenerateProductInformation().AsEnumerable().ToList().ForEach(item => { 
				
				db.saveProductInformation("ProductInformation", item.winNumber, item.productNumber, item.productDescription,item.packsPerPallet, item.productGroup,item.weightOfOuter); 
			
			});



			
		}
	}
}
