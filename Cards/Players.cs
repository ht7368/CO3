﻿using System;
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
        public int MaxMana = 13;
        public int Mana = 1;
        public int ManaTurn = 1;
        public int Health = 25;
        public List<BaseCard> Deck = new List<BaseCard>();
        public List<BaseCard> Hand = new List<BaseCard>();
        public List<MinionCard> Board = new List<MinionCard>();

        public BaseCard PlayerCard;

        //protected static Network Net = new Network();

        //public abstract Move NextMove();
        //public abstract bool HasNextMove();
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
