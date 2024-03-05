using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Notepad_2._0
{
    public partial class NotepadApp : Form
    {
        bool mousedown;
        DataTable notes = new DataTable(); //  This will act as a backend for the datagridview
        bool editing = false; // This will be used to keep in check if the notes are being edited or not
        public NotepadApp()
        {
            InitializeComponent();
            this.FormClosing += NotepadApp_FormClosing;
            this.Load += NotepadApp_Load;
        }
        private void NotepadApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveNotesToFile();
        }

        private void NotepadApp_Load(object sender, EventArgs e)
        {
            Console.WriteLine("NotepadApp_Load executed.");

            // Add columns if they don't exist
            if (!notes.Columns.Contains("Title"))
                notes.Columns.Add("Title");
            if (!notes.Columns.Contains("Note"))
                notes.Columns.Add("Note");

            LoadNotesFromFile();
        }


        private void SaveNotesToFile()
        {
            try
            {
                string json = notes.ToJson();
                File.WriteAllText("notes.json", json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving notes to file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadNotesFromFile()
        {
            try
            {
                if (File.Exists("notes.json"))
                {
                    string json = File.ReadAllText("notes.json");
                    DataTable newNotes = json.ToDataTable();

                    // Clear the existing notes DataTable and copy the new data
                    notes.Clear();
                    foreach (DataRow row in newNotes.Rows)
                    {
                        notes.Rows.Add(row.ItemArray);
                    }

                    // Set the DataSource for the DataGridView
                    SavedData_Gridview.DataSource = notes;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading notes from file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Save_button_Click executed.");

            try
            {
                // Check if either Title or Note fields is empty
                if (string.IsNullOrWhiteSpace(Title_Textbox.Text) || string.IsNullOrWhiteSpace(Note_Textbox.Text))
                {
                    MessageBox.Show("Please enter both a Title and a Note before saving.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;  // Exit the method without further execution
                }

                // Assuming notes is initialized properly in the form's constructor or Load event
                int rowIndex = SavedData_Gridview.CurrentCell?.RowIndex ?? -1;

                if (editing && IsValidRowIndex(rowIndex))
                {
                    notes.Rows[rowIndex]["Title"] = Title_Textbox.Text;
                    notes.Rows[rowIndex]["Note"] = Note_Textbox.Text;
                }
                else
                {
                    notes.Rows.Add(Title_Textbox.Text, Note_Textbox.Text);
                }

                // Save data to file
                SaveNotesToFile();

                // Set the DataSource for the DataGridView
                SavedData_Gridview.DataSource = notes;

                // Refresh DataGridView (optional, depending on your setup)
                SavedData_Gridview.Refresh();

                Title_Textbox.Text = "";
                Note_Textbox.Text = "";
                editing = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Save_button_Click: {ex.Message}");
                MessageBox.Show($"An error occurred while saving. Error details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsValidRowIndex(int rowIndex)
        {
            throw new NotImplementedException();
        }

        private void Load_button_Click(object sender, EventArgs e)
        {
            if (SavedData_Gridview.CurrentCell != null && notes != null && notes.Rows.Count > SavedData_Gridview.CurrentCell.RowIndex)
            {
                Title_Textbox.Text = notes.Rows[SavedData_Gridview.CurrentCell.RowIndex].ItemArray[0].ToString();
                Note_Textbox.Text = notes.Rows[SavedData_Gridview.CurrentCell.RowIndex].ItemArray[1].ToString();
                editing = true;
            }
            else
            {
                // Handle the case when data is not available or in an unexpected state
                MessageBox.Show("Cannot load data. Check if the data is available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Newnote_button_Click(object sender, EventArgs e)
        {
            if (SavedData_Gridview.SelectedCells.Count > 0)
            {
                // If a note is selected, allow editing the note
                if (editing)
                {
                    // If editing, modify the current note with the new values
                    notes.Rows[SavedData_Gridview.CurrentCell.RowIndex]["Title"] = Title_Textbox.Text;
                    notes.Rows[SavedData_Gridview.CurrentCell.RowIndex]["Note"] = Note_Textbox.Text;
                    editing = false; // Exiting edit mode after modifying
                }
                else
                {
                    MessageBox.Show("No note is currently being edited.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Clear the textboxes
                Title_Textbox.Text = "";
                Note_Textbox.Text = "";
            }
            else
            {
                MessageBox.Show("Please select a note to edit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Delete_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (notes != null && SavedData_Gridview.CurrentCell != null)
                {
                    int rowIndex = SavedData_Gridview.CurrentCell.RowIndex;
                    notes.Rows[rowIndex].Delete();

                    // Save data to file after deletion
                    SaveNotesToFile();
                }
                else
                {
                    Console.WriteLine("Either notes or CurrentCell is null");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting note: {ex.Message}");
                MessageBox.Show("An error occurred while deleting. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SavedData_Gridview_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Title_Textbox.Text = notes.Rows[SavedData_Gridview.CurrentCell.RowIndex].ItemArray[0].ToString(); //
            Note_Textbox.Text = notes.Rows[SavedData_Gridview.CurrentCell.RowIndex].ItemArray[1].ToString();
            editing = true;
        }

        private void exitapp_img_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void guna2GradientPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2GradientPanel2_MouseDown(object sender, MouseEventArgs e)
        {
            mousedown = true;
        }

        private void guna2GradientPanel2_MouseMove(object sender, MouseEventArgs e)
        {
            if(mousedown)
            {
                int mousex = MousePosition.X - 400;
                int mousey = MousePosition.Y - 20;
                this.SetDesktopLocation(mousex, mousey);

            }
        }

        private void guna2GradientPanel2_MouseUp(object sender, MouseEventArgs e)
        {
            mousedown=false;
        }
    }
}
