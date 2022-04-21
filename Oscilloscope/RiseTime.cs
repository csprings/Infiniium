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
    [Display("Rise Time", Group: "Oscilloscope", Description: "Insert a description here")]
    public class RiseTime : TestStep
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change
        [Display("Channel")]
        public Channel Chan { get; set; }
        //[Display("List")]
        //[Unit("Hz", UseEngineeringPrefix: true)]
        //public double[] List { get; set; }
        //private List<string> _sweepValues;
        //[Display("Sweep Values")]
        //public List<string> sweepValues
        //{
        //    get { return _sweepValues; }
        //    set
        //    {
        //        _sweepValues = value;
        //        OnPropertyChanged("sweepValues1");
        //    }
        //}

        #endregion

        public enum Channel
        {
            Channel1,
            Channel2,
            Channel3,
            Channel4
        }
        public RiseTime()
        {
            Chan = Channel.Channel1;
            // ToDo: Set default values for properties / settings.
            //List = new double[] { 100000000, 5000000000, 5000000000, 20000000000 };
            //_sweepValues = new List<string> { "100MHz", "5GHz", "10GHz", "15GHz", "20GHz" };
        }

        public static double HzConvert(string x)
        {
            x = x.ToUpper();
            x = x.Replace(" ", "");
            int numericValue;
            bool isNumber = int.TryParse(x, out numericValue);
            bool megaHertz = x.Contains("MHZ");
            bool kiloHertz = x.Contains("KHZ");
            bool hertz = x.Contains("HZ");
            bool gigaHertz = x.Contains("GHZ");
            bool teraHertz = x.Contains("THZ");

            if (isNumber == true)
            {
                double result = Convert.ToDouble(x);
                return result;
            }
            else if (kiloHertz == true)
            {
                x = x.Replace("KHZ", "000");
                double result = Convert.ToDouble(x);
                return (result);
            }
            else if (megaHertz == true)
            {
                x = x.Replace("MHZ", "000000");
                double result = Convert.ToDouble(x);
                return (result);
            }
            else if (gigaHertz == true)
            {
                x = x.Replace("GHZ", "000000000");
                double result = Convert.ToDouble(x);
                return (result);
            }
            else if (teraHertz == true)
            {
                x = x.Replace("THZ", "000000000000");
                double result = Convert.ToDouble(x);
                return (result);
            }
            else if (hertz == true)
            {
                x = x.Replace("HZ", "");
                double result = Convert.ToDouble(x);
                return (result);
            }
            else
            {
                return (-100);

            }
        }
        public override void Run()
        {
            //double value = HzConvert(sweepValues[5]);
            //Log.Info(Convert.ToString(value));
            //Log.Info(sweepValues[5]);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
