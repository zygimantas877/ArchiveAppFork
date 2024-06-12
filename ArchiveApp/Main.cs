using ArchiveApp.Collections;
using ArchiveApp.Data;
using ArchiveApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace ArchiveApp
{
    public partial class Main : Form
    {
        Func<int, int> start = x => (x - 1) * 25;
        Func<int, int> end = x => x * 25;

        Images currentImages;

        public Main(int pageNr, User user)
        {
            InitializeComponent();
            this.pageNr = pageNr;
            this.label5.Text = pageNr.ToString();
            this.User = user;
            this.userId = user.Id;
            this.UserIdLabel.Text = this.userId.ToString();
            this.startInd = 0;
            this.endInd = 25;
            Console.WriteLine("User = " + this.userId);
            Console.WriteLine(((pageNr-1)*25+1) + " - " + (pageNr*25+1));

            loadComboBox();
            Images images;
            foundImages(out images, new List<string>(){ "" }, "Group - All");
            currentImages = images;
            loadImages(currentImages);
        }

        private void logout_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void nextPage(object sender, EventArgs e)
        {
            if (pageNr >= (int)Math.Ceiling(currentImages.Count() * 1.0 / 25)) return;
            this.pageNr = pageNr+1;
            updatePage(pageNr);
        }

        private void previousPage(object sender, EventArgs e)
        {
            if (this.pageNr <= 1) return;
            this.pageNr = pageNr-1;
            updatePage(pageNr);
        }

        private void updatePage(int page)
        {
            this.startInd = start(page);
            this.endInd = end(page);
            this.label5.Text = page.ToString();
            Console.WriteLine(this.startInd + " - " + this.endInd);

            loadImages(currentImages);
        }

        private void loadImages(Images images)
        {
            Func<int, int, string> ind = (x, i) => ((i + 1) - (x - 1) * 25).ToString();

            for (int i = this.startInd; i < this.endInd; i++)
            {
                //if (allImages.Count() <= i) return;

                string pixtureBoxName = "pictureBox" + ind(pageNr, i);
                Console.WriteLine(pixtureBoxName);

                PictureBox pictureBox = Controls.Find(pixtureBoxName, true).FirstOrDefault() as PictureBox;

                try
                {
                    if (images.Count() <= i)
                    {
                        pictureBox?.Image?.Dispose();
                        pictureBox.Image = null;
                    }
                    Image originalImage = Image.FromFile(images[i].Location);
                    Image clonedImage = (Image)originalImage.Clone();
                    pictureBox?.Image?.Dispose();
                    pictureBox.Image = clonedImage;
                    originalImage.Dispose();
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine("pictureBox or pixture not found");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An unexpected error occurred: " + ex.Message);
                }
            }
        }

        private void loadComboBox()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                List<Models.Image> allImages = (from img in context.Images
                                               where img.UserId == this.userId
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
                groupHash.Add("Group - All");

                foreach(Models.Image image in images)
                {
                    groupHash.Add(image.Group.Name);
                }
                groupComboBox.DataSource = groupHash.ToList();
            }
        }

        private void addNew_Click(object sender, EventArgs e)
        {
            AddImage addImage = new AddImage(this.User);
            
            var result = addImage.ShowDialog();
            loadComboBox();
            Images images;
            foundImages(out images, new List<string>() { "" }, "Group - All");
            currentImages = images;
            loadImages(currentImages);
        }
        private void accountButton_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= 25; i++)
            {
                string pixtureBoxName = "pictureBox" + i;
                PictureBox pictureBox = Controls.Find(pixtureBoxName, true).FirstOrDefault() as PictureBox;

                if (pictureBox.Image != null) pictureBox.Image.Dispose();
            }

            Account account = new Account(this.User);

            var result = account.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.User = account.returnUser;
                this.userId = this.User.Id;
            }

            loadImages(currentImages);
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            string search = searchTextBox.Text;
            List<string> tagList = search.Split(' ').ToList();
            string groupName = groupComboBox.Text;

            Images images;
            foundImages(out images, tagList, groupName);
            currentImages = images;
            this.pageNr = 1;
            updatePage(pageNr);
        }

        private void foundImages(out Images images, List<string> tagList, string groupName)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                //LINQ
                var userImages = from image in context.Images
                                 where image.UserId == this.userId
                                 select image;

                var imagesWithGroups = from img in userImages
                                       join gr in context.Groups
                                       on img.Id equals gr.ImageId into imageGroup
                                       from g in imageGroup.DefaultIfEmpty() // Perform left join with Groups
                                       select new { Image = img, Group = g };

                var imagesWithTags = imagesWithGroups.ToList().Select(result =>
                {
                    var image = result.Image;

                    var imageTags = (from tag in context.Tags
                                     where tag.ImageId == image.Id
                                     select tag).ToList();

                    image.Group = result.Group;
                    image.Tags = imageTags;

                    return image;
                });

                Images allImages = new Images(imagesWithTags.ToList());
                images = new Images();
                foreach (Models.Image image in allImages)
                {
                    if (image.Group.Name == groupName || groupName == "Group - All")
                    {
                        if (tagList.Count() == 1 && tagList[0] == "")
                        {
                            images.Add(image);
                            continue;
                        }
                        foreach (Tag tag in image.Tags)
                        {
                            if (tagList.Contains(tag.Name))
                            {
                                images.Add(image);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void goToFirstPage(object sender, EventArgs e)
        {
            this.pageNr = 1;
            updatePage(pageNr);
        }

        private void goToLastPage(object sender, EventArgs e)
        {
            this.pageNr = (int)Math.Ceiling(currentImages.Count() * 1.0 / 25);
            updatePage(pageNr);
        }

        private void openImage(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            int boxNr;

            Match match = Regex.Match(pictureBox.Name, @"\d+");
            if (match.Success)
            {
                boxNr = int.Parse(match.Value);
            }
            else
            {
                Console.WriteLine("No number found in the string.");
                return;
            }

            try
            {
                Models.Image image = currentImages[this.startInd + boxNr - 1];
                pictureBox.Image.Dispose();

                ViewImage viewImage = new ViewImage(image);
                var result = viewImage.ShowDialog();

                loadComboBox();
                Images images;
                foundImages(out images, new List<string>() { "" }, "Group - All");
                currentImages = images;
                loadImages(currentImages);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
