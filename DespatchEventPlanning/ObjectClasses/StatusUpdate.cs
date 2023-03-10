using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DespatchEventPlanning.ObjectClasses
{
	public class StatusUpdate : INotifyPropertyChanged
	{


		private string message;
		public event PropertyChangedEventHandler? PropertyChanged;

		public StatusUpdate()
		{

		}
		public string Message
		{
			get { return this.message; }
			set
			{
				this.message = value;
				this.OnPropertyChanged("Message");
			}
		}

		void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = this.PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}


		
	}
}
