using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Reflection;
using System.Management;

namespace PrivacyHideService
{
    public partial class MainService : ServiceBase
    {                
        public MainService()
        {
            InitializeComponent();            
        }                                 

        public static void ShowMsg(string msg)
        {            
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"c:\1\sdk_service.txt",true))
            {
                sw.WriteLine(msg);
            }

            //System.Windows.Forms.MessageBox.Show(msg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
        }

        protected override void OnStart(string[] args)
        {
            System.IO.File.WriteAllText(@"c:\1\regvalstart.txt", "start");

            string sid = ProcessExtensions.GetCurrentSessionUsername();

            System.IO.File.WriteAllText(@"c:\1\regvalsid.txt", sid);

            string account = new System.Security.Principal.SecurityIdentifier(sid).Translate(typeof(System.Security.Principal.NTAccount)).ToString();

            System.IO.File.WriteAllText(@"c:\1\regvalaccount.txt", account);

            string val=RegistryHelper2.GetAutoStartupKeyValueWithSid(sid, "PrivacyHide");

            System.IO.File.WriteAllText(@"c:\1\regval.txt",val);


            if (val == null)
            {
                return;
            }  
            else
            {
                bool suc=ProcessExtensions.StartProcessAsCurrentUser(val, "");

                System.IO.File.WriteAllText(@"c:\1\regvalsuc.txt", suc.ToString());
            }
            
        }

        protected override void OnStop()
        {
            
        }        
    }
}
