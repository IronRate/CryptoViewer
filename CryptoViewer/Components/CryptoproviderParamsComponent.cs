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
    public partial class CryptoproviderParamsComponent : UserControl
    {



        public CryptoproviderParamsComponent()
        {
            InitializeComponent();
        }
        #region Properties

        /// <summary>
        /// Наименование криптопровайдера
        /// </summary>
        public string ProviderName { get => textBox1.Text; set => textBox1.Text = value; }

        /// <summary>
        /// Тип криптопровайдера
        /// </summary>
        public int ProviderType { get => !string.IsNullOrWhiteSpace(textBox2.Text) ? System.Convert.ToInt32(textBox2.Text) : 0; set => textBox2.Text = value > 0 ? value.ToString() : null; }

        #endregion

    }


}
