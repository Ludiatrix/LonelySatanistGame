using LSG.Effects;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG
{
    [CreateAssetMenu(fileName = "DemonData_Priocept", menuName = "LSG/Demons/Make DemonData_Priocept")]
    public class DemonData_Priocept : DemonData
    {
        public override void ApplyEffect()
        {
            DataManager.Instance.gameObject.AddComponent<PrioceptEffects>();
        }
    }
}
