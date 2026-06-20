using LSG.Effects;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG
{
    [CreateAssetMenu(fileName = "DemonData_Tentaculara", menuName = "LSG/Demons/Make DemonData_Tentaculara")]
    public class DemonData_Tentaculara : DemonData
    {
        public override void ApplyEffect()
        {
            DataManager.Instance.gameObject.AddComponent<TentacularaEffects>();
        }
    }
}
