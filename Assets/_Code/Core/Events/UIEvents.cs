using UnityEngine.Events;

namespace LSG.Core
{
    public static class UIEvents
    {
        public static readonly UnityEventExtensions.UnityBoolEvent ToggleDialogueWindow = new UnityEventExtensions.UnityBoolEvent();
        public static readonly UnityEventExtensions.UnityStringEvent SetNamePlateText = new UnityEventExtensions.UnityStringEvent();
        public static readonly UnityEventExtensions.UnityStringEvent SetDialogueText = new UnityEventExtensions.UnityStringEvent();
        public static readonly UnityEventExtensions.UnityBoolEvent ToggleSummoningButtons = new UnityEventExtensions.UnityBoolEvent();
        public static readonly UnityEventExtensions.UnityBoolEvent ToggleEncounterButtons = new UnityEventExtensions.UnityBoolEvent();
        public static readonly UnityEventExtensions.UnityBoolEvent ToggleStoreButtons = new UnityEventExtensions.UnityBoolEvent();
        public static readonly UnityEventExtensions.UnityBoolEvent FlipDialogueText = new UnityEventExtensions.UnityBoolEvent();
        public static readonly UnityEvent DisableButtons = new UnityEvent();
        public static UnityEventExtensions.UnityBoolEvent TogglePageArt = new UnityEventExtensions.UnityBoolEvent();
        public static readonly UnityEvent StoreButtonClicked = new UnityEvent();
    }
}