﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards
{
    // Various effects will need to trigger at various points in the game: For example at the start of a turn.
    // How this will be achieved is by keeping a Dictionary<Effect, Action<GameState>> which maps each event to an effect.
    // This effect takes the form of an Action<GameState>, that is, a procedure taking GameState and modifying it.
    public enum Effect
    {
        // Before the turns starts
        TurnStart,
        // Before the turn passes
        TurnEnd,
        // Before play resolution
        CardPlayed,
        // After death
        MinionKilled,
        // About to attack
        MinionAttacking, 
        // After bering placed in hand
        CardDrawn,
    }

    // Contains the entirety of the game's logical state.
    // This will be passed about to rendering and logic code to modify the state.
    // Notably, the rendering code will only see this, rendering elements seperate from logic.
    public class GameState
    {
        public const int NUM_RESERVED_CODES = 3;

        public BasePlayer PlayerOne;
        public BasePlayer PlayerTwo;
        public PowerCard CurrentPower;
        public Move LastMove;
        public bool IsP1Turn; // Is it player one's turn?

        public Random RNG;

        //public GameBox Box;

        public GameState(byte[] firstDeckcode, byte[] secondDeckcode, bool isP1turn, int randomSeed)
        {
            RNG = new Random(randomSeed);

            PlayerTwo = new BasePlayer();
            PlayerOne = new BasePlayer();

            IsP1Turn = isP1turn;

            // Important: 
            // If any desynchronisations occur, the game will not be properly playable
            // It is important that each card has the same ID for each user,
            // Which means they ought to appear in the same order
            // HOWEVER, by default this will not be the case,
            // as PlayerOne will refer to different players for each user
            // So we change the initialization order 
            if (isP1turn)
            {
                PlayerTwo.Deck = secondDeckcode
                    .Select(x => Cards.CardFromID(x))
                    .Select(x => x.Build(this, PlayerTwo))
                    .ToList();
                PlayerOne.Deck = firstDeckcode
                    .Select(x => Cards.CardFromID(x))
                    .Select(x => x.Build(this, PlayerOne))
                    .ToList();
                PlayerOne.PlayerCard = new HeroCard(this)
                {
                    Owner = PlayerOne,
                    Description = "",
                    Name = "",
                    ManaCost = 0,
                };
                PlayerTwo.PlayerCard = new HeroCard(this)
                {
                    Owner = PlayerTwo,
                    Description = "",
                    Name = "",
                    ManaCost = 0,
                };
            }
            else
            {
                PlayerOne.Deck = firstDeckcode
                    .Select(x => Cards.CardFromID(x))
                    .Select(x => x.Build(this, PlayerOne))
                    .ToList();
                PlayerTwo.Deck = secondDeckcode
                    .Select(x => Cards.CardFromID(x))
                    .Select(x => x.Build(this, PlayerTwo))
                    .ToList();
                PlayerTwo.PlayerCard = new HeroCard(this)
                {
                    Owner = PlayerTwo,
                    Description = "",
                    Name = "",
                    ManaCost = 0,
                };
                PlayerOne.PlayerCard = new HeroCard(this)
                {
                    Owner = PlayerOne,
                    Description = "",
                    Name = "",
                    ManaCost = 0,
                };
            }

            _GameState();
        }

        public void _GameState()
        {
            CurrentPower = Cards.CardFromID(8).Build(this, ActivePlayer) as PowerCard;

            RNG.Shuffle(ActivePlayer.Deck);
            RNG.Shuffle(InactivePlayer.Deck);

            for (int i = 0; i < 5; i++)
            {
                PlayerOne.DrawCard();
                PlayerTwo.DrawCard();
            }
        }

        // Helper functions
        public BasePlayer ActivePlayer
        {
            get
            {
                if (IsP1Turn)
                    return PlayerOne;
                else
                    return PlayerTwo;
            }
        }

        public BasePlayer InactivePlayer
        {
            get
            {
                if (!IsP1Turn)
                    return PlayerOne;
                else
                    return PlayerTwo;
            }
        }

        // Using ActivePlayer as it is the same for both clients
        public IEnumerable<BaseCard> AllCards()
        {
            List<BaseCard> Cards = new List<BaseCard>();
            foreach (var c in ActivePlayer.Hand)
                Cards.Add(c);
            foreach (var c in ActivePlayer.Board)
                Cards.Add(c);
            foreach (var c in InactivePlayer.Board)
                Cards.Add(c);
            foreach (var c in InactivePlayer.Hand)
                Cards.Add(c);
            return Cards;
        }

        public IEnumerable<MinionCard> AllOnboardMinions()
        {
            List<MinionCard> Minions = new List<MinionCard>();
            foreach (var c in ActivePlayer.Board)
                Minions.Add(c);
            foreach (var c in InactivePlayer.Board)
                Minions.Add(c);
            return Minions;
        }

        public void BroadcastEffect(Effect effect)
        {
            if (CurrentPower.Effects.ContainsKey(effect))
                CurrentPower.Effects[effect](this, CurrentPower);
         
            // Anything with an effect will have it called
            foreach (MinionCard m in AllOnboardMinions())
                if (m.Effects.ContainsKey(effect))
                    m.Effects[effect](this, m);
        }

        // SPECIAL CODES FOR MOVE PROCESSING
        // 0 Opponent left - concede button
        // 1 Turn passed - pass button
        // 2 Card drawn - draw button

        public const int OPP_CONCEDE = 0;
        public const int TURN_PASS = 1;
        public const int CARD_DRAW = 2;

        // Process a move generated either over network or locally and resolve it's events
        // This function will not perform anything if the move is invalid
        public void ProcessMove(Move nextMove)
        {
            if (nextMove.Selected == OPP_CONCEDE)
            {
                PlayerTwo.Health = 0;
                return;
            }
            else if (nextMove.Selected == TURN_PASS)
            {
                SwitchTurns();
                return;
            }
            else if (nextMove.Selected == CARD_DRAW)
            {
                ActivePlayer.ManualDrawCard();
                return;
            }

            BaseCard Selected = nextMove.Selected.AsCard();
            BaseCard Targeted = nextMove.Targeted.AsCard();
            if (IsP1Turn && !Selected.IsPlayable(nextMove))
                return;

            this.LastMove = nextMove;
            Selected.Owner.Hand.Remove(Selected);

            Selected.Play();
            ResolveActions();
        }

        // Resolving an action involves:
        // Removing minions that have no health
        public void ResolveActions()
        {
            List<MinionCard> PendingRemove = new List<MinionCard>();
            foreach (var c in AllCards())
                if (c is MinionCard m)
                    if (m.Health <= 0)
                        PendingRemove.Add(m);
            foreach (MinionCard m in PendingRemove)
            {
                PlayerOne.Board.Remove(m);
                PlayerTwo.Board.Remove(m);
                BroadcastEffect(Effect.MinionKilled);
            }
        }

        public void SwitchTurns()
        {
            BroadcastEffect(Effect.TurnEnd);

            // Process end-of-turn mana changes
            ActivePlayer.ManaTurn += 1;
            ActivePlayer.Mana += ActivePlayer.ManaTurn < 6 ? ActivePlayer.ManaTurn : 6;
            if (ActivePlayer.Mana > ActivePlayer.MaxMana)
                ActivePlayer.Mana = ActivePlayer.MaxMana;

            // Allow minions to attack again
            foreach (MinionCard m in AllOnboardMinions())
                m.CanAttack = true;

            // Allow players to draw again
            PlayerOne.HasNotDrawn = true;
            PlayerTwo.HasNotDrawn = true;

            // Switch turn flag
            IsP1Turn = !IsP1Turn;

            BroadcastEffect(Effect.TurnStart);
            ResolveActions();
        }
    }
}