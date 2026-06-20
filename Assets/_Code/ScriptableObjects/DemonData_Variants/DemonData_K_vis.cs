using LSG.Effects;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG
{
    [CreateAssetMenu(fileName = "DemonData_K_vis", menuName = "LSG/Demons/Make DemonData_K_vis")]
    public class DemonData_K_vis : DemonData
    {
        public override void ApplyEffect()
        {
            DataManager.Instance.gameObject.AddComponent<K_visEffects>();
        }
    }
}
