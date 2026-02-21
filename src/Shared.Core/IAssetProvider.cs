namespace Shared.Core;
/// <summary>
/// Asset wird speicherschonend geladen, damit es nicht zu lange im Speicher bleibt.
/// </summary>
/// <returns> Ein AssetData-Paket. Es muss Disposed werden</returns>
public interface IAssetProvider
{
    ValueTask<AssetData> RentAssetAsync(string name, CancellationToken ct = default);
}
