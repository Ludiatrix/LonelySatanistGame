
using LSG.Effects;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG
{
    [CreateAssetMenu(fileName = "DemonData_Chad", menuName = "LSG/Demons/Make DemonData_Chad")]
    public class DemonData_Chad : DemonData
    {
        public override void ApplyEffect()
        {
            DataManager.Instance.gameObject.AddComponent<ChadEffects>();
        }
    }
}