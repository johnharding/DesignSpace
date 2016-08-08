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

        private void Create3DViewPort()
        {
            var hVp3D = new HelixViewport3D();
            //hVp3D.FitView(new System.Windows.Media.Media3D.Vector3D(0,0,1), )
            //hVp3D.Arrange(new Rect(200, 200, 300, 300));
            //hVp3D.Width = 200;
            var lights = new DefaultLights();
            var teaPot = new Teapot();
            hVp3D.Children.Add(lights);
            hVp3D.Children.Add(teaPot);
            this.AddChild(hVp3D);

        }
    }
}
