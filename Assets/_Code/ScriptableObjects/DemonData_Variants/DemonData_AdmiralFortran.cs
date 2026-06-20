using LSG.Effects;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG
{
    [CreateAssetMenu(fileName = "DemonData_AdmiralFortran", menuName = "LSG/Demons/Make DemonData_AdmiralFortran")]
    public class DemonData_AdmiralFortran : DemonData
    {
        public override void ApplyEffect()
        {
            DataManager.Instance.gameObject.AddComponent<AdmiralFortranEffects>();
        }
    }
}