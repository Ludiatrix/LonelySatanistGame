using LSG.ScriptableObjects;
using UnityEngine.Events;

namespace LSG.Core
{
    public class UnityEventExtensions
    {
        public class UnityStateEvent : UnityEvent<Enums.GameState> { }
        public class UnityPageReadEvent : UnityEvent { }
        public class UnityPageDataEvent : UnityEvent<PageData> { }
        public class UnityDemonDataEvent : UnityEvent<DemonData> { }
        public class UnityIntEvent : UnityEvent<int> { }
        public class UnityBoolEvent : UnityEvent<bool> { }
        public class UnityStringEvent : UnityEvent<string> { }
    }
}