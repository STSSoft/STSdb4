using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Concurrent;
using STSdb4.General.Communication;

namespace STSdb4.Server
{
    public partial class MainForm : Form
    {
        private UsersAndExceptionHandler handler;


        public MainForm()
        {
            InitializeComponent();
            ElementSize();
            MinimizeTray.Visible = false;
            handler = new UsersAndExceptionHandler();
            handler.Start();

            startToolStripMenuItem.Enabled = false;
            stopToolStripMenuItem.Enabled = true;

            this.Text = "STSdb4Server Running!";
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                e.Cancel = true;
            handler.Stop();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            MinimizeTray.Visible = false;
        }

        private void ServerForm_Resize(object sender, EventArgs e)
        {
            ElementSize();

            if (FormWindowState.Minimized == this.WindowState)
            {
                MinimizeTray.Visible = true;
                this.Hide();
            }
            else
                MinimizeTray.Visible = false;
        }

        private void Disconnect_Click(object sender, EventArgs e)
        {
            handler.Disconnect(usersList.SelectedItems);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var error in handler.GetExceptions())
            {
                errorList.Items.Insert(0, new ListViewItem(new[] { error.Key, error.Value }));
                try
                {
                    STSdb4Service.Service.EventLog.WriteEntry(error.Value, System.Diagnostics.EventLogEntryType.Error);
                }
                catch
                {
                }
            }

            if (errorList.Items.Count == 101)
                errorList.Items.RemoveAt(100);

            string[] selectedItems = new string[usersList.SelectedItems.Count];

            if (usersList.SelectedItems.Count > 0)
            {
                for (int i = 0; i < usersList.SelectedItems.Count; i++)
                    selectedItems[i] = usersList.SelectedItems[i].Text;
            }

            if (handler.IsFinishRefresh)
            {
                usersList.Items.Clear();
                foreach (var client in handler.GetClients())
                    usersList.Items.Add(client);
            }

            if (selectedItems.Length > 0)
            {
                foreach (var select in selectedItems)
                {
                    foreach (ListViewItem user in usersList.Items)
                    {
                        if (user.Text.Equals(select))
                        {
                            user.Selected = true;
                            break;
                        }
                    }
                }
            }
            Disconnect.Enabled = usersList.Items.Count > 0 && !handler.IsDisconnecting;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void ElementSize()
        {
            User.Width = this.Width;
            Time.Width = this.Width / 2 - 20;
            Error.Width = this.Width / 2;
        }

        private void usersList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            e.Item.Selected = usersList.Items.Count > 0 && !handler.IsDisconnecting;
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.StorageEngineServer.TcpServer.Start();
            Program.StorageEngineServer.Start();
            handler.Start();
            timer.Start();
            startToolStripMenuItem.Enabled = false;
            stopToolStripMenuItem.Enabled = true;

            this.Text = "STSdb4Server Running!";
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Stop();
            handler.Stop();
            Program.StorageEngineServer.Stop();
            Program.StorageEngineServer.TcpServer.Stop();
            usersList.Items.Clear();
            stopToolStripMenuItem.Enabled = false;
            startToolStripMenuItem.Enabled = true;

            this.Text = "STSdb4Server Stopped!";
        }
    }
}
