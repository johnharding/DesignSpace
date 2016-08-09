using HelixToolkit.Wpf;
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
using System.Windows.Shapes;

namespace DesignSpace
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DesignSpaceWindow : Window
    {
        public DesignSpaceWindow()
        {
            this.InitializeComponent();
            
            UserControl1 myControl1 = new UserControl1();
            UserControl1 myControl2 = new UserControl1();
            UserControl1 myControl3 = new UserControl1();

            // Create the Grid
            Grid myGrid = new Grid();
            myGrid.Width = 1000;
            myGrid.Height = 600;
            myGrid.HorizontalAlignment = HorizontalAlignment.Left;
            myGrid.VerticalAlignment = VerticalAlignment.Top;
            myGrid.ShowGridLines = true;

            // Define the Columns
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            myGrid.ColumnDefinitions.Add(colDef1);
            myGrid.ColumnDefinitions.Add(colDef2);
            myGrid.ColumnDefinitions.Add(colDef3);

            // Define the Rows
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            RowDefinition rowDef3 = new RowDefinition();
            RowDefinition rowDef4 = new RowDefinition();
            myGrid.RowDefinitions.Add(rowDef1);
            myGrid.RowDefinitions.Add(rowDef2);
            myGrid.RowDefinitions.Add(rowDef3);
            myGrid.RowDefinitions.Add(rowDef4);


            Grid.SetColumn(myControl1, 1);
            Grid.SetRow(myControl1, 1);

            Grid.SetColumn(myControl2, 2);
            Grid.SetRow(myControl2, 2);

            myGrid.Children.Add(myControl1);
            myGrid.Children.Add(myControl2);
            //myGrid.Children.Add(myControl3);


            this.AddChild(myGrid);
        }


    }
}
