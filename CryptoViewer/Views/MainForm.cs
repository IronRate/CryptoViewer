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
                var providerTypes = CryptoViewer.Native.CryptoApiHelper.GetProviderTypes();
                foreach (var providerType in providerTypes)
                {
                    var node = treeView1.Nodes.Add(providerType.Value.ToString(), providerType.Key);
                    //node.Tag = providerType.Value;
                }
            }
            catch (Exception)
            {


            }

            try
            {
                var providerTypes = CryptoViewer.Native.CryptoApiHelper.GetProviders();
                foreach (var providerType in providerTypes)
                {
                    var nodes = treeView1.Nodes.Find(providerType.Value.ToString(), false);
                    if (nodes.Length > 0)
                    {
                        var node = nodes[0].Nodes.Add(providerType.Key);
                        node.Tag = providerType.Value;
                    }
                    else
                    {
                        var node = treeView1.Nodes.Add(providerType.Key);
                        node.Tag = providerType.Value;
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

            treeView1.EndUpdate();

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string providerName = null;
            int providerType = 0;

            if (e.Node != null)
            {
                providerName = e.Node.Text;
                if (e.Node.Tag != null)
                {
                    providerType = (int)e.Node.Tag;
                }
            }
            cryptoproviderParamsComponent1.ProviderName = providerName;
            cryptoproviderParamsComponent1.ProviderType = providerType;

            if (providerType != 0)
            {
                var providerHandle = CryptoViewer.Native.CryptoApiHelper.AcquireProvider(new System.Security.Cryptography.CspParameters(providerType, providerName));
                var algorithms = CryptoViewer.Native.CryptoApiHelper.GetProviderAlgorithms(providerHandle);
                cryptoAlgorithmsComponent1.Algorithms = algorithms;

                var cspInfo = CryptoViewer.Native.CryptoApiHelper.GetCSPInfo(providerHandle);
                cspInfoComponent1.CSPName = cspInfo.Name;
                cspInfoComponent1.CSPVersion = cspInfo.Version;

            }
            else {
                cryptoAlgorithmsComponent1.Algorithms = null;
            }

            


        }

    }
}

