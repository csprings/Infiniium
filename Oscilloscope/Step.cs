using OpenTap;   // Use OpenTAP infrastructure/core components (log,TestStep definition, etc)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oscilloscope
{
    [Display("IDN", Group: "Oscilloscope", Description: "Insert description here")]
    public class Step : TestStep
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change.
        [Display("Instrument")]
        public Scope OSC { get; set; }
        #endregion

        public Step()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            
            string idn = OSC.ScpiQuery("*IDN?");
            Log.Info(idn);
            Log.Error(idn);
            Log.Debug(idn);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
