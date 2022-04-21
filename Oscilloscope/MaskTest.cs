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
    [Display("MaskTest", Group: "Oscilloscope", Description: "Insert a description here")]
    public class MaskTest : TestStep
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change
        [Display("Instrument", Order: 1)]
        public Scope OSC { get; set; }
        [Display("Channel", Order: 2)]
        public Channel Chan { get; set; }
        [FilePath(FilePathAttribute.BehaviorChoice.Open, "msk")]
        [Display("Mask File", Order: 3)]
        public string MaskFile { get; set; }
        [Display("Offline?", Order:4)]
        public bool offline { get; set; }
        [FilePath(FilePathAttribute.BehaviorChoice.Open, "Waveform Files(*.wfm, *.h5, *.csv, *.tsv)|*.wfm; *.h5; *.csv; *.tsv")]
        [Display("Waveform File", Order:4.1)]
        [EnabledIf("offline", true, HideIfDisabled = true)]
        public string WaveformFilePath { get; set; }

        #endregion
        public enum Channel
        {
            Channel1,
            Channel2,
            Channel3,
            Channel4
        }

        public MaskTest()
        {
            // ToDo: Set default values for properties / settings.
            Chan = Channel.Channel1;
            MaskFile = "C:\\TEMP\\mask.msk";
            offline = true;
            WaveformFilePath = "C:\\TEMP\\EyeDiagram.wfm";
        }

        public override void Run()
        {
            OSC.ScpiCommand(":MTESt1:ENABle ON");
            Compeletion();
            OSC.ScpiCommand($":MTESt1:AMASk:SOURce {Chan}");
            Compeletion(); 
            OSC.ScpiCommand($":MTESt1:LOAD \"{MaskFile}\"");
            Compeletion();
            OSC.ScpiCommand($":MTESt:STARt");
            if (offline == true)
            {
                OSC.ScpiCommand($":DISK:LOAD \"{WaveformFilePath}\", {Chan}");
                Compeletion();
            }
            OSC.ScpiCommand($":MTESt:STOP");
            Compeletion();
            string numberOfBits = OSC.ScpiQuery(":MTESt:FOLDing:COUNt:UI?");

            Log.Info("Tested Number of UIs: "+ numberOfBits);
            double numberOfFailedBits = OSC.ScpiQuery<double>(":MTESt1:COUNt:FUI?");

            Log.Info("Number of UIs that failed: "+numberOfFailedBits.ToString());

            if (numberOfFailedBits == 0)
                UpgradeVerdict(Verdict.Pass);
            else
                UpgradeVerdict(Verdict.Fail);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
        public void Compeletion()
        {
            int checkComplete = OSC.ScpiQuery<int>("*OPC?");
            while (checkComplete == 0)
            {
                checkComplete = OSC.ScpiQuery<int>("*OPC?");
            }

        }

    }
}
