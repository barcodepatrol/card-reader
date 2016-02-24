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
using System.Xml;

namespace Card_Reader
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// =========================================================
		// ======================= Variables =======================
		// =========================================================
		XmlDocument xml;

		// =========================================================
		// ==================== Event Handlers =====================
		// =========================================================

		// Constructor
		public MainWindow()
		{
			InitializeComponent();
			xml = new XmlDocument();
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
				LoadXml(fibox.FileName);
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
				LoadXml((string)FileList.Items[FileList.SelectedIndex]);
			}
		}

		// =========================================================
		// ===================== XML Functions =====================
		// =========================================================

		// Loads the entire XML Document and displays it on the screen
		private void LoadXml(string file)
		{
			try
			{
				// Load the file
				xml.Load(file);

				// Displays the xml data
				CurrentFile.Text           = GetName(file);
				CurrentDescriptionBox.Text = GetDesc(file);
			}
			catch (XmlException)
			{
				MessageBox.Show("XML Load Failure");
			}
		}

		// Returns the name of the document
		private string GetName(string file)
		{
			// Select the name node's child value: return's the name
			return xml.SelectSingleNode("/card/main/name").FirstChild.Value;
		}

		// Returns the name of the document
		private string GetDesc(string file)
		{
			// Select the name node's child value: return's the description
			return xml.SelectSingleNode("/card/main/description").FirstChild.Value;
		}
	}
}
