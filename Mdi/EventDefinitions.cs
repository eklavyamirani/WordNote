#define PLAY
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;
using System.Net;

namespace WordNote
{
    public partial class WordNote : Form
    {
        #region WordNote Event Definitions.
        /// <summary>
        /// On Resizing the form, the textbox's dimensions are updated accordingly
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Arguments</param>
        
        private void Resize_Area(object sender, EventArgs e)
        {
            //this.txtbox1.Size = this.Size;
            this.txtbox1.Size = new Size(this.Width, this.Height - 80);
        }


        /// <summary>
        /// Creating a new WordNote WinForm on Clicking new.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void New_Clicked(object sender, EventArgs e)
        {
            WordNote frmchild = new WordNote();
            frmchild.Show();
        }

        private void Open_Clicked(object sender, EventArgs e)
        {
            OpenFileDialog d1 = new OpenFileDialog();
            d1.Filter = "Text|*.txt";
            DialogResult Result = d1.ShowDialog();
            filename = d1.FileName;
            if (Result == DialogResult.OK)
            {
                System.IO.FileStream file1 = new System.IO.FileStream(filename, System.IO.FileMode.Open);
                StreamReader sr = new StreamReader(file1);
                string line = sr.ReadToEnd();
                UpdateText(line);
                sr.Close();
                file1.Close();
            }
            else
                return;

        }

        private void SaveAs_Clicked(object sender, EventArgs e)
        {
            MenuItem temp = (MenuItem)sender;
            if ((!File.Exists(filename)) || (temp.Text == "&Save As..."))
            {
                SaveFileDialog s1 = new SaveFileDialog();
                s1.Filter = "Text|*.txt";
                DialogResult Result = s1.ShowDialog();


                if (Result == DialogResult.OK)
                {
                    filename = s1.FileName;
                }
                else
                    return;
            }
            try
            {
                FileStream file1 = new FileStream(filename, FileMode.Create);
                StreamWriter sw = new StreamWriter(file1);
                String Line = this.txtbox1.Text;
                sw.WriteLine(Line);
                sw.Close();
                file1.Close();
                Is_Saved = true;
            }
            catch (UnauthorizedAccessException)
            {
                DialogResult result = MessageBox.Show("You are Unauthorised to Save the File here? (Are you sure you have Write access to the target location?)", "Problem While Saving the File", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    SaveAs_Clicked(sender, e);
                }
            }
            catch (Exception e2)
            { MessageBox.Show(e2.ToString()); }
        }

        private void Print_Clicked(object sender, EventArgs e)
        {

            PrintDialog PrintDlg = new PrintDialog();
            System.Drawing.Printing.PrintDocument Document = new System.Drawing.Printing.PrintDocument();
            PrintDlg.Document = Document;
            Document.PrintPage += new PrintPageEventHandler(Document_Printing);

            if ((PrintDlg.ShowDialog() == DialogResult.OK))
            {
                Document.Print();
            }
        }

        private void Document_Printing(object sender, PrintPageEventArgs p)
        {
            p.Graphics.DrawString(txtbox1.Text, new Font("Times New Roman", 14F), Brushes.Black, 0, 0);
        }

        private void PrintPreview_Clicked(object sender, EventArgs e)
        {
            PrintPreviewDialog PPdlg1 = new PrintPreviewDialog();
            PrintDocument Pdoc1 = new PrintDocument();
            Pdoc1.PrintPage += new PrintPageEventHandler(Document_Printing);
            PPdlg1.Document = Pdoc1;
            PPdlg1.ShowDialog();
        }

        private void Exit_Clicked(object sender, EventArgs e)
        {

            if (!Is_Saved)
            {
                DialogResult Result = MessageBox.Show("The Document has been modified since the last time it was saved... Are you sure you want to exit without saving?", "Docment Modified", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                    this.Close();
                else if (Result == DialogResult.No)
                    this.Show();
            }
            else
                this.Close();
        }
        /// <summary>
        /// txtbox1.TextChanged event:If the text in the text box is changed, or not saved, and to update the status bar contents on changing text.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnsaveUpdate(object sender, EventArgs e)
        {
            Is_Saved = false;
            if (this.txtbox1.Text.Length > 0)
                this.CountWords(this.txtbox1.Text);
            else
            {
                this.SentenceCount = 0;
                this.LineCount = this.WordCount = 1;
                
            }
        }

        private void Font_Clicked(object sender, EventArgs e)
        {
            FontDialog fdlg1 = new FontDialog();
            DialogResult result = fdlg1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtbox1.Font = fdlg1.Font;
            }
        }

        private void FontColor_Clicked(object sender, EventArgs e)
        {
            ColorDialog cdlg1 = new ColorDialog();
            if (cdlg1.ShowDialog() == DialogResult.OK)
            {
                txtbox1.ForeColor = cdlg1.Color;
            }
        }
        private void PerformAction(string ptext)
        {
            switch (ptext)
            {
                case "&Cut":
                    this.txtbox1.Cut();
                    break;
                case "&Copy":
                    this.txtbox1.Copy();
                    break;
                case "&Paste":
                    this.txtbox1.Paste();
                    break;
                case "Un&do":
                    this.txtbox1.Undo();
                    break;
                case "&Redo":
                    this.txtbox1.Redo();
                    break;
                case "&Select":
                    this.txtbox1.Select();
                    break;
                case "Select &All":
                    this.txtbox1.SelectAll();
#if PLAY
                    TranslateDialogBox SelectLanguage = new TranslateDialogBox();
                    
#endif
                    break;

            }
        }

        private void Edit_Clicked(object sender, EventArgs e)
        {
                        
            try
            {
                MenuItem ch = (MenuItem)sender;
                PerformAction(ch.Text);
                return;
            }
            catch { }
            try
            {
                ToolStripMenuItem ch = (ToolStripMenuItem)sender;
                PerformAction(ch.Text);
                return;
            }
            catch { }


        }

        private void Help_Clicked(object sender, EventArgs e)
        {
            string About = "WordNote™\nBeta Release\nMaybe Bugged\nChangeLog\n1.Get file from website\n2. Added Right click.\n3.Added Bing Translate\nMade By Eklavya Mirani";
            MessageBox.Show(About, "About WordNote™ ver.1.0.1.0", MessageBoxButtons.OK);
        }

        private void InvertColors_Clicked(object sender, EventArgs e)
        {
            if (this.txtbox1.BackColor == Color.White)
            {
                this.txtbox1.BackColor = Color.Black;
                this.txtbox1.ForeColor = Color.White;
            }
            else
            {
                this.txtbox1.BackColor = Color.White;
                this.txtbox1.ForeColor = Color.Black;
            }

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!Is_Saved)
            {
                DialogResult Result = MessageBox.Show("The Document has been modified since the last time it was saved... Are you sure you want to exit without saving?", "Docment Modified", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                    e.Cancel = false;
                else if (Result == DialogResult.No)
                    e.Cancel = true;

            }
            else
            {
                e.Cancel = false;
            }
        }

        public void OpenFromWebsiteClicked(object sender, EventArgs EA)
        {

            try
            {
                string Input=UserInput();
                WebRequest RequestPage = WebRequest.Create(Input);
                WebResponse RequestPageResponse = RequestPage.GetResponse();
                Stream ResponseStream = RequestPageResponse.GetResponseStream();
                StreamReader SR = new StreamReader(ResponseStream);
                String Content = HTMLTagsRemover.StripTagsRegexBody(SR.ReadToEnd());
                UpdateText(Content);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error opening requested webpage");
            }
        }

        public void TranslateClicked(object sender, EventArgs EA)
        {
            TranslateDialogBox SelectLanguage = new TranslateDialogBox();
            SelectLanguage.ShowDialog();
            if ((SelectLanguage.Result == DialogResult.OK))
            {
                TranslateText ob1 = new TranslateText();
                UpdateText(ob1.Translate(this.txtbox1.SelectedText, SelectLanguage.Language));
            }
        }

        
        
        #endregion
    }
}