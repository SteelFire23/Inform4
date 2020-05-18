using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace Inform4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog files = new FolderBrowserDialog();
            if (files.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                DirectoryInfo Dir = new DirectoryInfo(files.SelectedPath);
                FileInfo[] first = Dir.GetFiles();
                int count = 0;
                for (int i = 0; i < first.Length; i++)
                {
                    if (ImgTest(first[i]) != -1)
                    {
                        FillData(first[i], ref dataGridView1, ref dataGridView2, count);
                        count++;
                    }
                }
            }
        }
        public static int ImgTest(FileInfo file)
        {
            string[] gf = new string[] { "png", "jpg", "bmp", "jpeg" };
            for (int i = 0; i < gf.Length; i++)
            {
                if (file.Name.Contains(gf[i])) return 1;
            }
            return -1;
        }
        public static void FillData(FileInfo S, ref DataGridView dataGridView1, ref DataGridView dataGridView2, int i)
        {
            dataGridView1.Rows.Add();
            dataGridView2.Rows.Add();
            Bitmap picture = new Bitmap(S.FullName);
            dataGridView1[0, i].Value = S.Name;
            dataGridView1[1, i].Value = picture.Width.ToString();
            dataGridView1[2, i].Value = picture.Height.ToString();
            dataGridView1[3, i].Value = "Не заданно";
            dataGridView2[0, i].Value = picture;

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dataGridView2.CurrentCell.RowIndex;
            pictureBox1.Image = (Image)dataGridView2[0, i].Value;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView1.Rows.Add();
            dataGridView2.Rows.Add();
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                Bitmap img = new Bitmap(file.FileName);
                dataGridView2[0, 0].Value = (Image)img;
                dataGridView1[0, 0].Value = CorrectName(file.FileName);
                dataGridView1[1, 0].Value = img.Width.ToString();
                dataGridView1[2, 0].Value = img.Height.ToString();
                dataGridView1[3, 0].Value = "Не заданно";
            }
        }
        public static string CorrectName(string S)
        {
            char[] A = S.ToCharArray();
            string tmp = "";
            for (int i = A.Length - 1; i >= 0; i--)
            {
                if (A[i].ToString() != @"\")
                {
                    tmp += A[i];
                }
                else { break; }
            }
            return Rev(tmp);
        }
        public static string Rev(string S)
        {
            char[] A = S.ToCharArray();
            Array.Reverse(A);
            string tmp = "";
            for (int i = 0; i < A.Length; i++)
            {
                tmp += A[i];
            }
            return tmp;
        }

        private void copyrightTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 forma = new Form2();
            forma.Owner = this;
            forma.ShowDialog();
            toolStripMenuItem1.Text = forma.textBox1.Text;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void copyrightDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dir = new FolderBrowserDialog();
            if (dir.ShowDialog() == DialogResult.OK)
            {
                toolStripMenuItem2.Text = dir.SelectedPath;
            }
        }

        private void addCopyrightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCopyRight(ref dataGridView2, ref dataGridView1, pictureBox1.Image, toolStripMenuItem1);
        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveImage(pictureBox1.Image);
        }

        private void batchModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                Image img = (Image)dataGridView2[0, i].Value;
                string path = toolStripMenuItem2.Text + @"\";
                AddCopyRight(ref dataGridView2, ref dataGridView1, img, toolStripMenuItem1);
                img.Save(path + Guid.NewGuid() + ".jpeg", ImageFormat.Jpeg);
            }
        }
        public static void AddCopyRight(ref DataGridView dataGridView2, ref DataGridView dataGridView1, Image img, ToolStripMenuItem toolStripMenuItem1)
        {
            if (toolStripMenuItem1.Text != null)
            {
                Graphics G = Graphics.FromImage(img);
                G.DrawString(toolStripMenuItem1.Text, new Font("Verdana", (float)15), new SolidBrush(Color.Red), 15, img.Height / 2);

                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    if (dataGridView2[0, i].Value == img)
                    {
                        dataGridView2[1, i].Value = Properties.Resources.Check;
                        dataGridView1[3, i].Value = toolStripMenuItem1.Text + " [ " + DateTime.Now.ToShortTimeString() + " ]";
                    }
                }
            }
            else { MessageBox.Show("Введите текст копирайта"); }
        }
        public static void SaveImage(Image img)
        {
            try
            {
                SaveFileDialog file = new SaveFileDialog();
                file.Filter = "jpeg(*.jpeg) | *.jpeg";
                if (file.ShowDialog() == DialogResult.OK)
                {
                    img.Save(file.FileName, ImageFormat.Jpeg);
                }
            }
            catch { MessageBox.Show("Выберите изображение для сохранения !"); }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Delete)
            {
                int i = dataGridView2.CurrentRow.Index;
                if (i != -1) dataGridView2.Rows.RemoveAt(i);
            }
        }
    }
}
    

