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
using System.Xml;

namespace ImageGPSExtractor
{
    public partial class frmGpsMetaData : Form
    {
        private List<string> _fileNames = new List<string>();
        private List<ImageGpsMetaData> _imageGpsMetaData = new List<ImageGpsMetaData>();

        public frmGpsMetaData(List<string> filenames)
        {
            InitializeComponent();
            InitializeSaveFileDialog();

            this.progressBar1.Visible = false;
            this.listView1.Columns[0].Width = this.listView1.Width / 2;
            this.listView1.Columns[1].Width = this.listView1.Width / 8;
            this.listView1.Columns[2].Width = this.listView1.Width / 8;
            this.listView1.Columns[3].Width = this.listView1.Width / 8;
            this.listView1.Columns[3].Width = this.listView1.Width / 8;

            _fileNames.AddRange(filenames);
            GetGpsData();
        }

        private void InitializeSaveFileDialog()
        {
            this.saveFileDialog1.Filter = "Text (*.txt)|*.txt";
            this.saveFileDialog1.Title = "Chose Text File";
        }

        private void InitializeOpenFileDialog()
        {
            this.saveFileDialog1.Filter = "Text (*.txt)|*.txt";
            this.saveFileDialog1.Title = "Chose Text File";
        }

        private void GetGpsData()
        {
            this.progressBar1.Maximum = _fileNames.Count();
            this.progressBar1.Value = 0;
            this.progressBar1.Visible = true;

            TaskScheduler taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Factory.StartNew(() =>
            {
                foreach (string filename in _fileNames)
                {
                    if (!File.Exists(filename))
                        continue;

                    Image image = Image.FromFile(filename);
                    GpsMetaData gpsData = image.GetGpsInfo();
                    image.Dispose();

                    ImageGpsMetaData imgGpsMetaData = new ImageGpsMetaData();
                    imgGpsMetaData.Altitude = gpsData.Altitude;
                    imgGpsMetaData.Latitude = gpsData.Latitude;
                    imgGpsMetaData.Longitude = gpsData.Longitude;
                    imgGpsMetaData.Timestamp = gpsData.Timestamp;
                    imgGpsMetaData.FileName = filename;
                    _imageGpsMetaData.Add(imgGpsMetaData);

                    var token = Task.Factory.CancellationToken;
                    Task reportProgressTask = Task.Factory.StartNew(() =>
                    {
                        this.progressBar1.Value += 1;
                        ListViewItem item = this.listView1.Items.Add(filename);

                        if (gpsData.Latitude != Double.MinValue && gpsData.Longitude != Double.MinValue)
                        {
                            item.SubItems.Add(gpsData.Latitude.ToString());
                            item.SubItems.Add(gpsData.Longitude.ToString());
                        }

                        if (gpsData.Altitude != Double.MinValue)
                        {
                            item.SubItems.Add(gpsData.Altitude.ToString());
                        }

                        if (gpsData.Timestamp != DateTime.MinValue)
                        {
                            item.SubItems.Add(gpsData.Timestamp.ToString("MM/dd/yyyy hh:mm:ss"));
                        }

                        if (this.progressBar1.Value == this.progressBar1.Maximum)
                            this.progressBar1.Visible = false;

                    }, token, TaskCreationOptions.None, taskScheduler);
                    reportProgressTask.Wait();
                }                
            });
        }

        private void CreateGpxData()
        {
            this.progressBar1.Maximum = _imageGpsMetaData.Count() * 2;
            this.progressBar1.Value = 0;
            this.progressBar1.Visible = true;

            TaskScheduler taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Factory.StartNew(async () =>
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                string outputFileName = txtSaveTo.Text;
                XmlWriter xmlWriter = XmlWriter.Create(outputFileName, settings);

                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("gpx");

                foreach (ImageGpsMetaData data in _imageGpsMetaData)
                {
                    await Task.Delay(500);

                    xmlWriter.WriteStartElement("wpt");
                    xmlWriter.WriteAttributeString("lat", data.Latitude.ToString());
                    xmlWriter.WriteAttributeString("lon", data.Longitude.ToString());
                    xmlWriter.WriteElementString("ele", data.Altitude.ToString());
                    xmlWriter.WriteElementString("name", Path.GetFileNameWithoutExtension(data.FileName));
                    xmlWriter.WriteElementString("time", data.Timestamp.ToString("MM/dd/yyyy hh:mm:ss"));
                    xmlWriter.WriteElementString("sym", "trail head");
                    xmlWriter.WriteElementString("thumbnail", @"http://www.calebvandyke.com/utah2013/" + Path.GetFileName(data.FileName) + "_thumb.jpg");
                    xmlWriter.WriteElementString("url", @"http://www.calebvandyke.com/utah2013/" + Path.GetFileName(data.FileName));
                    xmlWriter.WriteEndElement();

                    var token = Task.Factory.CancellationToken;
                    Task reportProgressTask = Task.Factory.StartNew(() =>
                    {
                        this.progressBar1.Value += 1;

                        if (this.progressBar1.Value == this.progressBar1.Maximum)
                            this.progressBar1.Visible = false;


                    }, token, TaskCreationOptions.None, taskScheduler);
                    reportProgressTask.Wait();
                }

                xmlWriter.WriteStartElement("trk");
                xmlWriter.WriteElementString("name", "trackname");
                xmlWriter.WriteStartElement("trkseg");

                foreach (ImageGpsMetaData data in _imageGpsMetaData)
                {
                    await Task.Delay(500);

                    xmlWriter.WriteStartElement("trkpt");
                    xmlWriter.WriteAttributeString("lat", data.Latitude.ToString());
                    xmlWriter.WriteAttributeString("lon", data.Longitude.ToString());
                    xmlWriter.WriteElementString("ele", data.Altitude.ToString());
                    xmlWriter.WriteElementString("name", Path.GetFileName(data.FileName));
                    xmlWriter.WriteElementString("time", data.Timestamp.ToString("MM/dd/yyyy hh:mm:ss"));
                    xmlWriter.WriteEndElement();

                    var token = Task.Factory.CancellationToken;
                    Task reportProgressTask = Task.Factory.StartNew(() =>
                    {
                        this.progressBar1.Value += 1;

                        if (this.progressBar1.Value == this.progressBar1.Maximum)
                            this.progressBar1.Visible = false;

                        
                    }, token, TaskCreationOptions.None, taskScheduler);
                    reportProgressTask.Wait();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            });
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.saveFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                this.txtSaveTo.Text = this.saveFileDialog1.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateGpxData();
        }
    }
}
