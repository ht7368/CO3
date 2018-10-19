using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards
{
    // A player could be either local or over the network; this will cover both cases.
    // In the network case, functions to get some information will end up sending network requests.
    public abstract class BasePlayer
    {
        public int MaxMana = 1;
        public int Mana = 1;
        public int Health = 25;
        public List<BaseCard> Deck = new List<BaseCard>();
        public List<BaseCard> Hand = new List<BaseCard>();
        public List<MinionCard> Board = new List<MinionCard>();

        //protected static Network Net = new Network();

        public abstract IEnumerator<Move> TakeTurn();
    }

    public class LocalPlayer : BasePlayer
    {
        public override IEnumerator<Move> TakeTurn()
        {
            yield break;
        }
    }

    public class NetworkPlayer : BasePlayer
    {
        public override IEnumerator<Move> TakeTurn()
        {
            yield break;
        }
    }
}
