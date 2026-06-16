using System;
using UnityEngine;

namespace LSG
{
    /// <summary>
    /// PhaseObjects are just handy shorthand for holding state-object relationships.c
    /// </summary>
    [Serializable]
    public class PhaseObject
    {
        public GameObject GameObject;
        public Enums.GameState Phase;
    }
}