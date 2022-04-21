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
    [Display("EyeDiagram", Group: "Oscilloscope", Description: "Drawing eye diagram")]
    public class EyeDiagram : TestStep
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change
        [Display("Instrument", Order:1)]
        public Scope OSC { get; set; }
        [Display("Channel", Order:2)]
        public Channel Chan { get; set; }
        [Display("Clock Method", Order:3)]
        public ClockMethod Clock { get; set; }
        [Display("Data Rate Method", Order: 4.1)]
        [EnabledIf("Clock", ClockMethod.SEMI, ClockMethod.FOPLL, ClockMethod.SOPLL, HideIfDisabled = true)]
        public SelectDataRate DataRateMethod { get; set; }
        [Display("Data Rate", Order:4.2)]
        [EnabledIf("Clock", ClockMethod.SEMI, ClockMethod.FOPLL, ClockMethod.SOPLL, HideIfDisabled = true)]
        [EnabledIf("DataRateMethod", SelectDataRate.Manual, HideIfDisabled = true)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double DataRate { get; set; }
        [Display("Loop Bandwidth", Order:4.3)]
        [EnabledIf("Clock", ClockMethod.FOPLL, ClockMethod.SOPLL, HideIfDisabled = true)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LoopBandwidth { get; set; }
        [Display("Peaking", Order: 4.4)]
        [EnabledIf("Clock", ClockMethod.SOPLL, HideIfDisabled = true)]
        [Unit("dB")]
        public double Peaking { get; set; }
        [Display("Explicit Clock Source", Order:5)]
        [EnabledIf("Clock", ClockMethod.Explicit, HideIfDisabled = true)]
        public Channel ExplicitClock { get; set; }

        #endregion
        public enum Channel
        {
            Channel1,
            Channel2,
            Channel3,
            Channel4
        }

        public enum ClockMethod
        {
            [Display("Constant Frequency(Fully Automatic)")]
            Fixed,
            [Display("Constant Frequency(Semi Automatic)")]
            SEMI,
            [Display("First Order PLL")]
            FOPLL,
            [Display("Second Order PLL")]
            SOPLL,
            [Display("Explicit Clock")]
            Explicit

        }
        public enum SelectDataRate
        {
            Auto,
            Manual
        }
        public EyeDiagram()
        {
            // ToDo: Set default values for properties / settings.
            Chan = Channel.Channel1;
            Clock = ClockMethod.SEMI;
            DataRate = 1E+09;
            LoopBandwidth = 4E+06;
            Peaking = 0.707;
            ExplicitClock = Channel.Channel2;
        }

        public override void Run()
        {
            // ToDo: Add test case code.
            double dataRate;
            if (DataRateMethod == SelectDataRate.Auto)
            {
                OSC.ScpiCommand($":MEASure:DATarate {Chan}, Auto");
                dataRate = OSC.ScpiQuery<double>($":MEASure:DATarate? {Chan}, Auto");
            }
            else
            {
                dataRate = DataRate;
            }
            
            OSC.ScpiCommand($":ANALyze:CLOCk:METHod:SOURce {Chan}");
            switch (Clock)
            {
                case ClockMethod.Fixed: 
                    OSC.ScpiCommand($":ANALyze:CLOCk:METHod FIXed, AUTO");
                    break;
                case ClockMethod.SEMI:
                    OSC.ScpiCommand($":ANALyze:CLOCk:METHod FIXed, SEMI, {dataRate}");
                    break;
                case ClockMethod.FOPLL:
                    OSC.ScpiCommand($":ANALyze:CLOCk:METHod:JTF FOPLL, {dataRate}, {LoopBandwidth}");
                    break;
                case ClockMethod.SOPLL:
                    OSC.ScpiCommand($":ANALyze:CLOCk:METHod:JTF SOPLL,{dataRate}, {LoopBandwidth}, {Peaking}");
                    break;
                case ClockMethod.Explicit:
                    OSC.ScpiCommand($":ANALyze:CLOCk:METHod EXPlicit,{ExplicitClock}, Rising");
                    break;
            }
            OSC.ScpiCommand(":MTESt:FOLDing ON");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            //UpgradeVerdict(Verdict.Pass);
        }
    }
}
