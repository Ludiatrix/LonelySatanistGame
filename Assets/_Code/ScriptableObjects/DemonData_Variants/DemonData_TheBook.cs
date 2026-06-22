using LSG.Effects;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DemonData_TheBook", menuName = "LSG/Demons/Make DemonData_TheBook")]
    public class DemonData_TheBook : DemonData
    {
        public override void ApplyEffect()
        {
            Debug.Log("Book Book!");
        }
    }
}