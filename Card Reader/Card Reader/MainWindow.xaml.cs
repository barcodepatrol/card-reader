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
using System.IO;

namespace Card_Reader
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		// Select a file and open it
		private void OpenFile_Click(object sender, RoutedEventArgs e)
		{
			// Create OpenFileDialog 
			Microsoft.Win32.OpenFileDialog fibox = new Microsoft.Win32.OpenFileDialog();

			// Set filter for file extension and default file extension 
			fibox.DefaultExt = ".xml";
			fibox.Filter = "XML Files (*.xml)|*.xml";

			// Show open file dialog box
			Nullable<bool> result = fibox.ShowDialog();

			// Process open file dialog box results
			if (result == true)
			{
				// Open document
				CurrentFile.Text = fibox.FileName;
			}
		}

		// Select a folder and load its contents into the file list
		private void OpenFolder_Click(object sender, RoutedEventArgs e)
		{
			// Create FolderBrowserDialog
			System.Windows.Forms.FolderBrowserDialog fobox = new System.Windows.Forms.FolderBrowserDialog();

			// Save folder files
			System.Windows.Forms.DialogResult result = fobox.ShowDialog();

			// If they did not cancel or close the form
			if (result != System.Windows.Forms.DialogResult.Cancel)
			{
				// Sets the FileList to display the files
				FileList.ItemsSource = Directory.GetFiles(fobox.SelectedPath);
			}
		}

		// Selects a file from the FileList when it is double clicked on
		private void FileList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			// If they double clicked with the left mouse button
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				// Sets the CurrentFile's text to equal the current selected file
				CurrentFile.Text = (string)FileList.Items[FileList.SelectedIndex];
			}
		}
	}
}
