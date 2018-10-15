using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CryptoViewer.Native;

namespace CryptoViewer.Components
{
    public partial class CryptoAlgorithmsComponent : UserControl
    {
        List<ProviderAlgorithm> _algorithms;

        public CryptoAlgorithmsComponent()
        {
            InitializeComponent();
        }

        public List<ProviderAlgorithm> Algorithms
        {
            get => _algorithms;
            set {
                _algorithms = value;
                this._refresh();
            }
        }

        private void _refresh() {
            listView1.BeginUpdate();
            listView1.Items.Clear();
            if (_algorithms != null)
            {
                foreach (var alg in _algorithms)
                {
                    var item = listView1.Items.Add(alg.Name);
                    item.SubItems.Add($"{alg.BitLength} (0x{alg.BitLength.ToString("X")})");
                    item.SubItems.Add($"{alg.AlgorithmId} (0x{alg.AlgorithmId.ToString("X")})");
                    item.Tag = alg;
                }
            }
            listView1.EndUpdate();
        }

        
    }
}
