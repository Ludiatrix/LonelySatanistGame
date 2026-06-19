using LSG.Effects;
using LSG.Core;
using LSG.Interfaces;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG
{
    [CreateAssetMenu(fileName = "DemonData_Sweenglyne", menuName = "LSG/Demons/Make DemonData_Sweenglyne")]
    public class DemonData_Sweenglyne : DemonData
    {
        public override void ApplyEffect()
        {
            DataManager.Instance.EffectDataSource.AddEffect<SweenglyneEffects>();
        }
    }
}
