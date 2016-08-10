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
        private List<object> persGeo = new List<object>();


        /// <summary>
        /// Main constructor
        /// </summary>
        public DesignSpaceComponent() 
            : base("DesignSpace", "DS", "Generates new grasshopper networks automatically", "Extra", "Rosebud")
        {   
        }

        /// <summary>
        /// Register component inputs
        /// </summary>
        /// <param name="pm"></param>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pm)
        {
            pm.AddNumberParameter("Sliders", "S", "(genotype) Connect sliders here", GH_ParamAccess.list);
            pm.AddGeometryParameter("Volatile Geometry", "vG", "(phenotype) Connect geometry that is dependent on sliders here", GH_ParamAccess.item);
            pm.AddGeometryParameter("Persistent Geometry", "pG", "(phenotype) Connect geometry that is independent of sliders here (e.g. site context)", GH_ParamAccess.item);

            pm[0].WireDisplay = GH_ParamWireDisplay.faint;
            pm[1].WireDisplay = GH_ParamWireDisplay.faint;
            pm[2].WireDisplay = GH_ParamWireDisplay.faint;

            pm[1].Optional = true;
            pm[2].Optional = true;
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
                // Clear the crap out
                counter = 0;
                sliders.Clear();
                persGeo.Clear();

                
                
                foreach (IGH_Param param in this.Params.Input[0].Sources)
                {
                    Grasshopper.Kernel.Special.GH_NumberSlider slider = param as Grasshopper.Kernel.Special.GH_NumberSlider;
                    if (slider != null)
                    {
                        sliders.Add(slider);
                    }

                    
                }


            }

            else
            {
                // We need one more calculaion after the slider has updated later.
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
                    object localObj = null;
                    DA.GetData("Persistent Geometry", ref localObj);
                    persGeo.Add(localObj);
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
                    // this.ExpireSolution(true);
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
                if (myObject is GH_Mesh)
                {
                    GH_Mesh myGHMesh = new GH_Mesh();
                    myGHMesh = (GH_Mesh)myObject;
                    Mesh myLocalMesh = new Mesh();
                    GH_Convert.ToMesh(myGHMesh, ref myLocalMesh, GH_Conversion.Primary);
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
