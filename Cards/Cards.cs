using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards
{
    abstract class Card
    {
        public int ManaCost { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Card(string name, int manaCost, string description)
        {
            ManaCost = manaCost;
            Name = name;
            Description = description;
        }

        public abstract void Play(GameState state);
    }

    class MinionCard : Card
    {
        public Dictionary<Effect, Action<GameState, MinionCard>> Effects = new Dictionary<Effect, Action<GameState, MinionCard>>();
        // Health and attack values need to be modifyable
        public int Attack;
        public int Health;

        public MinionCard(string name, int manaCost, int attack, int health, string description) : base(name, manaCost, description)
        {
            Attack = attack;
            Health = health;
        }

        public void Play(GameState state)
        {
            state.ActivePlayer().Board.Add(this);
        }
    }

    class PowerCard : Card
    {
        public PowerCard(string name, int manaCost, string description) : base(name, manaCost, description)
        {
            // Nothing here for now
        }

        public void Play(GameState state)
        {
            state.CurrentPower = this;
        }

        public Dictionary<Effect, Action<GameState, PowerCard>> Effects = new Dictionary<Effect, Action<GameState, PowerCard>>();
    }

    class SpellCard : Card
    {
        public Action<GameState, SpellCard> Effect;

        public SpellCard(string name, int manaCost, string description) : base(name, manaCost, description)
        {
            // Nothing here for now
        }

        public void Play(GameState state)
        {
            this.Effect(state, this);
        }
    }

    class Deck
    {
        private List<Card> Cards;

    }
}
