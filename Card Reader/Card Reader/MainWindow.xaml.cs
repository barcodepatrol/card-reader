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
using System.Collections.ObjectModel;

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
		List<XmlNode> cards;
		ObservableCollection<string> names;
		XmlNode currentNode;
		string currentPath = "";

		// =========================================================
		// ==================== Event Handlers =====================
		// =========================================================

		// Constructor
		public MainWindow()
		{
			InitializeComponent();
			xml   = new XmlDocument();
			cards = new List<XmlNode>();
			names = new ObservableCollection<string>();
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

		// Saves the currently selected file
		private void SaveFile_Click(object sender, RoutedEventArgs e)
		{
			// If we have loaded a file already
			if (currentPath.Length > 0)
			{
				// Saves the new xml document to the currently selected path
				SaveXml(currentPath);

				// Clear Boxes, reload all of the data
				LoadXml(currentPath);
				
				// Update the GUI
				UpdateGUI();
			}
		}

		// Saves the new card info
		private void SaveCard_Click(object sender, RoutedEventArgs e)
		{
			if (currentPath.Length > 0)
			{
				// Saves the new card data into the xml deck
				SaveCardXml(ref currentNode);

				// Update the GUI
				UpdateGUI();
			}
		}

		// Creates a new xml file and selects it as the current file
		private void AddDeck_Click(object sender, RoutedEventArgs e)
		{
			// Only works when a file is already loaded in and selected
			if (currentPath != "")
			{
				try
				{
					// Create a new XmlDocument
					XmlDocument newDeck = new XmlDocument();

					// Load a deck node
					newDeck.LoadXml("<deck></deck>");

					// Save the data to a file and format the output
					XmlTextWriter writer = new XmlTextWriter((Directory.GetParent(currentPath) + "//new_deck.xml"), Encoding.UTF8);
					writer.Formatting    = Formatting.Indented;
					newDeck.Save(writer);

					// Close the file
					writer.Close();

					// Select and load the current deck
					LoadXml(Directory.GetParent(currentPath) + "//new_deck.xml");
				}
				catch (XmlException)
				{
					MessageBox.Show("Unable to create new deck.");
				}
			}
			else
			{
				MessageBox.Show("Please select a file first.");
			}


		}

		// Creates a new XmlNode that is added to cards and to names
		private void AddCard_Click(object sender, RoutedEventArgs e)
		{
			// We can only add cards if we already have an xml document open
			if (currentPath != "")
			{
				try
				{
					// Create our new XmlNode
					XmlNode newNode = xml.CreateNode("element", "card", "");
			
					// Create main, name, and description nodes
					// name and desc must also have two Text nodes created for them
					XmlNode main = xml.CreateNode("element", "main", "");
					XmlNode name = xml.CreateNode("element", "name", "");
					XmlNode desc = xml.CreateNode("element", "description", "");
					XmlNode nameT = xml.CreateNode("text", "name", "");
					XmlNode descT = xml.CreateNode("text", "description", "");

					// Fill the nodes with default values
					nameT.Value = "default";
					descT.Value = "default";

					// Add nodes to the card node
					name.AppendChild(nameT);
					desc.AppendChild(descT);
					main.AppendChild(name);
					main.AppendChild(desc);
					newNode.AppendChild(main);

					// Add this new card node to our XmlFile
					xml.SelectSingleNode("./deck").AppendChild(newNode);

					// Update our cards and names lists
					cards.Add(newNode);
					names.Add("default");
			
					// Finally update GUI to reflect new card
					UpdateGUI();
				}
				catch (XmlException)
				{
					MessageBox.Show("Card Creation Failed!");
				}
			}
			else
			{
				MessageBox.Show("Please select a file first.");
			}
		}

		// Selects a file from the FileList when it is double clicked on
		private void CardList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			// If they double clicked with the left mouse button
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				// Update the GUI
				UpdateGUI();

				// Sets the CurrentFile's text to equal the current selected file
				currentNode = cards[CardList.SelectedIndex];
				LoadCard(currentNode);
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

				// Clear the card and names list
				cards.Clear();
				names.Clear();

				// Loop through all of the cards
				foreach (XmlNode xmln in xml.SelectNodes("/deck/card"))
				{
					cards.Add(xmln);
					names.Add(GetName(xmln));
				}

				// Set the CardList equal to card's name source
				CardList.ItemsSource = names;
			}
			catch (XmlException)
			{
				MessageBox.Show("XML Load Failure");
			}
		}

		// Loads a single card and displays it on the screen
		private void LoadCard(XmlNode xn)
		{
			try
			{
				// Displays the xml data
				CurrentFile.Text           = GetName(xn);
				CurrentDescriptionBox.Text = GetDesc(xn);
			}
			catch (XmlException)
			{
				MessageBox.Show("XML Data Failure");
			}
		}

		// Returns the name of the document
		private string GetName(XmlNode xn)
		{
			// Select the name node's child value: return's the name
			return xn.SelectSingleNode("./main/name").FirstChild.Value;
		}

		// Returns the name of the document
		private string GetDesc(XmlNode xn)
		{
			// Select the name node's child value: return's the description
			return xn.SelectSingleNode("./main/description").FirstChild.Value;
		}

		// Saves the entire XML Document
		private void SaveXml(string file)
		{
			try
			{
				// Clear Nodes
				xml.SelectSingleNode("./deck").RemoveAll();

				// Add our nodes back in
				foreach (XmlNode xmln in cards)
				{
					xml.SelectSingleNode("./deck").AppendChild(xmln);
				}

				// Saves the edited file
				xml.Save(file);

				// Save Confirmation box
				MessageBox.Show("File Saved");
			}
			catch (XmlException)
			{
				MessageBox.Show("XML Save Failure");
			}
		}

		// Saves a single card into the xml document
		private void SaveCardXml(ref XmlNode xn)
		{
			try
			{
				// Saves the card data in the xml file
				SaveName(ref xn);
				SaveDesc(ref xn);
			}
			catch (XmlException)
			{
				MessageBox.Show("XML Data Save Failure");
			}
		}

		// Saves the new name of the card if it exists
		private void SaveName(ref XmlNode xn)
		{
			// NewFileBox must have a string in it to rename the card
			if (NewFileBox.Text.Length >  0)
			{
				// Set the name node equal to the new name
				xn.SelectSingleNode("./main/name").FirstChild.Value = NewFileBox.Text;

				// Update the name of the file
				names[CardList.SelectedIndex] = NewFileBox.Text;
			}
		}

		// Saves the new description of the card if it exists
		private void SaveDesc(ref XmlNode xn)
		{
			// NewDescriptionBox must have a string in it to change the card's description
			if (NewDescriptionBox.Text.Length >  0)
			{
				// Set the description node equal to the new name
				xn.SelectSingleNode("./main/description").FirstChild.Value = NewDescriptionBox.Text;
			}
		}

		
		// =========================================================
		// =================== Helper Functions ====================
		// =========================================================
		
		// Updates certain GUI aspects
		private void UpdateGUI()
		{
			ClearBoxes();
			UpdateCardList();
			CurrentFile.Text = "No Selection";
			CurrentDescriptionBox.Text = "No Selection";
		}

		// Clears the current textboxes
		private void ClearBoxes()
		{
			NewDescriptionBox.Text = "";
			NewFileBox.Text        = "";
		}

		// Updates the current names of the cards in the list
		private void UpdateCardList()
		{
			CardList.ItemsSource = names;
		}
	}
}
