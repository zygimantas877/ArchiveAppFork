using ArchiveApp.Data;
using ArchiveApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ArchiveApp
{
    public partial class Account : Form
    {
        public Account(User user)
        {
            InitializeComponent();
            Console.WriteLine(user.Id);
            this.user = user;
            this.newLocation = "";
            setInfo();
        }

        private void setInfo()
        {
            accountInfoLabel.Text = this.user.ToString("ML");
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (this.newLocation != "")
            {
                try
                {
                    foreach (string sourceFilePath in Directory.GetFiles(this.user.ArchiveLocation))
                    {
                        string fileName = Path.GetFileName(sourceFilePath);
                        string destinationFilePath = Path.Combine(this.newLocation, fileName);
                        File.Move(sourceFilePath, destinationFilePath);
                    }

                    //test
                    foreach (string sourceSubFolderPath in Directory.GetDirectories("C:\\Users\\karol\\Desktop\\t"))
                    {
                        Console.WriteLine(sourceSubFolderPath);
                        string subFolderName = Path.GetFileName(sourceSubFolderPath);
                        string destinationSubFolderPath = Path.Combine("C:\\Users\\karol\\Desktop\\a", subFolderName);
                        Console.WriteLine(destinationSubFolderPath);
                        Directory.Move(sourceSubFolderPath, destinationSubFolderPath);
                        Console.WriteLine("Done");
                    }

                    // Move the subfolders
                    foreach (string sourceSubFolderPath in Directory.GetDirectories(this.user.ArchiveLocation))
                    {
                        Console.WriteLine(sourceSubFolderPath);
                        string subFolderName = Path.GetFileName(sourceSubFolderPath);
                        string destinationSubFolderPath = Path.Combine(this.newLocation, subFolderName);
                        Console.WriteLine(destinationSubFolderPath);
                        Directory.Move(sourceSubFolderPath, destinationSubFolderPath);
                    }

                    MessageBox.Show("Folder contents moved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Console.WriteLine("Folder contents moved successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error has occured moving files and folders", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(ex.Message);
                    return;
                }

                using (DatabaseContext context = new DatabaseContext())
                {
                    List<Models.Image> allImages = (from image in context.Images
                                                    where image.UserId == this.user.Id
                                                    select image).ToList();

                    foreach (Models.Image image in allImages)
                    {
                        Group group = (from gr in context.Groups
                                       where gr.ImageId == image.Id
                                       select gr).FirstOrDefault();

                        group.Location = this.newLocation + "\\" + group.Name;

                        image.Location = group.Location + "\\" + image.Name;
                    }
                    context.SaveChanges();
                }
            }

            this.returnUser = this.user;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void changeButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            folderDialog.Description = "Select a folder";
            folderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            folderDialog.ShowNewFolderButton = true;

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                this.newLocation = folderDialog.SelectedPath;
                this.user.ArchiveLocation = this.newLocation;
            }
            setInfo();
            Console.WriteLine(this.user.ToString("1L"));
        }
    }
}
