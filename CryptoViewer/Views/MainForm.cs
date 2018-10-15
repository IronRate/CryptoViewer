using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoViewer.Views
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.refreshProviders();
        }

        private void refreshProviders()
        {
            treeView1.BeginUpdate();
                treeView1.Nodes.Clear();
            try
            {
                var providers=CryptoViewer.Native.CryptoApiHelper.GetProviders();
                foreach (var provider in providers) {
                    var node = treeView1.Nodes.Add(provider.Key);
                    node.Tag = provider.Value;
                }
            }
            catch (Exception)
            {


            }

            treeView1.EndUpdate();

        }
    }
}
