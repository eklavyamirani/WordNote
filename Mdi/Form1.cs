//#define PLAY
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;


namespace WordNote
{
    static class myInfo
    {
        public const string AppID = "71E987FB135995E637A063F516342421381B6C39";
    }
    class abc
    {
        public const int AW_HIDE = 0X10000;
        public const int AW_ACTIVATE = 0X20000;
        const int AW_HOR_POSITIVE = 0X1;
        const int AW_HOR_NEGATIVE = 0X2;
        public const int AW_SLIDE = 0X40000;
        public const int AW_BLEND = 0X80000;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int AnimateWindow
        (IntPtr hwand, int dwTime, int dwFlags);
    }

    public partial class WordNote : Form
    {
        public WordNote()
        {
            InitializeComponent();
            InitializeMenu();            
        }



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
        }



        private string filename;
        private bool Is_Saved = true;
        private uint WordCount,LineCount,SentenceCount;
        
        #region Windows Forms Controls
        private System.Windows.Forms.MainMenu mainMenu1;
        private RichTextBox txtbox1;
        private StatusStrip WordNoteStatus;
        private ToolStripStatusLabel WordCountStatus;
        private ContextMenuStrip RightClickShortcut;
        private ToolStripMenuItem ContextMenuSelectAll;
        delegate void UpdateTextbox(string source);
        #endregion


        #region Windows Form code

        
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.mainMenu1 = new MainMenu();
            this.txtbox1=new RichTextBox();
            this.RightClickShortcut=new System.Windows.Forms.ContextMenuStrip();
            this.ContextMenuSelectAll = new ToolStripMenuItem("Select &All");
            
            
            //ContextMenu : RightClickShortcut
            this.RightClickShortcut.Items.Add(ContextMenuSelectAll);
            this.RightClickShortcut.Enabled = true;
            
            //ContextMenu Items
            //Select All
            this.ContextMenuSelectAll.Text = "Select &All";
            this.ContextMenuSelectAll.Click += new EventHandler(Edit_Clicked);
            //Toolstrip Items
            this.WordCountStatus = new ToolStripStatusLabel(WordCount.ToString() + " Words " + SentenceCount.ToString() + " Sentences " + LineCount.ToString() + " Lines ");
            this.WordNoteStatus = new StatusStrip();
            this.WordNoteStatus.Location = new Point(0, this.Bottom-30);
            this.WordCountStatus.Text = WordCount.ToString() + " Words " + SentenceCount.ToString() + " Sentences " + LineCount.ToString() + " Lines ";
            
            
            //Form Layout
            this.IsMdiContainer=true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Text = "WordNote Beta -Eklavya Mirani";
            this.Size=new Size(800,600);
            this.Menu = mainMenu1;
            
            this.txtbox1.BackColor = Color.White;
            this.txtbox1.ForeColor = Color.Black;
            this.txtbox1.Font = new Font("Lucida Sans", 16f);
            this.SizeChanged += new EventHandler(Resize_Area);
            if(System.IO.File.Exists(@"WordNote.ico"))
                this.Icon =new Icon(@"WordNote.ico");

            //StatusBar
            this.WordNoteStatus.Items.Add(this.WordCountStatus);                      
            
            //textBox Layout
            this.txtbox1.Location=new System.Drawing.Point(0,0);
            this.txtbox1.Size = new Size(this.Width, this.Height - 150);
            this.txtbox1.TextChanged += new EventHandler(UnsaveUpdate);
                             
            //Adding Controls
            this.Controls.Add(this.txtbox1);
            this.Controls.Add(this.WordNoteStatus); 
            this.ContextMenuStrip = this.RightClickShortcut;
            this.txtbox1.ContextMenuStrip = this.RightClickShortcut;


              
        }

        private void InitializeMenu()
        {
            MenuItem File = mainMenu1.MenuItems.Add("&File");
            File.MenuItems.Add(new MenuItem("&New", new EventHandler(this.New_Clicked), Shortcut.CtrlN));
            File.MenuItems.Add(new MenuItem("&Open", new EventHandler(this.Open_Clicked), Shortcut.CtrlO));
            File.MenuItems.Add("-");
            File.MenuItems.Add(new MenuItem("Open From &Website... (Html page only)", new EventHandler(this.OpenFromWebsiteClicked), Shortcut.CtrlW));
            File.MenuItems.Add(new MenuItem("&Save As...", new EventHandler(this.SaveAs_Clicked)));
            File.MenuItems.Add(new MenuItem("&Save", this.SaveAs_Clicked, Shortcut.CtrlS));
            File.MenuItems.Add("-");
            File.MenuItems.Add(new MenuItem("&Print Preview", new EventHandler(PrintPreview_Clicked), Shortcut.CtrlShiftP));
            File.MenuItems.Add(new MenuItem("&Print", new EventHandler(Print_Clicked), Shortcut.CtrlP));
            File.MenuItems.Add("-");
            File.MenuItems.Add(new MenuItem("&Exit", new EventHandler(Exit_Clicked), Shortcut.AltF4));

            MenuItem Edit = mainMenu1.MenuItems.Add("&Edit");
            Edit.MenuItems.Add(new MenuItem("&Select", new EventHandler(Edit_Clicked)));
            Edit.MenuItems.Add(new MenuItem("Select &All", new EventHandler(Edit_Clicked), Shortcut.CtrlA));
            Edit.MenuItems.Add("-");
            Edit.MenuItems.Add(new MenuItem("&Cut", new EventHandler(Edit_Clicked), Shortcut.CtrlX));
            Edit.MenuItems.Add(new MenuItem("&Copy", new EventHandler(Edit_Clicked), Shortcut.CtrlC));
            Edit.MenuItems.Add(new MenuItem("&Paste", new EventHandler(Edit_Clicked), Shortcut.CtrlV));
            Edit.MenuItems.Add("-");
            Edit.MenuItems.Add(new MenuItem("Un&do", new EventHandler(Edit_Clicked), Shortcut.CtrlZ));
            Edit.MenuItems.Add(new MenuItem("&Redo", new EventHandler(Edit_Clicked), Shortcut.CtrlShiftZ));


            MenuItem Format = mainMenu1.MenuItems.Add("Fo&rmat");
            Format.MenuItems.Add(new MenuItem("&Font", new EventHandler(Font_Clicked), Shortcut.CtrlH));
            Format.MenuItems.Add(new MenuItem("&Font Color", new EventHandler(FontColor_Clicked), Shortcut.CtrlShiftH));
            Format.MenuItems.Add(new MenuItem("&Invert Colors", new EventHandler(InvertColors_Clicked), Shortcut.CtrlShiftI));

            MenuItem Tools = mainMenu1.MenuItems.Add("&Tools");
            MenuItem Translate = new MenuItem("&Translate using Bing", new EventHandler(TranslateClicked), Shortcut.CtrlT);
            Tools.MenuItems.Add(Translate);

            MenuItem Help = mainMenu1.MenuItems.Add("&Help");
            Help.MenuItems.Add(new MenuItem("&About", new EventHandler(Help_Clicked), Shortcut.F1));
        }

        #endregion
        /// <summary>
        /// Counts the no. of Words, Lines and Sentences used by the text. Is called by the UnSaveUpdate method(txtbox1.TextChanged Event)
        /// </summary>
        /// <param name="CheckString">Passing the entire string to check.</param>
        private void CountWords(string CheckString)
        {
            this.LineCount = this.WordCount = 1;
            this.SentenceCount = 0;
            for(int i=0;i<CheckString.Length;i++)
            {
                switch (CheckString[i])
                {
                    case '\n':
                        this.LineCount++;
                        break;

                    case ' ':                        
                        this.WordCount++;
                        break;
                    case '.':
                        this.SentenceCount++;
                        this.WordCount++;
                        break;
                    
                }
            }
            this.WordCountStatus.Text=this.WordCount.ToString() + " Words " + this.SentenceCount.ToString() + " Sentences " + this.LineCount.ToString() + " Lines ";
        }

        public void UpdateText(string source)
        {

            if (this.txtbox1.InvokeRequired)
            {
                UpdateTextbox S1 = new UpdateTextbox(UpdateText);
                BeginInvoke(S1, new object[] { source });
            }
            else
            {
                this.txtbox1.Text += source;
            }
        }

        private string UserInput()
        {
            
            CustomDialogBox OpenFromWebsiteDialogBox = new CustomDialogBox();
            OpenFromWebsiteDialogBox.ShowDialog();
            if (OpenFromWebsiteDialogBox.Result == DialogResult.OK)
            {
                return OpenFromWebsiteDialogBox.SearchInput;
            }
            else
                return string.Empty;
        }

        protected override void  OnLoad(EventArgs e)

        {
            base.OnLoad(e);
#if PLAY
            abc.AnimateWindow(this.Handle, 500, abc.AW_ACTIVATE | abc.AW_BLEND);
            this.Refresh();
#endif
        }

        

            
            
        
    }
}
