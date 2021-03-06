﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards
{
    // This class has to be static - if there was multiple instances of it,
    // The IDs couldn't be synchronised, and there may exist duplicates.
    // It wouldn't make sense to have multiple ID generators, anyway.
    // Furthermore, this makes it accessible everywhere - pretty handy.
    public static class IdGenerator
    {
        // This is a list of all cards registered with an ID to the game.
        private static Dictionary<uint, BaseCard> ExistingCards = new Dictionary<uint, BaseCard>();
        // Has to start above 0 since some IDs are reserved status codes.
        private static uint CurrMaxId = GameState.NUM_RESERVED_CODES;
        // Gives a card am ID and adds it to the list of registered cards.
        public static uint NextId(BaseCard newCard)
        {
            if (CurrMaxId == uint.MaxValue)
                throw new OverflowException();
            ExistingCards.Add(CurrMaxId, newCard);
            CurrMaxId += 1;
            return CurrMaxId - 1;
        }

        // Fetches a card from the global list of cards by ID
        // This should almost certainly be converted to a hashmap at some point
        public static BaseCard GetById(uint id)
        {
            if (id < GameState.NUM_RESERVED_CODES)
                return null;
            if (ExistingCards.ContainsKey(id))
                return ExistingCards[id];
            return null;
        }
    }
}
