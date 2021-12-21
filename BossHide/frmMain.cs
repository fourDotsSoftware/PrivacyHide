using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;

namespace PrivacyHide
{
    public partial class frmMain : PrivacyHide.CustomForm
    {
        public static string[] args = null;
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.IntPtr vKey);
        UserActivityHook actHook;
        public ArrayList HiddenWindows = new ArrayList();
        public List<string> HiddenWindowsFilenames = new List<string>();

        public bool HidefrmMain = false;

        public bool AppsHidden = false;
        private bool OnLoad = false;

        private string HotKey = "";
        private bool HotKeyControl = false;
        private bool HotKeyAlt = false;
        private bool HotKeyShift = false;
        private bool HotKeyWin = false;

        public static frmMain Instance = null;

        public frmMain()
        {
            InitializeComponent();
            Instance = this;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtFilename.Text.Trim() == String.Empty)
            {
                Module.ShowMessage("Please specify the Application's Filename !");
                return;
            }

            string fn = System.IO.Path.GetFileName(txtFilename.Text);
            string ext = System.IO.Path.GetExtension(txtFilename.Text);

            if (ext == "")
            {
                fn += ".exe";
                ext = System.IO.Path.GetExtension(fn);
            }

            if (ext.ToLower() != ".exe")
            {
                Module.ShowMessage("Please specify a valid Executable Application as the Filename");
                return;
            }
            else
            {
                if (lstApps.Items.IndexOf(fn) < 0)
                {
                    lstApps.Items.Add(fn);
                }
            }

            SaveSettings();
        }

        private void btnBrowseAdd_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Executable Files (*.*)|*.exe";
            openFileDialog1.CheckFileExists = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fn = System.IO.Path.GetFileName(openFileDialog1.FileName);
                string ext = System.IO.Path.GetExtension(openFileDialog1.FileName);


                if (ext.ToLower() != ".exe")
                {
                    Module.ShowMessage("Please specify a valid Executable Application as the Filename");
                    return;
                }
                else
                {
                    if (lstApps.Items.IndexOf(fn) < 0)
                    {
                        lstApps.Items.Add(fn);
                    }
                }
            }

            SaveSettings();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            while (lstApps.SelectedItems.Count > 0)
            {
                lstApps.Items.Remove(lstApps.SelectedItems[0]);
            }

            SaveSettings();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            if (System.Threading.Thread.CurrentThread.CurrentUICulture ==
                            System.Globalization.CultureInfo.InvariantCulture)
            {
                System.Diagnostics.Process.Start(Application.StartupPath + "\\Privacy Hide - Users Manual.chm");
            }
            else
            {
                System.Diagnostics.Process.Start(Application.StartupPath + "\\"+
                    System.Threading.Thread.CurrentThread.CurrentUICulture.ToString()+                   
                    "\\Privacy Hide - Users Manual.chm");
            }
            
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            //FileStream fs = new FileStream(@"c:\a\1.txt", FileMode.Open,FileAccess.Read, FileShare.Read|FileShare.Write|FileShare.Delete);
            //fs.Unlock(0, fs.Length-1);
            //fs.Close();
                       
            
            frmAbout f = new frmAbout();
            f.ShowDialog();
        }

        private void SaveSettings()
        {
            if (OnLoad) return;

            RegistryKey key = Registry.CurrentUser;

            try
            {
                key = key.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

                if (key == null)
                {
                    Module.ShowMessage("Error. Could not Save if Application will start automatically with Windows");
                    return;
                }

                if (chkStartWithWindows.Checked)
                {
                    //1key.SetValue("PrivacyHide", "\"" + Application.StartupPath + "\\PrivacyHideLauncher.exe\" \"" + Application.StartupPath + "\\PrivacyHide.exe\"");

                    key.SetValue("PrivacyHide", "\"" + Application.StartupPath + "\\PrivacyHideLauncher.exe\"");

                    /*
                    if (key.GetValue("PrivacyHide") == null)
                    {
                        //3key.SetValue("PrivacyHide", "\""+Application.StartupPath + "\\PrivacyHide.exe\"");

                        key.SetValue("PrivacyHide", "\"" + Application.StartupPath + "\\PrivacyHideLauncher.exe\" \"" + Application.StartupPath + "\\PrivacyHide.exe\"");
                    }*/
                }
                else
                {
                    if (key.GetValue("PrivacyHide") != null)
                    {
                        key.DeleteValue("PrivacyHide");
                    }
                }

                key.Close();

                key = Registry.CurrentUser;

                key = Registry.CurrentUser.OpenSubKey("Software");

                RegistryKey key0 = key.OpenSubKey("4dots Software", true);

                if (key0 == null)
                {
                    key0 = key.CreateSubKey("4dots Software");
                }

                key = key0.OpenSubKey("PrivacyHide", true);

                if (key == null)
                {
                    key = key0.CreateSubKey("PrivacyHide");
                }

                //key = key.OpenSubKey(@"Software\4dots Software\PrivacyHide", true);

                key.SetValue("HideBossHide", chkHideTrayIcon.Checked);
                key.SetValue("AllAppsHidden", chkAll.Checked);
                key.SetValue("ConfiguredOnce", true.ToString());

                key.SetValue("HotKeyControl", chkCtrl.Checked.ToString());
                key.SetValue("HotKeyShift", chkShift.Checked.ToString());
                key.SetValue("HotKeyAlt", chkAlt.Checked.ToString());
                key.SetValue("HotKeyWin", chkWin.Checked.ToString());
                key.SetValue("HotKey", cmbHotKey.Text);

                key.SetValue("Admin", chkAdmin.Checked.ToString());

                key = key.OpenSubKey("Applications", true);

                string[] valn = key.GetValueNames();

                for (int k = 0; k < valn.Length; k++)
                {
                    key.DeleteValue(valn[k]);
                }

                for (int k = 0; k < lstApps.Items.Count; k++)
                {
                    key.SetValue("Application #" + k.ToString(), lstApps.Items[k].ToString());
                }

                key.Close();
            }
            catch (Exception ex)
            {
                Module.ShowError(ex);
            }
            finally
            {
                try { key.Close(); }
                catch { }
            }

            SetShortCutText();

        }

        private void FillHotKeyList()
        {
            char c = 'A';
            int ic=(int)c;

            for (int k = 1; k <= 26; k++)
            {
                int inew=ic+k-1;
                char cnew=(char)inew;

                cmbHotKey.Items.Add(cnew);
            }                      

            for (int k = 0; k <= 9; k++)
            {
                cmbHotKey.Items.Add(k.ToString());
            }

            for (int k = 1; k <= 12; k++)
            {
                cmbHotKey.Items.Add("F" + k.ToString());
            }

            cmbHotKey.SelectedIndex = cmbHotKey.Items.Count - 1;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //CursorManager.ClearCursor();
            
            this.Text = Module.ApplicationTitle;

            //Module.TargetCursor = new Cursor(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("BossHide.Resources.target_red_32.ico"));
            AddLanguageMenuItems();

            RegistryKey key = Registry.CurrentUser;
            RegistryKey key2 = Registry.CurrentUser;
            bool ConfiguredOnce = false;
            cmbHotKey.Items.Clear();

            FillHotKeyList();

            try
            {
                OnLoad = true;
                key = key.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

                if (key == null)
                {
                    Module.ShowMessage("Error. Could not specify if Application will start automatically with Windows");
                }

                if (key.GetValue("PrivacyHide") == null)
                {
                    chkStartWithWindows.Checked = false;
                }
                else
                {
                    chkStartWithWindows.Checked = true;
                }

                key = Registry.CurrentUser.OpenSubKey("Software");

                RegistryKey key0 = key.OpenSubKey("4dots Software", true);

                if (key0==null)
                {
                    key0 = key.CreateSubKey("4dots Software");
                }

                key = key0.OpenSubKey("PrivacyHide", true);

                if (key==null)
                {
                    key = key0.CreateSubKey("PrivacyHide");
                }

                //key = key.OpenSubKey(@"Software\4dots Software\PrivacyHide",true);
                /*
                if (key == null)
                {
                    key=Registry.CurrentUser.CreateSubKey(@"Software\4dots Software\PrivacyHide");
                }
                */

                chkHideTrayIcon.Checked = bool.Parse(key.GetValue("HideBossHide", false).ToString());

                chkAll.Checked = bool.Parse(key.GetValue("AllAppsHidden", false).ToString());

                ConfiguredOnce = bool.Parse(key.GetValue("ConfiguredOnce", false).ToString());

                chkCtrl.Checked = bool.Parse(key.GetValue("HotKeyControl",false).ToString());
                chkShift.Checked = bool.Parse(key.GetValue("HotKeyShift", false).ToString());
                chkAlt.Checked = bool.Parse(key.GetValue("HotKeyAlt", true).ToString());
                chkWin.Checked = bool.Parse(key.GetValue("HotKeyWin", false).ToString());

                chkAdmin.Checked = bool.Parse(key.GetValue("Admin", false).ToString());

                HotKey = key.GetValue("HotKey", "1").ToString();                              

                for (int o = 0; o < cmbHotKey.Items.Count; o++)
                {
                    if (cmbHotKey.Items[o].ToString() == HotKey)
                    {
                        cmbHotKey.SelectedIndex = o;
                        break;
                    }
                }

                HotKeyShift = chkShift.Checked;
                HotKeyAlt = chkAlt.Checked;
                HotKeyControl = chkCtrl.Checked;
                HotKeyWin = chkWin.Checked;

                lstApps.Items.Clear();

                key2= key.OpenSubKey("Applications");

                if (key2 == null)
                {
                    key.CreateSubKey("Applications");
                }

                key = key2;

                if (key != null)
                {
                    string[] valn = key.GetValueNames();
                    lstApps.Items.Clear();

                    for (int k = 0; k < valn.Length; k++)
                    {
                        if (valn[k].Trim() != String.Empty)
                        {
                            lstApps.Items.Add(key.GetValue(valn[k]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Module.ShowError(ex);
            }
            finally
            {
                OnLoad = false;
                //key.Close();

                if (key2 != null)
                {
                    key2.Close();
                }
            }
            //actHook = new UserActivityHook(); // crate an instance
            //actHook.OnMouseActivity+=new MouseEventHandler(actHook_OnMouseActivity);
            //actHook.Start();
            //actHook.KeyDown += new KeyEventHandler(actHook_KeyDown);

            if (!ConfiguredOnce || (args!=null && args.Length>0 && args[0]=="-ShowOptions"))
            {
                this.Show();
                this.ShowInTaskbar = true;
                this.BringToFront();
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.Hide();
                this.Visible = false;
                this.ShowInTaskbar = false;
            }

            notifyIcon1.Visible = !chkHideTrayIcon.Checked;
            SetShortCutText();

            ResizeControls();
        }               

        void actHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (CheckIsKeyPressed(Keys.A))
            {
                MessageBox.Show("pressed !");

            }
            bool acontrol = false;
            bool ashift = false;
            bool aalt = false;
            bool awin = false;

            if ((Control.ModifierKeys & Keys.Shift) != 0)
            {
                ashift = true;
            }

            if ((Control.ModifierKeys & Keys.Control) != 0)
            {
                acontrol = true;
            }

            if ((Control.ModifierKeys & Keys.Alt) != 0)
            {
                aalt = true;
            }

            if ((Control.ModifierKeys & Keys.LWin) != 0)
            {
                awin = true;
            }

            if (HotKeyShift != ashift) return;
            if (HotKeyAlt != aalt) return;
            if (HotKeyControl != acontrol) return;
            if (HotKeyWin != awin) return;

            bool pressed = false;

            switch (HotKey)
            {
                case "A":
                    if (CheckIsKeyPressed(Keys.A)) pressed = true;
                    break;
                case "B":
                    if (CheckIsKeyPressed(Keys.B)) pressed = true;
                    break;
                case "C":
                    if (CheckIsKeyPressed(Keys.C)) pressed = true;
                    break;
                case "D":
                    if (CheckIsKeyPressed(Keys.D)) pressed = true;
                    break;
                case "E":
                    if (CheckIsKeyPressed(Keys.E)) pressed = true;
                    break;
                case "F":
                    if (CheckIsKeyPressed(Keys.F)) pressed = true;
                    break;
                case "G":
                    if (CheckIsKeyPressed(Keys.G)) pressed = true;
                    break;
                case "H":
                    if (CheckIsKeyPressed(Keys.H)) pressed = true;
                    break;
                case "I":
                    if (CheckIsKeyPressed(Keys.I)) pressed = true;
                    break;
                case "J":
                    if (CheckIsKeyPressed(Keys.J)) pressed = true;
                    break;
                case "K":
                    if (CheckIsKeyPressed(Keys.K)) pressed = true;
                    break;
                case "L":
                    if (CheckIsKeyPressed(Keys.L)) pressed = true;
                    break;
                case "M":
                    if (CheckIsKeyPressed(Keys.M)) pressed = true;
                    break;
                case "N":
                    if (CheckIsKeyPressed(Keys.N)) pressed = true;
                    break;
                case "O":
                    if (CheckIsKeyPressed(Keys.O)) pressed = true;
                    break;
                case "P":
                    if (CheckIsKeyPressed(Keys.P)) pressed = true;
                    break;
                case "Q":
                    if (CheckIsKeyPressed(Keys.Q)) pressed = true;
                    break;
                case "R":
                    if (CheckIsKeyPressed(Keys.R)) pressed = true;
                    break;
                case "S":
                    if (CheckIsKeyPressed(Keys.S)) pressed = true;
                    break;
                case "T":
                    if (CheckIsKeyPressed(Keys.T)) pressed = true;
                    break;
                case "U":
                    if (CheckIsKeyPressed(Keys.U)) pressed = true;
                    break;
                case "V":
                    if (CheckIsKeyPressed(Keys.V)) pressed = true;
                    break;
                case "W":
                    if (CheckIsKeyPressed(Keys.W)) pressed = true;
                    break;
                case "X":
                    if (CheckIsKeyPressed(Keys.X)) pressed = true;
                    break;
                case "Y":
                    if (CheckIsKeyPressed(Keys.Y)) pressed = true;
                    break;
                case "Z":
                    if (CheckIsKeyPressed(Keys.Z)) pressed = true;
                    break;
                case "F1":
                    if (CheckIsKeyPressed(Keys.F1)) pressed = true;
                    break;
                case "F2":
                    if (CheckIsKeyPressed(Keys.F2)) pressed = true;
                    break;
                case "F3":
                    if (CheckIsKeyPressed(Keys.F3)) pressed = true;
                    break;
                case "F4":
                    if (CheckIsKeyPressed(Keys.F4)) pressed = true;
                    break;
                case "F5":
                    if (CheckIsKeyPressed(Keys.F5)) pressed = true;
                    break;
                case "F6":
                    if (CheckIsKeyPressed(Keys.F6)) pressed = true;
                    break;
                case "F7":
                    if (CheckIsKeyPressed(Keys.F7)) pressed = true;
                    break;
                case "F8":
                    if (CheckIsKeyPressed(Keys.F8)) pressed = true;
                    break;
                case "F9":
                    if (CheckIsKeyPressed(Keys.F9)) pressed = true;
                    break;
                case "F10":
                    if (CheckIsKeyPressed(Keys.F10)) pressed = true;
                    break;
                case "F11":
                    if (CheckIsKeyPressed(Keys.F11)) pressed = true;
                    break;
                case "F12":
                    if (CheckIsKeyPressed(Keys.F12)) pressed = true;
                    break;


                case "0":
                    if (CheckIsKeyPressed(Keys.D0)) pressed = true;
                    break;
                case "1":
                    if (CheckIsKeyPressed(Keys.D1)) pressed = true;
                    break;
                case "2":
                    if (CheckIsKeyPressed(Keys.D2)) pressed = true;
                    break;
                case "3":
                    if (CheckIsKeyPressed(Keys.D3)) pressed = true;
                    break;
                case "4":
                    if (CheckIsKeyPressed(Keys.D4)) pressed = true;
                    break;
                case "5":
                    if (CheckIsKeyPressed(Keys.D5)) pressed = true;
                    break;
                case "6":
                    if (CheckIsKeyPressed(Keys.D6)) pressed = true;
                    break;
                case "7":
                    if (CheckIsKeyPressed(Keys.D7)) pressed = true;
                    break;
                case "8":
                    if (CheckIsKeyPressed(Keys.D8)) pressed = true;
                    break;
                case "9":
                    if (CheckIsKeyPressed(Keys.D9)) pressed = true;
                    break;
                
                default:
                    break;

            }

            if (pressed)
            {
                MessageBox.Show("Pressed");

                if (!AppsHidden)
                {
                    HideApps();
                    AppsHidden = true;
                }
                else
                {
                    ShowApps();
                    AppsHidden = false;
                }
            }
        }

        

        public void HideApps()
        {
            lock (HiddenWindows)
            {
                try
                {
                    HiddenWindows.Clear();
                    HiddenWindowsFilenames.Clear();

                    ArrayList windows = WindowsEnumeratorHelper.GetOpenWindows();
                    ArrayList window_fp = new ArrayList();

                    WindowsManager wm = new WindowsManager();

                    for (int k = 0; k < windows.Count; k++)
                    {
                        try
                        {

                            OpenWindow op = (OpenWindow)windows[k];
                            string fp = System.IO.Path.GetFileName(wm.GetWindowFilePathFromHandle(op.Handle)).ToLower();

                            bool found = false;

                            for (int m = 0; m < lstApps.Items.Count; m++)
                            {
                                if (lstApps.Items[m].ToString().ToLower() == fp)
                                {
                                    found = true;
                                    break;
                                }
                            }

                            if (found || chkAll.Checked) // && fp.ToLower()!="explorer.exe"))
                            {
                                WindowsManager.HideWindow(op.Handle);
                                HiddenWindows.Add(op.Handle);
                                HiddenWindowsFilenames.Add(fp);
                            }
                        }
                        catch { }
                    }

                    HidefrmMain = this.Visible;
                    this.Visible = false;

                }
                catch { }
                
            }

            AppsHidden = true;
            SetShortCutText();
        }

        public void ShowApps()
        {
            lock (HiddenWindows)
            {
                lock (HiddenWindowsFilenames)
                {
                    for (int k = 0; k < HiddenWindows.Count; k++)
                    {
                        try
                        {
                            IntPtr window = (IntPtr)HiddenWindows[k];
                            WindowsManager.ShowWindow(window);
                        }
                        catch { }
                    }

                    HiddenWindows.Clear();
                    HiddenWindowsFilenames.Clear();
                }
            }

            if (HidefrmMain)
            {
                HidefrmMain = false;
                this.Visible = true;
            }

            AppsHidden = false;
            SetShortCutText();
        }

        private void frmMain_Activated(object sender, EventArgs e)
        {
            //this.Visible = false;
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.StartupPath + "\\readme.txt");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowApps();

            Application.Exit();
        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void addApplicationsToBeHiddenToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void hideShowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!AppsHidden)
            {
                HideApps();
                AppsHidden = true;
            }
            else
            {
                ShowApps();
                AppsHidden = false;
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            hideAllApplicationsToolStripMenuItem.Checked = chkAll.Checked;

            addApplicationsToBeHiddenToolStripMenuItem.DropDownItems.Clear();

            Process[] procs = System.Diagnostics.Process.GetProcesses();

            ArrayList al = new ArrayList();
            
            for (int k = 0; k < procs.Length; k++)
            {
                try
                {
                    if (procs[k].MainWindowHandle == IntPtr.Zero) continue;

                    //3string procname = System.IO.Path.GetFileNameWithoutExtension(procs[k].MainModule.FileName);

                    string procname = System.IO.Path.GetFileNameWithoutExtension(ProcessFilenameRetriever.GetExecutablePath(procs[k]));

                    if (al.IndexOf(procname) >= 0) continue;

                    al.Add(procname);

                    //3ToolStripMenuItem ti = new ToolStripMenuItem(System.IO.Path.GetFileNameWithoutExtension(procs[k].MainModule.FileName));

                    bool found = false;

                    for (int m = 0; m < lstApps.Items.Count; m++)
                    {
                        if (System.IO.Path.GetFileNameWithoutExtension(lstApps.Items[m].ToString()).ToLower() == procname.ToLower())
                        {
                            found = true;
                            break;
                        }
                    }

                    ToolStripMenuItem ti = new ToolStripMenuItem(procname);

                    //3ti.Tag = System.IO.Path.GetFileName(procs[k].MainModule.FileName);

                    ti.Tag = System.IO.Path.GetFileName(ProcessFilenameRetriever.GetExecutablePath(procs[k]));

                    ti.Click += new EventHandler(AddAppToHide_Click);

                    ti.Checked = found;

                    addApplicationsToBeHiddenToolStripMenuItem.DropDownItems.Add(ti);
                }
                catch { }
            }

        }

        void AddAppToHide_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ti = (ToolStripMenuItem)sender;

            if (ti.Checked)
            {
                lock (HiddenWindows)
                {
                    lock (HiddenWindowsFilenames)
                    {
                        int k = 0;

                        while (k<HiddenWindows.Count)
                        {
                            try
                            {
                                if (System.IO.Path.GetFileNameWithoutExtension(HiddenWindowsFilenames[k]).ToLower()
                                    == System.IO.Path.GetFileNameWithoutExtension(ti.Tag.ToString()).ToLower())
                                {
                                    IntPtr window = (IntPtr)HiddenWindows[k];
                                    WindowsManager.ShowWindow(window);

                                    HiddenWindows.RemoveAt(k);
                                    HiddenWindowsFilenames.RemoveAt(k);
                                }
                                else
                                {
                                    k++;
                                }
                            }
                            catch
                            {
                                k++;
                            }
                        }                        
                    }
                }

                ti.Checked = false;

                int ind = lstApps.Items.IndexOf(ti.Tag);

                if (ind>=0)
                {
                    lstApps.Items.RemoveAt(ind);
                }                
            }
            else
            {
                ti.Checked = true;

                if (lstApps.Items.IndexOf(ti.Tag) < 0)
                {
                    lstApps.Items.Add(ti.Tag);
                }
            }
                
            /*
            if (lstApps.Items.IndexOf(ti.Tag) < 0)
            {
                lstApps.Items.Add(ti.Tag);
            }
            */
            SaveSettings();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            ShowApps();
            CursorManager.DestroyCursor(CursorManager.TargetCursor);
            if (actHook != null)
            {
                actHook.Stop();
            }
           
        }

        private void chkStartWithWindows_CheckedChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void chkHideTrayIcon_CheckedChanged(object sender, EventArgs e)
        {
            SaveSettings();
            notifyIcon1.Visible = !chkHideTrayIcon.Checked;
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            SaveSettings();
            HotKey = cmbHotKey.Text;
            HotKeyShift = chkShift.Checked;
            HotKeyAlt = chkAlt.Checked;
            HotKeyControl = chkCtrl.Checked;
            HotKeyWin = chkWin.Checked;

            this.Hide();
            this.ShowInTaskbar = false;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                configureToolStripMenuItem_Click(null, null);
            }
        }

        private bool CheckIsKeyPressed(Keys k)
        {
            if (Module.Modex64)
            {
                return 0 != (GetAsyncKeyState(new IntPtr((Int64)(k))) & (Int64)0x8000);
            }
            else
            {
                return 0 != (GetAsyncKeyState(new IntPtr((Int32)(k))) & (Int32)0x8000);
            }
            /*
            int state = Convert.ToInt32(GetAsyncKeyState(k).ToString());
            switch (state)
            {
                case 0:
                    return false;
                    break;
                case 1:
                    //lbl.Text = "A is not currently pressed, but has been pressed since the last call.";
                    return false;
                    break;
                case -32767:
                    return true;
                    break;
                default:
                    return false;
                    break;
            }*/
        }

        private void timerHide_Tick(object sender, EventArgs e)
        {
            /*
            if (CheckIsKeyPressed(Keys.A))
            {
                MessageBox.Show("pressed !");

            }*/
            bool acontrol = false;
            bool ashift = false;
            bool aalt = false;
            bool awin = false;

            if ((Control.ModifierKeys & Keys.Shift) != 0)
            {
                ashift = true;
            }

            if ((Control.ModifierKeys & Keys.Control) != 0)
            {
                acontrol = true;
            }

            if ((Control.ModifierKeys & Keys.Alt) != 0)
            {
                aalt = true;
            }

            if ((Control.ModifierKeys & Keys.LWin) != 0)
            {
                awin = true;
            }

            if (HotKeyShift != ashift) return;
            if (HotKeyAlt != aalt) return;
            if (HotKeyControl != acontrol) return;
            if (HotKeyWin != awin) return;

            bool pressed = false;

            switch (HotKey)
            {
                case "A":
                    if (CheckIsKeyPressed(Keys.A)) pressed = true;
                    break;
                case "B":
                    if (CheckIsKeyPressed(Keys.B)) pressed = true;
                    break;
                case "C":
                    if (CheckIsKeyPressed(Keys.C)) pressed = true;
                    break;
                case "D":
                    if (CheckIsKeyPressed(Keys.D)) pressed = true;
                    break;
                case "E":
                    if (CheckIsKeyPressed(Keys.E)) pressed = true;
                    break;
                case "F":
                    if (CheckIsKeyPressed(Keys.F)) pressed = true;
                    break;
                case "G":
                    if (CheckIsKeyPressed(Keys.G)) pressed = true;
                    break;
                case "H":
                    if (CheckIsKeyPressed(Keys.H)) pressed = true;
                    break;
                case "I":
                    if (CheckIsKeyPressed(Keys.I)) pressed = true;
                    break;
                case "J":
                    if (CheckIsKeyPressed(Keys.J)) pressed = true;
                    break;
                case "K":
                    if (CheckIsKeyPressed(Keys.K)) pressed = true;
                    break;
                case "L":
                    if (CheckIsKeyPressed(Keys.L)) pressed = true;
                    break;
                case "M":
                    if (CheckIsKeyPressed(Keys.M)) pressed = true;
                    break;
                case "N":
                    if (CheckIsKeyPressed(Keys.N)) pressed = true;
                    break;
                case "O":
                    if (CheckIsKeyPressed(Keys.O)) pressed = true;
                    break;
                case "P":
                    if (CheckIsKeyPressed(Keys.P)) pressed = true;
                    break;
                case "Q":
                    if (CheckIsKeyPressed(Keys.Q)) pressed = true;
                    break;
                case "R":
                    if (CheckIsKeyPressed(Keys.R)) pressed = true;
                    break;
                case "S":
                    if (CheckIsKeyPressed(Keys.S)) pressed = true;
                    break;
                case "T":
                    if (CheckIsKeyPressed(Keys.T)) pressed = true;
                    break;
                case "U":
                    if (CheckIsKeyPressed(Keys.U)) pressed = true;
                    break;
                case "V":
                    if (CheckIsKeyPressed(Keys.V)) pressed = true;
                    break;
                case "W":
                    if (CheckIsKeyPressed(Keys.W)) pressed = true;
                    break;
                case "X":
                    if (CheckIsKeyPressed(Keys.X)) pressed = true;
                    break;
                case "Y":
                    if (CheckIsKeyPressed(Keys.Y)) pressed = true;
                    break;
                case "Z":
                    if (CheckIsKeyPressed(Keys.Z)) pressed = true;
                    break;
                case "F1":
                    if (CheckIsKeyPressed(Keys.F1)) pressed = true;
                    break;
                case "F2":
                    if (CheckIsKeyPressed(Keys.F2)) pressed = true;
                    break;
                case "F3":
                    if (CheckIsKeyPressed(Keys.F3)) pressed = true;
                    break;
                case "F4":
                    if (CheckIsKeyPressed(Keys.F4)) pressed = true;
                    break;
                case "F5":
                    if (CheckIsKeyPressed(Keys.F5)) pressed = true;
                    break;
                case "F6":
                    if (CheckIsKeyPressed(Keys.F6)) pressed = true;
                    break;
                case "F7":
                    if (CheckIsKeyPressed(Keys.F7)) pressed = true;
                    break;
                case "F8":
                    if (CheckIsKeyPressed(Keys.F8)) pressed = true;
                    break;
                case "F9":
                    if (CheckIsKeyPressed(Keys.F9)) pressed = true;
                    break;
                case "F10":
                    if (CheckIsKeyPressed(Keys.F10)) pressed = true;
                    break;
                case "F11":
                    if (CheckIsKeyPressed(Keys.F11)) pressed = true;
                    break;
                case "F12":
                    if (CheckIsKeyPressed(Keys.F12)) pressed = true;
                    break;

                case "0":
                    if (CheckIsKeyPressed(Keys.D0)) pressed = true;
                    break;
                case "1":
                    if (CheckIsKeyPressed(Keys.D1)) pressed = true;
                    break;
                case "2":
                    if (CheckIsKeyPressed(Keys.D2)) pressed = true;
                    break;
                case "3":
                    if (CheckIsKeyPressed(Keys.D3)) pressed = true;
                    break;
                case "4":
                    if (CheckIsKeyPressed(Keys.D4)) pressed = true;
                    break;
                case "5":
                    if (CheckIsKeyPressed(Keys.D5)) pressed = true;
                    break;
                case "6":
                    if (CheckIsKeyPressed(Keys.D6)) pressed = true;
                    break;
                case "7":
                    if (CheckIsKeyPressed(Keys.D7)) pressed = true;
                    break;
                case "8":
                    if (CheckIsKeyPressed(Keys.D8)) pressed = true;
                    break;
                case "9":
                    if (CheckIsKeyPressed(Keys.D9)) pressed = true;
                    break;
                

                default:
                    break;

            }

            if (pressed)
            {
                //MessageBox.Show("Pressed");

                if (!AppsHidden)
                {
                    HideApps();
                    AppsHidden = true;
                }
                else
                {
                    ShowApps();
                    AppsHidden = false;
                }
            }
        }

        private void SetShortCutText()
        {
            string shortcut="";
            if (HotKeyControl) shortcut+="Ctrl + ";
            if (HotKeyShift) shortcut+="Shift + ";
            if (HotKeyAlt) shortcut+="Alt + ";
            if (HotKeyWin) shortcut += "Win + ";
            shortcut+=HotKey;

            string strshow = TranslateHelper.Translate("Show");

            if (!AppsHidden)
            {
                strshow = TranslateHelper.Translate("Hide");
            }

            //hideShowToolStripMenuItem.Text=TranslateHelper.Translate("&Hide / Show")+"  "+shortcut;

            hideShowToolStripMenuItem.Text = strshow + "  - " + shortcut;
        }               
                               
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            //CursorManager.ChangeSystemCursor(@"C:\CodeGraphics\Cursors.Vista\Cursors\arrow_m.cur");
            CursorManager.ChangeSystemCursor(Properties.Resources.target_red_321);
            DoDragDrop(this, DragDropEffects.Link);
            
        }                

        private void frmMain_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {            
            if (e.Action == DragAction.Drop || e.Action == DragAction.Cancel)
            {
                //CursorManager.DestroyCursor(CursorManager.TargetCursor);
                CursorManager.ClearCursor();
            
                IntPtr hwnd = Win32Api.WindowFromPoint(Win32Api.GetCursorPosition());
                uint procid = 0;
                Win32Api.GetWindowThreadProcessId(hwnd, out procid);

                if (procid != 0)
                {
                    Process pr = System.Diagnostics.Process.GetProcessById((int)procid);
                    string fn = "";
                    try
                    {

                        //3fn = System.IO.Path.GetFileName(pr.MainModule.FileName).ToLower();

                        fn = System.IO.Path.GetFileName(ProcessFilenameRetriever.GetExecutablePath(pr));

                        //MessageBox.Show(fn);

                        bool found = false;

                        for (int d = 0; d < lstApps.Items.Count; d++)
                        {
                            if (fn == lstApps.Items[d].ToString().ToLower())
                            {
                                found = true;
                            }
                        }

                        if (!found)
                        {
                            lstApps.Items.Add(fn);
                        }
                    }
                    catch { }
                }

                
                

                

                //CursorManager.ChangeSystemCursor(@"C:\CodeGraphics\Cursors.Vista\Cursors\arrow_m.cur");
            }
            else if (e.Action == DragAction.Continue)
            {
                CursorManager.ChangeSystemCursor(Properties.Resources.target_red_321);

                IntPtr hwnd = Win32Api.WindowFromPoint(Win32Api.GetCursorPosition());
                RECT r;

                Win32Api.GetWindowRect(hwnd, out r);
                Rectangle re = r.ToRectangle();
                Win32Api.DrawReversibleFrame(re);
                
            }
        }                

        private void frmMain_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
        }        

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            frmMain_Load(null, null);
        }

        #region Localization

        private void AddLanguageMenuItems()
        {
            for (int k = 0; k < frmLanguage.LangCodes.Count; k++)
            {
                ToolStripMenuItem ti = new ToolStripMenuItem();
                ti.Text = frmLanguage.LangDesc[k];
                ti.Tag = frmLanguage.LangCodes[k];
                ti.Image = frmLanguage.LangImg[k];
                ti.Click += new EventHandler(tiLang_Click);
                languageToolStripMenuItem.DropDownItems.Add(ti);
            }
        }

        void tiLang_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ti = (ToolStripMenuItem)sender;
            string langcode = ti.Tag.ToString();
            ChangeLanguage(langcode);

            for (int k = 0; k < languageToolStripMenuItem.DropDownItems.Count; k++)
            {
                ToolStripMenuItem til = (ToolStripMenuItem)languageToolStripMenuItem.DropDownItems[k];
                if (til == ti)
                {
                    til.Checked = true;
                }
                else
                {
                    til.Checked = false;
                }
            }
        }

        private bool InChangeLanguage = false;

        private void ChangeLanguage(string language_code)
        {
            try
            {
                InChangeLanguage = true;

                Properties.Settings.Default.Language = language_code;
                frmLanguage.SetLanguage();

                Properties.Settings.Default.Save();
                Module.ShowMessage("Please restart the application !");
                Application.Exit();
                return;

                bool maximized = (this.WindowState == FormWindowState.Maximized);
                this.WindowState = FormWindowState.Normal;

                /*
                RegistryKey key = Registry.CurrentUser;
                RegistryKey key2 = Registry.CurrentUser;

                try
                {
                    key = key.OpenSubKey("Software\\4dots Software", true);

                    if (key == null)
                    {
                        key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\4dots Software");
                    }

                    key2 = key.OpenSubKey(frmLanguage.RegKeyName, true);

                    if (key2 == null)
                    {
                        key2 = key.CreateSubKey(frmLanguage.RegKeyName);
                    }

                    key = key2;

                    //key.SetValue("Language", language_code);
                    key.SetValue("Menu Item Caption", TranslateHelper.Translate("Change PDF Properties"));
                }
                catch (Exception ex)
                {
                    Module.ShowError(ex);
                    return;
                }
                finally
                {
                    key.Close();
                    key2.Close();
                }
                */
                //1SaveSizeLocation();

                //3SavePositionSize();

                this.Controls.Clear();

                InitializeComponent();

                //SetupOnLoad();

                if (maximized)
                {
                    this.WindowState = FormWindowState.Maximized;
                }

                this.ResumeLayout(true);
            }
            finally
            {
                InChangeLanguage = false;
            }
        }

        #endregion

        private void downloadMultipleSearchAndReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.multiplereplace.com");
        }

        private void downloadPDFMergeSplitToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.pdfdocmerge.com/pdfmergesplittool/");

        }

        private void downloadFreeFileUnlockerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.freefileunlocker.com");
        }

        private void downloadImgTransformerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.imgtransformer.com");
        }

        private void downloadFreeImagemapperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/imagemapper2/");
        }

        private void downloadCopyPathToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/copypathtoclipboard/");
        }

        private void downloadCopyTextContentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/copytextcontents/");
        }

        private void downloadOpenCommandPromptHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/open_command_prompt_here/");
        }

        private void downloadFreeColorwheelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/colorwheel/");
        }

        
        private void downloadPDFPasswordRemoverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.pdfdocmerge.com/pdf_password_remover/");
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com");
        }

        private void tiHelpFeedback_Click(object sender, EventArgs e)
        {            
        }

        private void pleaseDonateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/donate.php");
        }

        private void dotsSoftwarePRODUCTCATALOGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/downloads/4dots-Software-PRODUCT-CATALOG.pdf");
        }

        private void followUsOnTwitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.4dots-software.com/support/bugfeature.php?app=" + System.Web.HttpUtility.UrlEncode(Module.ShortApplicationTitle));
        }

        private void visit4dotsSoftwareWebpageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.4dots-software.com");
        }

        private void feedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.4dots-software.com/support/bugfeature.php?app=" + System.Web.HttpUtility.UrlEncode(Module.ShortApplicationTitle));
        }

        private void checkForNewVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            frmAbout f = new frmAbout();
            f.ShowDialog();
        }

        private void helpUsersManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (System.Threading.Thread.CurrentThread.CurrentUICulture ==
                            System.Globalization.CultureInfo.InvariantCulture)
            {
                System.Diagnostics.Process.Start("\"" + Application.StartupPath + "\\Privacy Hide - Users Manual.chm\"");
            }
            else
            {
                System.Diagnostics.Process.Start("\"" + Application.StartupPath + "\\" +
                    System.Threading.Thread.CurrentThread.CurrentUICulture.ToString() +
                    "\\Privacy Hide - Users Manual.chm\"");
            }
        }

        private void dotsSoftwareYoutubeChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/channel/UCovA-lld9Q79l08K-V1QEng");
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == MessageHelper.WM_COPYDATA)
            {
                MessageHelper2.COPYDATASTRUCT mystr = new MessageHelper2.COPYDATASTRUCT();
                Type mytype = mystr.GetType();
                mystr = (MessageHelper2.COPYDATASTRUCT)m.GetLParam(mytype);

                string arg = mystr.lpData;

                if (arg == "SHOW")
                {
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    this.Show();
                    this.BringToFront();
                }
            }
            else if (m.Msg == MessageHelper2.WM_ACTIVATEAPP)
            {
                this.Show();

                this.WindowState = FormWindowState.Normal;
                this.Show();
                this.BringToFront();
            }
            else
            {
                base.WndProc(ref m);
            }

        }

        private void hideAllApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideAllApplicationsToolStripMenuItem.Checked= !hideAllApplicationsToolStripMenuItem.Checked;
            chkAll.Checked = hideAllApplicationsToolStripMenuItem.Checked;
            SaveSettings();
        }
    }
}

