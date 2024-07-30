using UnityEngine;

namespace Tests.Runtime
{
    [CreateAssetMenu(fileName = "ComponentTestData", menuName = "Test/ComponentTest/Data", order = 0)]
    public class ComponentTestData : ScriptableObject
    {
        public GameObject prefab;
        public int iterations = 1000000;
        public int measurementCount = 100;
        public int warmUpCount = 5;
    }
}