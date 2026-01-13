class Comparator
{
    public static boolean CompareA(int i, int j)
    {
        return (i - 4) > (j * j);
    }

    public static boolean CompareB(int i, int j)
    {
        return (i >= 0) || (i == j);
    }

    public static boolean CompareC(int i, int j)
    {
        return (i != j) && i > -j && i < j;
    }

    public static boolean CompareD(int i, int j)
    {
        return (i != 0) && (i >= j);
    }
}