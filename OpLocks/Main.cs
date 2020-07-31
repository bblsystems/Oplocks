using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace OpLocks
{
    public partial class Main : Form
    {
        public Main(string[] args)
        {
            InitializeComponent();

            if (args.Length == 1)
            {
                disableOplocks();
                Environment.Exit(0);
            }

            object keyVal1, keyVal2, keyVal3;

            keyVal1 = Registry.GetValue(@"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\MRXSmb\Parameters", "OplocksDisabled", null);
            keyVal2 = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "EnableOplocks", null);
            keyVal3 = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "SMB2", null);

            if ((keyVal1 != null && keyVal2 != null && keyVal3 != null) && ((int)keyVal1 == 1 && (int)keyVal2 == 0 && (int)keyVal3 == 0))
            {
                label1.Text = "Oplocks are already disabled on this computer.  Click Ok to disable again.";
            }
        }

        private void disableOplocks()
        {

            this.updateKey(@"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\MRXSmb\Parameters",
                            "OplocksDisabled", 1);

            this.updateKey(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters",
                            "EnableOplocks", 0);

            this.updateKey(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters",
                            "SMB2", 0);
        }


        private void disable_Click(object sender, EventArgs e)
        {
            disableOplocks();
            MessageBox.Show("Registry updated. Please reboot your computer.");
            Application.Exit();
        }

        private void updateKey(string keyName, string valueName, object value)
        {
            object oldValue = Registry.GetValue(keyName, valueName, null);

            if (oldValue != null)
            {
                this.backupKey(valueName, oldValue);
            }

            Registry.SetValue(keyName, valueName, value);
        }

        private void backupKey(string valueName, object value)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\OplocksOldValues"))
            {
                object backupValue = key.GetValue(valueName, null);

                if (backupValue == null)
                {
                    key.SetValue(valueName, value);
                }
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            this.updateKey(@"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\MRXSmb\Parameters",
                "OplocksDisabled", 0);

            this.updateKey(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters",
                            "EnableOplocks", 1);

            this.updateKey(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters",
                            "SMB2", 1);

            MessageBox.Show("Registry updated to re-enable oplocks. Please reboot your computer.");
        }
    }
}
