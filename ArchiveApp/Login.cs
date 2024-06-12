using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArchiveApp.Data;
using ArchiveApp.Models;

namespace ArchiveApp
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.archiveLocation = "";
            this.Select();
        }

        private void loginPageLoad(object sender, EventArgs e)
        {
            setPlaceholderText();
        }

        private void setPlaceholderText()
        {
            nameField.Text = "Name";
            passwordField.Text = "Password";
            registerNameField.Text = "Name";
            registerPasswordField.Text = "Password";
            registerRepeatPassField.Text = "Repeat password";
            nameField.ForeColor = Color.LightGray;
            passwordField.ForeColor = Color.LightGray;
            registerNameField.ForeColor = Color.LightGray;
            registerPasswordField.ForeColor = Color.LightGray;
            registerRepeatPassField.ForeColor = Color.LightGray;

            loginErrorMessage.Text = "";
            registerErrorMessage.Text = "";
        }

        private void login_Click(object sender, EventArgs e)
        {
            string name = nameField.Text;
            string pass = passwordField.Text;

            List<string> input = new List<string>();
            input.Add(name);
            input.Add(pass);

            switch (input)
            {
                case List<string> list when list[0] is "Name":
                    Console.WriteLine("Name can't be empty or 'Name'");
                    loginErrorMessage.Text = "Name can't be empty or 'Name'";
                    loginErrorMessage.ForeColor = Color.Red;
                    return;
                case List<string> list when list[1] is "Password":
                    Console.WriteLine("Password can't be empty or 'Password'");
                    loginErrorMessage.Text = "Password can't be empty or 'Password'";
                    loginErrorMessage.ForeColor = Color.Red;
                    return;
                default:
                    Console.WriteLine("Input accepted");
                    break;
            }

            User foundUser;
            using (DatabaseContext context = new DatabaseContext())
            {
                User loginUser = new User(name, pass);

                foundUser = (from user in context.Users
                            where user.Name == name
                            select user).FirstOrDefault();

                if (loginUser != foundUser) 
                {
                    loginErrorMessage.Text = "Username or password is incorrect";
                    loginErrorMessage.ForeColor = Color.Red;
                    return;
                }
            }

            Console.WriteLine("Loged in");
            setPlaceholderText();

            Main main = new Main(1, foundUser);
            this.Hide();
            var result = main.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.Show();
            }
            else
            {
                this.Close();
            }
        }

        private void nameField_Enter(object sender, EventArgs e)
        {
            if(nameField.Text == "Name")
            {
                nameField.Text = "";
                nameField.ForeColor = Color.Black;
            }
        }

        private void nameField_Leave(object sender, EventArgs e)
        {
            if(nameField.Text == "")
            {
                nameField.Text = "Name";
                nameField.ForeColor = Color.LightGray;
            }
        }

        private void passwordField_Enter(object sender, EventArgs e)
        {
            if (passwordField.Text == "Password")
            {
                passwordField.Text = "";
                passwordField.ForeColor = Color.Black;
            }
        }

        private void passwordField_Leave(object sender, EventArgs e)
        {
            if (passwordField.Text == "")
            {
                passwordField.Text = "Password";
                passwordField.ForeColor = Color.LightGray;
            }
        }

        private void Page_Click(object sender, EventArgs e)
        {
            loginErrorMessage.Select();
        }

        private void register_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Registering");
            string name = registerNameField.Text;
            string pass = registerPasswordField.Text;
            string repeatPass = registerRepeatPassField.Text;

            List<string> input = new List<string>();
            input.Add(name);
            input.Add(pass);
            input.Add(repeatPass);
            input.Add(this.archiveLocation);
 
            switch (input)
            {
                case List<string> list when list[0] is "Name":
                    Console.WriteLine("Name can't be empty or 'Name'");
                    registerErrorMessage.Text = "Name can't be empty or 'Name'";
                    registerErrorMessage.ForeColor = Color.Red;
                    return;
                case List<string> list when list[1] is "Password":
                    Console.WriteLine("Password can't be empty or 'Password'");
                    registerErrorMessage.Text = "Password can't be empty or 'Password'";
                    registerErrorMessage.ForeColor = Color.Red;
                    return;
                case List<string> list when list[2] is "Repeat password":
                    Console.WriteLine("Repeat password can't be empty or 'Repeat password'");
                    registerErrorMessage.Text = "Repeat password can't be empty or 'Repeat password'";
                    registerErrorMessage.ForeColor = Color.Red;
                    return;
                case List<string> list when list[1] != list[2]:
                    Console.WriteLine("Passwords have to match");
                    registerErrorMessage.Text = "Passwords have to match";
                    registerErrorMessage.ForeColor = Color.Red;
                    return;
                case List<string> list when list[3] is "":
                    Console.WriteLine("Please select archive location");
                    registerErrorMessage.Text = "Please select archive location";
                    registerErrorMessage.ForeColor = Color.Red;
                    return;
                default:
                    Console.WriteLine("Input accepted");
                    break;
            }

            using (DatabaseContext context = new DatabaseContext())
            {
                User newUser = new User(name, pass, this.archiveLocation);

                var allUsers = from user in context.Users
                               select user;

                foreach (User u in allUsers)
                {
                    Console.WriteLine($"id= {u.Id}");
                    Console.WriteLine($"id= {u.Name}");
                    Console.WriteLine($"id= {u.Password}");
                    if (u.Name == newUser.Name) 
                    {
                        Console.WriteLine("Name Already exists");
                        registerErrorMessage.Text = "Name Already exists";
                        registerErrorMessage.ForeColor = Color.Red;
                        return;
                    }
                }

                context.Users.Add(newUser);
                context.SaveChanges();

                registerErrorMessage.Text = "User registered succesfully";
                registerErrorMessage.ForeColor = Color.Green;
            }
            Console.WriteLine("Added");
        }

        private void registerNameField_Enter(object sender, EventArgs e)
        {
            if (registerNameField.Text == "Name")
            {
                registerNameField.Text = "";
                registerNameField.ForeColor = Color.Black;
            }
        }

        private void registerNameField_Leave(object sender, EventArgs e)
        {
            if (registerNameField.Text == "")
            {
                registerNameField.Text = "Name";
                registerNameField.ForeColor = Color.LightGray;
            }
        }

        private void registerPasswordField_Enter(object sender, EventArgs e)
        {
            if (registerPasswordField.Text == "Password")
            {
                registerPasswordField.Text = "";
                registerPasswordField.ForeColor = Color.Black;
            }
        }

        private void registerPasswordField_Leave(object sender, EventArgs e)
        {
            if (registerPasswordField.Text == "")
            {
                registerPasswordField.Text = "Password";
                registerPasswordField.ForeColor = Color.LightGray;
            }
        }

        private void registerRepeatPassField_Enter(object sender, EventArgs e)
        {
            if (registerRepeatPassField.Text == "Repeat password")
            {
                registerRepeatPassField.Text = "";
                registerRepeatPassField.ForeColor = Color.Black;
            }
        }

        private void registerRepeatPassField_Leave(object sender, EventArgs e)
        {
            if (registerRepeatPassField.Text == "")
            {
                registerRepeatPassField.Text = "Repeat password";
                registerRepeatPassField.ForeColor = Color.LightGray;
            }
        }

        private void addFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            folderDialog.Description = "Select a folder";
            folderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            folderDialog.ShowNewFolderButton = true;

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                this.archiveLocation = folderDialog.SelectedPath;
                Console.WriteLine(this.archiveLocation);
            }
        }
    }
}
