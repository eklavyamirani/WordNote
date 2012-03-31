using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace WordNote
{
    class LanguageIds
    {

        private string LanguageCode;         
        private string LanguageName;
        public LanguageIds(string _LanguageCode, string _LanguageName)
        {
            this.LanguageCode = _LanguageCode;
            this.LanguageName = _LanguageName;
        }
        public string GetLanguageCode()
        {
            return this.LanguageCode;
        }
        public override string ToString()
        {
            return this.LanguageName;
        }
    };
    
    class TranslateDialogBox:Form
    {
        private ComboBox LanguageList;
        private List<string> LanguageIDs;
        private Label DialogLabel;
        private Button OKButton;
        new Button CancelButton;
        public DialogResult Result;
        
        private string language;
        public string Language
        {
            get
            {
                return language;
            }
            private set
            {
                language = value;
            }
        }
        

        public TranslateDialogBox()
        {
            InitialiseComponents();
        }

        private void InitialiseComponents()
        {
            this.DialogLabel = new Label();
            this.LanguageList = new ComboBox();
            this.LanguageIDs=new List<string>();
            this.OKButton = new Button();
            this.CancelButton = new Button();
            this.Result = DialogResult.Cancel;
            this.Language = string.Empty;
            
            //List of Languages available to Translate
            Object[] Languages = new Object[]{new LanguageIds("en","English"),new LanguageIds("es","Spanish"),new LanguageIds("hi","Hindi"),new LanguageIds("de","German"),new LanguageIds("fr","French")};
            
            //Form Properties
            this.Size=new System.Drawing.Size(280,220);


            //Dialog Label
            this.DialogLabel.Text = "Select Language to Translate text into";
            this.DialogLabel.Location = new System.Drawing.Point(20, 20);
            this.DialogLabel.Size = new System.Drawing.Size(200, 30);
            this.DialogLabel.Font = new System.Drawing.Font("Lucida Sans", 16f);

            //ListBox Properties
            
            this.LanguageList.DrawMode = DrawMode.Normal;
            this.LanguageList.IntegralHeight = true;            
            this.LanguageList.Location = new System.Drawing.Point(50, 80);
            
            this.LanguageList.Items.AddRange(Languages);
            this.LanguageList.Sorted = true;
            this.LanguageList.TabIndex = 0;
            this.LanguageList.SelectionStart = 0;
            
            //this.LanguageList.SelectedValueChanged+=new EventHandler(EnableOkay);
            //this.LanguageList.Show();

            ////OKButton Properties
            this.OKButton.Location = new System.Drawing.Point(10, 120);
            this.OKButton.Click+=new EventHandler(OKButtonClick);
            this.OKButton.Text = "OKAY";

            //CancelButton Properties
            this.CancelButton.Location = new System.Drawing.Point(140, 120);
            this.CancelButton.Click += new EventHandler(CancelButtonClicked);
            this.CancelButton.Text = "Cancel";

            this.Controls.AddRange(new Control[]{this.LanguageList,this.OKButton,this.CancelButton,this.DialogLabel});
            this.SuspendLayout();
        }

        protected void OKButtonClick(object sender, EventArgs EA)
        {
            this.Result = DialogResult.OK;
            ChangeLanguage();
            this.Close();
        }

        protected void CancelButtonClicked(object sender, EventArgs EA)
        {
            this.Result = DialogResult.Cancel;
            this.Language = string.Empty;
            this.Close();
        }
        protected void ChangeLanguage()
        {
            LanguageIds obj = (LanguageIds)this.LanguageList.SelectedItem;
            this.Language = obj.GetLanguageCode();
            
        }

        protected new DialogResult ShowDialog()
        {
            this.Show(this);
            return this.Result;
        }

        
        
    }
}
