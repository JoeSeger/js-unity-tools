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
        private GameObject _testObject;
        private ComponentTestData _componentTestData;

        [SetUp]
        public void SetUp()
        {
            _componentTestData = Resources.FindObjectsOfTypeAll<ComponentTestData>().FirstOrDefault();
            if (_componentTestData != null && _componentTestData.prefab != null)
            {
                _testObject = Object.Instantiate(_componentTestData.prefab);
            }
            else
            {
                _testObject = new GameObject("Performance Test Object ");
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (_testObject == null) return;
            Object.DestroyImmediate(_testObject);
            _testObject = null;
        }

        protected IEnumerator RunTestCoroutine(string sampleGroup, Type type)
        {
            if (_componentTestData == null || _testObject == null) yield break;

            // Ensure the test object has the required component
            if (!_testObject.TryGetComponent(type, out var _))
            {
                _testObject.AddComponent(type);
            }

            // Warm-up phase
            for (var i = 0; i < _componentTestData.warmUpCount; i++)
            {
                _testObject.TryGetComponent(type, out _);
                yield return null;
            }

            if (_componentTestData.useStopWatch)
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
            for (var j = 0; j < _componentTestData.iterations; j++)
            {
                _testObject.TryGetComponent(type, out _);
            }
            stopwatch.Stop();
            UnityEngine.Debug.Log($"{sampleGroup} took {stopwatch.ElapsedMilliseconds} ms");
            yield return null;
        }

        private IEnumerator MeasureUsingUnityPerfTest(string sampleGroup, Type type)
        {
            Measure.Method(() =>
            {
                for (var j = 0; j < _componentTestData.iterations / _componentTestData.measurementCount; j++)
                {
                    _testObject.TryGetComponent(type, out _);
                }
            })
            .SampleGroup(sampleGroup)
            .MeasurementCount(_componentTestData.measurementCount)
            .WarmupCount(_componentTestData.warmUpCount)
            .Run();

            yield return null;
        }

        protected IEnumerator RunEditorTestCoroutine(string sampleGroup, Type type)
        {
            if (_componentTestData == null || _testObject == null) yield break;

            // Ensure the test object has the required component
            if (!_testObject.TryGetComponent(type, out var _))
            {
                _testObject.AddComponent(type);
            }

            if (_componentTestData.useStopWatch)
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
            for (var j = 0; j < _componentTestData.iterations; j++)
            {
                _testObject.TryGetComponent(type, out _);
            }
            stopwatch.Stop();
            UnityEngine.Debug.Log($"{sampleGroup} took {stopwatch.ElapsedMilliseconds} ms");
            yield return null;
        }

        private IEnumerator MeasureUsingUnityPerfTestEditor(string sampleGroup, Type type)
        {
            Measure.Method(() =>
            {
                for (var j = 0; j < _componentTestData.iterations / _componentTestData.measurementCount; j++)
                {
                    _testObject.TryGetComponent(type, out _);
                }
            })
            .SampleGroup(sampleGroup)
            .MeasurementCount(_componentTestData.measurementCount)
            .WarmupCount(_componentTestData.warmUpCount)
            .Run();

            yield return null;
        }
    }
}
