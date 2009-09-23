using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Parise.RaisersEdge.ConnectionMonitor.Monitors;
using Parise.RaisersEdge.ConnectionMonitor.Data.Entities;
using System.Text.RegularExpressions;

namespace Parise.RaisersEdge.ConnectionMonitor.Desktop
{    
    public partial class Form1 : Form
    {
        private System.Collections.Stack _balloonStack = new System.Collections.Stack();
        private Parise.RaisersEdge.ConnectionMonitor.Monitors.DesktopREConnectionMonitor _monitor = new Parise.RaisersEdge.ConnectionMonitor.Monitors.DesktopREConnectionMonitor();
        private int lastActiveCount = 0;

        private class QuickBallon
        {
            public int delay { get; set; }
            public string title { get; set; }
            public string body { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
            _monitor.FreedConnection += new Parise.RaisersEdge.ConnectionMonitor.Monitors.REConnectionMonitor.FreedConnectionEvent(_monitor_FreedConnection);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _monitor.Dispose();
            base.OnClosing(e);
        }

        void _monitor_FreedConnection(Parise.RaisersEdge.ConnectionMonitor.Monitors.FreedEventArgs e)
        {
            //UpdateActiveConnDisplay();
        }

        private void taskIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //UpdateActiveConnDisplay();

            this.MaximizeBox = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
           
            //logoutTimer.Enabled = Properties.Settings.Default.MonitoringEnabled;

            _monitor.Settings = Enum.GetValues(typeof(MonitorSettings)).Cast<MonitorSettings>().Where(s => s != MonitorSettings.Unknown).ToDictionary(a => a, s => Properties.Settings.Default.PropertyValues[s.ToString()] == null ? "" : (string)Properties.Settings.Default.PropertyValues[s.ToString()].PropertyValue);
            connectionWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(connectionWorker_RunWorkerCompleted);
            //connectionListWorker.RunWorkerAsync(listView1);
            connectionWorker.RunWorkerAsync();
            connectionListWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(connectionListWorker_RunWorkerCompleted);

            navigationPanel.SelectedIndexChanged += new EventHandler(navigationPanel_SelectedIndexChanged);
            navPaneConnection.Text += " v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        void navigationPanel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (navigationPanel.SelectedNavigationPage == navPaneSettings)
            {
                activeLicenseLimit.Value = decimal.Parse(Desktop.Properties.Settings.Default.NumLicenses);
                maxIdleTime.Value = decimal.Parse(Properties.Settings.Default.LeastMinutesIdle);
                //foreach (string host in Properties.Settings.Default.ExcludedHosts.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                //{
                //    lstExclusion.Items.Add(host);
                //}
                txtDbConnection.Text = Properties.Settings.Default.DBConnectionString;
            }
        }

        void connectionListWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled && isGoodConnection)
            {
                panelLoadingLicense.Visible = true;
                connectionListWorker.RunWorkerAsync();
            }
        }


        //private void UpdateNonDataUI()
        //{
        //    lblLicenseInUse.Text = "0";            
        //    startMonitoringToolStripMenuItem.Text = logoutTimer.Enabled ? "Stop Monitoring" : "Start Monitoring";
        //    this.Text = logoutTimer.Enabled ? "RE Connection Monitor ::: Active" : "RE Connection Monitor ::: Inactive";
        //    lblMonitorStatus.Text = logoutTimer.Enabled ? "Active" : "Inactive";
        //    lblMonitorStatus.ForeColor = logoutTimer.Enabled ? Color.DarkGreen : Color.Tomato;
        //}

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !clickedExit;
            if (!clickedExit)
            {
                taskIcon.ShowBalloonTip(5000, "RECM Is Still Running!", "Double-click this icon to open RECM again", ToolTipIcon.None);
                this.Hide();
            }
        }

        //private void UpdateActiveConnDisplay()
        //{
        //    var db = new Parise.RaisersEdge.ConnectionMonitor.Data.RecmDataContext(Properties.Settings.Default.DBConnectionString);

        //    listView1.Items.Clear();

        //    var locks = db.LockConnections_AllActiveREConnections_ClientOnly.OrderByDescending(a => a.REProcess.IdleTime.TotalMilliseconds).ToList();
        //            var listItems = locks
        //            .Select(a => 
        //                new ListViewItem(
        //                new string[] 
        //            {
        //                Math.Round((((float)a.REProcess.IdleTime.TotalMinutes / float.Parse(Properties.Settings.Default.LeastMinutesIdle)) * 100.0), 0) + "%",
        //                a.Lock.User.Name,
        //                a.REProcess.IdleTimeFormatted("{h:D2}:{m:D2}:{s:D2}"),
        //                "0" 
        //            }));

        //    var users = new List<string> { };

        //    foreach(var item in listItems)
        //    {
        //        if (!users.Contains(item.SubItems[1].Text))
        //        {
        //            users.Add(item.SubItems[1].Text);
        //            listView1.Items.Add(item);
        //        }
        //    }

        //    foreach (ListViewItem listItem in listView1.Items)
        //    {
        //        float perc = float.Parse(listItem.SubItems[3].Text) * 100;
        //        if (perc >= 50 && perc <= 75)
        //        {
        //            listItem.BackColor = Color.Yellow;
        //        }
        //        else if (perc > 75)
        //        {
        //            listItem.BackColor = Color.Red;
        //            listItem.ForeColor = Color.White;
        //        }
        //    }
        //}

        private void startMonitoringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //logoutTimer.Enabled = !logoutTimer.Enabled;
            //Properties.Settings.Default.MonitoringEnabled = logoutTimer.Enabled;
            //Properties.Settings.Default.Save();
            //startMonitoringToolStripMenuItem.Text = logoutTimer.Enabled ? "Stop Monitoring" : "Start Monitoring";
            //this.Text = logoutTimer.Enabled ? "RE Connection Monitor ::: Active" : "RE Connection Monitor ::: Inactive";
            ////lblMonitorStatus.Text = logoutTimer.Enabled ? "Active" : "Inactive";
            ////lblMonitorStatus.ForeColor = logoutTimer.Enabled ? Color.DarkGreen : Color.Tomato;
        }


        private void timer1_Tick_1(object sender, EventArgs e)
        {
            
        }
        delegate void AddRangeCallback(ListViewItem[] items);
        private void AddRangeItems(ListViewItem[] items)
        {
            lstViewLicenses.Items.AddRange(items);
        }

        delegate void SetOutageMessageCallBack(string msg);
        private void SetOutageMessage(string msg)
        {
            //lblOutageMessage.Text = msg;
        }

        delegate void SetVisibilityCallBack(object item, bool visible);
        private void SetVisibilityLevel(object item, bool visible)
        {
            var vis = item.GetType().GetProperty("Visible");
            vis.SetValue(item, visible, null);
            //lblLicenseInUse.Text = inUse.ToString();
        }

        private int GetImageIndex(FilteredLockConnection a)
        {
            var pi = Math.Round((((float)a.REProcess.IdleTime.TotalMinutes / float.Parse(Properties.Settings.Default.LeastMinutesIdle)) * 100.0), 0);
            if (pi >= 100.0)
            {
                return 2;
            }
            else if (Properties.Settings.Default.ExcludedREUsers.Contains(a.Lock.User.Name) || Properties.Settings.Default.ExcludedHosts.Contains(a.Lock.MachineName))
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            bool noFails = true;
            while (noFails)
            {
                try
                {

                    if (connectionListWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }

                    var db = new Parise.RaisersEdge.ConnectionMonitor.Data.RecmDataContext(Properties.Settings.Default.DBConnectionString);

                    var currentInUse = db.LockConnections_AllActiveREConnectionsAliveOnly_ClientOnly.Select(a => a.Lock.User.Name).Distinct().Count();

                    if (currentInUse != lastActiveCount || settingsChanged)
                    {
                        panelLoadingLicense.Invoke((MethodInvoker)delegate()
                        {
                            panelLoadingLicense.Visible = true;
                        });

                        settingsChanged = false;
                        lastActiveCount = currentInUse;
                        //if (lblLicenseInUse.InvokeRequired)
                        //{
                        //    lblLicenseInUse.Invoke(new SetLicenseInUseLabelCallBack(SetLicenseInUseLabel), lastActiveCount);
                        //}
                        //if (Properties.Settings.Default.MonitoringEnabled) _monitor.FreeConnections(true);                                        


                        var locks = db.LockConnections_AllActiveREConnections_ClientOnly.OrderByDescending(a => a.REProcess.IdleTime.TotalMilliseconds).ToList();
                        var listItems = locks
                        .Select(a =>
                            new ListViewItem()
                        {
                            Text = a.Lock.User.Name + " - " + a.REProcess.IdleTimeFormatted("{h:D2}:{m:D2}:{s:D2}"),
                            ImageIndex = GetImageIndex(a),
                            Group = lstViewLicenses.Groups[GetImageIndex(a) - 1]
                        });

                        var users = new List<string> { };

                        //listView1.Invoke(new ClearItemsCallback(ClearItems), null);

                        var filterItems = new List<ListViewItem>();

                        foreach (var item in listItems)
                        {
                            var uName = Regex.Split(item.SubItems[0].Text, " - ")[0];
                            if (!users.Contains(uName))
                            {
                                users.Add(uName);
                                filterItems.Add(item);
                            }
                        }

                        lstViewLicenses.Invoke((MethodInvoker)delegate()
                        {
                            lstViewLicenses.Items.Clear();
                        });

                        lstViewLicenses.Invoke(new AddRangeCallback(AddRangeItems), new object[]
                        {                            
                            filterItems.ToArray()
                        });

                        lstViewLicenses.Invoke((MethodInvoker)delegate()
                        {
                            var licCount = int.Parse(Properties.Settings.Default.NumLicenses);
                            var imgIdx = currentInUse >= licCount ? 1 : 0;
                            
                            lstViewLicenses.Items.Add(new ListViewItem(currentInUse.ToString() + " of " + Properties.Settings.Default.NumLicenses, imgIdx, lstViewLicenses.Groups[0]));
                        });
                        //lstViewLicenses.Invoke((MethodInvoker)delegate()
                        //{
                        //    lstViewLicenses.Items.AddRange(filterItems.ToArray());
                        //});

                        //foreach (ListViewItem listItem in listView1.Items)
                        //{
                        //    float perc = float.Parse(listItem.SubItems[3].Text) * 100;
                        //    if (perc >= 50 && perc <= 75)
                        //    {
                        //        listItem.BackColor = Color.Yellow;
                        //    }
                        //    else if (perc > 75)
                        //    {
                        //        listItem.BackColor = Color.Red;
                        //        listItem.ForeColor = Color.White;
                        //    }
                        //}
                    }
                }
                catch (Exception err)
                {
                    //lblMonitorStatus.Text = logoutTimer.Enabled ? "Active" : "Inactive";
                    //lblMonitorStatus.ForeColor = logoutTimer.Enabled ? Color.DarkGreen : Color.Tomato;
                    //panelBadConnection.Invoke(new SetVisibilityCallBack(SetVisibilityLevel), new object[]
                    //{
                    //    panelBadConnection,
                    //    true
                    //});
                    ////outageContainer.Visible = true;
                    //outageContainer.Invoke(new SetVisibilityCallBack(SetVisibilityLevel), new object[]
                    //{
                    //    outageContainer,
                    //    true
                    //});

                    //lblOutageMessage.Invoke(new SetOutageMessageCallBack(SetOutageMessage), new object[]
                    //{
                    //    "There was an error connecting to the server, please check your connection.\n\nBouncing is disabled.\n\n" + err.Message
                    //});
                    //noFails = false;
                    //MessageBox.Show("There was an error connecting to the server, please check your connection.\n\nBouncing is disabled.", "RECM Error", MessageBoxButtons.OK);
                }
                panelLoadingLicense.Invoke((MethodInvoker)delegate()
                {
                    panelLoadingLicense.Visible = false;
                });
                if (connectionListWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                System.Threading.Thread.Sleep(500);
                if (connectionListWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
            }
            //bool noFails = true;
            //while (noFails)
            //{
            //    try
            //    {
            //        IntegrationDataDataContext db = new IntegrationDataDataContext();
            //        var connList = db.RaisersEdgeConnections.ToList();
            //        int activeConn = connList.Select(c => c.hostname).Distinct().Count();
            //        if (this.listView1.InvokeRequired)
            //        {
            //            listView1.Invoke(new ClearItemsCallback(ClearItems), null);
            //            if (lblLicenseInUse.InvokeRequired)
            //            {
            //                lblLicenseInUse.Invoke(new SetLicenseInUseLabelCallBack(SetLicenseInUseLabel), activeConn);
            //            }
            //            ListViewItem[] items = connList.Where(a => a.program_name.Trim().Equals("The Raiser's Edge")).OrderByDescending(a => a.secondsidle).ToList()
            //                .Select(a => new ListViewItem(new string[] { WMIWrapper.GetExplorerOwner(a.hostname.Trim()),
            //            a.minutessidle.Value.ToString("000"),
            //            ((float)((float)a.secondsidle.Value / ((float)Properties.Settings.Default.MaxIdleTime*(float)60))).ToString("00.00%"),
            //            ((float)((float)a.secondsidle.Value / ((float)Properties.Settings.Default.MaxIdleTime*(float)60))).ToString("00.000")})).ToArray();

            //            foreach (ListViewItem listItem in items)
            //            {
            //                float perc = float.Parse(listItem.SubItems[3].Text) * 100;
            //                if (perc >= 50 && perc <= 80)
            //                {
            //                    listItem.BackColor = Color.Yellow;
            //                }
            //                else if (perc > 80)
            //                {
            //                    listItem.BackColor = Color.Red;
            //                    listItem.ForeColor = Color.White;
            //                }
            //            }

            //            listView1.Invoke(new AddRangeCallback(AddRangeItems), new object[]
            //    {
            //        items
            //    });
            //        }
            //        else
            //        {
            //            listView1.Items.Clear();
            //            //IntegrationDataDataContext db = new IntegrationDataDataContext();
            //            listView1.Items.AddRange(
            //                    connList.Where(a => a.program_name.Trim().Equals("The Raiser's Edge")).OrderByDescending(a => a.secondsidle).ToList()
            //                    .Select(a => new ListViewItem(new string[] { a.hostname.Trim(),
            //            a.minutessidle.Value.ToString("000"),
            //            ((float)((float)a.secondsidle.Value / ((float)Properties.Settings.Default.MaxIdleTime*(float)60))).ToString("00.00%"),
            //            ((float)((float)a.secondsidle.Value / ((float)Properties.Settings.Default.MaxIdleTime*(float)60))).ToString("00.000")})).ToArray());

            //            foreach (ListViewItem listItem in listView1.Items)
            //            {
            //                float perc = float.Parse(listItem.SubItems[3].Text) * 100;
            //                if (perc >= 50 && perc <= 75)
            //                {
            //                    listItem.BackColor = Color.Yellow;
            //                }
            //                else if (perc > 75)
            //                {
            //                    listItem.BackColor = Color.Red;
            //                    listItem.ForeColor = Color.White;
            //                }
            //            }
            //        }
            //        System.Threading.Thread.Sleep(1000);
            //    }
            //    catch (Exception err)
            //    {
            //        noFails = false;
            //    }
            //}
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while(true)
                {
                    QuickBallon q = (QuickBallon)_balloonStack.Pop();
                    taskIcon.ShowBalloonTip(q.delay, q.title, q.body, ToolTipIcon.Info);
                    System.Threading.Thread.Sleep(q.delay);
                 }
            }
            catch
            {
            }
        }

        private bool clickedExit = false;
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clickedExit = true;
            logoutTimer.Enabled = false;
            Close();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            clickedExit = true;
            logoutTimer.Enabled = false;
            Close();
        }

        private void settingsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Options o = new Options();
            o.ShowDialog();
            panelConnecting.Visible = true;
            panelBadConnection.Visible = false;

            navPaneConnection.ImageIndex = 0;
            if (connectionWorker.IsBusy)
                connectionWorker.CancelAsync();
            else
                connectionWorker.RunWorkerAsync();
            //Update
        }

        private void connectionWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (connectionWorker.CancellationPending)
                {
                    e.Cancel = true;
                }
                else
                {
                    var db = new Parise.RaisersEdge.ConnectionMonitor.Data.RecmDataContext(Properties.Settings.Default.DBConnectionString);
                    db.LockConnections.Count();
                    e.Result = db;
                }
            }
            catch (Exception err)
            {
                if (connectionWorker.CancellationPending)
                {
                    e.Cancel = true;
                }
                else
                {
                    e.Result = err;
                }
            }
        }

        void connectionWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                var resultObj = e.Result;

                if (resultObj is Exception)
                {
                    var ex = (Exception)resultObj;
                    lblOutageMsg.Text = ex.Message;
                    panelBadConnection.Visible = true;
                    panelConnecting.Visible = false;
                    panelGoodConnection.Visible = false;
                    isGoodConnection = false;
                    navPaneConnection.ImageIndex = 1;
                }
                else
                {
                    isGoodConnection = true;
                    panelConnecting.Visible = false;
                    panelGoodConnection.Visible = true;
                    navPaneConnection.ImageIndex = 2;
                    if (connectionListWorker.IsBusy)
                        connectionListWorker.CancelAsync();
                    else
                        connectionListWorker.RunWorkerAsync(resultObj);
                }
            }
            else
            {
                connectionWorker.RunWorkerAsync();
            }
        }

        bool isGoodConnection = false;

        bool settingsChanged = false;
        bool dbStringChanged = false;
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dbStringChanged = !Properties.Settings.Default.DBConnectionString.Equals(txtDbConnection.Text);

            settingsChanged = !Properties.Settings.Default.NumLicenses.Equals(activeLicenseLimit.Value.ToString()) ||
                !Properties.Settings.Default.LeastMinutesIdle.Equals(maxIdleTime.Value.ToString()) ||
                dbStringChanged;

            Properties.Settings.Default.NumLicenses = activeLicenseLimit.Value.ToString();
            Properties.Settings.Default.LeastMinutesIdle = maxIdleTime.Value.ToString();
            //Properties.Settings.Default.ExcludedHosts = string.Empty;
            //Properties.Settings.Default.ExcludedHosts = string.Join(";", lstExclusion.Items.Cast<string>().ToArray());
            Properties.Settings.Default.DBConnectionString = txtDbConnection.Text;
            Properties.Settings.Default.Save();


            if (dbStringChanged)
            {
                navigationPanel.SelectNavigationPage(navPaneConnection);
                panelBadConnection.Visible = false;
                panelConnecting.Visible = true;
                panelGoodConnection.Visible = false;

                panelLoadingLicense.Visible = true;
                if (connectionWorker.IsBusy)
                    connectionWorker.CancelAsync();
                else
                    connectionWorker.RunWorkerAsync();
            }
        }
    }
}
