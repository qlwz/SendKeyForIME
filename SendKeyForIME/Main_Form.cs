using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SendKeyForIME.Properties;

namespace SendKeyForIME
{
    public enum KeyModifiers
    {
        Alt = 1,
        Ctrl = 2,
        None = 0,
        Shift = 4,
        WindowsKey = 8
    }

    public partial class Main_Form : Form
    {
        const int WM_HOTKEY = 0x312;
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);

        [DllImport("user32.dll")]
        static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [System.Runtime.InteropServices.DllImport("user32.dll")] //申明API函数 
        public static extern bool RegisterHotKey(
        IntPtr hWnd, // handle to window 
        int id, // hot key identifier 
        uint fsModifiers, // key-modifier options 
        Keys vk // virtual-key code 
        );

        private const int VK_CONTROL = 0x11;
        private const int VK_SHIFT = 0x10;

        public int KeyId { get; set; }
        public Main_Form()
        {
            InitializeComponent();
            this.Icon = Resources.ICO;
            this.Closing += Main_Form_Closing;
        }

        private void Main_Form_Closing(object sender, CancelEventArgs e)
        {
            UnregisterHotKey(this.Handle, this.KeyId);
        }

        private void Main_Form_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Hide();

            this.KeyId = 10;
            var result = RegisterHotKey(this.Handle, this.KeyId, KeyModifiers.Ctrl, Keys.Space);
            if (!result)
            {
                MessageBox.Show("快捷键【Ctrl + Space】已经被其他程序占用", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                base.Close();
            }
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                int id = m.WParam.ToInt32();
                if (id == this.KeyId)
                {
                    this.Hotkey_SendKeyForIME();
                }
            }
            base.WndProc(ref m);
        }

        private void Hotkey_SendKeyForIME()
        {
            keybd_event(VK_CONTROL, 0, 0, 0);
            keybd_event(VK_SHIFT, 0, 0, 0);
            keybd_event(VK_SHIFT, 0, 2, 0);
        }
    }
}
