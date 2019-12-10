using System;
using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.Events;
using Ignite2019.IoT.Orleans.States;
using Orleans;
using Orleans.EventSourcing;
using Orleans.Providers;
using Orleans.Streams;

namespace Ignite2019.IoT.Orleans.Grains
{
  public class DeviceGrain : JournaledGrain<ShadowDevice>, IDeviceGrain
  {
    private IAsyncStream<DeviceEvent> _deviceEventStream;

    public override Task OnActivateAsync()
    {
      var streamProvider = this.GetStreamProvider("SMSProvider");
      var streamId = Guid.NewGuid();
      _deviceEventStream = streamProvider.GetStream<DeviceEvent>(streamId, "DeviceEvent");
      return base.OnActivateAsync();
    }

    public async Task HandleEvent(DeviceEvent deviceEvent)
    {
      this.RaiseEvent(deviceEvent);

      await ConfirmEvents();

      await _deviceEventStream.OnNextAsync(deviceEvent);

    }

    public async Task<ShadowDevice> GetShadowDeviceAsync()
    {
      await RefreshNow();
      return this.State;
    }
  }
}
