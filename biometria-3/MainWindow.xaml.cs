using Microsoft.Win32;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace biometria_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bitmap? sourceImage = null;
        Bitmap? imageToEdit = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg;*.png)|*.jpg;*.png|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                imageToEdit = this.sourceImage = new Bitmap($"{fileName}");
                OriginalImage.Source = ImageSourceFromBitmap(this.sourceImage);
                /* histogramValues = Algorithm.getHistogramData(new Bitmap($"{fileName}"));
                 HistogramImage.Source = ImageSourceFromBitmap(Algorithm.Histogram(this.sourceImage.Width, this.sourceImage.Height, histogramValues));
                 int[] LUT = Algorithm.calculateLUT(histogramValues);
                 StretchedHistogram.Source = ImageSourceFromBitmap(Algorithm.StretchedHistogram(new Bitmap($"{fileName}"), LUT));*/ //sorki najmocniej, będziesz musiał poscalać z powrotem

                //HistogramImage.Source = ImageSourceFromBitmap(Algorithm.Histogram(new Bitmap($"{fileName}"), histogramValues));

            }
        }

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]

        public static extern bool DeleteObject([In] IntPtr hObject);

        public ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        private void Niblack_Click(object sender, RoutedEventArgs e)
        {
            if (sourceImage == null)
            {
                MessageBox.Show("You haven't uploaded any files", "Image error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Bitmap bitmap = new Bitmap(this.sourceImage.Width, this.sourceImage.Height);
            bitmap = (Bitmap)this.imageToEdit.Clone();
            ReadyImage.Source = ImageSourceFromBitmap(Algorithm.Niblack(bitmap, Range.Value/10-1));
        }

        private void Sauvola_Click(object sender, RoutedEventArgs e)
        {
            if (sourceImage == null)
            {
                MessageBox.Show("You haven't uploaded any files", "Image error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Bitmap bitmap = new Bitmap(this.sourceImage.Width, this.sourceImage.Height);
            bitmap = (Bitmap)this.imageToEdit.Clone();
            ReadyImage.Source = ImageSourceFromBitmap(Algorithm.Sauvola(bitmap, Range.Value / 10 - 1));
        }

        private void Phansalkar_Click(object sender, RoutedEventArgs e)
        {
            if (sourceImage == null)
            {
                MessageBox.Show("You haven't uploaded any files", "Image error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Bitmap bitmap = new Bitmap(this.sourceImage.Width, this.sourceImage.Height);
            bitmap = (Bitmap)this.imageToEdit.Clone();
            ReadyImage.Source = ImageSourceFromBitmap(Algorithm.Phansalkar(bitmap, Range.Value / 10 - 1));
        }

        private void Kapur_Click(object sender, RoutedEventArgs e)
        {
            //Brak mi wiedzy na rekursję
            if (sourceImage == null)
            {
                MessageBox.Show("You haven't uploaded any files", "Image error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Bitmap bitmap = new Bitmap(this.sourceImage.Width, this.sourceImage.Height);
            bitmap = (Bitmap)this.imageToEdit.Clone();
            ReadyImage.Source = ImageSourceFromBitmap(Algorithm.MidGrey(bitmap));
        }

        private void LuWu_Click(object sender, RoutedEventArgs e)
        {
            if (sourceImage == null)
            {
                MessageBox.Show("You haven't uploaded any files", "Image error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Bitmap bitmap = new Bitmap(this.sourceImage.Width, this.sourceImage.Height);
            bitmap = (Bitmap)this.imageToEdit.Clone();
            ReadyImage.Source = ImageSourceFromBitmap(Algorithm.Median(bitmap));
        }

        private void Bernsen_Click(object sender, RoutedEventArgs e)
        {
            if (sourceImage == null)
            {
                MessageBox.Show("You haven't uploaded any files", "Image error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Bitmap bitmap = new Bitmap(this.sourceImage.Width, this.sourceImage.Height);
            bitmap = (Bitmap)this.imageToEdit.Clone();
            ReadyImage.Source = ImageSourceFromBitmap(Algorithm.Bernsen(bitmap, (int)Range.Value, (int)Limit.Value));

        }

        private void Limit_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }
    }
}
