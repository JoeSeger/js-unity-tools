using System;
using System.Linq;
using NUnit.Framework;
using Tests.Runtime;
using Unity.PerformanceTesting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tests.Editor
{
    public class EditorGetComponentPerformanceTests
    {
        private GameObject _testObject;
        private ComponentTestData _componentTestData;

        [SetUp]
        public void SetUp()
        {
            _componentTestData = Resources.FindObjectsOfTypeAll<ComponentTestData>().FirstOrDefault();
            if (_componentTestData != null && _testObject == null)
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

        [Test, Performance]
        public void TestGetComponent_DamageableComponent() =>
            RunTest("GetComponent<DamageableComponent>", typeof(DamageableComponent));

        [Test, Performance]
        public void TestGetComponent_IDamageable() =>
            RunTest("GetComponent<IDamageable>", typeof(IDamageable));

        private void RunTest(string sampleGroup, Type type)
        {
            if (_componentTestData == null || _testObject == null) return;

            Measure.Method(() =>
                {
                    for (int i = 0; i < _componentTestData.iterations; i++)
                    {
                        _testObject.TryGetComponent(type, out var _);
                    }
                })
                .SampleGroup(sampleGroup)
                .MeasurementCount(_componentTestData.measurementCount)
                .WarmupCount(_componentTestData.warmUpCount)
                .Run();
        }
    }
}