using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace WordNote
{
    public class CustomDialogBox:Form
    {
        public DialogResult Result;
        protected Button OKButton;
        new protected Button CancelButton;
        private TextBox UserInput;
        public String SearchInput;

        public CustomDialogBox()
        {
            InitialiseComponent();
            
        }

        
        private void InitialiseComponent()
        {
            this.OKButton = new Button();
            this.CancelButton = new Button();
            this.UserInput = new TextBox();

            //Form Properties
            this.Size = new System.Drawing.Size(280, 160);
            

            //OKButton Properties
            this.OKButton.Location = new System.Drawing.Point(10, 60);
            this.OKButton.Size = new System.Drawing.Size(120, 20);
            this.OKButton.Click += new EventHandler(OKButtonClicked);
            this.OKButton.Enabled = false; //unclickable until user inputs string in the search field
            this.OKButton.Text = "Retrieve WebPage";

            //CancelButton Properties
            this.CancelButton.Location = new System.Drawing.Point(140, 60);
            this.CancelButton.Size = new System.Drawing.Size(120,20);
            this.CancelButton.Click+= new EventHandler(CancelButtonClicked);
            this.CancelButton.Text = "Cancel";

            //UserInput Properties
            this.UserInput.Location = new System.Drawing.Point(0,0);//10,25
            this.UserInput.TextChanged+=new EventHandler(UserInputTextChanged);
            this.UserInput.Size = new System.Drawing.Size(230, 10);

            //Controls
            this.Controls.AddRange(new Control[] { this.OKButton, this.CancelButton, this.UserInput });
        }

        protected void OKButtonClicked(object sender, EventArgs EA)
        {
            this.SearchInput = this.UserInput.Text;
            this.Result = DialogResult.OK;
            this.Close();
        }

        protected void CancelButtonClicked(object sender, EventArgs EA)
        {
            this.SearchInput = null;
            this.Result = CancelButton.DialogResult;
            this.Close();
        }
        protected void UserInputTextChanged(object sender, EventArgs EA)
        {
            if (this.UserInput.Text.Length > 0)
                this.OKButton.Enabled = true;
            else
                this.OKButton.Enabled = false;
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if (this.Result == DialogResult.OK || this.Result == DialogResult.Cancel)
            {
                e.Cancel=false;
            }
            
        }
        
    }
}
