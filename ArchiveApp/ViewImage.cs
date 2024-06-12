using ArchiveApp.Collections;
using ArchiveApp.Data;
using ArchiveApp.Models;
using Microsoft.EntityFrameworkCore;
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
    public partial class ViewImage : Form
    {
        public ViewImage(Models.Image image)
        {
            InitializeComponent();
            this.image = image;
            loadImage(image);
            loadInfo(image);
        }

        private void loadImage(Models.Image image)
        {
            Image originalImage = Image.FromFile(image.Location);
            Image clonedImage = (Image)originalImage.Clone();
            pictureBox.Image = clonedImage;
            originalImage.Dispose();
        }

        private void loadInfo(Models.Image image)
        {
            panel1.BringToFront();
            nameLabel.Text = image.Name;
            groupLabel.Text = image.Group.Name;

            string tags = "";
            foreach (Tag tag in image.Tags)
            {
                tags += tag.Name + "\n";
            }
            tagsLabel.Text = tags;
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            panel2.BringToFront();
            nameTextBox.Text = this.image.Name;
            loadComboBox();
            groupComboBox.Text = this.image.Group.Name;

            string tags = "";
            foreach (Tag tag in this.image.Tags)
            {
                tags += tag.Name + " ";
            }
            tagsTextBox.Text = tags;
        }

        private void loadComboBox()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                List<Models.Image> allImages = (from img in context.Images
                                                where img.UserId == image.UserId
                                                join gr in context.Groups
                                                on img.Id equals gr.ImageId into imageGroup
                                                from g in imageGroup.DefaultIfEmpty() // Perform left join
                                                select new Models.Image
                                                {
                                                    Id = img.Id,
                                                    Name = img.Name,
                                                    Location = img.Location,
                                                    UserId = img.UserId,
                                                    User = img.User,
                                                    Group = g, // Assign the Group
                                                    Tags = img.Tags
                                                }).ToList();

                Images images = new Images(allImages);
                var groupHash = new HashSet<string>();

                foreach (Models.Image image in images)
                {
                    groupHash.Add(image.Group.Name);
                }
                groupComboBox.DataSource = groupHash.ToList();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            panel1.BringToFront();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string name = nameTextBox.Text;
            string groupName = groupComboBox.Text;
            string tagString = tagsTextBox.Text;

            if (name == "" || !(name.Contains(".png") || name.Contains(".jpg")) || name.Length <= 4)
            {
                MessageBox.Show("name has to have .png or .jpg", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (groupName == "")
            {
                MessageBox.Show("Please add a group", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (tagString == "")
            {
                MessageBox.Show("Please add at least one tag", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (this.image.Group.Name != groupName)
            {
                var info = new DirectoryInfo(this.image.Location);
                Console.WriteLine(info.Parent.Parent.FullName);
                string groupLocation = info.Parent.Parent.FullName + "\\" + groupName;
                Group group = new Group(groupName, groupLocation);

                if (!createDir(groupLocation)) return;

                string fileLoacation = moveFile(groupLocation, this.image.Location, name);
                if (fileLoacation == "") return;
                if (fileLoacation == "exists")
                {
                    MessageBox.Show("This image is already added", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!Directory.EnumerateFileSystemEntries(info.Parent.FullName).Any())
                {
                    Directory.Delete(info.Parent.FullName);
                }

                this.image.Name = name;
                this.image.Location = fileLoacation;
                this.image.Group = group;
            }
            else if (this.image.Name != name)
            {
                var info = new DirectoryInfo(this.image.Location);
                string fileLoacation = moveFile(info.Parent.FullName, this.image.Location, name);
                if (fileLoacation == "") return;
                if (fileLoacation == "exists")
                {
                    MessageBox.Show("This image is already added", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.image.Name = name;
                this.image.Location = fileLoacation;
            }

            Tag[] tags;
            if (tagString.Contains(" ")) tags = splitTags(tagString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            else tags = splitTags(tagString);
            this.image.Tags = tags;

            using (DatabaseContext context = new DatabaseContext())
            {
                var foundImage = context.Images.Include(img => img.Group).FirstOrDefault(x => x.Id == this.image.Id);
                if (foundImage != null)
                {
                    context.Entry(foundImage).Collection(image => image.Tags).Load();

                    foundImage.Name = this.image.Name;
                    foundImage.Location = this.image.Location;
                    foundImage.Group = this.image.Group;

                    if (foundImage.Tags == null)
                    {
                        foundImage.Tags = new List<Tag>();
                    }
                    else
                    {
                        // Clear existing tags
                        foundImage.Tags.Clear();
                    }

                    foreach (var tag in this.image.Tags)
                    {
                        foundImage.Tags.Add(tag);
                    }
                    context.SaveChanges();
                }
            }

            loadImage(this.image);
            loadInfo(this.image);
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

        private string moveFile(string groupL, string fileL, string fileName)
        {
            string destinationFilePath = Path.Combine(groupL, fileName);
            Console.WriteLine(destinationFilePath);
            Console.WriteLine(fileL);
            if (File.Exists(destinationFilePath)) return "exists";
            try
            {
                pictureBox.Image?.Dispose();
                File.Move(fileL, destinationFilePath);
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

        private bool removeFile(string fileL)
        {
            try
            {
                pictureBox.Image?.Dispose();
                File.Delete(fileL);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Deleting file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!removeFile(this.image.Location)) return;

                var info = new DirectoryInfo(this.image.Location);
                if (!Directory.EnumerateFileSystemEntries(info.Parent.FullName).Any())
                {
                    Directory.Delete(info.Parent.FullName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            using (DatabaseContext context = new DatabaseContext())
            {
                var foundImage = context.Images.Include(img => img.Group).FirstOrDefault(x => x.Id == this.image.Id);
                if (foundImage != null)
                {
                    context.Entry(foundImage).Collection(image => image.Tags).Load();

                    context.Images.Remove(foundImage);
                    context.SaveChanges();
                }
            }

            MessageBox.Show("File deleted succesfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private void ViewImage_FormClosing(object sender, FormClosingEventArgs e)
        {
            pictureBox?.Image.Dispose();
        }
    }
}
