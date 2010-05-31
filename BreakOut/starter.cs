using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BreakOut
{
    public partial class starter : Form
    {
        public starter()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show("你确定退出经典的阿凡达大战春哥吗？","来自M78星云的询问",MessageBoxButtons.OKCancel,MessageBoxIcon.Question);
            if (dg == DialogResult.OK)
            {
                this.Dispose();
            }
        }

        private void starter_Load(object sender, EventArgs e)
        {
            Image bg=Resource1 .starter;
            starter st = (starter)sender;
            this.BackgroundImage = bg;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Size = bg.Size;
            button1.Location = new Point(st.Size .Width /2-button1 .Size .Width/2,st.Size.Height/3);
            button2.Location = new Point(button1.Location.X, button1.Location.Y + 50);
            button4.Location = new Point(button1.Location.X, button2.Location.Y + 50);
            button3.Location = new Point(button1.Location.X, button4.Location.Y + 50);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            help hp = new help();
            hp.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            us u = new us();
            u.ShowDialog();
        }
    }
}
