﻿namespace Loupedeck.MsfsPlugin
{
    using System;

    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Timers;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    using FSUIPC;


    class SimulatorDAO
    {
        private static Offset<int> verticalSpeed = new Offset<int>(0x02C8);
        private static Offset<int> verticalSpeedAP = new Offset<int>(0x07F2);
        private static Offset<double> compass = new Offset<double>(0x02CC);
        private static Offset<Int16> compassAP = new Offset<Int16>(0x07CC);
        private static Offset<int> fps = new Offset<int>(0x0274);
        private static Offset<int> altitude = new Offset<int>(0x0574);
        private static Offset<int> altitudeAP = new Offset<int>(0x07D4);
        private static Offset<int> apSwitch = new Offset<int>(0x07BC);

        private static Timer timer = new System.Timers.Timer();

        public static void Initialise()
        {
            timer.Interval = 500;
            timer.Enabled = true;
            timer.Elapsed += refresh;
        }

        public static void Disconnect()
        {
            FSUIPCConnection.Close();
            timer.Enabled = false;
            MsfsData.Instance.State = false;
        }


        public static void refresh(Object source, EventArgs e)
        {
            try
            {
                if (FSUIPCConnection.IsOpen)
                {
                    FSUIPCConnection.Process();
                    if (MsfsData.Instance.DirtyAP)
                    {
                        verticalSpeedAP.Value = (Int32)(MsfsData.Instance.CurrentAPVerticalSpeed * 256d / (60d * 3.28084d));
                        compassAP.Value = (Int16)(MsfsData.Instance.CurrentAPHeading * 182);
                        altitudeAP.Value = (Int32)(MsfsData.Instance.CurrentAPAltitude * 65536 / 3.28);
                        apSwitch.Value = (Int32)MsfsData.Instance.ApSwitch;
                        MsfsData.Instance.DirtyAP = false;
                    }
                    else
                    {
                        MsfsData.Instance.CurrentAPVerticalSpeed = (int)((verticalSpeedAP.Value / 256d) * 60d * 3.28084d);
                        MsfsData.Instance.CurrentAPHeading = compassAP.Value / 182;
                        MsfsData.Instance.CurrentAPAltitude = (Int32)Math.Round(altitudeAP.Value / 65536 * 3.28 / 10.0) * 10;
                        MsfsData.Instance.ApSwitch = apSwitch.Value;
                    }

                    MsfsData.Instance.CurrentHeading = (int)compass.Value;
                    double verticalSpeedMPS = verticalSpeed.Value / 256d;
                    double verticalSpeedFPM = verticalSpeedMPS * 60d * 3.28084d;
                    MsfsData.Instance.CurrentVerticalSpeed = (int)verticalSpeedFPM;
                    MsfsData.Instance.CurrentAltitude = (int)(altitude.Value * 3.28);
                    MsfsData.Instance.Fps = 32768 / (fps.Value + 1);
                                       
                }
                else
                {
                    FSUIPCConnection.Open();
                    timer.Interval = 500;
                }
            }
            catch (FSUIPCException ex)
            {
                MsfsData.Instance.State = false;
                timer.Interval = 2000;
            }
            if (FSUIPCConnection.IsOpen)
            {
                MsfsData.Instance.State = true;
            }
            MsfsData.Instance.changed();
        }

    }
}
