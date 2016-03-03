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
		string attribute = "No Attribute Change";

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
				// Load document
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
			
					// Create main, name, description nodes
					// Every element node needs a text node as well (denoted with capital T)
					XmlNode main = xml.CreateNode("element", "main", "");
					XmlNode name = xml.CreateNode("element", "name", "");
					XmlNode desc = xml.CreateNode("element", "description", "");
					XmlNode targ = xml.CreateNode("element", "target", "");
					XmlNode atrb = xml.CreateNode("element", "attribute", "");
					XmlNode atra = xml.CreateNode("element", "amount", "");
					XmlNode nameT = xml.CreateNode("text", "name", "");
					XmlNode descT = xml.CreateNode("text", "description", "");
					XmlNode targT = xml.CreateNode("text", "target", "");
					XmlNode atrbT = xml.CreateNode("text", "attribute", "");
					XmlNode atraT = xml.CreateNode("text", "amount", "");

					// Fill the nodes with default values
					nameT.Value = "default";
					descT.Value = "default";
					targT.Value = "default";
					atrbT.Value = "default";
					atraT.Value = "0";

					// Add nodes to the card node
					name.AppendChild(nameT);
					desc.AppendChild(descT);
					targ.AppendChild(targT);
					atrb.AppendChild(atrbT);
					atra.AppendChild(atraT);
					main.AppendChild(name);
					main.AppendChild(desc);
					main.AppendChild(targ);
					main.AppendChild(atrb);
					main.AppendChild(atra);
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

		// Saves a string based off of the selected attribute
		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			// Save the sending menu item
			MenuItem mitem = (MenuItem)sender;

			// Save the item's name as the selected attribute
			attribute = mitem.Header.ToString();
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

				// Set the deck (file) name
				CurrentDeck.Text = System.IO.Path.GetFileNameWithoutExtension(file);

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
				CurrentCard.Text           = GetName(xn);
				CurrentDescriptionBox.Text = GetDesc(xn);
				OldAttributeBox.Text	   = GetAtra(xn);
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

		// Returns the description of the document
		private string GetDesc(XmlNode xn)
		{
			// Select the name node's child value: return's the description
			return xn.SelectSingleNode("./main/description").FirstChild.Value;
		}

		// Returns the target of the object
		private string GetTarg(XmlNode xn)
		{
			// Select the name node's child value: return's the description
			return xn.SelectSingleNode("./main/target").FirstChild.Value;
		}

		// Returns the attribute of the object
		private string GetAtrb(XmlNode xn)
		{
			// Select the name node's child value: return's the description
			return xn.SelectSingleNode("./main/attribute").FirstChild.Value;
		}

		// Returns the attribute amount of the object
		private string GetAtra(XmlNode xn)
		{
			// Select the name node's child value: return's the description
			return xn.SelectSingleNode("./main/amount").FirstChild.Value;
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

				// Rename the file if the user entered new deck name
				RenameFile();

				// Save Confirmation box
				MessageBox.Show("File Saved");
			}
			catch (XmlException)
			{
				MessageBox.Show("XML Save Failure");
			}
		}

		// Renames the file to the new name if provided
		private void RenameFile()
		{
			// First check if we are given a new name
			if (NewDeckBox.Text != "")
			{
				try
				{
					// Save the current directory path
					string curDir = System.IO.Path.GetDirectoryName(currentPath);

					// For each file in our current directory...
					foreach (string file in Directory.GetFiles(curDir))
					{
						// ...make sure we don't have a file with that name already
						if (File.Exists(curDir + "//" + NewDeckBox.Text + ".xml"))
						{
							MessageBox.Show("File with that name already exists. No changes made to name.");
							return;
						}
					}

					// If we haven't returned, then the name is good
					// Rename the file to the new deck name
					File.Copy(currentPath, curDir + "//" + NewDeckBox.Text + ".xml");
					currentPath = curDir + "//" + NewDeckBox.Text + ".xml";

					// Clear the deck name box
					NewDeckBox.Text = "";
				}
				catch (IOException)
				{
					MessageBox.Show("IO Exception");
				}
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
				SaveTarg(ref xn);
				SaveAtrb(ref xn);
				SaveAtra(ref xn);
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
			if (NewCardBox.Text.Length >  0)
			{
				// Set the name node equal to the new name
				xn.SelectSingleNode("./main/name").FirstChild.Value = NewCardBox.Text;

				// Update the name of the file
				names[CardList.SelectedIndex] = NewCardBox.Text;
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

		// Saves the new target of the card if it exists
		private void SaveTarg(ref XmlNode xn)
		{
			// If no target is selected
			if (RB_NoTarget.IsChecked == true)
			{
				// Save target as "No Target"
				xn.SelectSingleNode("./main/target").FirstChild.Value = "No Target";
			}
			if (RB_SelfTarget.IsChecked == true)
			{
				// Save target as "Self Target"
				xn.SelectSingleNode("./main/target").FirstChild.Value = "Self Target";
			}
			if (RB_TargetOthers.IsChecked == true)
			{
				// Save target as "Target Others"
				xn.SelectSingleNode("./main/target").FirstChild.Value = "Target Others";
			}
		}

		// Saves the new attribute of the card if it exists
		private void SaveAtrb(ref XmlNode xn)
		{
			// Selects and saves the attribute currently stored in the program
			xn.SelectSingleNode("./main/attribute").FirstChild.Value = attribute;
		}

		// Saves the new attribute amount of the card if it exists
		private void SaveAtra(ref XmlNode xn)
		{
			// NewDescriptionBox must have a string in it to change the card's description
			if (AttributeChangeBox.Text.Length >  0)
			{
				// Set the description node equal to the new name
				xn.SelectSingleNode("./main/amount").FirstChild.Value = AttributeChangeBox.Text;
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
			CurrentCard.Text = "No Selection";
			CurrentDescriptionBox.Text = "No Selection";
		}

		// Clears the current textboxes
		private void ClearBoxes()
		{
			NewDescriptionBox.Text = "";
			NewCardBox.Text        = "";
		}

		// Updates the current names of the cards in the list
		private void UpdateCardList()
		{
			CardList.ItemsSource = names;
		}
	}
}
