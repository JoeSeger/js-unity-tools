using System.Collections;
using System.Linq;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime
{
    public class RunTimeGetComponentPerformanceTests
    {
        private GameObject _testObject;
        private ComponentTestData _componentTestData;

        [SetUp]
        public void SetUp()
        {
            _componentTestData = Resources.FindObjectsOfTypeAll<ComponentTestData>().FirstOrDefault();
            if (_componentTestData != null)
            {
                _testObject = Object.Instantiate(_componentTestData.prefab);
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (_testObject != null)
            {
                Object.DestroyImmediate(_testObject);
            }
        }

        [UnityTest, Performance]
        public IEnumerator TestGetComponent_DamageableComponent()
        {
            yield return RunTest("GetComponent<DamageableComponent>", typeof(DamageableComponent));
        }

        [UnityTest, Performance]
        public IEnumerator TestGetComponent_IDamageable()
        {
            yield return RunTest("GetComponent<IDamageable>", typeof(IDamageable));
        }

        private IEnumerator RunTest(string sampleGroup, System.Type type)
        {
            if (_componentTestData == null || _testObject == null) yield break;

            // Warm-up phase
            for (int i = 0; i < _componentTestData.warmUpCount; i++)
            {
                _testObject.TryGetComponent(type, out _);
                yield return null;
            }

            // Measurement phase
            Measure.Method(() =>
            {
                for (int j = 0; j < _componentTestData.iterations / _componentTestData.measurementCount; j++)
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
