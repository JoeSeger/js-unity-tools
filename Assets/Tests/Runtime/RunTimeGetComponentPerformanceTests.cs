using System.Collections;
using Unity.PerformanceTesting;
using UnityEngine.TestTools;

namespace Tests.Runtime
{
    public class RunTimeGetComponentPerformanceTests : GetComponentPerformanceTestsBase
    {
        [UnityTest, Performance]
        public IEnumerator TestGetComponent_DamageableComponent()
        {
            yield return RunTestCoroutine("GetComponent<DamageableComponent>", typeof(DamageableComponent));
        }

        [UnityTest, Performance]
        public IEnumerator TestGetComponent_IDamageable()
        {
            yield return RunTestCoroutine("GetComponent<IDamageable>", typeof(IDamageable));
        }
    }
}