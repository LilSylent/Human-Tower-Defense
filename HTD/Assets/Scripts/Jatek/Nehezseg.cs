public enum Nehezsegek { Könnyű = 0, Közepes = 1, Nehéz = 2 };

public class Nehesegvezerlo
{
    private static Nehezsegek nehezseg;

    public static Nehezsegek Nehezseg { get => nehezseg; set => nehezseg = value; }
}
