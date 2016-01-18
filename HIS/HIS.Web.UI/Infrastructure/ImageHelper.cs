using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace HIS.Web.UI 
{
    public class ImageHelper
    {

        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public static byte[] CreateThumbnail(byte[] imageByte, bool maintainAspectRatio, int desiredWidth, int desiredHeight)
        {
            byte[] byteArray = new byte[0];
            Bitmap bmp;
            try
            {
                MemoryStream memStream = new MemoryStream(imageByte);
                System.Drawing.Image img = System.Drawing.Image.FromStream(memStream);

                if (maintainAspectRatio)
                {
                    AspectRatio aspectRatio = new AspectRatio();
                    aspectRatio.WidthAndHeight(img.Width, img.Height, desiredWidth, desiredHeight);
                    bmp = new Bitmap(img, aspectRatio.Width, aspectRatio.Height);
                }
                else
                {
                    bmp = new Bitmap(img, desiredWidth, desiredHeight);
                }
                byteArray = ToByteArray(bmp, ImageFormat.Jpeg);
                memStream.Dispose();
            }
            catch (Exception ex)
            {

            }
            return byteArray;
        }

        public static byte[] ToByteArray(Bitmap image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }

        public static string ToBase64StringThumbnail(byte[] imageByte, int desiredWidth, int desiredHeight)
        {

            if (imageByte.Length == 0)
                return String.Empty;

            byte[] thumbnail = CreateThumbnail(imageByte, true, desiredWidth, desiredHeight);
            string imageBase64 = Convert.ToBase64String(thumbnail);

            return imageBase64;
        }

        public static byte[] ReduceSize(byte[] imageByte)
        {
            byte[] byteArray = new byte[0];
            Bitmap bmp;

            try
            {
                using (MemoryStream memStream = new MemoryStream(imageByte))
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(memStream);

                    if (img.Width != 1024 || img.Height != 384)
                    {
                        AspectRatio aspectRatio = new AspectRatio();
                        aspectRatio.WidthAndHeight(img.Width, img.Height, 1024, 384);
                        bmp = new Bitmap(img, aspectRatio.Width, aspectRatio.Height);

                        byteArray = ToByteArray(bmp, ImageFormat.Jpeg);

                    }
                    else
                    {
                        byteArray = imageByte;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return byteArray;
        }
    }

    public class AspectRatio
    {
        public AspectRatio()
        {
        }
        private int d_Width = 0;
        private int d_Height = 0;
        public int Width
        {
            get { return d_Width; }
            set { d_Width = value; }
        }
        public int Height
        {
            get { return d_Height; }
            set { d_Height = value; }
        }
        /// <summary>
        /// Methord For Calculate Hight and Width
        /// </summary>
        /// <param name="aWidth"></param>
        /// <param name="aHeight"></param>
        /// <param name="dWidth"></param>
        /// <param name="dHeight"></param>
        public void WidthAndHeight(int aWidth, int aHeight, int dWidth, int dHeight)
        {
            double height = aHeight;
            double width = aWidth;
            double rWidht = Convert.ToDouble(dWidth);
            double rHeight = Convert.ToDouble(dHeight);
            int fWidth = 0;
            int fHeight = 0;
            double hRatio = 0.0;
            double vRatio = 0.0;
            if (width > rWidht)
            {
                hRatio = (rWidht / width);
                vRatio = (rHeight / height);

                if (vRatio > hRatio)
                {
                    fWidth = Convert.ToInt32((double)width * hRatio);
                    fHeight = Convert.ToInt32((double)height * hRatio);
                }
                else
                {
                    fWidth = Convert.ToInt32((double)width * vRatio);
                    fHeight = Convert.ToInt32((double)height * vRatio);
                }

            }
            else if (rWidht > width)
            {
                hRatio = (rWidht / width);
                vRatio = (rHeight / height);

                if (vRatio > hRatio)
                {
                    fWidth = Convert.ToInt32((double)width * hRatio);
                    fHeight = Convert.ToInt32((double)height * hRatio);
                }
                else
                {
                    fWidth = Convert.ToInt32((double)width * vRatio);
                    fHeight = Convert.ToInt32((double)height * vRatio);
                }
            }
            else if (height > rHeight)
            {
                hRatio = (rWidht / width);
                vRatio = (rHeight / height);

                if (vRatio > hRatio)
                {
                    fWidth = Convert.ToInt32((double)width * hRatio);
                    fHeight = Convert.ToInt32((double)height * hRatio);
                }
                else
                {
                    fWidth = Convert.ToInt32((double)width * vRatio);
                    fHeight = Convert.ToInt32((double)height * vRatio);
                }
            }
            else if (rHeight > height)
            {
                hRatio = (rWidht / width);
                vRatio = (rHeight / height);

                if (vRatio > hRatio)
                {
                    fWidth = Convert.ToInt32((double)width * hRatio);
                    fHeight = Convert.ToInt32((double)height * hRatio);
                }
                else
                {
                    fWidth = Convert.ToInt32((double)width * vRatio);
                    fHeight = Convert.ToInt32((double)height * vRatio);
                }
            }
            d_Width = fWidth;
            d_Height = fHeight;
        }
    }
}