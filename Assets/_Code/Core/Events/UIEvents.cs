using UnityEngine.Events;
using static LSG.Core.UnityEventExtensions;

namespace LSG.Core
{
    public static class UIEvents
    {
        public static readonly UnityEventExtensions.UnityBoolEvent ToggleDialogueWindow = new UnityEventExtensions.UnityBoolEvent();
        public static readonly UnityEventExtensions.UnityStringEvent SetNamePlateText = new UnityEventExtensions.UnityStringEvent();
        public static readonly UnityEventExtensions.UnityStringEvent SetDialogueText = new UnityEventExtensions.UnityStringEvent();
        public static readonly UnityEventExtensions.UnityStringEvent AppendDialogueText = new UnityEventExtensions.UnityStringEvent();
        public static readonly UnityEventExtensions.UnityBoolEvent ToggleSummoningButtons = new UnityEventExtensions.UnityBoolEvent();
        public static readonly UnityEventExtensions.UnityBoolEvent ToggleEncounterButtons = new UnityEventExtensions.UnityBoolEvent();
        public static readonly UnityEventExtensions.UnityBoolEvent ToggleStoreButtons = new UnityEventExtensions.UnityBoolEvent();
        // Shows/hides the "Use Optional Power" button shown alongside the summoning buttons.
        public static readonly UnityEventExtensions.UnityBoolEvent ToggleOptionalButton = new UnityEventExtensions.UnityBoolEvent();
        public static readonly UnityEventExtensions.UnityBoolEvent ToggleEndgameButtons = new UnityEventExtensions.UnityBoolEvent();
        public static readonly UnityEventExtensions.UnityBoolEvent FlipDialogueText = new UnityEventExtensions.UnityBoolEvent();
        public static readonly UnityEventExtensions.UnityBoolEvent ToggleResourceUI = new UnityEventExtensions.UnityBoolEvent();
        public static readonly UnityEvent DisableButtons = new UnityEvent();
        public static UnityEventExtensions.UnityBoolEvent TogglePageArt = new UnityEventExtensions.UnityBoolEvent();
        public static readonly UnityEvent StoreButtonClicked = new UnityEvent();
        public static readonly UnityCardDataEvent DisplayNecronomiconPage = new UnityCardDataEvent();
        public static readonly UnityEvent TurnNecronomiconPage = new UnityEvent();
        public static readonly UnityBoolEvent ToggleNecronomicon = new UnityBoolEvent();
    }
}