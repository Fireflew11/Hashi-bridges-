using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hashi__bridges_
{
    public partial class form1 : Form
    {
        public form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void EasyBtn_Click(object sender, EventArgs e)
        {
            GameWindow gameWindow = new GameWindow(6, 9);
            gameWindow.Show();
        }
        private void MediumBtn_Click(object sender, EventArgs e)
        {
            GameWindow gameWindow = new GameWindow(8, 12);
            gameWindow.Show();
        }
        private void HardBtn_Click(object sender, EventArgs e)
        {
            GameWindow gameWindow = new GameWindow(10, 15);
            gameWindow.Show();
        }
        private void HeavyBtn_Click(object sender, EventArgs e)
        {
            GameWindow gameWindow = new GameWindow(13, 18);
            gameWindow.Show();
        }
        private void InsaneBtn_Click(object sender, EventArgs e)
        {
            GameWindow gameWindow = new GameWindow(18, 21);
            gameWindow.Show();
        }
        private void HighScoresBtn_Click(object sender, EventArgs e)
        {
            TopScoresForm topScoresForm = new TopScoresForm();
            topScoresForm.Show();
        }

    }
}
