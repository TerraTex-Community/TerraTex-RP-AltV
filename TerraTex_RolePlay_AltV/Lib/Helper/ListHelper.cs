namespace TerraTex_RolePlay_AltV_Server.Lib.Helper;

public static class ListHelper
{
    public static void Shuffle<T>(this IList<T> list, int howOftenToShuffle = 1)
    {
        Random rnd = new Random();
        for (int i = 0; i < howOftenToShuffle; i++)
        {
            int n = list.Count;
            while (n > 1)
            {
                int k = rnd.Next(0, n) % n;
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}