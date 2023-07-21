using System.Drawing.Imaging;
using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Windows.Forms;

namespace 图片读写
{
    public partial class Form1 : Form
    {
        private List<string> ImageFiles = new();
        private List<string> OtherFiles = new();

        public Form1()
        {
            InitializeComponent();
        }

        //private void FiledataReader(string DataFile)
        //{
        //    using (FileStream fs = new FileStream(DataFile, FileMode.Open, FileAccess.Read))
        //    {
        //        BinaryReader reader = new BinaryReader(fs);

        //        int j=0;
        //        int iSize = 0;

        //        // 读取图片数据
        //        while (reader.PeekChar() != -1)  // 当还有数据可读时
        //        {

        //            int   imageSize = reader.ReadInt32();    // 读取图片大小
        //                                                   //  imageData 是一个 byte[] 类型的变量，它包含了图片的数据
        //          byte[] imageData = reader.ReadBytes(imageSize);  // 读取图片数据
        //            if (imageData.Length >= 8 && imageData[0] == 0x89 && imageData[1] == 0x50 && imageData[2] == 0x4E && imageData[3] == 0x47 &&
        //           imageData[4] == 0x0D && imageData[5] == 0x0A && imageData[6] == 0x1A && imageData[7] == 0x0A)
        //            {

        //                using (MemoryStream ms = new MemoryStream(imageData))
        //                {
        //                    Image image = Image.FromStream(ms);

        //                    // 在这里显示图片
        //                    pictureBox1.Image = image;
        //                    j = j+1;
        //                    // break;
        //                }                  


        //            }
        //            // 检查 JPEG 文件的魔术数字
        //            else if (imageData.Length >= 2 && imageData[0] == 0xFF && imageData[1] == 0xD8)
        //            {

        //                using (MemoryStream ms = new MemoryStream(imageData))
        //                {
        //                    Image image = Image.FromStream(ms);

        //                    // 在这里显示图片
        //                    pictureBox1.Image = image;
        //                    j = j+1;
        //                    // break;
        //                }



        //            }
        //            else 
        //            { 
        //            }

        //            iSize = iSize + imageSize;
        //            MessageBox.Show($"读取到第{j}张图片，大小为 {imageSize} 字节");
        //        }

        //        MessageBox.Show($"读取到{j}张图片，大小为 {iSize} 字节");


        //        MessageBox.Show("所有数据已从流文件中读取");
        //        reader.Close();
        //    }
        //}

        private void FiledataReader(string DataFile)
        {
            using (FileStream fs = new FileStream(DataFile, FileMode.Open, FileAccess.Read))
            {
                BinaryReader reader = new BinaryReader(fs);

                // 创建一个 Panel 控件
                Panel panel = new Panel();
                panel.Location = new Point(10, 10);
                panel.Size = new Size(500, 500);
                panel.AutoScroll = true;
                this.Controls.Add(panel);

                // 读取图片数据
                int i = 0;
                int x = 0;
                int y = 0;
                while (reader.PeekChar() != -1)  // 当还有数据可读时
                {
                    int imageIndex = i + 1; // 记录图片顺序
                                            // 记录文件位置

                    int imageSize = reader.ReadInt32();  // 读取图片大小
                    byte[] imageData = reader.ReadBytes(imageSize);  // 读取图片数据

                    // 检查 PNG 文件的魔术数字
                    if (imageData.Length >= 8 && imageData[0] == 0x89 && imageData[1] == 0x50 && imageData[2] == 0x4E && imageData[3] == 0x47 &&
                        imageData[4] == 0x0D && imageData[5] == 0x0A && imageData[6] == 0x1A && imageData[7] == 0x0A)
                    {
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            Image image = Image.FromStream(ms);

                            // 在这里显示图片
                            PictureBox pictureBox = new PictureBox();
                            pictureBox.Image = image;
                            pictureBox.Location = new Point(x, y);
                            pictureBox.Size = new Size(100, 100);
                            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox.Click += PictureBox_Click;
                            panel.Controls.Add(pictureBox);
                            i++;
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
                    // 检查 JPEG 文件的魔术数字
                    else if (imageData.Length >= 2 && imageData[0] == 0xFF && imageData[1] == 0xD8)
                    {
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            Image image = Image.FromStream(ms);

                            // 在这里显示图片
                            PictureBox pictureBox = new PictureBox();
                            pictureBox.Image = image;
                            pictureBox.Location = new Point(x, y);
                            pictureBox.Size = new Size(100, 100);
                            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox.Click += PictureBox_Click;
                            panel.Controls.Add(pictureBox);
                            i++;
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
                    else
                    {
                        break;
                    }
                }

                MessageBox.Show("所有数据已从流文件中读取");
                reader.Close();
            }
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

                // 写入图片数据
                foreach (string imageFile in imageFiles)
                {
                    byte[] imageData = File.ReadAllBytes(imageFile);

                    writer.Write(imageData.Length);  // 写入图片大小
                    writer.Write(imageData);         // 写入图片数据

                    MessageBox.Show($"图片文件 {imageFile} 已写入");
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

            //  DataFile = "output.dat";


        }
        string DataFile = "output.dat";
        private void button2_Click(object sender, EventArgs e)
        {

            FiledataReader(DataFile);
        }
    }
}