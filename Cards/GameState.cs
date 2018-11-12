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
        public BasePlayer PlayerOne;
        public BasePlayer PlayerTwo;
        public PowerCard CurrentPower;
        public Move LastMove;
        private bool IsP1Turn = true; // Is it player one's turn?

        public GameState()
        {
            PlayerOne = new LocalPlayer();
            PlayerTwo = new NetworkPlayer();
        }

        public BasePlayer ActivePlayer()
        {
            if (IsP1Turn)
                return PlayerOne;
            else
                return PlayerTwo;
        }

        public BasePlayer InactivePlayer()
        {
            if (!IsP1Turn)
                return PlayerOne;
            else
                return PlayerTwo;
        }

        public void BroadcastEffect(Effect effect)
        {
            if (CurrentPower.Effects.ContainsKey(effect))
                CurrentPower.Effects[effect](this);
            foreach (MinionCard c in ActivePlayer().Board)
                if (c.Effects.ContainsKey(effect))
                    c.Effects[effect](this);
            foreach (MinionCard c in ActivePlayer().Board)
                if (c.Effects.ContainsKey(effect))
                    c.Effects[effect](this);
        }

        // Process a move generated either over network or locally and resolve it's events
        public void ProcessMove(Move nextMove)
        {
            BaseCard Selected = IdGenerator.GetById(nextMove.Selected);
            BaseCard Targeted = IdGenerator.GetById(nextMove.Targeted);
            // Only the current player can play cards - this invariant means this works.
            this.LastMove = nextMove;
            ActivePlayer().Hand.Remove(Selected);
            Selected.Play(this);
        }
    }
    // This class has to be static - if there was multiple instances of it,
    // The IDs couldn't be synchronised, and there may exist duplicates.
    // It wouldn't make sense to have multiple ID generators, anyway.
    // Furthermore, this makes it accessible everywhere - pretty handy.
    public static class IdGenerator
    {
        // This is a list of all cards registered with an ID to the game.
        private static List<BaseCard> AllCards = new List<BaseCard>();
        // Has to start at 1, 0 is used to represent no target (null value).
        private static uint CurrMaxId = 1;
        // Gives a card am ID and adds it to the list of registered cards.
        public static uint NextId(BaseCard newCard)
        {
            if (CurrMaxId == uint.MaxValue)
                throw new OverflowException();
            AllCards.Add(newCard);
            CurrMaxId += 1;
            return CurrMaxId - 1;
        }
        
        // Fetches a card from the global list of cards by ID
        public static BaseCard GetById(uint id)
        {
            if (id == 0)
                return null;
            foreach (BaseCard c in AllCards)
                if (c.Id == id)
                    return c;
            return null;
        }
    }   
}