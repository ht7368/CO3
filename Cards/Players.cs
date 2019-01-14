using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards
{
    // A player could be either local or over the network; this will cover both cases.
    // In the network case, functions to get some information will end up sending network requests.
    public class BasePlayer
    {
        public int MaxMana = 13;
        public int Mana = 1;
        public int ManaTurn = 1;
        public int Health = 25;
        public List<BaseCard> Deck = new List<BaseCard>();
        public List<BaseCard> Hand = new List<BaseCard>();
        public List<MinionCard> Board = new List<MinionCard>();
        public bool CanDraw = true;

        public BaseCard PlayerCard;

        //protected static Network Net = new Network();

        //public abstract Move NextMove();
        //public abstract bool HasNextMove();

        public virtual void DrawCard()
        {
            if (!CanDraw || Mana == 0 || Hand.Count >= 10)
                return;
            CanDraw = false;
            Mana -= 1;
            // TODO: If empty, concede
            Hand.Add(Deck[0]);
            Deck.RemoveAt(0);

            PlayerCard.Game.BroadcastEffect(Effect.CardDrawn);
        }
    }

    public class LocalPlayer : BasePlayer
    {
        public bool HasNextMove()
        {
            return false;
        }

        public Move NextMove()
        {
            return null;
        }
    }

    public class NetworkPlayer : BasePlayer
    {
        public bool HasNextMove()
        {
            return false;
        }

        public Move NextMove()
        {
            return null;
        }
    }
}
