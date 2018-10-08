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
    enum Effect
    {
        GameStart,
        TurnStart,
        TurnEnd,
        CardPlayed,
        MinionKilled,
        MinionAttacking, // About to attack
    }

    // Contains the entirety of the game's logical state.
    // This will be passed about to rendering and logic code to modify the state.
    // Notably, the rendering code will only see this, rendering elements seperate from logic.
    class GameState
    {
        public BasePlayer PlayerOne;
        public BasePlayer PlayerTwo;
        public PowerCard CurrentPower;
        private bool IsP1Turn; // Is it player one's turn?

        public BasePlayer CurrentPlayer()
        {
            if (IsP1Turn)
                return PlayerOne;
            else
                return PlayerTwo;
        }

        public void ProcessMove(Move nextMove)
        {
            BaseCard Selected = IdGenerator.GetById(nextMove.Selected);
            BaseCard Targeted = IdGenerator.GetById(nextMove.Targeted);
            // Only the current player can play cards - this invariant means this works.
            CurrentPlayer().Hand.Remove(Selected);

            // Match on the type of card - every one has a different effect.
            // These are all the classes that inherit from BaseCard, so no default needed.
            switch (Selected)
            {
                case MinionCard card:
                    card.Play(new Play<MinionCard>(this, card, nextMove));
                    break;
                case SpellCard card:
                    card.Play(new Play<SpellCard>(this, card, nextMove));
                    break;
                case PowerCard card:
                    card.Play(new Play<PowerCard>(this, card, nextMove));
                    break;
            }
        }
    }
    // This class has to be static - if there was multiple instances of it,
    // The IDs couldn't be synchronised, and there may exist duplicates.
    // It wouldn't make sense to have multiple ID generators, anyway.
    // Furthermore, this makes it accessible everywhere - pretty handy.
    static class IdGenerator
    {
        private static List<BaseCard> AllCards = new List<BaseCard>();
        // Has to start at 1, 0 is used to represent no target.
        private static uint CurrMaxId = 1;
        public static uint NextId(BaseCard newCard)
        {
            if (CurrMaxId == uint.MaxValue)
                throw new OverflowException();
            AllCards.Add(newCard);
            CurrMaxId += 1;
            return CurrMaxId - 1;
        }

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