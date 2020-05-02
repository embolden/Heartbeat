using System;
using System.Collections.Generic;
using System.Text;

namespace Heartbeat
{
    class ModConfig
    {
        public bool HeartBeatEnabled { get; set; } = true;
        public float HeartBeatAlertPercent { get; set; } = 45.0F;
    }
}
