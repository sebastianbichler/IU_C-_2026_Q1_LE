using System.Buffers;

namespace Shared.Core;

/// <summary>
/// Hie wird ein Asset das in Speicher geladen ist beschrieben.
/// Hier wird ein struct genutzt damit die Daten direkt im Stack liegen und nicht auf dem Heap, was die Performance verbessert.
/// Es enthält Informationen über die Größe, den Typ und die Daten des Assets. 
/// </summary>
public readonly struct AssetData : IDisposable
{
    // Speicher reservieren  vom ArrayPool
    public IMemoryOwner<byte> Owner { get; }

    // Sichere Zugriff auf die Daten aber nur lesend
    public ReadOnlyMemory<byte> Bytes => Owner.Memory;

    // Name des Assets x.png, y.obj, z.wav etc.
    public string Name { get; }

    //Konstruktor
    public AssetData(IMemoryOwner<byte> owner, string name)
    {
        Owner = owner;
        Name = name;
    }

    public void Dispose()
    {
        //Array wird freigegeben und zurückgegeben an den Pool
        Owner?.Dispose();
    }
}
