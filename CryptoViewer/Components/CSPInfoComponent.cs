using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoViewer.Components
{
    public partial class CSPInfoComponent : UserControl
    {
        public CSPInfoComponent()
        {
            InitializeComponent();
        }

        public string CSPName { get => textBox1.Text; set => textBox1.Text = value; }

        public string CSPVersion { get => textBox2.Text; set => textBox2.Text = value; }
    }
}
