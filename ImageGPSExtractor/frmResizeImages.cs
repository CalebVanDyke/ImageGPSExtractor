using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageGPSExtractor
{
    public partial class frmResizeImages : Form
    {
        private List<string> _fileNames = new List<string>();

        public frmResizeImages(List<string> filenames)
        {
            InitializeComponent();

            this.progressBar1.Visible = false;

            _fileNames.AddRange(filenames);           
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.folderBrowserDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                this.txtSaveTo.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void ResizeImage(string filename, string dest)
        {
            if (!Directory.Exists(dest))
            {
                MessageBox.Show("The destination directory does not exist.");
                return;
            }

            int resizeToValue;
            bool result = Int32.TryParse(this.txtResizeToValue.Text, out resizeToValue);
            if (result == false || resizeToValue < 0 || resizeToValue > 4000)
            {
                MessageBox.Show("Please enter a valid number greater than zero and less than 4000 for max size.");
                return;
            }

            Image image = Image.FromFile(filename);
            if (image.Width > resizeToValue && image.Height > resizeToValue)
            {
                int originalWidth = image.Width;
                int originalHeight = image.Height;
                float percentWidth = (float)resizeToValue / (float)originalWidth;
                float percentHeight = (float)resizeToValue / (float)originalHeight;
                float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
                int newWidth = (int)(originalWidth * percent);
                int newHeight = (int)(originalHeight * percent);

                Image newImage = new Bitmap(newWidth, newHeight);
                using (Graphics graphicsHandle = Graphics.FromImage(newImage))
                {
                    graphicsHandle.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
                }
                image.Dispose();

                int compression = 75; // A value form 1 - 100
                EncoderParameters eps = new EncoderParameters(1);
                eps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, compression);
                ImageCodecInfo ici = GetEncoderInfo("image/jpeg");

                string destFilename = Path.Combine(dest, Path.GetFileNameWithoutExtension(filename) + "_thumb.jpg");
                newImage.Save(destFilename, ici, eps);
                newImage.Dispose();
            }
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        private void btnResize_Click(object sender, EventArgs e)
        {
            this.progressBar1.Maximum = _fileNames.Count();
            this.progressBar1.Value = 0;
            this.progressBar1.Visible = true;

            TaskScheduler taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Factory.StartNew(async () =>
            {
                foreach (string fileName in _fileNames)
                {
                    await Task.Delay(500);
                    ResizeImage(fileName, this.txtSaveTo.Text);

                    var token = Task.Factory.CancellationToken;
                    Task reportProgressTask = Task.Factory.StartNew(() =>
                    {
                        this.progressBar1.Value += 1;

                        if (this.progressBar1.Value == this.progressBar1.Maximum)
                            this.progressBar1.Visible = false;

                    }, token, TaskCreationOptions.None, taskScheduler);
                    reportProgressTask.Wait();
                }
            });
        }        
    }
}
