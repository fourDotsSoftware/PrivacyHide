using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using Microsoft.Win32;

namespace PrivacyHide
{
    public class CursorManager
    {
        #region Win32Api

        private class Win32Api
        {
            [DllImport("user32.dll")]
            internal static extern IntPtr LoadCursorFromFile(string lpFileName);

            [DllImport("user32.dll")]
            internal static extern bool SetSystemCursor(IntPtr hcur, uint id);

            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr handle);

            [DllImport("user32.dll")]
            public static extern IntPtr CopyCursor(IntPtr pcur);                        

            [DllImport("user32.dll")]
            public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SystemParametersInfo(int uAction, uint uParam, uint lpvParam, uint fuWinIni);

            internal const int OCR_NORMAL = 32512;
            internal const uint OCR_NO = 32648;
            internal const int IDC_ARROW = 32512;

            internal const int SPI_SETCURSORS = 0x57;
        }

        #endregion Win32Api

        public static IntPtr TargetCursor = IntPtr.Zero;
        private static IntPtr OldCursor = IntPtr.Zero;

        public static string CursorFilepath = "";
        public static string OldCursorFilepath = "";


        public static string ArrowRegistryValue = "";
        public static string NoRegistryValue = "";
        
        [DllImport("user32.dll")]
        public static extern bool DestroyCursor(IntPtr hCursor);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public extern static bool DestroyIcon(IntPtr handle);

        public static void ClearCursor()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Cursors", true);

            try
            {
                DestroyCursor(TargetCursor);

                if (ArrowRegistryValue != String.Empty)
                {
                    key.SetValue("Arrow", ArrowRegistryValue);
                    key.SetValue("No", NoRegistryValue);
                }

                uint k = 0;
                Win32Api.SystemParametersInfo(Win32Api.SPI_SETCURSORS, 0,  k, 0);

                if (CursorFilepath != String.Empty && System.IO.File.Exists(CursorFilepath))
                {
                    System.IO.File.Delete(CursorFilepath);
                }
                
            }
            finally
            {
                key.Close();
            }



            //Win32Api.SetSystemCursor(IntPtr.Zero, Win32Api.OCR_NORMAL);

            //Win32Api.SetSystemCursor(TargetCursor, Win32Api.OCR_NORMAL);
            //DestroyCursor(TargetCursor);
            
            /*
            
            bool b = Win32Api.SetSystemCursor(IntPtr.Zero, Win32Api.OCR_NORMAL);
            CursorManager.DestroyCursor(CursorManager.TargetCursor);
            CursorManager.DestroyIcon(CursorManager.TargetCursor);

            TargetCursor = IntPtr.Zero;

            if (CursorManager.CursorFilepath != String.Empty && System.IO.File.Exists(CursorManager.CursorFilepath))
            {
                System.IO.File.Delete(CursorManager.CursorFilepath);
            }

            IntPtr cur = Win32Api.LoadCursor(IntPtr.Zero, Win32Api.IDC_ARROW);
            Win32Api.SetSystemCursor(Cursors.Arrow, Win32Api.OCR_NORMAL);
             */
             
        }

        public static void ChangeSystemCursor(byte[] CursorBinaryArray)
        {
            if (TargetCursor != IntPtr.Zero)
            {
                ClearCursor();
            }

            OldCursor=Win32Api.CopyCursor(Win32Api.LoadCursor(IntPtr.Zero,Win32Api.OCR_NORMAL));

            if (CursorFilepath != String.Empty && System.IO.File.Exists(CursorFilepath))
            {
                System.IO.File.Delete(CursorFilepath);
            }

            string ani = string.Empty;
            ani = System.IO.Path.GetTempFileName();//File extension is not mandatory

            CursorFilepath = ani;

            using (System.IO.FileStream wr = System.IO.File.Create(ani))
            {
                using (System.IO.BinaryWriter bwr = new System.IO.BinaryWriter(wr))
                {
                    bwr.Write(CursorBinaryArray);
                }
            }

            TargetCursor = Win32Api.LoadCursorFromFile(ani);
            //to set form cursor set the system cursor set the winform’s cursor as ->  this.Cursor = new Cursor(hAni); and on closing destroy the handle

            bool b = Win32Api.SetSystemCursor(TargetCursor, Win32Api.OCR_NORMAL);
            //b = Win32Api.SetSystemCursor(TargetCursor, Win32Api.OCR_NO);
            //System.IO.File.Delete(ani);
        }

        public static void ChangeSystemCursor(System.Drawing.Icon ico)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Cursors", true);

            try
            {
                if (ArrowRegistryValue == String.Empty)
                {
                    ArrowRegistryValue = key.GetValue("Arrow").ToString();
                    NoRegistryValue = key.GetValue("No").ToString();
                }

                if (CursorFilepath != String.Empty && System.IO.File.Exists(CursorFilepath))
                {
                    System.IO.File.Delete(CursorFilepath);
                }

                string ani = string.Empty;
                ani = System.IO.Path.GetTempFileName();//File extension is not mandatory

                StreamWriter iconWriter = new StreamWriter(ani);
                ico.Save(iconWriter.BaseStream);
                iconWriter.Close();
                iconWriter.Dispose();

                CursorFilepath = ani;

                TargetCursor = Win32Api.LoadCursorFromFile(ani);

                key.SetValue("Arrow", CursorFilepath);
                key.SetValue("No", CursorFilepath);

                uint k = 0;
                Win32Api.SystemParametersInfo(Win32Api.SPI_SETCURSORS, (uint)0, (uint)k,(uint)0);
            }
            finally
            {
                key.Close();
            }

            /*
            if (TargetCursor != IntPtr.Zero)
            {
                ClearCursor();
            }

            //IntPtr loadcur = Win32Api.LoadCursor(IntPtr.Zero, Win32Api.OCR_NORMAL);
            //OldCursor = Win32Api.CopyCursor(loadcur);
            IntPtr OldCursor = Win32Api.LoadCursor(IntPtr.Zero, Win32Api.IDC_ARROW);

            if (CursorFilepath != String.Empty && System.IO.File.Exists(CursorFilepath))
            {
                System.IO.File.Delete(CursorFilepath);
            }

            string ani = string.Empty;
            ani = System.IO.Path.GetTempFileName();//File extension is not mandatory

            StreamWriter iconWriter = new StreamWriter(ani);            
            ico.Save(iconWriter.BaseStream);
            iconWriter.Close();
            iconWriter.Dispose();                       

            TargetCursor = Win32Api.LoadCursorFromFile(ani);
            CursorFilepath = ani;
            //to set form cursor set the system cursor set the winform’s cursor as ->  this.Cursor = new Cursor(hAni); and on closing destroy the handle

            bool b = Win32Api.SetSystemCursor(TargetCursor, Win32Api.OCR_NORMAL);
            //b = Win32Api.SetSystemCursor(TargetCursor, Win32Api.OCR_NO);
            //System.IO.File.Delete(ani);

            //MessageBox.Show(b.ToString());
            //Win32Api.DestroyCursor(TargetCursor);
            */
        }

        public static void ChangeSystemCursor(string filepath)
        {
            if (TargetCursor != IntPtr.Zero)
            {
                ClearCursor();
            }

            OldCursor = Win32Api.CopyCursor(Win32Api.LoadCursor(IntPtr.Zero, Win32Api.OCR_NORMAL));

            TargetCursor = Win32Api.LoadCursorFromFile(filepath);
            //to set form cursor set the system cursor set the winform’s cursor as ->  this.Cursor = new Cursor(hAni); and on closing destroy the handle

            bool b = Win32Api.SetSystemCursor(TargetCursor, Win32Api.OCR_NORMAL);
            //b = Win32Api.SetSystemCursor(TargetCursor, Win32Api.OCR_NO);
            //System.IO.File.Delete(ani);

           // MessageBox.Show(b.ToString());

            //Win32Api.DestroyCursor(TargetCursor);

        }
    }
}
