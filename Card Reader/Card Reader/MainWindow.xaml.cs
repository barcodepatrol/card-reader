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
		List<string> paths;
		string currentPath;
		string folderPath;

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
				currentPath = fibox.FileName;
				LoadXml(currentPath);
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
				// Sets the FileList to display the files by their names (not paths)
				// Key: name of card | Value: file path
				List<string> names = new List<string>();
				paths              = new List<string>();

				// Save the folder path
				folderPath = fobox.SelectedPath;

				// Gets the name of each file and adds it to our list
				foreach (string file in Directory.GetFiles(folderPath))
				{
					try
					{
						xml.Load(file);
						names.Add(xml.SelectSingleNode("/card/main/name").FirstChild.Value);
						paths.Add(file);
					}
					catch (XmlException)
					{
						names.Add("Empty/Broken Xml File");
						paths.Add(file);
					}
				}

				// Set the file list to display the names
				// When we access the filelist via names, we access paths not names
				FileList.ItemsSource = names;
			}
		}

		// Selects a file from the FileList when it is double clicked on
		private void FileList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			// If they double clicked with the left mouse button
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				// Sets the CurrentFile's text to equal the current selected file
				currentPath = paths[FileList.SelectedIndex];
				LoadXml(currentPath);
			}
		}

		// Saves the currently selected file
		private void SaveFile_Click(object sender, RoutedEventArgs e)
		{
			// If we have loaded a file already
			if (currentPath.Length > 0)
			{
				// Saves the new xml document to the currently selected path
				SaveXml(currentPath);

				// We reload the same Xml so that the current data is wiped
				LoadXml(currentPath);
				UpdateFileList();
				ClearBoxes();
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

		// Saves the entire XML Document
		private void SaveXml(string file)
		{
			try
			{
				// Change values
				SaveName(file);
				SaveDesc(file);

				// Saves the edited file
				xml.Save(file);

				// Save Confirmation box
				MessageBox.Show("File Saved");
			}
			catch (XmlException)
			{
				MessageBox.Show("XML Load Failure");
			}
		}

		// Saves the new name of the card if it exists
		private void SaveName(string file)
		{
			// NewFileBox must have a string in it to rename the card
			if (NewFileBox.Text.Length >  0)
			{
				// Set the name node equal to the new name
				xml.SelectSingleNode("/card/main/name").FirstChild.Value = NewFileBox.Text;
			}
		}

		// Saves the new description of the card if it exists
		private void SaveDesc(string file)
		{
			// NewDescriptionBox must have a string in it to change the card's description
			if (NewDescriptionBox.Text.Length >  0)
			{
				// Set the description node equal to the new name
				xml.SelectSingleNode("/card/main/description").FirstChild.Value = NewDescriptionBox.Text;
			}
		}

		
		// =========================================================
		// =================== Helper Functions ====================
		// =========================================================
		
		// Clears the current textboxes
		private void ClearBoxes()
		{
			NewDescriptionBox.Text = "";
			NewFileBox.Text        = "";
		}

		// Updates the names in the FileList
		private void UpdateFileList()
		{
			// If our folder path exists (we have opened a folder)
			if (folderPath.Length > 0)
			{
				// Create a list of the file names
				List<string> names = new List<string>();

				// Gets the name of each file and adds it to our list
				foreach (string file in Directory.GetFiles(folderPath))
				{
					try
					{
						// Load and update list of names
						xml.Load(file);
						names.Add(xml.SelectSingleNode("/card/main/name").FirstChild.Value);
					}
					catch (XmlException)
					{
						names.Add("Empty/Broken Xml File");
					}
				}

				// Save the new FileList
				FileList.ItemsSource = names;
			}
		}
	}
}
