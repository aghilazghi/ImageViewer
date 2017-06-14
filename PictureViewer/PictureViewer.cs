using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PictureViewer
{
    public partial class FrmPictureviewer : Form
    {
        private bool _isImageLoaded;

        public FrmPictureviewer()
        {
            InitializeComponent();
        }

        private void frmPictureViewer_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.Bisque;
        }

        private void btnLoadPicture_Click(object sender, EventArgs e)
        {

            var dialog = new OpenFileDialog
            {
                Filter =
                    @"JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif",
                Title = @"Select an Image File"
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            pictureBox1.Load(dialog.FileName);
            _isImageLoaded = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = checkBox1.Checked ? 
                                    PictureBoxSizeMode.StretchImage : 
                                    PictureBoxSizeMode.Normal;
        }

        private void btnRotate_Click(object sender, EventArgs e)
        {
            if (!_isImageLoaded) return;

            var bitmap = (Bitmap) pictureBox1.Image;
            pictureBox1.Image = RotateImage(bitmap, 90.0f);
        }

        private static Bitmap RotateImage(Image bmp, float angle)
        {
            var w = bmp.Width;
            var h = bmp.Height;
            var tempImage = new Bitmap(w, h);
            var graphic = Graphics.FromImage(tempImage);

            graphic.DrawImageUnscaled(bmp, 1, 1);
            graphic.Dispose();

            var path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            var matrix = new Matrix();
            matrix.Rotate(angle);

            var rectangle = path.GetBounds(matrix);
            var newImage = new Bitmap(Convert.ToInt32(rectangle.Width), Convert.ToInt32(rectangle.Height));

            graphic = Graphics.FromImage(newImage);
            graphic.TranslateTransform(-rectangle.X, -rectangle.Y);
            graphic.RotateTransform(angle);
            graphic.InterpolationMode = InterpolationMode.HighQualityBilinear;
            graphic.DrawImageUnscaled(tempImage, 0, 0);
            graphic.Dispose();
            tempImage.Dispose();

            return newImage;
        }

       
    }
}
