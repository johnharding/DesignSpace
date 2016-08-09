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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;
using Grasshopper.Kernel.Types;

namespace DesignSpace
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            Create3DViewPort();
        }

        /// <summary>
        /// Create the 3d viewport
        /// </summary>
        private void Create3DViewPort()
        {
            var hVp3D = new HelixViewport3D();
            //hVp3D.FitView(new System.Windows.Media.Media3D.Vector3D(0,0,1), )
            //hVp3D.Arrange(new Rect(200, 200, 300, 300));
            //hVp3D.Width = 200;

            hVp3D.ShowFrameRate = true;
            hVp3D.ViewCubeOpacity = 0.5;

            var lights = new DefaultLights();
            var teaPot = new Teapot();
            hVp3D.Children.Add(lights);
            //hVp3D.Children.Add(teaPot);

            /*
             * Whenever you can, use Visual3D objects for unique instances of objects within your scene. 
             * This usage contrasts with that of Model3D objects, which are lightweight objects that are optimized to be shared and reused. 
             * For example, use a Model3Dobject to build a model of a car; and use ten ModelVisual3D objects to place ten cars in your scene.
             */

            MeshGeometry3D myMesh = new MeshGeometry3D();
            Point3DCollection myPoints = new Point3DCollection();
            myPoints.Add(new Point3D(20,20,20));
            myPoints.Add(new Point3D(120,120,20));
            myPoints.Add(new Point3D(20,20,120));
            myMesh.Positions = myPoints;

            myMesh.TriangleIndices.Add(0);
            myMesh.TriangleIndices.Add(1);
            myMesh.TriangleIndices.Add(2);

            // Define material that will use the gradient.
            //DiffuseMaterial myDiffuseMaterial = new DiffuseMaterial(Brushes.Black);

            // Add this gradient to a MaterialGroup.
            //MaterialGroup myMaterialGroup = new MaterialGroup();
            //myMaterialGroup.Children.Add(myDiffuseMaterial);

            DiffuseMaterial wireframe_material = new DiffuseMaterial(Brushes.Red);
            GeometryModel3D WireframeModel = new GeometryModel3D(myMesh, wireframe_material);

            ModelVisual3D monkey = new ModelVisual3D();
            monkey.Content = WireframeModel;

            hVp3D.Children.Add(monkey);

            //hVp3D.IsEnabled = false;

            this.AddChild(hVp3D);


            
        }
    }
}
