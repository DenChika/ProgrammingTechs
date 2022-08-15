using System.Collections.Generic;

namespace DenChika_Analyzer.Test;

public class TestClass
{
    public Dictionary<List<List<int>>[], Dictionary<string[], List<int>[]>> dictionary { get; set; }
    public List<int[]> TestMethod1(List<List<int>> list, List<string> ling)
    {
        var x = new List<int[]>();
        return x;
    }

    private List<int> TestMethod2(List<List<int>> list, List<string> ling)
    {
        var x = new List<int>();
        return x;
    }

    public int TestMethod3(int magic1 = 10)
    {
        string magic2 = "magic";
        magic2 = "MaGiC";
        string magic3 = string.Empty;
        string magic4 = magic2;
        if (magic3 == "MaGiC")
        {
            string magic5 = "magic";
            magic3 = "MAGIC";
            int a = 10;
        }
        for (int i = 0; i < 3; i++)
        {
            magic3 += "magiC";
        }

        return magic1;
    }

    public int TestMethod4()
    {
        return TestMethod3(5);
    }
}