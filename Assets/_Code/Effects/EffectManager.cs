using System;
using System.Collections.Generic;
using LSG.Interfaces;
using UnityEngine;

namespace LSG.Effects
{
    public class EffectManager : MonoBehaviour
    {
        private List<IEffectable> _demonEffects = new List<IEffectable>();

        public void AddEffect<T>() where T : MonoBehaviour, IEffectable
        {
            if (gameObject.TryGetComponent<T>(out _))
                return;

            T effect = gameObject.AddComponent<T>();
            _demonEffects.Add(effect);
        }
    }
}