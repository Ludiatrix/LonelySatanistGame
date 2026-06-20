using LSG.Effects;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG
{
    [CreateAssetMenu(fileName = "DemonData_Papiyawn", menuName = "LSG/Demons/Make DemonData_Papiyawn")]
    public class DemonData_Papiyawn : DemonData
    {
        public override void ApplyEffect()
        {
            Debug.Log("Bark Bark!");
        }
    }
}
