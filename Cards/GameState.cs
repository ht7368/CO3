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
        public const bool DEBUG_ENABLED = true;
        public const int NUM_RESERVED_CODES = 3;

        public BasePlayer PlayerOne;
        public BasePlayer PlayerTwo;
        public PowerCard CurrentPower;
        public Move LastMove;
        public bool IsP1Turn; // Is it player one's turn?
        public bool PendingUpdate = false;
        public Network Net;

        public Random RNG;

        //public GameBox Box;

        public GameState(byte[] deckcode, string hostname)
        {
            PlayerTwo = new BasePlayer();
            PlayerOne = new BasePlayer();

            IsP1Turn = false;
            int Seed = (int)(DateTime.Now.Ticks % int.MaxValue);

            Net = new Network(hostname);

            Net.SendRandomSeed(Seed);
            RNG = new Random(Seed);

            System.Windows.Forms.MessageBox.Show(caption: $"{Seed}", text: $"{Enumerable.Repeat(0, 20).Select(x => RNG.Next())}");

            Net.SendDeck(deckcode);
            byte[] OppDeck = Net.RecieveDeck();

            PlayerTwo.Deck = OppDeck
                .Select(x => Cards.CardFromID(x))
                .Select(x => x.Build(this, PlayerTwo))
                .ToList();
            PlayerOne.Deck = deckcode
                .Select(x => Cards.CardFromID(x))
                .Select(x => x.Build(this, PlayerOne))
                .ToList();

            _GameState();
        }

        public GameState(byte[] deckcode)
        {
            PlayerOne = new BasePlayer();
            PlayerTwo = new BasePlayer();

            IsP1Turn = true;
            Net = new Network();

            int Seed = Net.RecieveRandomSeed();
            RNG = new Random(Seed);

            System.Windows.Forms.MessageBox.Show(caption: $"{Seed}", text: $"{Enumerable.Repeat(0, 20).Select(x => RNG.Next())}");

            byte[] OppDeck = Net.RecieveDeck();
            Net.SendDeck(deckcode);

            PlayerOne.Deck = deckcode
                .Select(x => Cards.CardFromID(x))
                .Select(x => x.Build(this, PlayerOne))
                .ToList();
            PlayerTwo.Deck = OppDeck
                .Select(x => Cards.CardFromID(x))
                .Select(x => x.Build(this, PlayerTwo))
                .ToList();

            _GameState();
        }

        public void _GameState()
        {
            CurrentPower = Cards.CardFromID(8).Build(this, ActivePlayer) as PowerCard;

            RNG.Shuffle(ActivePlayer.Deck);
            RNG.Shuffle(InactivePlayer.Deck);

            PlayerOne.PlayerCard = new HeroCard(this)
            {
                Description = "",
                Name = "",
                ManaCost = 0,
            };
            PlayerTwo.PlayerCard = new HeroCard(this)
            {
                Description = "",
                Name = "",
                ManaCost = 0,
            };

            for (int i = 0; i < 5; i++)
            {
                PlayerOne.DrawCard();
                PlayerTwo.DrawCard();
            }

            Task.Run(() => AsyncWaitForMove());
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
            foreach (var c in ActivePlayer.Hand)
                yield return c;
            foreach (var c in ActivePlayer.Board)
                yield return c;
            foreach (var c in InactivePlayer.Board)
                yield return c;
            foreach (var c in InactivePlayer.Hand)
                yield return c;
            yield break;
        }

        public IEnumerable<MinionCard> AllOnboardMinions()
        {
            foreach (var c in ActivePlayer.Board)
                yield return c;
            foreach (var c in InactivePlayer.Board)
                yield return c;
            yield break;
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
        public string ProcessMove(Move nextMove)
        {
            // TODO: if 0
            if (nextMove.Selected == OPP_CONCEDE)
            {
                return null;
            }
            else if (nextMove.Selected == TURN_PASS)
            {
                DoMove(nextMove);
                Net.Send(nextMove);
                return null;
            }
            else if (nextMove.Selected == CARD_DRAW)
            {
                DoMove(nextMove);
                Net.Send(nextMove);
                return null;
            }

            BaseCard Selected = nextMove.Selected.AsCard();
            BaseCard Targeted = nextMove.Targeted.AsCard();
            if (!Selected.IsPlayable(nextMove))
            {
                return "YOU CANNOT MAKE THAT MOVE.";
            }

            Net.Send(nextMove);
            DoMove(nextMove);
            return "";
        }

        public void DoMove(Move nextMove)
        {
            if (nextMove.Selected == OPP_CONCEDE)
            {
                return;
            }
            else if (nextMove.Selected == TURN_PASS)
            {
                SwitchTurns();
                return;
            }
            else if (nextMove.Selected == CARD_DRAW)
            {
                // TODO: Force loss if deck is empty
                ActivePlayer.ManualDrawCard();
                return;
            }

            BaseCard Selected = nextMove.Selected.AsCard();
            BaseCard Targeted = nextMove.Targeted.AsCard();

            this.LastMove = nextMove;
            InactivePlayer.Hand.Remove(Selected);
            ActivePlayer.Hand.Remove(Selected);

            Targeted.Play();
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
            ActivePlayer.Mana += ActivePlayer.ManaTurn < 5 ? ActivePlayer.ManaTurn : 5;
            if (ActivePlayer.Mana > ActivePlayer.MaxMana)
                ActivePlayer.Mana = ActivePlayer.MaxMana;

            // Allow minions to attack again
            foreach (BaseCard c in AllCards())
                if (c is MinionCard)
                    (c as MinionCard).CanAttack = true;

            // Allow players to draw again
            PlayerOne.HasNotDrawn = true;
            PlayerTwo.HasNotDrawn = true;

            // Switch turn flag
            IsP1Turn = !IsP1Turn;
            BroadcastEffect(Effect.TurnStart);
        }

        public async void AsyncWaitForMove()
        {
            while (true)
            {
                Move nextMove = await Net.Recieve();
                DoMove(nextMove);
                PendingUpdate = true;
            }
        }
    }
}