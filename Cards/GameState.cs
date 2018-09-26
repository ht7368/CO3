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
    }

    static class IdGenerator
    {
        // Has to start at 1, 0 is used to represent no target.
        private static uint CurrMaxId = 1;
        public static uint NextId()
        {
            if (CurrMaxId == uint.MaxValue)
                throw new OverflowException();
            CurrMaxId += 1;
            return CurrMaxId - 1;
        }

    }
}
