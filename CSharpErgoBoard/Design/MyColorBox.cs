using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace CSharpErgoBoard.Design
{
    public partial class MyColorBox : Form
    {
        private Boolean m_selected = false;
        private Boolean m_gotColor = false;
        private static Color m_selectedColor = new Color();
        private UInt32 m_maxNumber = 255;
        private Boolean m_customSelected = false;
        private Button m_selectedButton = new Button();

        public Boolean Selected { get => m_selected; set => m_selected = value; }
        public Boolean GotColor { get => m_gotColor; set => m_gotColor = value; }
        public static Color SelectedColor { get => m_selectedColor; set => m_selectedColor = value; }

        public MyColorBox(in Boolean darkMode)
        {
            InitializeComponent();

            m_selectedColor = new Color();
            this.m_selected = false;
            this.m_gotColor = false;
            this.id_numericRed.Maximum = m_maxNumber;
            this.id_numericBlue.Maximum = m_maxNumber;
            this.id_numericGreen.Maximum = m_maxNumber;

            if (darkMode)
            {
                Color kindofBlack = Color.FromArgb(255, 50, 50, 50);
                Color backgroundBlack = Color.FromArgb(255, 20, 20, 20);
                this.BackColor = kindofBlack;
                this.ForeColor = Color.WhiteSmoke;
            }
            else
            {
                this.BackColor = Color.WhiteSmoke;
                this.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// If the closing button was selected before the okay button was selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Id_mainWindow_Load(Object sender, EventArgs e) => FormClosing += WindowClosing;

        /// <summary>
        /// Closing processes
        /// </summary>
        private void WindowClosing(object sender, FormClosingEventArgs reason)
        {
            if (m_selected == false)
            {
                this.DialogResult = DialogResult.Cancel;
            }
            else if (m_gotColor == false)
            {
                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void Id_buttonR1C1_Click(Object sender, EventArgs e)
        {
            m_selected = false;
            m_selectedColor = id_buttonR1C1.BackColor;
            id_labelChosenColor.BackColor = m_selectedColor;
            id_numericRed.Value = id_labelChosenColor.BackColor.R;
            id_numericBlue.Value = id_labelChosenColor.BackColor.B;
            id_numericGreen.Value = id_labelChosenColor.BackColor.G;
            m_selected = true;
            m_gotColor = true;
        }
        private void Id_buttonR2C1_Click(Object sender, EventArgs e)
        {
            m_selected = false;
            m_selectedColor = id_buttonR2C1.BackColor;
            id_labelChosenColor.BackColor = m_selectedColor;
            id_numericRed.Value = id_labelChosenColor.BackColor.R;
            id_numericBlue.Value = id_labelChosenColor.BackColor.B;
            id_numericGreen.Value = id_labelChosenColor.BackColor.G;
            m_selected = true;
            m_gotColor = true;
        }
        private void Id_buttonR3C1_Click(Object sender, EventArgs e)
        {
            m_selected = false;
            m_selectedColor = id_buttonR3C1.BackColor;
            id_labelChosenColor.BackColor = m_selectedColor;
            id_numericRed.Value = m_selectedColor.R;
            id_numericBlue.Value = m_selectedColor.B;
            id_numericGreen.Value = m_selectedColor.G;
            m_selected = true;
            m_gotColor = true;
        }
        private void Id_buttonR4C1_Click(Object sender, EventArgs e)
        {
            m_selected = false;
            m_selectedColor = id_buttonR4C1.BackColor;
            id_labelChosenColor.BackColor = m_selectedColor;
            id_numericRed.Value = m_selectedColor.R;
            id_numericBlue.Value = m_selectedColor.B;
            id_numericGreen.Value = m_selectedColor.G;
            m_selected = true;
            m_gotColor = true;
        }
        private void Id_buttonR5C1_Click(Object sender, EventArgs e)
        {
            m_selected = false;
            m_selectedColor = id_buttonR5C1.BackColor;
            id_labelChosenColor.BackColor = m_selectedColor;
            id_numericRed.Value = m_selectedColor.R;
            id_numericBlue.Value = m_selectedColor.B;
            id_numericGreen.Value = m_selectedColor.G;
            m_selected = true;
            m_gotColor = true;
        }
        private void Id_buttonR6C1_Click(Object sender, EventArgs e)
        {
            m_selected = false;
            m_selectedColor = id_buttonR6C1.BackColor;
            id_labelChosenColor.BackColor = m_selectedColor;
            id_numericRed.Value = m_selectedColor.R;
            id_numericBlue.Value = m_selectedColor.B;
            id_numericGreen.Value = m_selectedColor.G;
            m_selected = true;
            m_gotColor = true;
        }
        private void Id_buttonR7C1_Click(Object sender, EventArgs e)
        {
            m_selected = false;
            m_selectedColor = id_buttonR7C1.BackColor;
            id_labelChosenColor.BackColor = m_selectedColor;
            id_numericRed.Value = m_selectedColor.R;
            id_numericBlue.Value = m_selectedColor.B;
            id_numericGreen.Value = m_selectedColor.G;
            m_selected = true;
            m_gotColor = true;
        }
        private void Id_buttonR8C1_Click(Object sender, EventArgs e)
        {
            m_selected = false;
            m_selectedColor = id_buttonR8C1.BackColor;
            id_labelChosenColor.BackColor = m_selectedColor;
            id_numericRed.Value = m_selectedColor.R;
            id_numericBlue.Value = m_selectedColor.B;
            id_numericGreen.Value = m_selectedColor.G;
            m_selected = true;
            m_gotColor = true;
        }

        private void Id_buttonR1C2_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR2C2_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR3C2_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR4C2_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR5C2_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR6C2_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR7C2_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR8C2_Click(Object sender, EventArgs e)
        {

        }

        private void Id_buttonR1C3_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR2C3_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR3C3_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR4C3_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR5C3_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR6C3_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR7C3_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR8C3_Click(Object sender, EventArgs e)
        {

        }

        private void Id_buttonR1C4_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR2C4_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR3C4_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR4C4_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR5C4_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR6C4_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR7C4_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR8C4_Click(Object sender, EventArgs e)
        {

        }

        private void Id_buttonR1C5_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR2C5_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR3C5_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR4C5_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR5C5_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR6C5_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR7C5_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR8C5_Click(Object sender, EventArgs e)
        {

        }

        private void Id_buttonR1C6_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR2C6_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR3C6_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR5C6_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR6C6_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR7C6_Click(Object sender, EventArgs e)
        {

        }
        private void Id_buttonR8C6_Click(Object sender, EventArgs e)
        {

        }

        private void Id_numericRed_ValueChanged(Object sender, EventArgs e)
        {
            if (m_selected)
            {
                Color newColor = Color.FromArgb((Int32)id_numericRed.Value,
                                                (Int32)id_numericGreen.Value,
                                                (Int32)id_numericBlue.Value);
                m_selectedColor = newColor;
                id_labelChosenColor.BackColor = m_selectedColor;
                if (m_customSelected)
                {
                    m_selectedButton.BackColor = m_selectedColor;
                }
            }
        }
        private void Id_numericBlue_ValueChanged(Object sender, EventArgs e)
        {
            if (m_selected)
            {
                Color newColor = Color.FromArgb((Int32)id_numericRed.Value,
                                                (Int32)id_numericGreen.Value,
                                                (Int32)id_numericBlue.Value);
                m_selectedColor = newColor;
                id_labelChosenColor.BackColor = m_selectedColor;
                if (m_customSelected)
                {
                    m_selectedButton.BackColor = m_selectedColor;
                }
            }
        }
        private void Id_numericGreen_ValueChanged(Object sender, EventArgs e)
        {
            if (m_selected)
            {
                Color newColor = Color.FromArgb((Int32)id_numericRed.Value,
                                                (Int32)id_numericGreen.Value,
                                                (Int32)id_numericBlue.Value);
                m_selectedColor = newColor;
                id_labelChosenColor.BackColor = m_selectedColor;
                if (m_customSelected)
                {
                    m_selectedButton.BackColor = m_selectedColor;
                }
            }
        }

        private void Id_buttonSelectColor_Click(Object sender, EventArgs e)
        {
            if (m_selectedColor.IsEmpty)
            {
                m_selected = false;
                m_gotColor = false;
            }
            else
            {
                m_selected = true;
                m_gotColor = true;
                this.Close();
            }
        }

        private void id_buttonCustom1_Click(Object sender, EventArgs e)
        {
            m_selected = false;
            m_customSelected = true;
            m_selectedColor = id_buttonCustom1.BackColor;
            m_selectedButton = id_buttonCustom1;
            id_labelChosenColor.BackColor = m_selectedColor;
            id_numericRed.Value = id_labelChosenColor.BackColor.R;
            id_numericBlue.Value = id_labelChosenColor.BackColor.B;
            id_numericGreen.Value = id_labelChosenColor.BackColor.G;
            m_selected = true;
            m_gotColor = true;
        }
    }
}
