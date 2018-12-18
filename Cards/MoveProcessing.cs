using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards
{
    // A move is a class that can be used to represent a change from one gamestate to the next.
    // It consists of the ID of a selected card, which is the one being played. This cannot be null!
    // There is also the ID of a targeted card, for example if a spell is being cast onto a minion,
    // The spell is the selected card, and the minion is the targeted card.
    // The targeted card can be null, because some cards do not have targets, this is represented by 0.
    public class Move
    {
        public uint Selected;
        public uint Targeted;

        // Constructors...

        // Construct with no targeted card
        public Move(uint selected)
        {
            this._Move(selected, targeted: 0);
        }

        // Constructed with a target card
        public Move(uint selected, uint targeted)
        {
            this._Move(selected, targeted);
        }

        private void _Move(uint selected, uint targeted)
        {
            if (selected == 0)
                throw new ArgumentException();
            Selected = selected;
            Targeted = targeted;
        }
    }

    static class IdExt
    {
        public static BaseCard AsCard(this uint u)
        {
            return IdGenerator.GetById(u);
        }

        public static T AsCardT<T>(this uint u) where T: BaseCard
        {
            return IdGenerator.GetById(u) as T;
        }
    }
}

    // We will pass in cards clicked into this class,
    // It is designed to create moves from this
    /*public class MoveProcessor
    {
        GameState GameTracked;
        List<BaseCard> Played = new List<BaseCard>();

        public MoveProcessor(GameState game)
        {
            GameTracked = game;
        }

        public void AddUserAction(BaseCard cardClicked)
        {
            Played.Add(cardClicked);
        }

        public void Clear()
        {
            Played.Clear();
        }

        public int NumberOfMoves()
        {
            return Played.Count();
        }

        public Move ProcessMoves()
        {
            Move M = _ProcessMoves();
            Clear();
            return M;
        }

        public Move _ProcessMoves()
        {
            // A spell card can be played on a target, a board or "everything"
            // A minion card can be played onto your board
            // A placed minion can attack an enemy minion or the enemy's hero
            // A power can be played in the power slot
            switch (Played[0])
            {
                case PowerCard p:
                    return new Move(p.Id, 0);

                case MinionCard m:
                    if (m.OnBoard)
                        return new Move(m.Id, Played[1].Id);
                    else
                        return new Move(m.Id, 0);

                case SpellCard s:
                    if (s.isTargeted)
                        return new Move(s.Id, Played[1].Id);
                    else
                        return new Move(s.Id, 0);

                default:
                    throw new Exception();
            }
        }
    }
}
*/