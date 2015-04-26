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
using System.Threading;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters;

namespace ImageGPSExtractor
{
    public partial class frmMain : Form
    {
        //private List<ImageGpsMetaData> _fileMetaData = new List<ImageGpsMetaData>();
        private List<string> _fileNames = new List<string>();

        public frmMain()
        {
            InitializeComponent();
            InitializeOpenFileDialog();

            this.listView1.Columns[1].Width = this.listView1.Width - this.listView1.Columns[0].Width;
        }

        private void InitializeOpenFileDialog()
        {
            this.openFileDialog1.Filter = "Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF| All files (*.*)|*.*";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "Chose Images";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                int count = this.listView1.Items.Count;
                foreach (String fileName in this.openFileDialog1.FileNames)
                {
                    if (this.chkBoxDuplicates.Checked == true)
                    {
                        if (_fileNames.Contains(fileName))
                        {
                            continue;
                        }
                    }

                    count++;
                    ListViewItem item = this.listView1.Items.Add(count.ToString());
                    item.SubItems.Add(fileName);
                    _fileNames.Add(fileName);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this._fileNames.Clear();
            this.listView1.Items.Clear();
        }

        private void btnGetGpsMetaData_Click(object sender, EventArgs e)
        {
            frmGpsMetaData form = new frmGpsMetaData(_fileNames);
            form.Show();
        }

        private void btnResizeImages_Click(object sender, EventArgs e)
        {
            frmResizeImages form = new frmResizeImages(_fileNames);
            form.Show();
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    List<string> paths = new List<string>();
        //    foreach (ListViewItem item in this.listView1.Items)
        //    {
        //        paths.Add(item.Text);
        //    }
        //    this.progressBar1.Maximum = paths.Count();
        //    this.progressBar1.Value = 0;

        //    TaskScheduler taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        //    Task.Factory.StartNew(() =>
        //    {
        //        foreach (string path in paths)
        //        {
        //            if (!File.Exists(path))
        //                continue;

        //            Image image = Image.FromFile(path);
        //            GpsMetaData gpsData = image.GetGpsInfo();
        //            image.Dispose();

        //            ImageGpsMetaData imgGpsMetaData = new ImageGpsMetaData();
        //            imgGpsMetaData.Altitude = gpsData.Altitude;
        //            imgGpsMetaData.Latitude = gpsData.Latitude;
        //            imgGpsMetaData.Longitude = gpsData.Longitude;
        //            imgGpsMetaData.Timestamp = gpsData.Timestamp;
        //            imgGpsMetaData.Path = path;
                    
        //            _fileMetaData.Add(imgGpsMetaData);

        //            var token = Task.Factory.CancellationToken;
        //            Task reportProgressTask = Task.Factory.StartNew(() =>
        //            {
        //                this.progressBar1.Value += 1;
        //                ListViewItem item = this.listView1.FindItemWithText(path);
        //                if (gpsData.Latitude != Double.MinValue && gpsData.Longitude != Double.MinValue)
        //                {
        //                    ListViewItem.ListViewSubItem subItemLat = item.SubItems[1];
        //                    subItemLat.Text = gpsData.Latitude.ToString();
        //                    ListViewItem.ListViewSubItem subItemLong = item.SubItems[2];
        //                    subItemLong.Text = gpsData.Longitude.ToString();
        //                }
        //                if (gpsData.Altitude != Double.MinValue)
        //                {
        //                    ListViewItem.ListViewSubItem subItemAlt = item.SubItems[3];
        //                    subItemAlt.Text = gpsData.Altitude.ToString();
        //                }
        //                if (gpsData.Timestamp != DateTime.MinValue)
        //                {
        //                    ListViewItem.ListViewSubItem subItemGpsDateTime = item.SubItems[4];
        //                    subItemGpsDateTime.Text = gpsData.Timestamp.ToString("MM/dd/yyyy hh:mm:ss");
        //                }
        //            }, token, TaskCreationOptions.None, taskScheduler);
        //            reportProgressTask.Wait();
        //        }
        //    });
        //}

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    var test2 = JsonConvert.SerializeObject(_fileMetaData, Formatting.Indented);
        //}

        //private void button6_Click(object sender, EventArgs e)
        //{
        //    DialogResult dr = this.folderBrowserDialog1.ShowDialog();
        //    if (dr == System.Windows.Forms.DialogResult.OK)
        //    {
        //        this.textBox1.Text = this.folderBrowserDialog1.SelectedPath;
        //    }
        //}

        //private void button5_Click(object sender, EventArgs e)
        //{
        //    if (_fileMetaData.Count == 0)
        //    {
        //        MessageBox.Show("There are no images to resize");
        //        return;
        //    }

        //    foreach (ImageGpsMetaData data in _fileMetaData)
        //    {
        //        ResizeImage(data.Path, this.textBox1.Text);
        //    }
        //}
    }
}
