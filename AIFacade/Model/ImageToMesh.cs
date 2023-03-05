using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Rhino;
using Rhino.Geometry;

namespace AIFacade.Model
{
    internal class ImageToMesh
    {
        public static Point3d[,] ImageToPoint(string Path, double depth, double width, double height, out Color[,] colors)
        {




            Bitmap img = new Bitmap(Path);

            Point3d[,] points = new Point3d[img.Width, img.Height];
            colors = new Color[img.Width, img.Height];



            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);


                    double smoothdepth = Math.Round(pixel.R / 50.0) * 50;
                    if (pixel.B - 5 > pixel.R && pixel.B - 5 > pixel.G)
                        smoothdepth = 0;
                    double pixelDepth = Linear(0, 255, smoothdepth, 0, depth);
                    double widthdepth = Linear(0, img.Width, i, 0, width);
                    double heightdepth = Linear(0, img.Height, j, 0, height);
                    points[i, j] = new Point3d(widthdepth, heightdepth, pixelDepth);
                    colors[i, j] = img.GetPixel(i, j);
                }
            }

            string name = System.IO.Path.GetFileName(Path);
            string hh = Path.Replace("_Depth", "_Real");
            if (name.Contains("_Depth") && System.IO.File.Exists(hh))
            {
                Bitmap imgcolor = new Bitmap(hh);
                for (int i = 0; i < imgcolor.Width; i++)
                {
                    for (int j = 0; j < imgcolor.Height; j++)
                    {
                        colors[i, j] = imgcolor.GetPixel(i, j);
                    }
                }

            }
           

            return points;
        }

        public static Point3d[,] ImageToPoint(BitmapImage btm, double depth, double width, double height, out Color[,] colors)
        {




            Bitmap img = BitmapImage2Bitmap(btm);

            Point3d[,] points = new Point3d[img.Width, img.Height];
            colors = new Color[img.Width, img.Height];



            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);


                    double smoothdepth = Math.Round(pixel.R / 50.0) * 50;
                    if (pixel.B - 5 > pixel.R && pixel.B - 5 > pixel.G)
                        smoothdepth = 0;
                    double pixelDepth = Linear(0, 255, smoothdepth, 0, depth);
                    double widthdepth = Linear(0, img.Width, i, 0, width);
                    double heightdepth = Linear(0, img.Height, j, 0, height);
                    points[i, j] = new Point3d(widthdepth, heightdepth, pixelDepth);
                    colors[i, j] = img.GetPixel(i, j);
                }
            }

            


            return points;
        }


        public static Mesh PointsToMesh(Point3d[,] points, Color[,] colors)
        {
            Mesh meshfinal = new Mesh();
            int width = points.GetLength(0);
            int height = points.GetLength(1);
            List<Point3d> lst = points.Cast<Point3d>().ToList();
            List<Color> colors1 = new List<Color>();



            for (int i = 0; i < width - 2; i++)
            {
                for (int j = 0; j < height - 2; j++)
                {
                    meshfinal.Vertices.AddVertices(new List<Point3d>() { points[i, j], points[i + 1, j], points[i + 1, j + 1], points[i, j + 1] });
                    colors1.Add(colors[i, j]);
                    colors1.Add(colors[i+1, j]);
                    colors1.Add(colors[i+1, j+1]);
                    colors1.Add(colors[i, j+1]);
                    int count = meshfinal.Vertices.Count;
                    meshfinal.Faces.AddFace(new MeshFace(count - 4, count - 3, count - 2, count - 1));
                }
            }

            meshfinal.VertexColors.SetColors(colors1.ToArray());
            return meshfinal;
        }
        public static double Linear(double oldMin, double oldMax, double oldvalue, double newMin, double newMax)
        {
            return ((oldvalue - oldMin) / (oldMax - oldMin)) * (newMax - newMin) + newMin;
        }

        private static  Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
    }
}
