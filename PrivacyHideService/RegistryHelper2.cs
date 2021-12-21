using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace PrivacyHideService
{
    class RegistryHelper2
    {
        public static string GetKeyValue(string AppRegKey, string keystr)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser;

                key = key.OpenSubKey("Software",true);

                RegistryKey key2 = null;

                try
                {
                    key2 = key.OpenSubKey("4dots Software",true);

                    if (key2 == null)
                    {
                        key2 = key.CreateSubKey("4dots Software");
                    }

                    RegistryKey key3 = key2.OpenSubKey(AppRegKey,true);

                    try
                    {

                        if (key3 == null)
                        {
                            key3 = key2.CreateSubKey(AppRegKey);
                        }

                        if (key3 != null)
                        {
                            return key3.GetValue(keystr, "").ToString();
                        }

                        return "";

                    }
                    finally
                    {
                        if (key3 != null)
                        {
                            key3.Close();
                        }
                    }

                }
                finally
                {
                    if (key2 != null)
                    {
                        key2.Close();
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetKeyValueWithSid(string Sid,string AppRegKey, string keystr)
        {
            try
            {
                RegistryKey key = Registry.Users.OpenSubKey(Sid, true);

                key = key.OpenSubKey("Software", true);

                RegistryKey key2 = null;

                try
                {
                    key2 = key.OpenSubKey("4dots Software", true);

                    if (key2 == null)
                    {
                        key2 = key.CreateSubKey("4dots Software");
                    }

                    RegistryKey key3 = key2.OpenSubKey(AppRegKey, true);

                    try
                    {

                        if (key3 == null)
                        {
                            key3 = key2.CreateSubKey(AppRegKey);
                        }

                        if (key3 != null)
                        {
                            return key3.GetValue(keystr, "").ToString();
                        }

                        return "";

                    }
                    finally
                    {
                        if (key3 != null)
                        {
                            key3.Close();
                        }
                    }

                }
                finally
                {
                    if (key2 != null)
                    {
                        key2.Close();
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetAutoStartupKeyValueWithSid(string Sid, string AppRegKey)
        {
            try
            {
                RegistryKey key = Registry.Users.OpenSubKey(Sid, false);

                key = key.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", false);

                if (key==null) return null;

                string val=key.GetValue("PrivacyHide","").ToString();

                try
                {
                    key.Close();                    
                }
                catch { }

                if (val==string.Empty) return null;               

                return val;                                
            }
            catch
            {
                return null;
            }
        }

        public static bool SetKeyValue(string AppRegKey, string keystr, string keyvalue)
        {
            RegistryKey key = Registry.CurrentUser;

            key = key.OpenSubKey("Software");

            RegistryKey key2 = null;

            try
            {
                key2 = key.OpenSubKey("4dots Software",true);

                if (key2 == null)
                {
                    key2 = key.CreateSubKey("4dots Software");
                }

                RegistryKey key3 = key2.OpenSubKey(AppRegKey, true);

                try
                {

                    if (key3 == null)
                    {
                        key3 = key2.CreateSubKey(AppRegKey);

                        key3.Close();
                    }

                    key3 = key2.OpenSubKey(AppRegKey, true);

                    if (key3 != null)
                    {
                        key3.SetValue(keystr, keyvalue);
                    }

                }
                finally
                {
                    if (key3 != null)
                    {
                        key3.Close();
                    }
                }
            }
            finally
            {
                if (key2 != null)
                {
                    key2.Close();
                }
            }

            return true;
        }

        public static string GetKeyValueLM(string AppRegKey, string keystr)
        {
            try
            {
                RegistryKey key = Registry.LocalMachine;

                key = key.OpenSubKey("Software",true);

                RegistryKey key2 = null;

                try
                {
                    key2 = key.OpenSubKey("4dots Software",true);

                    if (key2 == null)
                    {
                        key2 = key.CreateSubKey("4dots Software");
                    }

                    RegistryKey key3 = key2.OpenSubKey(AppRegKey,true);

                    try
                    {

                        if (key3 == null)
                        {
                            key3 = key2.CreateSubKey(AppRegKey);
                        }

                        if (key3 != null)
                        {
                            return key3.GetValue(keystr, "").ToString();
                        }

                        return "";

                    }
                    finally
                    {
                        if (key3 != null)
                        {
                            key3.Close();
                        }
                    }

                }
                finally
                {
                    if (key2 != null)
                    {
                        key2.Close();
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetKeyValueLMLowPriv(string AppRegKey, string keystr)
        {
            try
            {
                RegistryKey key = Registry.LocalMachine;

                key = key.OpenSubKey("Software");

                RegistryKey key2 = null;

                try
                {
                    key2 = key.OpenSubKey("4dots Software");

                    RegistryKey key3 = key2.OpenSubKey(AppRegKey);

                    try
                    {                        

                        if (key3 != null)
                        {
                            return key3.GetValue(keystr, "").ToString();
                        }

                        return "";

                    }
                    finally
                    {
                        if (key3 != null)
                        {
                            key3.Close();
                        }
                    }

                }
                finally
                {
                    if (key2 != null)
                    {
                        key2.Close();
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static bool SetKeyValueLM(string AppRegKey, string keystr, string keyvalue)
        {
            RegistryKey key = Registry.LocalMachine;

            key = key.OpenSubKey("Software",true);

            RegistryKey key2 = null;

            try
            {
                key2 = key.OpenSubKey("4dots Software",true);

                if (key2 == null)
                {
                    key2 = key.CreateSubKey("4dots Software");
                }

                RegistryKey key3 = key2.OpenSubKey(AppRegKey, true);

                try
                {

                    if (key3 == null)
                    {
                        key3 = key2.CreateSubKey(AppRegKey);

                        key3.Close();
                    }

                    key3 = key2.OpenSubKey(AppRegKey, true);

                    if (key3 != null)
                    {
                        key3.SetValue(keystr, keyvalue);
                    }

                }
                finally
                {
                    if (key3 != null)
                    {
                        key3.Close();
                    }
                }
            }
            finally
            {
                if (key2 != null)
                {
                    key2.Close();
                }
            }

            return true;
        }
    }  
}
