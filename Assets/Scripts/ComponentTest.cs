using UnityEngine;
using System.Diagnostics;

public class ComponentTest : MonoBehaviour
{
    public int iterations = 1000000;

    private void Start()
    {
        RunTest();
    }

    private void RunTest()
    {
        // GetComponent with Component
        var stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            var component = GetComponent<DamageableComponent>();
        }
        stopwatch.Stop();
        UnityEngine.Debug.Log("GetComponent<DamageableComponent> time: " + stopwatch.ElapsedMilliseconds + " ms");

        // GetComponent with Interface
        stopwatch.Reset();
        stopwatch.Start();
        for (int i = 0; i < iterations; i++)
        {
            var component = GetComponent<IDamageable>();
        }
        stopwatch.Stop();
        UnityEngine.Debug.Log("GetComponent<IDamageable> time: " + stopwatch.ElapsedMilliseconds + " ms");
    }
}