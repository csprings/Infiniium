// Author: MyName
// Copyright:   Copyright 2022 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Oscilloscope
{
    [Display("Frequency", Group: "Oscilloscope", Description: "Insert a description here")]
    public class Frequency : TestStep
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change
        [Display("Instrument")]
        public Scope OSC { get; set; }
        [Display("Channel")]
        public Channel Chan { get; set; }
        [Display("List")]
        public int[] List1 { get; set; }
        #endregion

        public enum Channel
        {
            Channel1,
            Channel2,
            Channel3,
            Channel4
        }
        public Frequency()
        {
            Chan = Channel.Channel1;
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            OSC.ScpiCommand($"*RST");
            OSC.ScpiCommand($":Channel1:DISPlay OFF");
            OSC.ScpiCommand($":{Chan}:DISPlay On");
            string Freq = OSC.ScpiQuery(":MEASure:FREQuency? " + Chan);
            Log.Info(Freq + "Hz");
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
