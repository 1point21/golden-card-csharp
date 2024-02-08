using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Guldkortet
{
    public partial class ReturMeddelande : Form
    {
        public ReturMeddelande()
        {
            InitializeComponent();
        }

        /// <summary>
        /// metod för knapp "OK" - stänger fönstret
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
