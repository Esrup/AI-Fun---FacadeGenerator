using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Rhino.Geometry;

namespace AIFacade.View
{
    /// <summary>
    /// Interaction logic for Panel.xaml
    /// </summary>
    public partial class Panel : Window
    {
        public Panel()
        {
            InitializeComponent();
        }

        public BitmapImage CurrentImage { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //string path = "";
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //if (openFileDialog.ShowDialog() == true)
            //    path = openFileDialog.FileName;

            //if (path == "")
            //{
            //    return;
            //}

            if (!double.TryParse(width.Text, out double widthnumber))
                return;
            if (!double.TryParse(height.Text, out double heighthnumber))
                return;
            if (!double.TryParse(depth.Text, out double depthhnumber))
                return;

            Point3d[,] points = Model.ImageToMesh.ImageToPoint(CurrentImage, depthhnumber, widthnumber, heighthnumber, out Color[,] color);

            List<Point3d> lst = points.Cast<Point3d>().ToList();
            //Rhino.RhinoDoc.ActiveDoc.Objects.AddPointCloud(new PointCloud(lst));

            try
            {
                Mesh mesh = Model.ImageToMesh.PointsToMesh(points, color);
                //Mesh meshes = Mesh.CreateFromTessellation(lst, new List<List<Point3d>>() { edges }, Plane.WorldXY, true);
                Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(mesh);
            }
            catch (Exception ex)
            {

            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
               BitmapImage image= Model.httpreq.callSTDiff(promttext.Text);
                image1.Source = image;
                CurrentImage = image;
            }
            catch (Exception ex)
            {


            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
            
        }
    }
}
