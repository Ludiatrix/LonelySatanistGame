using System;
using System.Collections.Generic;
using System.Linq;
using LSG.Core;
using LSG.Interfaces;
using LSG.ScriptableObjects;
using UnityEngine;
using static LSG.Core.Enums;

namespace LSG.Effects
{
    public class EffectManager : MonoBehaviour
    {
        private List<IEffectable> _demonEffects = new List<IEffectable>();

        
        public void AddEffect<T>() where T : MonoBehaviour, IEffectable
        {
            if (gameObject.TryGetComponent<T>(out _))
                return;

            T effect = gameObject.AddComponent<T>();
            _demonEffects.Add(effect);
        }

        public void ResolveCardEffect(CardData card)
        {
            if (card == null)
                return;

            switch (card.EffectType)
            {
                case CardEffectType.None:
                    break;
                case CardEffectType.ShuffleReadPageBackIntoDeck:
                    ShuffleReadPageThisSummoningRoundBackIntoDeck();
                    break;
                case CardEffectType.RemoveCardFromThisSummoningRoundWithTapeCostOne:
                    RemoveCardFromThisSummoningRoundWithTapeCostOfOneAndAddBackToStoreDoNotTouchPower();
                    break;
                case CardEffectType.ReturnWhitePagesToDeckAndRemovePower:
                    ReturnWhitePagesToDeckAndShuffleAndRemovePowerFromBar();
                    break;
                case CardEffectType.RepairRandomCardAtNoTapeCost:
                    RepairRandomCardAtNoTapeCost();
                    break;
                case CardEffectType.GainPowerForEachOrangeRead:
                    GetOnePowerForEachOrangeCardReadThisSummoningRound();
                    break;
                case CardEffectType.ReturnRandomCardToDeckDoNotTouchPower:
                    ReturnRandomCardToDeckAndShuffleDoNotTouchPower();
                    break;
                case CardEffectType.PeekAtTwoReadOneAndShuffleTheRest:
                    PeekAtTwoReadOneAndShuffleTheRest();
                    break;
                case CardEffectType.PeekAtTwoReadOneAndLeaveOneAsideUntilNextSummoningRound:
                    PeekAtTwoReadOneAndLeaveOneAsideUntilNextSummoningRound();
                    break;
                case CardEffectType.PeekAtTwoReadUpToTwoShuffleAnyNotReadToBottomOfDeck:
                    PeekAtTwoReadUpToTwoShuffleAnyNotReadToBottomOfDeck();
                    break;
                default:
                    Debug.LogWarning($"Unhandled card effect type: {card.EffectType}");
                    break;
            }
        }

        // Optional: Peek at the next 2 pages. You may choose one page to read and shuffle any you choose not to read back into the book.
        public void PeekAtTwoReadOneAndShuffleTheRest()
        {
            CardData[] cards = DataManager.Instance.PlayerDeckSource.PeekAheadAtPlayerDeck(2);

            PickACardPayload payload = new PickACardPayload(
                "Peek at the next 2 pages. You may choose one page to read and shuffle any you choose not to read back into the book.",
                cards.ToList(), 1, PickACardAfterEffectState.ShuffleRestIntoDeck);

            GameEvents.PickACard?.Invoke(payload);
        }

        // Optional: Peek at the next 2 pages. You may choose one page to read. Any of the 2 pages not read are set aside and do not shuffle back into the Necronomicon until the next summoning.
        public void PeekAtTwoReadOneAndLeaveOneAsideUntilNextSummoningRound()
        {
            CardData[] cards = DataManager.Instance.PlayerDeckSource.PeekAheadAtPlayerDeck(2);

            PickACardPayload payload = new PickACardPayload("Optional: Peek at the next 2 pages. You may choose one page to read. Any of the 2 pages not read are set aside and do not shuffle back into the Necronomicon until the next summoning.",
                cards.ToList(), 1, PickACardAfterEffectState.SetAsideTheRest);

            GameEvents.PickACard?.Invoke(payload);
        }

        // Optional: Peek at the next 2 pages. You may choose up to two pages to read. Any pages not read are put on the back of the book (bottom of the deck)
        public void PeekAtTwoReadUpToTwoShuffleAnyNotReadToBottomOfDeck()
        {
            CardData[] cards = DataManager.Instance.PlayerDeckSource.PeekAheadAtPlayerDeck(2);

            PickACardPayload payload = new PickACardPayload("Peek at the next 2 pages. You may choose up to two pages to read. Any pages not read are put on the back of the book (bottom of the deck).",
                cards.ToList(), 2, PickACardAfterEffectState.PutOnBottom);

            GameEvents.PickACard?.Invoke(payload);
        }

        // Lose 2 Sanity and shuffle a random page already read this summoning back into the book
        public void ShuffleReadPageThisSummoningRoundBackIntoDeck()
        {
            var card = DataManager.Instance.PlayerDeckSource.RemoveCardFromPlayedCards();
            DataManager.Instance.PlayerDeckSource.AddCardToPlayerDeck(card);
        }

        // Optional: Tear up (remove) a random page you've read this summoning from the Necronomicon, maximum Tape cost 1. That page's power is not removed from the Power meter.
        public void RemoveCardFromThisSummoningRoundWithTapeCostOfOneAndAddBackToStoreDoNotTouchPower()
        {
            DataManager.Instance.PlayerDeckSource.RemoveCardFromPlayedCards(true,1);
        }

        // Optional: Return all white pages previously read this summoning to the Necronomicon (shuffle in). Their power is removed from the power meter.
        public void ReturnWhitePagesToDeckAndShuffleAndRemovePowerFromBar()
        {
            List<CardData> whiteCards = new List<CardData>();

            foreach (var possibleWhiteCard in DataManager.Instance.PlayerDeckSource.playedCards)
            {
                if (possibleWhiteCard.Suit == Enums.Suit.White)
                {
                    whiteCards.Add(possibleWhiteCard);
                }
            }

            foreach (var whiteCard in whiteCards)
            {
                DataManager.Instance.PlayerEconomySource.Power -= whiteCard.PageModifier.Power;
                DataManager.Instance.PlayerEconomySource.WhiteSuitPoints -= whiteCard.PageModifier.Power;
            }

            DataManager.Instance.PlayerDeckSource.playedCards.RemoveAll(card => card.Suit == Enums.Suit.White);
            DataManager.Instance.PlayerDeckSource.playerDeck.AddRange(whiteCards);
        }

        // Optional: Repair a random page of the Necronomicon and add it to the book. This does not cost any Tape.
        public void RepairRandomCardAtNoTapeCost()
        {

            List<CardData> ownedCards = new List<CardData>();
            ownedCards.AddRange(DataManager.Instance.PlayerDeckSource.playerDeck);
            ownedCards.AddRange(DataManager.Instance.PlayerDeckSource.playedCards);

            // Get a copy of the shop's list
            CardData[] cardDataArr = new CardData[] {};
            DataManager.Instance.PlayerDeckSource.ShopList.CopyTo(cardDataArr);

            foreach (var shopCard in cardDataArr)
            {
                if (!ownedCards.Contains(shopCard))
                {
                    // If the shop has it and the Player doesn't, then repair and add to book. This is a perma buy
                    DataManager.Instance.PlayerDeckSource.AddCardToPlayerDeck(shopCard);
                    return;
                }
            }
        }

        // Gain +1 Power for each Orange read so far this summoning
        public void GetOnePowerForEachOrangeCardReadThisSummoningRound()
        {
            int orangeCardCount = DataManager.Instance.PlayerDeckSource.playedCards.Count(t => t.Suit == Enums.Suit.Orange);
            
            DataManager.Instance.PlayerEconomySource.ModifyPower(orangeCardCount);
        }

        // Optional: Return a random page read this summoning to the Necronomicon (shuffle in). Its power is not removed from the power meter.
        public void ReturnRandomCardToDeckAndShuffleDoNotTouchPower()
        {
            var card = DataManager.Instance.PlayerDeckSource.RemoveCardFromPlayedCards();
            DataManager.Instance.PlayerDeckSource.AddCardToPlayerDeck(card);
        }
    }
}