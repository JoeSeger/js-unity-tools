using System.Collections;
using Tests.Runtime;
using Unity.PerformanceTesting;
using UnityEngine.TestTools;

namespace Tests.Editor
{
    public class EditorGetComponentPerformanceTests : GetComponentPerformanceTestsBase
    {
        [UnityTest, Performance]
        public IEnumerator TestGetComponent_DamageableComponent()
        {
            yield return RunEditorTestCoroutine("GetComponent<DamageableComponent>", typeof(DamageableComponent));
        }

        [UnityTest, Performance]
        public IEnumerator TestGetComponent_IDamageable()
        {
            yield return RunEditorTestCoroutine("GetComponent<IDamageable>", typeof(IDamageable));
        }
    }
}