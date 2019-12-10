using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace Ignite2019.IoT.Orleans.Grains
{
  public interface IInventoryGrain : IGrainWithIntegerKey
  {
    Task<bool> TryDecreaseAsync(int invQty);
  }

  public class InventoryGrain : Grain<InventoryState>, IInventoryGrain
  {
    public async Task<bool> TryDecreaseAsync(int invQty)
    {
      if (this.State.Inventory >= invQty)
      {
        this.State.Inventory -= invQty;
        await this.WriteStateAsync();
        return true;
      }

      return false;
    }
  }

  public class InventoryState
  {
    public int SkuId { get; set; }

    public int Inventory { get; set; }
  }

  public interface IOrderGrain : IGrainWithGuidKey
  {
    Task<bool> CreateOrderAsync(CartItem item);
  }

  public class CartItem
  {
    public int SkuId { get; set; }
    public int BuyQty { get; set; }
  }

}
