using System;

namespace Ignite2019.IoT.Orleans.Model
{
    public class ControlHistory
    {
        public Command Command { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}