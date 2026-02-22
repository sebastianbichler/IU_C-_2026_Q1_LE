namespace GAE.Domain.PlayerProfile;

public sealed class Inventory
{
    private readonly List<InventoryItem> _items = [];

    public IReadOnlyCollection<InventoryItem> Items => _items.AsReadOnly();

    public void AddItem(string itemCode, int quantity)
    {
        var existing = _items.FirstOrDefault(i => i.ItemCode == itemCode);

        if (existing is null)
        {
            _items.Add(new InventoryItem(itemCode, quantity));
            return;
        }

        _items.Remove(existing);
        _items.Add(existing.Add(quantity));
    }
}
