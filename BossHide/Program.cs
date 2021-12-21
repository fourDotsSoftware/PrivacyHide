using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace PrivacyHide
{  
    static class Program
    {                
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
           // Program.SetupLocalized();
           // Program.SetupNSILang();
            
            //ContextMenuHelper.WriteTest();
           // Environment.Exit(0);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            frmLanguage.SetLanguages();
            frmLanguage.SetLanguage();

            ExceptionHandlersHelper.AddUnhandledExceptionHandlers();

            if (args.Length > 0 && args[0].StartsWith("/uninstall"))
            {
                Module.DeleteApplicationSettingsFile();

                System.Diagnostics.Process.Start("https://www.4dots-software.com/support/bugfeature.php?uninstall=true&app=" + System.Web.HttpUtility.UrlEncode(Module.ShortApplicationTitle));

                /*
                frmUninstallQuestionnaire fq = new frmUninstallQuestionnaire();
                fq.ShowDialog();*/
                return;
                Environment.Exit(0);
            }           

            ActivateIfApplicationIsRunning(Module.ApplicationName.Replace(" ", ""));

            if (frmMain.Instance == null)
            {
                Application.Run(new frmMain());
            }
            else
            {
                if (!frmMain.Instance.IsDisposed)
                {
                    Application.Run(frmMain.Instance);
                }
            }

            Environment.Exit(0);

        }

        private static void ActivateIfApplicationIsRunning(string applicationName)
        {
            try
            {
                Process[] procs = System.Diagnostics.Process.GetProcessesByName(applicationName);

                int ialive = -1;
                for (int k = 0; k <= procs.GetUpperBound(0); k++)
                {
                    if (System.Diagnostics.Process.GetCurrentProcess().Id != procs[k].Id)
                    {
                        if (!procs[k].HasExited)
                        {
                            ialive = k;
                            break;
                        }
                    }
                }

                if (ialive != -1)
                {
                    MessageHelper2 msg = new MessageHelper2();
                    int result = 0;
                    //First param can be null

                    IntPtr hWnd = procs[ialive].MainWindowHandle;

                    while (hWnd == IntPtr.Zero)
                    {
                        Application.DoEvents();

                        MessageHelper2.PostMessage((IntPtr)MessageHelper2.HWND_BROADCAST, (UInt32)MessageHelper2.WM_ACTIVATEAPP, IntPtr.Zero, IntPtr.Zero);

                        procs = System.Diagnostics.Process.GetProcessesByName(applicationName);

                        ialive = -1;
                        for (int k = 0; k <= procs.GetUpperBound(0); k++)
                        {
                            if (System.Diagnostics.Process.GetCurrentProcess().Id != procs[k].Id)
                            {
                                if (!procs[k].HasExited)
                                {
                                    ialive = k;
                                    break;
                                }
                            }
                        }

                        if (ialive != -1)
                        {
                            hWnd = procs[ialive].MainWindowHandle;
                        }
                    }

                    msg.bringAppToFront(hWnd);

                    result = msg.sendWindowsStringMessage(hWnd, IntPtr.Zero, "SHOW");

                    Environment.Exit(0);
                    return;
                }
            }
            catch { }
        }

        public static void SetLanguage()
        {
            RegistryKey key = Registry.CurrentUser;
            RegistryKey key2 = Registry.CurrentUser;
            string lang = "";

            try
            {
                key = key.OpenSubKey("Software\\4dots Software", true);

                if (key == null)
                {
                    key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\4dots Software");
                }

                key2 = key.OpenSubKey("PrivacyHide", true);

                if (key2 == null)
                {
                    key2 = key.CreateSubKey("PrivacyHide");
                }

                key = key2;

                bool setlang = false;

                if (key.GetValue("Language") == null)
                {
                    frmLanguage fl = new frmLanguage();
                    fl.ShowDialog();

                    key.SetValue("Language", frmLanguage.SelectedLanguageCode);
                    setlang = true;

                }

                if (key != null && key.GetValue("Language") != null)
                {
                    lang = key.GetValue("Language").ToString();
                    Module.SelectedLanguage = lang;
                    if (lang == "en-US")
                    {
                        System.Threading.Thread.CurrentThread.CurrentUICulture =
                            System.Globalization.CultureInfo.InvariantCulture;

                        Application.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

                    }
                    else
                    {

                        try
                        {
                            System.Threading.Thread.CurrentThread.CurrentUICulture = new
                            System.Globalization.CultureInfo(lang);

                            Application.CurrentCulture = new System.Globalization.CultureInfo(lang);
                        }
                        catch (Exception ex)
                        {
                            Module.ShowError(ex);
                        }
                    }
                }                

            }
            catch (Exception exr)
            {
                Module.ShowError(exr);
            }
            finally
            {
                key.Close();
                key2.Close();
            }
        }
    }
}