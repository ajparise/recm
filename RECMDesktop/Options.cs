using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Parise.RaisersEdge.ConnectionMonitor.Desktop
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
        }

        private void Options_Load(object sender, EventArgs e)
        {
            activeLicenseLimit.Value = decimal.Parse(Desktop.Properties.Settings.Default.NumLicenses);
            maxIdleTime.Value = decimal.Parse(Properties.Settings.Default.LeastMinutesIdle);
            foreach (string host in Properties.Settings.Default.ExcludedHosts.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries))
            {
                lstExclusion.Items.Add(host);
            }
            txtDbConnection.Text = Properties.Settings.Default.DBConnectionString;
        }

        private void btnAddHost_Click(object sender, EventArgs e)
        {
            lstExclusion.Items.Add(txtHostName.Text);
            txtHostName.Text = string.Empty;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstExclusion.SelectedIndex != -1)
            {
                lstExclusion.Items.RemoveAt(lstExclusion.SelectedIndex);
            }
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Properties.Settings.Default.NumLicenses = activeLicenseLimit.Value.ToString();
            Properties.Settings.Default.LeastMinutesIdle = maxIdleTime.Value.ToString();
            Properties.Settings.Default.ExcludedHosts = string.Empty;
            Properties.Settings.Default.ExcludedHosts = string.Join(";", lstExclusion.Items.Cast<string>().ToArray());
            Properties.Settings.Default.DBConnectionString = txtDbConnection.Text;
            Properties.Settings.Default.Save();
            this.DialogResult = DialogResult.OK;
        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
