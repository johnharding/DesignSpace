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

namespace DesignSpace
{
    public class DesignSpaceComponent : GH_Component
    {
        public bool GO = false;
        private List<Grasshopper.Kernel.Special.GH_NumberSlider> sliders = new List<Grasshopper.Kernel.Special.GH_NumberSlider>();
        int counter;

        public DesignSpaceComponent() 
            : base("DesignSpace", "DS", "Generates new grasshopper networks automatically", "Extra", "Default")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pm)
        {
            pm.AddBooleanParameter("Go", "G", "Set to true to run through solutions", GH_ParamAccess.item, false);
            pm.AddNumberParameter("Sliders", "S", "Put numeric variables (sliders) in here", GH_ParamAccess.list);

            pm[1].WireDisplay = GH_ParamWireDisplay.faint;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pm)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
           

            // If we are currently static, then reset things and collect sliders
            if (!GO)
            {
                counter = 0;
                sliders.Clear();

                // Default value is false anyway remember
                DA.GetData("Go", ref GO);
                
                foreach (IGH_Param param in this.Params.Input[1].Sources)
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
                // Test to see if we can change the slider to 9
                sliders[0].Slider.Value = (decimal)counter;

                // Iterate the master counter
                counter++;

                // If we reach a limit, then stop
                if (counter == 5)
                {
                    GO = false;

                    //expire this component
                    this.ExpireSolution(true);
                }

            }

           
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
                return Properties.Resources.DesignSpaceIcon;
            }
        }
    }
}
