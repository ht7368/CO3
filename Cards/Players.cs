using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards
{
    // A player could be either local or over the network; this will cover both cases.
    // In the network case, functions to get some information will end up sending network requests.
    // It may end up caching data for performance reasons - we'll see (17/09).
    abstract class BasePlayer
    {
        public int MaxMana;
        public int Mana;
        public int Health;
        public List<Card> Deck;
        public List<Card> Hand;
        public List<MinionCard> Board;
    }

    class LocalPlayer : BasePlayer
    {

    }

    class NetworkPlayer : BasePlayer
    {

    }
}
