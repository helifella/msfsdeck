﻿namespace Loupedeck.MsfsPlugin
{
    using System;

    using Loupedeck.MsfsPlugin.encoder;

    class VSpeedAPEncoder : DefaultEncoder
    {
        public VSpeedAPEncoder() : base("VS", "Autopilot VS encoder", "AP", true, -10000, 10000, 100)
        {
            this._bindings.Add(MsfsData.Instance.Register(new Binding(BindingKeys.AP_VSPEED)));
            this._bindings.Add(MsfsData.Instance.Register(new Binding(BindingKeys.VSPEED)));
        }
        protected override void RunCommand(String actionParameter) => this.SetValue(this._bindings[1].ControllerValue);
        protected override String GetDisplayValue() => "[" + this._bindings[0].ControllerValue + "]\n" + this._bindings[1].ControllerValue;
        protected override Int64 GetValue() => this._bindings[0].ControllerValue;
        protected override void SetValue(Int64 newValue) => this._bindings[0].SetControllerValue(newValue);
    }
}
