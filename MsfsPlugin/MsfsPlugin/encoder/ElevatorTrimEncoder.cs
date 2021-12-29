﻿namespace Loupedeck.MsfsPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Loupedeck.MsfsPlugin.encoder;

    class ElevatorTrimEncoder : DefaultEncoder
    {
        public ElevatorTrimEncoder() : base("Elevator Trim", "Elevator trim encoder", "Misc", true, -100, 100, 1) { }

        protected override void RunCommand(String actionParameter) => this.SetValue(0);
        protected override Int32 GetValue() => MsfsData.Instance.CurrentElevatorTrim;

        protected override Int32 SetValue(Int32 newValue) => MsfsData.Instance.CurrentElevatorTrim = (Int16)newValue;
    }
}