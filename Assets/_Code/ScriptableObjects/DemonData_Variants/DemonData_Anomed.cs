using LSG.Effects;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG
{
    [CreateAssetMenu(fileName = "DemonData_Anomed", menuName = "LSG/Demons/Make DemonData_Anomed")]
    public class DemonData_Anomed : DemonData
    {
        public override void ApplyEffect()
        {
            DataManager.Instance.gameObject.AddComponent<AnomedEffects>();
        }
    }
}