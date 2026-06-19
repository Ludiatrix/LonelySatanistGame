using LSG.Effects;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG
{
    [CreateAssetMenu(fileName = "DemonData_Ekzeema", menuName = "LSG/Demons/Make DemonData_Ekzeema")]
    public class DemonData_Ekzeema : DemonData
    {
        public override void ApplyEffect()
        {
            DataManager.Instance.gameObject.AddComponent<EkzeemaEffects>();
        }
    }
}
