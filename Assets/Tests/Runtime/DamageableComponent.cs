﻿using UnityEngine;

namespace Tests.Runtime
{
    public interface IDamageable
    {
        void TakeDamage(int amount);
    }

    public class DamageableComponent : MonoBehaviour, IDamageable
    {
        public void TakeDamage(int amount)
        {
            // Implementation here
        }
    }
}