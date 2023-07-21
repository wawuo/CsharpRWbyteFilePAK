using System.Drawing.Imaging;
using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Windows.Forms;

namespace 图片读写
{
    public partial class Form1 : Form
    {
        private List<string> ImageFiles = new();
        private List<string> OtherFiles = new();
        string DataFile = "output.dat";
        private Panel panel; // 将面板作为函数的成员变量
        public Form1()
        {
            InitializeComponent();
        }
        private void Form_Resize(object sender, EventArgs e)
        {
            // 计算 Panel 控件的新宽度
            int newWidth = this.ClientSize.Width - 30;
            int newHeight = this.ClientSize.Height - 600;

            // 更新 Panel 控件的宽度
            panel.Width = newWidth;
            panel.Height = newHeight;
        }

        private void FiledataReader(string DataFile)
        {
            if (panel == null)
            {
                panel = new Panel();
                panel.Location = new Point(10, 60);

                // 计算 Panel 控件的初始宽度和高度
                int initialWidth = this.ClientSize.Width - 30;
                int initialHeight = this.ClientSize.Height - 600;

                // 设置 Panel 控件的初始大小
                panel.Size = new Size(initialWidth, initialHeight);

                panel.AutoScroll = true;
                this.Resize += Form_Resize;
                this.Controls.Add(panel);
            }
            else
            {
                panel.Controls.Clear(); // 清空面板上的内容
            }

            ImageFiles.Clear();
            OtherFiles.Clear();
            using (FileStream fs = new FileStream(DataFile, FileMode.Open, FileAccess.Read))
            {
                BinaryReader reader = new BinaryReader(fs);

                int x = 0;
                int y = 0;
                int j = 0;

                while (reader.PeekChar() != -1)
                {
                    int imageIndex = j + 1; // 记录图片顺序
                    int imageSize = reader.ReadInt32();  // 读取图片大小
                    byte[] imageData = reader.ReadBytes(imageSize);  // 读取图片数据
                    UTF8Encoding utf8 = new UTF8Encoding();
                    int charCount = utf8.GetCharCount(imageData);
                    char[] chars = new char[charCount];
                    int charsDecodedCount = utf8.GetChars(imageData, 0, imageData.Length, chars, 0);


                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        Image image = Image.FromStream(ms);
                       // MessageBox.Show($"读取到{j}张图片，大小为 {imageSize} 字节");
                        // 在这里显示图片
                        PictureBox pictureBox = new PictureBox();
                        pictureBox.Image = image;
                        pictureBox.Location = new Point(x, y);
                        pictureBox.Size = new Size(100, 100);
                        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox.Click += PictureBox_Click;
                        panel.Controls.Add(pictureBox);
                        j = j + 1;
                        x += pictureBox.Width + 10;
                        if (x + pictureBox.Width > panel.Width)
                        {
                            x = 0;
                            y += pictureBox.Height + 10;
                        }
                        Label label = new Label();
                        label.Text = $" 文件位置: {imageIndex}"; // 显示图片大小和文件位置
                        label.Location = new Point(pictureBox.Location.X, pictureBox.Location.Y + pictureBox.Height);
                        label.AutoSize = true;
                        panel.Controls.Add(label);
                    }
                }

                reader.Close();
            }

            MessageBox.Show("所有数据已从流文件中读取");
        }


        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            if (pictureBox != null)
            {
                Image image = pictureBox.Image;
                if (image != null)
                {
                    // 在这里显示原始大小的图片
                    pictureBox1.Image = image;
                }
            }
        }

        private void WriteDataToFile(string DataFile, List<string> imageFiles, List<string> otherFiles)
        {
            using (FileStream fs = new FileStream(DataFile, FileMode.Create, FileAccess.Write))
            {
                BinaryWriter writer = new BinaryWriter(fs);
                foreach (string imageFile in imageFiles)
                {
                    byte[] imageData = File.ReadAllBytes(imageFile);

                    writer.Write(imageData.Length);
                    writer.Write(imageData);

                   // MessageBox.Show($"图片文件 {imageFile} 已写入");
                }

              

                writer.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ImageFiles.Clear();
            OtherFiles.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Browse Files";
            openFileDialog.Filter = "All Files (*.*)|*.*";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] selectedFiles = openFileDialog.FileNames;
                foreach (string selectedFile in selectedFiles)
                {
                    ImageFiles.Add(selectedFile);
                }
                WriteDataToFile(DataFile, ImageFiles, OtherFiles);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {


            ImageFiles.Clear();
            OtherFiles.Clear();
            //  FiledataReader = null;
            FiledataReader(DataFile);
        }


        private void button3_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择要读取的文件";
            openFileDialog.Filter = "二进制文件 (*.dat)|*.dat|所有文件 (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFile = openFileDialog.FileName;
                FiledataReader(selectedFile);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择要写入的图片文件";
            openFileDialog.Filter = "图片文件 (*.png;*.jpg)|*.png;*.jpg|所有文件 (*.*)|*.*";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] selectedFiles = openFileDialog.FileNames;

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "选择要保存的文件";
                saveFileDialog.Filter = "二进制文件 (*.dat)|*.dat|所有文件 (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFile = saveFileDialog.FileName;
                    WriteDataToFile(selectedFile, selectedFiles.ToList(), OtherFiles);
                    MessageBox.Show(selectedFile); 
                }
            }
        }


    }
}
