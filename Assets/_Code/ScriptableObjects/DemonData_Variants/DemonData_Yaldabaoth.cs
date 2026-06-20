using LSG.Effects;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG
{
    [CreateAssetMenu(fileName = "DemonData_Yaldabaoth", menuName = "LSG/Demons/Make DemonData_Yaldabaoth")]
    public class DemonData_Yaldabaoth : DemonData
    {
        public override void ApplyEffect()
        {
            DataManager.Instance.gameObject.AddComponent<YaldabaothEffects>();
        }
    }
}
