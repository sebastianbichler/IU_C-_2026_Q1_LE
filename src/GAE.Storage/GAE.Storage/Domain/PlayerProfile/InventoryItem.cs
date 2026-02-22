namespace GAE.Domain.PlayerProfile;

public sealed record InventoryItem
{
    public string ItemCode { get; }
    public int Quantity { get; }

    public InventoryItem(string itemCode, int quantity)
    {
        if (string.IsNullOrWhiteSpace(itemCode))
            throw new ArgumentException("Item code required.", nameof(itemCode));

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        ItemCode = itemCode;
        Quantity = quantity;
    }

    public InventoryItem Add(int amount) =>
        amount <= 0
            ? throw new ArgumentOutOfRangeException(nameof(amount))
            : this with { Quantity = Quantity + amount };
}
