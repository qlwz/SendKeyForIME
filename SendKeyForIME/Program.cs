using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SendKeyForIME
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                if (args[0].ToLower() == "/i" || args[0].ToLower() == "-i")
                {
                    SetRegistryIsStart(true);
                }
                else if (args[0].ToLower() == "/u" || args[0].ToLower() == "-u")
                {
                    SetRegistryIsStart(false);
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Main_Form());
            }
        }

        public static void SetRegistryIsStart(bool iIsStart)
        {
            if (iIsStart)
            {
                try
                {
                    var strAssName = Application.StartupPath + @"\" + Application.ProductName + @".exe";
                    var ShortFileName = Application.ProductName;

                    var rgkRun = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                    if (rgkRun == null)
                    {
                        rgkRun = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                        if (rgkRun != null)
                        {
                            rgkRun.SetValue(ShortFileName, strAssName);
                        }
                    }
                    else
                    {
                        rgkRun.SetValue(ShortFileName, strAssName);
                    }
                }
                catch
                {
                    // ignored
                }
            }
            else
            {
                try
                {
                    var rgkRun = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                    if (rgkRun != null)
                    {
                        rgkRun.DeleteValue(Application.ProductName, false);
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
