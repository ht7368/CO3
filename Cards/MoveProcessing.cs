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

    // Helper functions to transform an Id into its thing
    static class IdExt
    {
        public static BaseCard AsCard(this uint u)
        {
            return IdGenerator.GetById(u);
        }

        public static T AsCardT<T>(this uint u) where T : BaseCard
        {
            return (T)IdGenerator.GetById(u);
        }

        public static bool IsCardT<T>(this uint u) where T : BaseCard
        {
            return IdGenerator.GetById(u) is T;
        }
    }
}