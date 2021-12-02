﻿namespace Loupedeck.MsfsPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class APStateInput : PluginDynamicCommand, Notifiable
    {
        public APStateInput() : base("AP Master", "Display AP state", "AP")
        {
            MsfsData.Instance.register(this);
        }
        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize)
        {
            if (MsfsData.Instance.ApSwitch == 1)
            {
                return "AP Off";
            } else
            {
                return " AP On";
            }
        }

        public void Notify() => this.AdjustmentValueChanged();

        protected override void RunCommand(String actionParameter)
        {
            MsfsData.Instance.ApSwitch = (MsfsData.Instance.ApSwitch + 1) % 2;
        }
    }
}

