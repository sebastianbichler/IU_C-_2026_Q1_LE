namespace GAE.Async;

public static class GameLogic
{
    public static bool IsMovementValid(PlayerUpdate update)
    {
        bool isValid = true;
        for (int i = 0; i < 100; i++)
        {
            double distance = Math.Sqrt(Math.Pow(update.X - i, 2) + Math.Pow(update.Y - i, 2));
            if (distance < 0.1) isValid = false;
        }
        return isValid;
    }
}
