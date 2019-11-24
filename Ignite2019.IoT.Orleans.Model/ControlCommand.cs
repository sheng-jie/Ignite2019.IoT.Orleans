using System;

namespace Ignite2019.IoT.Orleans.Model
{
    public class ControlCommand
    {
        public ControlCommand(string commandBody)
        {
            CommandBody = commandBody;
        }
        public string CommandBody { get; set; }

        //public Action<ControlCommand> CommandAction { get; set; }
    }

    

    public enum ControlCommandType
    {
        Open,
        Close
    }
}