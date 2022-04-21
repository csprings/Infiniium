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
    [Display("Load Waveform", Group: "Oscilloscope", Description: "Load saved waveform")]
    public class LoadWaveform : TestStep
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change
        [Display("Instrument", Order:1)]
        public Scope OSC { get; set; }
        [FilePath(FilePathAttribute.BehaviorChoice.Open, "Waveform Files(*.wfm, *.h5, *.csv, *.tsv)|*.wfm; *.h5; *.csv; *.tsv")]
        [Display("Waveform File", Order:3)]
        public string WaveformFilePath { get; set; }
        [Display("Channel", Order:2)]
        public Channel Chan { get; set; }
        #endregion

        public enum Channel
        {
            Channel1,
            Channel2,
            Channel3,
            Channel4
        }
        public LoadWaveform()
        {
            // ToDo: Set default values for properties / settings.
            Chan = Channel.Channel1;
            WaveformFilePath = "C:\\TEMP\\EyeDiagram.wfm";
        }

        public override void Run()
        {
            // ToDo: Add test case code.
            OSC.ScpiCommand("*RST");
            OSC.ScpiCommand($":DISK:LOAD \"{WaveformFilePath}\", {Chan}");
            int checkComplete = OSC.ScpiQuery<int>("*OPC?");
            while (checkComplete == 0)
                { 
                checkComplete = OSC.ScpiQuery<int>("*OPC?"); 
                }
            
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
