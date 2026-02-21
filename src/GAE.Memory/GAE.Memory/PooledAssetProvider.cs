using System.Buffers;
using Shared.Core;

namespace GAE.Memory;
public class PooledAssetProvider : IAssetProvider
{
    private readonly string _rootPath;

    //Konstruktur
    public PooledAssetProvider (string rootPath)
    {
        _rootPath = rootPath;
    }
    public async ValueTask<AssetData> RentAssetAsync(string name, CancellationToken ct = default)
    {
        var fullPath = Path.Combine(_rootPath, name);
        var fileInfo = new FileInfo(fullPath);

        if (!fileInfo.Exists)
        {
            throw new FileNotFoundException($"Asset nicht gefunden: {fullPath}");
        }

        //größe ermitteln
        int lenght = (int)fileInfo.Length;

        //Leihen von speicher
        // Erhalten einen Buffer der mind. so groß ist wie die Lenght
        IMemoryOwner<byte> owner = MemoryPool<byte>.Shared.Rent(lenght);

        try
        {
            // Datei öffnen
            using var stream = new FileStream(
                fullPath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 4096,
                useAsync: true
                );

            //
            await stream.ReadAsync(owner.Memory.Slice(0, lenght), ct);

            return new AssetData(owner, name);
        }
        catch
        {
            // Muss Disposed werden interface zwingt uns dazu um speicher nicht unnötig zu blockieren
            // => ansonsten kommt es zum Memory Leak
            owner.Dispose();
            throw;
        }
    }
}
