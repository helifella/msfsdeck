﻿namespace Loupedeck.MsfsPlugin.encoder
{
    using System;
    using System.Diagnostics;

    public abstract class DefaultEncoder : PluginDynamicAdjustment, Notifiable
    {
        protected Int32 min;
        protected Int32 max;
        protected Int32 step;
        protected Binding _binding;

        public DefaultEncoder(String name, String desc, String category, Boolean resettable, Int32 min, Int32 max, Int32 step) : base(name, desc, category, resettable)
        {
            this.min = min;
            this.max = max;
            this.step = step;
        }

        protected override void ApplyAdjustment(String actionParameter, Int32 ticks)
        {
            var value = this.GetValue();
            value += ticks * this.step;
            if (value < this.min)
            { value = this.min; }
            else if (value > this.max)
            { value = this.max; }
            this.SetValue(value);
            this.ActionImageChanged();
        }
        protected override String GetAdjustmentValue(String actionParameter)
        {
            return this.GetDisplayValue();
        }
        public void Notify()
        {
            if (this._binding != null && this._binding.Key != null)
            {
                if (this._binding.HasMSFSChanged())
                {
                    Debug.WriteLine("Refesh " + this._binding.Key);
                    this._binding.Reset();
                    this.AdjustmentValueChanged();
                }
                else
                {
                    Debug.WriteLine("Skipping " + this._binding.Key);
                }
            }
            else
            {
                this.ActionImageChanged();
            }
        }
        protected virtual String GetDisplayValue() => this.GetValue().ToString();
        protected virtual Int32 GetValue() => 0;
        protected abstract void SetValue(Int32 value);
    }
}
