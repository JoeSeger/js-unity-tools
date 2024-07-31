using System;
using System.Linq;
using System.Collections;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tests.Runtime
{
    public abstract class GetComponentPerformanceTestsBase
    {
        protected GameObject TestObject;
        protected ComponentTestData ComponentTestData;

        [SetUp]
        public void SetUp()
        {
            ComponentTestData = Resources.FindObjectsOfTypeAll<ComponentTestData>().FirstOrDefault();
            if (ComponentTestData != null)
            {
                TestObject = Object.Instantiate(ComponentTestData.prefab);
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (TestObject != null)
            {
                Object.DestroyImmediate(TestObject);
                TestObject = null;
            }
        }

        protected IEnumerator RunTestCoroutine(string sampleGroup, Type type)
        {
            if (ComponentTestData == null || TestObject == null) yield break;

            // Warm-up phase
            for (int i = 0; i < ComponentTestData.warmUpCount; i++)
            {
                TestObject.TryGetComponent(type, out _);
                yield return null;
            }

            if (ComponentTestData.useStopWatch)
            {
                yield return MeasureUsingStopwatch(sampleGroup, type);
            }
            else
            {
                yield return MeasureUsingUnityPerfTest(sampleGroup, type);
            }
        }

        private IEnumerator MeasureUsingStopwatch(string sampleGroup, Type type)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for (int j = 0; j < ComponentTestData.iterations; j++)
            {
                TestObject.TryGetComponent(type, out _);
            }
            stopwatch.Stop();
            UnityEngine.Debug.Log($"{sampleGroup} took {stopwatch.ElapsedMilliseconds} ms");
            yield return null;
        }

        private IEnumerator MeasureUsingUnityPerfTest(string sampleGroup, Type type)
        {
            Measure.Method(() =>
            {
                for (int j = 0; j < ComponentTestData.iterations / ComponentTestData.measurementCount; j++)
                {
                    TestObject.TryGetComponent(type, out _);
                }
            })
            .SampleGroup(sampleGroup)
            .MeasurementCount(ComponentTestData.measurementCount)
            .WarmupCount(ComponentTestData.warmUpCount)
            .Run();

            yield return null;
        }

        protected IEnumerator RunEditorTestCoroutine(string sampleGroup, Type type)
        {
            if (ComponentTestData == null || TestObject == null) yield break;

            if (ComponentTestData.useStopWatch)
            {
                yield return MeasureUsingStopwatchEditor(sampleGroup, type);
            }
            else
            {
                yield return MeasureUsingUnityPerfTestEditor(sampleGroup, type);
            }
        }

        private IEnumerator MeasureUsingStopwatchEditor(string sampleGroup, Type type)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for (int j = 0; j < ComponentTestData.iterations; j++)
            {
                TestObject.TryGetComponent(type, out _);
            }
            stopwatch.Stop();
            UnityEngine.Debug.Log($"{sampleGroup} took {stopwatch.ElapsedMilliseconds} ms");
            yield return null;
        }

        private IEnumerator MeasureUsingUnityPerfTestEditor(string sampleGroup, Type type)
        {
            Measure.Method(() =>
            {
                for (int j = 0; j < ComponentTestData.iterations / ComponentTestData.measurementCount; j++)
                {
                    TestObject.TryGetComponent(type, out _);
                }
            })
            .SampleGroup(sampleGroup)
            .MeasurementCount(ComponentTestData.measurementCount)
            .WarmupCount(ComponentTestData.warmUpCount)
            .Run();

            yield return null;
        }
    }
}
