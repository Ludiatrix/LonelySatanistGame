using LSG.Effects;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG
{
    [CreateAssetMenu(fileName = "DemonData_Inari", menuName = "LSG/Demons/Make DemonData_Inari")]
    public class DemonData_Inari : DemonData
    {
        public override void ApplyEffect()
        {
            DataManager.Instance.gameObject.AddComponent<InariEffects>();
        }
    }
}
