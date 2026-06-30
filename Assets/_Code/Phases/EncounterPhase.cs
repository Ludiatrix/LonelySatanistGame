using System;
using System.Collections;
using LSG.Core;
using LSG.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace LSG.Phases
{
    /// <summary>
    /// The Encounter Phase!
    ///
    /// A successful stop rolls an entry on the Demon Encounter Table matching the accumulated power range. The demon introduces themselves via a short interactive dialogue segment.
    /// </summary>
    public class EncounterPhase : Phase
    {
        [SerializeField] private GameObject Container;
        [SerializeField] private Image demonImage;

        [Header("Portal Intro")]
        [Tooltip("The Portal_Animated_Sprite shown before the demon appears. Toggled on/off by this phase.")]
        [SerializeField] private GameObject portalAnimatedSprite;
        [Tooltip("Animator on the portal sprite, used to (re)start and time its animation. " +
                 "Optional — if unset we fall back to 'Portal Fallback Duration'.")]
        [SerializeField] private Animator portalAnimator;
        [Tooltip("Beat before the portal opens.")]
        [SerializeField] private float portalIntroDelay = 1.5f;
        [Tooltip("Used only if the portal animation length can't be read from the Animator.")]
        [SerializeField] private float portalFallbackDuration = 1f;
        [Tooltip("How long the summoned demon is shown before the dialogue window appears, " +
                 "so the player sees the demon get summoned before the dialogue starts.")]
        [SerializeField] private float dialogueDelay = 3f;

        [Header("Outcome Text")]
        [Tooltip("OutcomeText on the OutcomeTextCanvas. Faded in when the dice roll resolves.")]
        [SerializeField] private TMP_Text outcomeText;
        [SerializeField] private float outcomeFadeDuration = 0.5f;
        [SerializeField] private string acceptedText = "Date Accepted!";
        [SerializeField] private string rejectedText = "Rejected...";

        private PlayerEconomy _economy;
        private DemonData _chosenDemonThisPhase = null;
        private Coroutine _introRoutine;
        private Coroutine _outcomeRoutine;
        
        /// <summary>
        /// Start the Phase and Find a DemonData based on Power in the DemonDatingPool.
        /// </summary>
        public override void StartPhase()
        {
            Debug.Log("[EncounterPhase] Starting Phase!");
            base.StartPhase();
            Container.SetActive(true);
            UIEvents.ToggleResourceUI?.Invoke(false);
            FindAHottie();
            PhaseEvents.EncounterPhaseStarted?.Invoke();

            // Play the portal intro, which reveals the demon image when it finishes.
            if (_introRoutine != null) StopCoroutine(_introRoutine);
            _introRoutine = StartCoroutine(PortalIntro());
        }

        public override void EndPhase()
        {
            Debug.Log("[EncounterPhase] Ending Phase!");
            base.EndPhase();
            if (_introRoutine != null) StopCoroutine(_introRoutine);
            if (_outcomeRoutine != null) StopCoroutine(_outcomeRoutine);
            Container.SetActive(false);
            PhaseEvents.EncounterPhaseEnded?.Invoke();
        }

        /// <summary>
        /// The summon sequence: beat, open the portal, play its animation once, reveal the
        /// demon, then after a pause fire DemonEncountered to start the dialogue. The demon
        /// and portal are hidden up front so nothing shows until the portal opens, and the
        /// dialogue is held back so the player sees the demon summoned before it starts.
        /// </summary>
        private IEnumerator PortalIntro()
        {
            // Hide any dialogue left over from the previous phase (e.g. the card-effect text
            // shown during Summoning). It's re-shown for the demon once the summon finishes,
            // when DemonEncountered fires. Forced encounters (Papiyawn / The Book) reach here
            // without the player clicking "Stop", so Summoning never hid it on its way out.
            UIEvents.ToggleDialogueWindow?.Invoke(false);

            // The outcome text stays hidden until this encounter's dice roll resolves.
            if (outcomeText != null) outcomeText.alpha = 0f;

            if (demonImage != null) demonImage.enabled = false;
            if (portalAnimatedSprite != null) portalAnimatedSprite.SetActive(false);

            yield return new WaitForSeconds(portalIntroDelay);

            // Open the portal. Re-activating the object auto-plays the Animator's default
            // (non-looping) state from frame 0, so no explicit Play() call is needed.
            if (portalAnimatedSprite != null) portalAnimatedSprite.SetActive(true);

            // Wait for the animation to finish. We time it by the clip's own length rather
            // than polling Animator.normalizedTime, which can hang if the state never reports
            // exactly >= 1. WaitForSeconds always terminates, so the demon reveal always runs.
            float wait = portalFallbackDuration;
            if (portalAnimator != null)
            {
                yield return null; // let the Animator enter its state so the clip length is valid
                AnimatorStateInfo state = portalAnimator.GetCurrentAnimatorStateInfo(0);
                float speed = Mathf.Abs(portalAnimator.speed);
                if (state.length > 0f && speed > 0f) wait = state.length / speed;
            }
            yield return new WaitForSeconds(wait);

            // Close the portal and reveal the demon.
            if (portalAnimatedSprite != null) portalAnimatedSprite.SetActive(false);
            if (demonImage != null) demonImage.enabled = true;

            // Let the player take in the summoned demon, then start the dialogue. Firing
            // DemonEncountered here (rather than in FindAHottie) is what opens the dialogue
            // window and its encounter buttons.
            yield return new WaitForSeconds(dialogueDelay);
            GameEvents.DemonEncountered?.Invoke(_chosenDemonThisPhase);

            _introRoutine = null;
        }

        private void OnEnable()
        {
            GameEvents.TryToDateChosen?.AddListener(OnTryToDateChosen);
            GameEvents.GiveUpChosen?.AddListener(OnGiveUpChosen);
            GameEvents.DiceRollResult?.AddListener(OnDiceRollResult);
        }

        private void OnDisable()
        {
            GameEvents.TryToDateChosen?.RemoveListener(OnTryToDateChosen);
            GameEvents.GiveUpChosen?.RemoveListener(OnGiveUpChosen);
            GameEvents.DiceRollResult?.RemoveListener(OnDiceRollResult);
        }

        private void Start()
        {
            _economy = DataManager.Instance.PlayerEconomySource;
        }

        private void OnTryToDateChosen()
        {
            GoOnDate();
        }

        private void GoOnDate()
        {
            // Begin the DICE ROLL!
            GameEvents.DiceRollRequest?.Invoke();
        }

        private void OnDiceRollResult(int d20RollResult)
        {
            bool accepted = d20RollResult <= _economy.Rizz;

            // Fade in the outcome banner ("Date Accepted!" / "Rejected...").
            ShowOutcomeText(accepted);

            if (accepted)
            {
                SucceedDate();
            }
            else
            {
                // Longshot consolation: gain Rizz only after a failed date roll.
                _economy.Rizz++;
                FailDate();
            }
        }

        /// <summary>Sets the outcome banner text and fades it from transparent to opaque.</summary>
        private void ShowOutcomeText(bool accepted)
        {
            if (outcomeText == null) return;
            outcomeText.text = accepted ? acceptedText : rejectedText;
            if (_outcomeRoutine != null) StopCoroutine(_outcomeRoutine);
            _outcomeRoutine = StartCoroutine(FadeInOutcomeText());
        }

        private IEnumerator FadeInOutcomeText()
        {
            float elapsed = 0f;
            outcomeText.alpha = 0f;
            while (elapsed < outcomeFadeDuration)
            {
                elapsed += Time.deltaTime;
                outcomeText.alpha = Mathf.Clamp01(elapsed / outcomeFadeDuration);
                yield return null;
            }
            outcomeText.alpha = 1f;
            _outcomeRoutine = null;
        }

        private void OnGiveUpChosen()
        {
            FailDate();
        }

        private void SucceedDate()
        {
            // Date Succeeds!
            Debug.Log("[EncounterPhase] You have succeeded the date!");

            // The win condition: from now on the Title screen shows credits, not the intro.
            GameEvents.DateSucceeded?.Invoke();

            if (_chosenDemonThisPhase.demonName == "Beelzebabe")
            {
                // Look let's just make this easy on ourselves here
                UIEvents.ToggleEncounterButtons?.Invoke(false);
                GameEvents.ChangeState?.Invoke(Enums.GameState.WinPhase);
            } else
            {
                UIEvents.SetDialogueText?.Invoke(_chosenDemonThisPhase?.dateOutcome);
                // Winning a date ends the game: show the End Game button instead of Store.
                UIEvents.ToggleEndgameButtons?.Invoke(true);
            }
        }

        private void FailDate()
        {
            // Show the narrative outcome of the failed date and apply the boon/bane
            // effects. (The boonBaneDialogue is shown later, when an effect actually
            // fires during play — not here.)
            Debug.Log("[EncounterPhase] You have failed the date!");
            UIEvents.ToggleEncounterButtons?.Invoke(false);
            UIEvents.SetDialogueText?.Invoke(_chosenDemonThisPhase?.outcome);
            _chosenDemonThisPhase?.ApplyEffect();
        }

        // Sends the
        private void FindAHottie()
        {
            // Get a demon
            _chosenDemonThisPhase = DataManager.Instance.DemonDatingPoolSource.EncounterDemonBasedOnPower(_economy.Power);
            
            // Set up the demon visual. The Dialogue Window (and the DemonEncountered event that
            // drives it) is held back until the summon sequence finishes — see PortalIntro.
            demonImage.sprite = _chosenDemonThisPhase.demonSprite;
            demonImage.SetNativeSize();
        }
    }
}