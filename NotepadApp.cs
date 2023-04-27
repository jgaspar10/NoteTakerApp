using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad_2._0
{
    public partial class NotepadApp : Form
    {
        DataTable notes = new DataTable(); //  This will act as a backend for the datagridview
        bool editing = false; // This will be using to keep in check if the notes are being edited or not
        public NotepadApp()
        {
            InitializeComponent();
        }

        private void NotepadApp_Load(object sender, EventArgs e)
        {
            notes.Columns.Add("Title"); // entry for the DataTable
            notes.Columns.Add("Note"); // entry for the DataTable

            SavedData_Gridview.DataSource = notes; // This will set the DataSource (Gridview) equals to the DataTable
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            if(editing)
            {
               notes.Rows[SavedData_Gridview.CurrentCell.RowIndex]["Title"] = Title_Textbox.Text;
               notes.Rows[SavedData_Gridview.CurrentCell.RowIndex]["Title"] = Note_Textbox.Text;
            }
            else
            {
                notes.Rows.Add(Title_Textbox.Text, Note_Textbox.Text);
            }
            Title_Textbox.Text = "";
            Note_Textbox.Text = "";
            editing = false;

        }

        private void Load_button_Click(object sender, EventArgs e)
        {
            Title_Textbox.Text = notes.Rows[SavedData_Gridview.CurrentCell.RowIndex].ItemArray[0].ToString(); //
            Note_Textbox.Text = notes.Rows[SavedData_Gridview.CurrentCell.RowIndex].ItemArray[1].ToString();
            editing = true;


        }

        private void Newnote_button_Click(object sender, EventArgs e)
        {
            Title_Textbox.Text = "";
            Note_Textbox.Text = "";
        }

        private void Delete_button_Click(object sender, EventArgs e)
        {
            try
            {
                notes.Rows[SavedData_Gridview.CurrentCell.RowIndex].Delete(); //  This makes sure that the user has selected an index on the DatagridView and deletes it
            }
            catch (Exception ex) { Console.WriteLine("Not a valid note"); } // show a message in case the user tries to delete an invalid row or column
        }

        private void SavedData_Gridview_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Title_Textbox.Text = notes.Rows[SavedData_Gridview.CurrentCell.RowIndex].ItemArray[0].ToString(); //
            Note_Textbox.Text = notes.Rows[SavedData_Gridview.CurrentCell.RowIndex].ItemArray[1].ToString();
            editing = true;
        }

        private void exitapp_img_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
