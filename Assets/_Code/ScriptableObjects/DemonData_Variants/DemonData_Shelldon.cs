using LSG.Effects;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG
{
    [CreateAssetMenu(fileName = "DemonData_Shelldon", menuName = "LSG/Demons/Make DemonData_Shelldon")]
    public class DemonData_Shelldon : DemonData
    {
        public override void ApplyEffect()
        {
            DataManager.Instance.gameObject.AddComponent<ShelldonEffects>();
        }
    }
}
