using LSG.Effects;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG
{
    [CreateAssetMenu(fileName = "DemonData_Coldkatta", menuName = "LSG/Demons/Make DemonData_Coldkatta")]
    public class DemonData_Coldkatta : DemonData
    {
        public override void ApplyEffect()
        {
            DataManager.Instance.gameObject.AddComponent<ColdkattaEffects>();
        }
    }
}