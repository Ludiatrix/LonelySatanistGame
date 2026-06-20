using LSG.Effects;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG
{
    [CreateAssetMenu(fileName = "DemonData_Beelzebabe", menuName = "LSG/Demons/Make DemonData_Beelzebabe")]
    public class DemonData_Beelzebabe : DemonData
    {
        public override void ApplyEffect()
        {
            DataManager.Instance.gameObject.AddComponent<BeelzebabeEffects>();
        }
    }
}