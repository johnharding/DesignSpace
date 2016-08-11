using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using System.Windows.Forms;
using System.Drawing;
using Rhino.Geometry;

namespace DesignSpace
{
    public class DesignSpaceComponent : GH_Component
    {

        private DesignSpaceWindow myMainWindow;
        public bool GO = false;
        int counter;
        private List<Grasshopper.Kernel.Special.GH_NumberSlider> sliders = new List<Grasshopper.Kernel.Special.GH_NumberSlider>();
        private List<double> sliderValues = new List<double>();
        private List<object> persGeo = new List<object>();


        /// <summary>
        /// Main constructor
        /// </summary>
        public DesignSpaceComponent() 
            : base("DesignSpace", "DS", "Displays multiple parameter instances in one place", "Extra", "Rosebud")
        {   
        }

        /// <summary>
        /// Register component inputs
        /// </summary>
        /// <param name="pm"></param>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pm)
        {
            pm.AddNumberParameter("Sliders", "S", "(genotype) Connect sliders here", GH_ParamAccess.list);
            //pm.AddGeometryParameter("Volatile Geometry", "vG", "(phenotype) Connect geometry that is dependent on sliders here", GH_ParamAccess.item);
            pm.AddGeometryParameter("Geometry", "G", "(phenotype) Connect geometry here - currently only meshes", GH_ParamAccess.list);

            pm[0].WireDisplay = GH_ParamWireDisplay.faint;
            pm[1].WireDisplay = GH_ParamWireDisplay.faint;
            pm[1].Optional = true;

        }

        /// <summary>
        /// Register component outputs
        /// </summary>
        /// <param name="pm"></param>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pm)
        {
            pm.AddNumberParameter("ChosenOnes", "Ch", "Each list contains a collection of selected parameters", GH_ParamAccess.tree);
        }

        /// <summary>
        /// Grasshopper solve method
        /// </summary>
        /// <param name="DA"></param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // If we are currently static, then reset things and collect sliders
            if (!GO)
            {
                // Spring clean
                counter = 0;
                sliders.Clear();
                persGeo.Clear();
                sliderValues.Clear();

                // Collect the sliders up (just one at the moment)
                foreach (IGH_Param param in this.Params.Input[0].Sources)
                {
                    Grasshopper.Kernel.Special.GH_NumberSlider slider = param as Grasshopper.Kernel.Special.GH_NumberSlider;
                    if (slider != null)
                    {
                        sliders.Add(slider);
                    } 
                }

                // Now set the value list
                // TODO: Replace with a tree, not just the first slider!
                if (sliders != null)
                {
                    if (sliders[0].Slider.Type == Grasshopper.GUI.Base.GH_SliderAccuracy.Integer)
                    {
                        int min = (int)sliders[0].Slider.Minimum;
                        int max = (int)sliders[0].Slider.Maximum;

                        for (int j = min; j <= max; j++)
                        {
                            sliderValues.Add((double)Math.Round((double)j, 0));
                        }
                    }

                    else if (sliders[0].Slider.Type == Grasshopper.GUI.Base.GH_SliderAccuracy.Float)
                    {
                        double min, max;

                        min = (double)slider.Slider.Minimum;
                        max = (double)slider.Slider.Maximum;

                        double absRange = max - min;
                        double increment = absRange / (MAXVALUES - 1);

                        for (double j = min; j <= max; j += increment)
                            mySliderValues.Add((double)Convert.ToDouble(Convert.ToString(Math.Round(j, 2)))); //really, really, really stupid

                    }
                }



            }

            // So if GO = true...
            else
            {
                // Get the slider values.
                // TODO: Include more than one slider.
                if(counter<5)
                    sliders[0].Slider.Value = (decimal)counter;


                // First things first...
                if(counter==0)
                {
                    
                }
                
                // We have to do this stuff AFTER the slider has moved and the component is expired (tricky).
                else
                { 
                    // Collect the object at the current instance
                    List<object> localObjs = new List<object>();
                    DA.GetDataList("Geometry", localObjs);
                    
                    // Currently we only take meshes
                    Mesh joinedMesh = new Mesh();

                    for(int i=0; i<localObjs.Count; i++)
                    {
                        if (localObjs[i] is GH_Mesh)
                        {
                            GH_Mesh myGHMesh = new GH_Mesh();
                            myGHMesh = (GH_Mesh)localObjs[i];
                            Mesh myLocalMesh = new Mesh();
                            GH_Convert.ToMesh(myGHMesh, ref myLocalMesh, GH_Conversion.Primary);
                            myLocalMesh.Faces.ConvertQuadsToTriangles();
                            joinedMesh.Append(myLocalMesh);
                        }
                    }

                    persGeo.Add(joinedMesh);
                }   


                // If we reach a limit, then stop and launch the window
                if (counter == 5)
                {
                    GO = false;

                    // Instantiate the window and export the geometry to WPF3D
                    myMainWindow = new DesignSpaceWindow(GetPersMeshList());
                    myMainWindow.Show();

                    // Reset the counter
                    counter = 0;

                    // Expire this component
                    this.ExpireSolution(true);
                }

                // NOW iterate the master counter
                counter++;

            }

            // We need some interaction with the form before sending out the chosen phenotypes.
            DA.SetData(0, 444);

        }

        /// <summary>
        /// Returns persisent meshes
        /// </summary>
        /// <returns></returns>
        public List<Mesh>GetPersMeshList()
        {           
            List<Mesh> myMeshes = new List<Mesh>();
            foreach (object myObject in persGeo)
            {
                if (myObject is Mesh)
                {
                    Mesh myLocalMesh = (Mesh)myObject;
                    myMeshes.Add(myLocalMesh);
                }
            }

            return myMeshes;
        }


        public override Guid ComponentGuid
        {
            get { return new Guid("73F1D5F1-7208-4501-8C8C-66ED25BA5A1D"); }
        }

        public override void CreateAttributes()
        {
            m_attributes = new DesignSpaceAttributes(this);
        }


        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }

        protected override Bitmap Icon
        {
            get
            {
                return Properties.Resources.DoubleClickIcon;
            }
        }
    }
}
