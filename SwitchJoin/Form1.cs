using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SwitchJoin
{
    public partial class Form1 : Form
    {

        
        
        public Form1()
        {
            InitializeComponent();
            
        }
        
        
        public static List<int> Orders = new List<int>();


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void beamorder_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void columnorder_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void wallorder_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void floororder_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Orders.Add(int.Parse(textBeam.Text));
            Orders.Add(int.Parse(textColumn.Text));
            Orders.Add(int.Parse(textWall.Text));
            Orders.Add(int.Parse(textFloor.Text));
            
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
