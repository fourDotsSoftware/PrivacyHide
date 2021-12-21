using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Win32;

namespace PrivacyHideLauncher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args == null || args.Length == 0)
            {
                string fp = System.IO.Path.Combine(Application.StartupPath, "PrivacyHide.exe");

                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\4dots Software\PrivacyHide", false);

                if (key != null)
                {
                    string admin = key.GetValue("Admin", bool.FalseString).ToString();

                    if (admin == bool.TrueString)
                    {
                        fp = System.IO.Path.Combine(Application.StartupPath, "PrivacyHideAdmin.exe");
                    }
                }

                try
                {
                    System.Diagnostics.Process.Start(fp);
                }
                catch
                {

                }
            }
            else
            {
                try
                {
                    System.Diagnostics.Process.Start(args[0]);
                }
                catch
                {

                }
            }

            return;
        }
    }
}
