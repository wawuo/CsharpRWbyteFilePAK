using System.Drawing.Imaging;
using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Windows.Forms;

namespace 图片读写
{
    public partial class Form1 : Form
    {
       // private Panel panel; // 添加 panel 成员变量
        private List<string> ImageFiles = new();
        private List<string> OtherFiles = new();
        string DataFile = "output.dat";
        private Panel Panel;

        public Form1()
        {
            InitializeComponent();
        }
        private void CreatePanel()
        {
            // 创建一个 Panel 控件
            Panel = new Panel(); // 使用类的成员变量 panel
            Panel.Location = new Point(10, 10);
            Panel.Size = new Size(500, 500);
            Panel.AutoScroll = true;
           // Panel.Controls.Clear();
            this.Controls.Add(Panel);
        }
        private void FiledataReader(string DataFile)
        {
            if (Panel != null) // 检查 panel 是否已经初始化
            {
                // 清除 Panel 控件中的所有控件
                Panel.Controls.Clear();

                // 从窗体的 Controls 集合中移除旧的 Panel 控件
                this.Controls.Remove(Panel);

                // 调用 CreatePanel 方法重新创建 Panel 控件
                CreatePanel();

            }
            ImageFiles.Clear();
            OtherFiles.Clear();
            using (FileStream fs = new FileStream(DataFile, FileMode.Open, FileAccess.Read))
            {
                BinaryReader reader = new BinaryReader(fs);

                // 创建一个 Panel 控件
                CreatePanel();

                // 读取图片数据
                int i = 0;
                int x = 0;
                int y = 0;
                while (reader.PeekChar() != -1)  // 当还有数据可读时
                {
                    int imageIndex = i + 1; // 记录图片顺序
                    int j = 1;                      // 记录文件位置
                    int imageSize = reader.ReadInt32();  // 读取图片大小
                    byte[] imageData = reader.ReadBytes(imageSize);  // 读取图片数据
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        Image image = Image.FromStream(ms);
                        MessageBox.Show($"读取到{j}张图片，大小为 {imageSize} 字节");
                        // 在这里显示图片
                        PictureBox pictureBox = new PictureBox();
                        pictureBox.Image = image;
                        pictureBox.Location = new Point(x, y);
                        pictureBox.Size = new Size(100, 100);
                        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox.Click += PictureBox_Click;
                        Panel.Controls.Add(pictureBox);
                        j++;
                        x += pictureBox.Width + 10;
                        if (x + pictureBox.Width > Panel.Width)
                        {
                            x = 0;
                            y += pictureBox.Height + 10;
                        }
                        Label label = new Label();
                        label.Text = $" 文件位置: {imageIndex}"; // 显示图片大小和文件位置
                        label.Location = new Point(pictureBox.Location.X, pictureBox.Location.Y + pictureBox.Height);
                        label.AutoSize = true;
                        Panel.Controls.Add(label);

                    }
   
                }

                MessageBox.Show("所有数据已从流文件中读取");
                reader.Close();
            }
        }
        //private void FiledataReader(string DataFile)
        //{
        //    if (Panel != null) // 检查 panel 是否已经初始化
        //    {
        //        // 清除 Panel 控件中的所有控件
        //        Panel.Controls.Clear();

        //        // 从窗体的 Controls 集合中移除旧的 Panel 控件
        //        this.Controls.Remove(Panel);

        //        // 调用 CreatePanel 方法重新创建 Panel 控件
        //        CreatePanel();

        //    }
        //    ImageFiles.Clear();
        //    OtherFiles.Clear();
        //    using (FileStream fs = new FileStream(DataFile, FileMode.Open, FileAccess.Read))
        //    {
        //        BinaryReader reader = new BinaryReader(fs);

        //        // 创建一个 Panel 控件
        //        CreatePanel();

        //        // 读取图片数据
        //        int i = 0;
        //        int x = 0;
        //        int y = 0;
        //        while (reader.PeekChar() != -1)  // 当还有数据可读时
        //        {
        //            int imageIndex = i + 1; // 记录图片顺序
        //            int j = 1;                      // 记录文件位置

        //            int imageSize = reader.ReadInt32();  // 读取图片大小
        //            byte[] imageData = reader.ReadBytes(imageSize);  // 读取图片数据

        //            // 检查 PNG 文件的魔术数字
        //         // if (imageData.Length >= 8 && imageData[0] == 0x89 && imageData[1] == 0x50 && imageData[2] == 0x4E && imageData[3] == 0x47 &&
        //         //     imageData[4] == 0x0D && imageData[5] == 0x0A && imageData[6] == 0x1A && imageData[7] == 0x0A)
        //         // {
        //                using (MemoryStream ms = new MemoryStream(imageData))
        //                {
        //                    Image image = Image.FromStream(ms);

        //                    MessageBox.Show($"读取到{j}张图片，大小为 {imageSize} 字节");
        //                    // 在这里显示图片
        //                    PictureBox pictureBox = new PictureBox();
        //                    pictureBox.Image = image;
        //                    pictureBox.Location = new Point(x, y);
        //                    pictureBox.Size = new Size(100, 100);
        //                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        //                    pictureBox.Click += PictureBox_Click;
        //                    Panel.Controls.Add(pictureBox);
        //                    j++;
        //                    x += pictureBox.Width + 10;
        //                    if (x + pictureBox.Width > Panel.Width)
        //                    {
        //                        x = 0;
        //                        y += pictureBox.Height + 10;
        //                    }
        //                    Label label = new Label();
        //                    label.Text = $" 文件位置: {imageIndex}"; // 显示图片大小和文件位置
        //                    label.Location = new Point(pictureBox.Location.X, pictureBox.Location.Y + pictureBox.Height);
        //                    label.AutoSize = true;
        //                    Panel.Controls.Add(label);

        //                }
        //        //    }
        //            //// 检查 JPEG 文件的魔术数字
        //            //else if (imageData.Length >= 2 && imageData[0] == 0xFF && imageData[1] == 0xD8)
        //            //{
        //            //    using (MemoryStream ms = new MemoryStream(imageData))
        //            //    {
        //            //        Image image = Image.FromStream(ms);

        //            //        MessageBox.Show($"读取到{j}张图片，大小为 {imageSize} 字节");
        //            //        // 在这里显示图片
        //            //        PictureBox pictureBox = new PictureBox();
        //            //        pictureBox.Image = image;
        //            //        pictureBox.Location = new Point(x, y);
        //            //        pictureBox.Size = new Size(100, 100);
        //            //        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        //            //        pictureBox.Click += PictureBox_Click;
        //            //        Panel.Controls.Add(pictureBox);
        //            //        i++;
        //            //        x += pictureBox.Width + 10;
        //            //        if (x + pictureBox.Width > Panel.Width)
        //            //        {
        //            //            x = 0;
        //            //            y += pictureBox.Height + 10;
        //            //        }
        //            //        Label label = new Label();
        //            //        label.Text = $" 文件位置: {imageIndex}"; // 显示图片大小和文件位置
        //            //        label.Location = new Point(pictureBox.Location.X, pictureBox.Location.Y + pictureBox.Height);
        //            //        label.AutoSize = true;
        //            //        Panel.Controls.Add(label);


        //            //    }
        //            //}
        //            //else
        //            //{
        //            //    break;
        //            //}
        //        }

        //        MessageBox.Show("所有数据已从流文件中读取");
        //        reader.Close();
        //    }
        //} 

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

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 遍历 panel 中的所有子控件
            if (Panel != null) // 检查 panel 是否已经初始化
            {
                // 清除 Panel 控件中的所有控件
                Panel.Controls.Clear();

                // 从窗体的 Controls 集合中移除旧的 Panel 控件
                this.Controls.Remove(Panel);

                // 调用 CreatePanel 方法重新创建 Panel 控件
                CreatePanel();

            }
            ImageFiles.Clear();
            OtherFiles.Clear();
          //  FiledataReader = null;
            FiledataReader(DataFile);
        }
      

        private void button3_Click_1(object sender, EventArgs e)
        {
            // 遍历 panel 中的所有子控件
         

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择要读取的文件";
            openFileDialog.Filter = "二进制文件 (*.dat)|*.dat|所有文件 (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFile = openFileDialog.FileName;
                FiledataReader(selectedFile);
            }

        }
    }
}
