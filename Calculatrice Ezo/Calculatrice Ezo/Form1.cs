using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculatrice_Ezo
{
    public partial class Calculator : Form
    {
        public Calculator()
        {
            InitializeComponent();
        }

        private void Calculator_Load(object sender, EventArgs e)
        {
            tbFormula.Text = "";
            tbResult.Text = "";
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                var expression = FormulaParser.Parse(tbFormula.Text);

                tbResult.Text = expression.Evaluate().ToString();
            }
            catch (Exception ex)
            {
                tbResult.Text = "Erreur*";
            }

        }
    }
}
