using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JimmysUnityUtilities.Random;

public class RandomTest : MonoBehaviour
{
    static VeryFastRandomValueGenerator Cum;

    public int seed;

    void AddValue()
    {
        if (Cum == null)
            Cum = new VeryFastRandomValueGenerator(seed);

        results += Cum.GetNextRandom64Bits().ToString();
        results += '\n';
    }



    void TestPerformance()
    {
        var random = new VeryFastRandomValueGenerator(seed);
        var sw = new System.Diagnostics.Stopwatch();

        sw.Start();
        for (int i = 0; i < 1000000000; i++)
        {
            random.GetNextRandom64Bits();
        }
        sw.Stop();

        UnityEngine.Debug.Log($"Executed in {sw.Elapsed}");
    }



    private void OnValidate()
    {
        TestShit();
    }

    public int CumsPerValidation = 1;
    public double probabilityOfTrue;

    static JRandom Sex;
    void TestShit()
    {
        if (Sex == null)
            Sex = new JRandom(seed);



        results = string.Empty;
        for (int i = 0; i < CumsPerValidation; i++)
        {
            results += Sex.Chance(probabilityOfTrue);
            results += '\n';
        }
    }

    private static void TestStatistics()
    {
        int counter = 0;
        for (int i = 0; i < 10000; i++)
        {
            if (Sex.Byte() == byte.MinValue)
                counter++;
        }

        Debug.Log(counter);
    }



    [TextArea(minLines: 20, maxLines: 100000)]
    public string results;
}