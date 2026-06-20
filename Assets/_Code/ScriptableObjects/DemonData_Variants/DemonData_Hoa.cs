using LSG.Effects;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG
{
    [CreateAssetMenu(fileName = "DemonData_Hoa", menuName = "LSG/Demons/Make DemonData_Hoa")]
    public class DemonData_Hoa : DemonData
    {
        public override void ApplyEffect()
        {
            DataManager.Instance.gameObject.AddComponent<HoaEffects>();
        }
    }
}