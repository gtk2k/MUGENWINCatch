using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dnGREP;

namespace MUGENWINCatch
{
    public partial class frmSetting : Form
    {
        public frmSetting()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void btnLoiLoReference_Click(object sender, EventArgs e)
        {
            ofd.FileName = "LoiLoGameRecorder.exe";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtLoiLoPath.Text = ofd.FileName;
            }
        }

        private void btnLoiLoAVIOutputFolderReference_Click(object sender, EventArgs e)
        {
            var dlg = new FileFolderDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtLoiLoAVIOutputFolder.Text = dlg.SelectedPath;
            }
        }

        private void btnMUGENReference_Click(object sender, EventArgs e)
        {
            ofd.FileName = "mugen.exe";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtMUGENPath.Text = ofd.FileName;
            }
        }

        private void btnFFmpegReference_Click(object sender, EventArgs e)
        {
            ofd.FileName = "ffmeg.exe";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtFFmpegPath.Text = ofd.FileName;
            }
        }

        private void btnMP4OutputFolderReference_Click(object sender, EventArgs e)
        {
            var dlg = new FileFolderDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtMP4OutputFolder.Text = dlg.SelectedPath;
            }
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.loiloPath = txtLoiLoPath.Text;
            Properties.Settings.Default.aviPath = txtLoiLoAVIOutputFolder.Text;
            Properties.Settings.Default.mugenPath = txtMUGENPath.Text;
            Properties.Settings.Default.ffmpegPath = txtFFmpegPath.Text;
            Properties.Settings.Default.mp4Path = txtMP4OutputFolder.Text;
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLoiLoPath.Text) ||
                string.IsNullOrWhiteSpace(txtLoiLoAVIOutputFolder.Text) ||
                string.IsNullOrWhiteSpace(txtMUGENPath.Text) ||
                string.IsNullOrWhiteSpace(txtFFmpegPath.Text) ||
                string.IsNullOrWhiteSpace(txtMP4OutputFolder.Text))
            {
                btnReg.Enabled = false;
            }
            else
            {
                btnReg.Enabled = true;
            }
        }
    }
}
