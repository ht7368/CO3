﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards
{
    // A player could be either local or over the network; this will cover both cases.
    // In the network case, functions to get some information will end up sending network requests.
    abstract class BasePlayer
    {
        public int MaxMana;
        public int Mana;
        public int Health;
        public List<BaseCard> Deck;
        public List<BaseCard> Hand;
        public List<MinionCard> Board;

        protected static Network Net = new Network();

        public abstract IEnumerator<Move> TakeTurn();
    }

    class LocalPlayer : BasePlayer
    {
        public override IEnumerator<Move> TakeTurn()
        {
            yield break;
        }
    }

    class NetworkPlayer : BasePlayer
    {
        public override IEnumerator<Move> TakeTurn()
        {
            yield break;
        }
    }
}
