﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ObjRec.Core.Filters;
using ObjRec.Core.Filters.Sobel;
using Accord.Imaging;
using AForge.Imaging.Filters;

namespace ObjRec.UI
{
    public partial class MainWindow : Form
    {
        private readonly OpenFileDialog fileDialog = new OpenFileDialog {Filter = @"Picture Files (.bmp)|*.bmp|All Files (*.*)|*.*"};

        public MainWindow()
        {
            InitializeComponent();

            fileDialog.FileOk += (sender, args) =>
            {
                filenameTextbox.Text = fileDialog.FileName;

                LoadFile(filenameTextbox.Text);
            };
        }

        private void LoadFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    sourcePic.Image = Image.FromFile(fileName);
                    processedPic.Image = new Bitmap(sourcePic.Image);
                }
                catch (Exception)
                {
                    MessageBox.Show(
                        @"Invalid image format. Select another file",
                        @"Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            }

        private void loadFileButton_Click(object sender, System.EventArgs e)
        {
            LoadFile(filenameTextbox.Text);
        }

        private void filenameTextbox_Click(object sender, System.EventArgs e)
        {
            fileDialog.Reset();
            fileDialog.ShowDialog();
        }

        private async void OtsuFilter_Click(object sender, System.EventArgs e)
        {
            var filter = new ThresholdOtsuFilter();

            statusBarText.Text = @"Applying Otsu filter...";

            sourcePic.Image = new Bitmap(processedPic.Image);
            processedPic.Image = await filter.Apply(processedPic.Image);

            statusBarText.Text = $"Ready (Computed threshold : {filter.Threshold})";
        }

        private async void medianButton_Click(object sender, System.EventArgs e)
        {
            var filter = new MedianFilter();

            statusBarText.Text = @"Applying Median filter...";

            sourcePic.Image = new Bitmap(processedPic.Image);
            processedPic.Image = await filter.Apply(processedPic.Image);

            statusBarText.Text = @"Ready";
        }

        private async void sobelButton_Click(object sender, EventArgs e)
        {
            var filter = new SobelFilter();

            statusBarText.Text = @"Applying Sobel filter...";

            sourcePic.Image = new Bitmap(processedPic.Image);
            processedPic.Image = await filter.Apply(processedPic.Image);

            statusBarText.Text = @"Ready";
        }

        private async void sharpButton_Click(object sender, EventArgs e)
        {
            var filter = new SharpnessFilter();

            statusBarText.Text = @"Applying Sharpness filter...";

            sourcePic.Image = new Bitmap(processedPic.Image);
            processedPic.Image = await filter.Apply(processedPic.Image);

            statusBarText.Text = @"Ready";
        }

        private void harris_Click(object sender, EventArgs e)
         {
            // Open a image
            Bitmap image = new Bitmap(sourcePic.Image);

            double sigma = 1.4;
            float k = 0.04f;
            float threshold = 20000;
            // Create a new Harris Corners Detector using the given parameters
            HarrisCornersDetector harris = new HarrisCornersDetector(k)
            {
                Threshold = threshold,
                Sigma = sigma
            };

            // Create a new AForge's Corner Marker Filter
            CornersMarker corners = new CornersMarker(harris, Color.White);

            // Apply the filter and display it on a picturebox
            processedPic.Image = corners.Apply(image);
        }
    }
}
