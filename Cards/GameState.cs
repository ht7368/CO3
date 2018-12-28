using System;
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
        GameStart,
        TurnStart,
        TurnEnd,
        CardPlayed,
        MinionKilled,
        // About to attack
        MinionAttacking, 
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
        public bool IsP1Turn = true; // Is it player one's turn?

        public GameBox Box;


        public GameState()
        {
            PlayerOne = new LocalPlayer();
            PlayerTwo = new NetworkPlayer();

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

        public IEnumerable<BaseCard> AllCards()
        {
            foreach (BaseCard c in PlayerOne.Hand)
                yield return c;
            foreach (BaseCard c in PlayerOne.Board)
                yield return c;
            foreach (BaseCard c in PlayerTwo.Board)
                yield return c;
            foreach (BaseCard c in PlayerTwo.Hand)
                yield return c;
            yield break;
        }

        public void BroadcastEffect(Effect effect)
        {
            if (CurrentPower.Effects.ContainsKey(effect))
                CurrentPower.Effects[effect](this);
         
            // Anything with an effect will have it called
            foreach (var c in AllCards())
                if (c is MinionCard)
                    if ((c as MinionCard).Effects.ContainsKey(effect))
                        (c as MinionCard).Effects[effect](this);
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
            // TODO: if 0
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
                ActivePlayer.DrawCard();
                return;
            }

            BaseCard Selected = nextMove.Selected.AsCard();
            BaseCard Targeted = nextMove.Selected.AsCard();
            if (!Selected.IsPlayable(nextMove))
                return;

            this.LastMove = nextMove;
            InactivePlayer.Hand.Remove(Selected);
            ActivePlayer.Hand.Remove(Selected);

            Selected.Play();
            ResolveActions();
        }

        // Resolving an action involves:
        // Removing minions that have no health
        public void ResolveActions()
        {
            foreach (var c in AllCards())
                if (c is MinionCard)
                {
                    MinionCard m = c as MinionCard;
                    if (m.Health <= 0)
                    {
                        ActivePlayer.Board.Remove(m);
                        InactivePlayer.Board.Remove(m);
                        break;
                    }
                }
        }

        public void SwitchTurns()
        {
            // Process end-of-turn mana changes
            ActivePlayer.ManaTurn += 1;
            ActivePlayer.Mana += ActivePlayer.ManaTurn < 6 ? ActivePlayer.ManaTurn : 6;
            if (ActivePlayer.Mana > ActivePlayer.MaxMana)
                ActivePlayer.Mana = ActivePlayer.MaxMana;

            // Allow minions to attack again
            foreach (BaseCard c in AllCards())
                if (c is MinionCard)
                    (c as MinionCard).CanAttack = true;

            // Allow players to draw again
            PlayerOne.CanDraw = true;
            PlayerTwo.CanDraw = true;

            // Switch turn flag
            IsP1Turn = !IsP1Turn;
        }
    }
}