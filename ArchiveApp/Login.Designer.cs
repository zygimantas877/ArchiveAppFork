using System;

namespace ArchiveApp
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
            Console.WriteLine("Exit");
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.nameField = new System.Windows.Forms.TextBox();
            this.passwordField = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.loginErrorMessage = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.registerNameField = new System.Windows.Forms.TextBox();
            this.registerPasswordField = new System.Windows.Forms.TextBox();
            this.registerRepeatPassField = new System.Windows.Forms.TextBox();
            this.registerErrorMessage = new System.Windows.Forms.Label();
            this.archiveLocationButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // nameField
            // 
            this.nameField.Location = new System.Drawing.Point(139, 91);
            this.nameField.Name = "nameField";
            this.nameField.Size = new System.Drawing.Size(100, 20);
            this.nameField.TabIndex = 0;
            this.nameField.Enter += new System.EventHandler(this.nameField_Enter);
            this.nameField.Leave += new System.EventHandler(this.nameField_Leave);
            // 
            // passwordField
            // 
            this.passwordField.Location = new System.Drawing.Point(139, 128);
            this.passwordField.Name = "passwordField";
            this.passwordField.Size = new System.Drawing.Size(100, 20);
            this.passwordField.TabIndex = 1;
            this.passwordField.Enter += new System.EventHandler(this.passwordField_Enter);
            this.passwordField.Leave += new System.EventHandler(this.passwordField_Leave);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(151, 163);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Login";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.login_Click);
            // 
            // loginErrorMessage
            // 
            this.loginErrorMessage.AutoSize = true;
            this.loginErrorMessage.Location = new System.Drawing.Point(113, 203);
            this.loginErrorMessage.Name = "loginErrorMessage";
            this.loginErrorMessage.Size = new System.Drawing.Size(36, 13);
            this.loginErrorMessage.TabIndex = 3;
            this.loginErrorMessage.Text = "temp1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(555, 228);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Register";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.register_Click);
            // 
            // registerNameField
            // 
            this.registerNameField.Location = new System.Drawing.Point(541, 91);
            this.registerNameField.Name = "registerNameField";
            this.registerNameField.Size = new System.Drawing.Size(100, 20);
            this.registerNameField.TabIndex = 5;
            this.registerNameField.Enter += new System.EventHandler(this.registerNameField_Enter);
            this.registerNameField.Leave += new System.EventHandler(this.registerNameField_Leave);
            // 
            // registerPasswordField
            // 
            this.registerPasswordField.Location = new System.Drawing.Point(541, 128);
            this.registerPasswordField.Name = "registerPasswordField";
            this.registerPasswordField.Size = new System.Drawing.Size(100, 20);
            this.registerPasswordField.TabIndex = 6;
            this.registerPasswordField.Enter += new System.EventHandler(this.registerPasswordField_Enter);
            this.registerPasswordField.Leave += new System.EventHandler(this.registerPasswordField_Leave);
            // 
            // registerRepeatPassField
            // 
            this.registerRepeatPassField.Location = new System.Drawing.Point(541, 163);
            this.registerRepeatPassField.Name = "registerRepeatPassField";
            this.registerRepeatPassField.Size = new System.Drawing.Size(100, 20);
            this.registerRepeatPassField.TabIndex = 7;
            this.registerRepeatPassField.Enter += new System.EventHandler(this.registerRepeatPassField_Enter);
            this.registerRepeatPassField.Leave += new System.EventHandler(this.registerRepeatPassField_Leave);
            // 
            // registerErrorMessage
            // 
            this.registerErrorMessage.AutoSize = true;
            this.registerErrorMessage.Location = new System.Drawing.Point(499, 263);
            this.registerErrorMessage.Name = "registerErrorMessage";
            this.registerErrorMessage.Size = new System.Drawing.Size(36, 13);
            this.registerErrorMessage.TabIndex = 8;
            this.registerErrorMessage.Text = "temp2";
            // 
            // archiveLocationButton
            // 
            this.archiveLocationButton.Location = new System.Drawing.Point(593, 193);
            this.archiveLocationButton.Name = "archiveLocationButton";
            this.archiveLocationButton.Size = new System.Drawing.Size(86, 23);
            this.archiveLocationButton.TabIndex = 9;
            this.archiveLocationButton.Text = "Select Folder";
            this.archiveLocationButton.UseVisualStyleBackColor = true;
            this.archiveLocationButton.Click += new System.EventHandler(this.addFolder_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(501, 198);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Archive Location:";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.archiveLocationButton);
            this.Controls.Add(this.registerErrorMessage);
            this.Controls.Add(this.registerRepeatPassField);
            this.Controls.Add(this.registerPasswordField);
            this.Controls.Add(this.registerNameField);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.loginErrorMessage);
            this.Controls.Add(this.nameField);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.passwordField);
            this.Name = "Login";
            this.Text = "Login Page";
            this.Load += new System.EventHandler(this.loginPageLoad);
            this.Click += new System.EventHandler(this.Page_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private string archiveLocation;
        private System.Windows.Forms.TextBox nameField;
        private System.Windows.Forms.TextBox passwordField;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label loginErrorMessage;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox registerNameField;
        private System.Windows.Forms.TextBox registerPasswordField;
        private System.Windows.Forms.TextBox registerRepeatPassField;
        private System.Windows.Forms.Label registerErrorMessage;
        private System.Windows.Forms.Button archiveLocationButton;
        private System.Windows.Forms.Label label1;
    }
}

