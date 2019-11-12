using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ignite2019.IoT.Orleans.Models;

namespace Ignite2019.IoT.Orleans.Model
{
    public enum DeviceType
    {
        Lock,
        Box
    }


    public class Device
    {
        public string No { get; set; }
        public DeviceType Type { get; set; }

        public string Name { get; set; }
        public string Version { get; set; }
        public string Remark { get; set; }
    }

    public class DeviceState
    {
        public bool IsOnline { get; set; }

        public Device Detail { get; set; }

    }

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new int Id { get; set; }
        public string Name { get; set; }

        public string AvatarUrl { get; set; }
    }

    public class DeviceBind
    {
        public int UserId { get; set; }

        public Guid DeviceId { get; set; }

        public bool IsAdmin { get; set; }

        public DateTime BindTime { get; set; }

        public DeviceBindStatus Status { get; set; }
    }

    public class ControlHistory
    {
        public Command Command { get; set; }

        public DateTime UpdateTime { get; set; }
    }

    public class Command
    {
        public CommandType CommandType { get; set; }


    }

    public enum CommandType
    {
        Unlock,
        Online,
        Offline,
        Alarm
    }


}