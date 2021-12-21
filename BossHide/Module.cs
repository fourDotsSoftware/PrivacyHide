using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace PrivacyHide
{
    class Module
    {
        public static string ApplicationName = "Privacy Hide";
        public static string Version = "3.0";

        public static string ShortApplicationTitle = ApplicationName + " V" + Version;
        public static string ApplicationTitle = ShortApplicationTitle + " - 4dots Software";                
                
        public static string StoreUrl = "https://www.4dots-software.com/privacyhide/";

        private static int _ModeX64 = -1;

        public static string SelectedLanguage = "";

        public static Cursor TargetCursor = null;

        public static void ShowMessage(string msg)
        {
            MessageBox.Show(TranslateHelper.Translate(msg),ApplicationTitle,MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        public static void ShowError(Exception ex)
        {
            ShowError(TranslateHelper.Translate("Error"), ex);
        }

        public static void ShowError(string msg)
        {
            MessageBox.Show(TranslateHelper.Translate(msg), TranslateHelper.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);

        }     

        public static void ShowError(string msg, Exception ex)
        {
            ShowError(msg + "\n\n" + ex.Message);
        }

        public static int _Modex64 = -1;

        public static bool Modex64
        {
            get
            {
                if (_Modex64 == -1)
                {
                    if (Marshal.SizeOf(typeof(IntPtr)) == 8)
                    {
                        _Modex64 = 1;
                        return true;
                    }
                    else
                    {
                        _Modex64 = 0;
                        return false;
                    }
                }
                else if (_Modex64 == 1)
                {
                    return true;
                }
                else if (_Modex64 == 0)
                {
                    return false;
                }
                return false;
            }
        }

        public static bool DeleteApplicationSettingsFile()
        {
            try
            {
                string settingsFile = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;

                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.Save();

                System.IO.FileInfo fi = new System.IO.FileInfo(settingsFile);
                fi.Attributes = System.IO.FileAttributes.Normal;
                fi.Delete();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
