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
using Image = System.Drawing.Image;

namespace ArchiveApp
{
    public partial class AddImage : Form
    {
        public AddImage(User user)
        {
            InitializeComponent();
            this.user = user;
            this.archiveLocation = user.ArchiveLocation;
            this.path = "";
        }

        private void selectImage_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "PNG files(*.png)|*.png| jpg files(*.jpg)|*.jpg| All files(*.*)|*.*";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.path = dialog.FileName;
                    Console.WriteLine(path);

                    pictureBox1.ImageLocation = path;
                }
            } 
            catch (Exception ex)
            {
                MessageBox.Show("An error has occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
            }
        }

        private void addImage_Click(object sender, EventArgs e)
        {
            if (this.path == "")
            {
                MessageBox.Show("Please select an image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string groupName = groupTextBox.Text;
            string tagString = tagsTextBox.Text;
            if (groupName == "")
            {
                MessageBox.Show("Please add a group", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (tagString == "")
            {
                MessageBox.Show("Please add at least one tag", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            string groupLocation = this.archiveLocation + "\\" + groupName;
            Group group = new Group(groupName, groupLocation);

            Tag[] tags;
            if (tagString.Contains(" ")) tags = splitTags(tagString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            else tags = splitTags(tagString);

            if (!createDir(groupLocation)) return;

            string fileLoacation = moveFile(moveCheckBox.Checked, groupLocation, this.path);
            if (fileLoacation == "") return;

            using (DatabaseContext context = new DatabaseContext())
            {
                string fileName = Path.GetFileName(fileLoacation);
                Models.Image newImage = new Models.Image(fileName, fileLoacation, this.user.Id, group, tags);

                var foundImage = (from image in context.Images
                                 where image.Location == fileLoacation && image.UserId == this.user.Id
                                 select image).FirstOrDefault();

                if (foundImage != null)
                {
                    MessageBox.Show("This image is already added", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                context.Images.Add(newImage);
                context.SaveChanges();
            }

            pictureBox1.Image = null;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private Tag[] splitTags(params string[] strings)
        {
            Tag[] tags = new Tag[strings.Length];
            for (int i = 0; i < strings.Length; i++)
            {
                tags[i] = new Tag(strings[i].Trim());
            }
            return tags;
        }

        private string moveFile(bool move, string groupL, string fileL)
        {
            string destinationFilePath = Path.Combine(groupL, Path.GetFileName(fileL));
            if (File.Exists(destinationFilePath)) return destinationFilePath;
            try
            {
                destinationFilePath = Path.Combine(groupL, Path.GetFileName(fileL));
                if (move)
                {
                    File.Move(fileL, destinationFilePath);
                }
                else
                {
                    File.Copy(fileL, destinationFilePath);
                }
                return destinationFilePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error copying or moving file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        private bool createDir(string location)
        {
            if (!Directory.Exists(location))
            {
                try
                {
                    Directory.CreateDirectory(location);

                    Console.WriteLine("Folder created successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            return true;
        }
    }
}
