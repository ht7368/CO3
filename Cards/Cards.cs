using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards
{
    abstract class BaseCard
    {
        public int ManaCost { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public readonly uint Id;

        public BaseCard(string name, int manaCost, string description)
        {
            Id = IdGenerator.NextId(this);
            ManaCost = manaCost;
            Name = name;
            Description = description;
        }

        public abstract void Play(GameState state);
    }

    class MinionCard : BaseCard
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

        public override void Play(GameState state)
        {
            state.CurrentPlayer().Board.Add(this);
        }
    }

    class PowerCard : BaseCard
    {
        public PowerCard(string name, int manaCost, string description) : base(name, manaCost, description)
        {
            // Nothing here for now
        }

        public override void Play(GameState state)
        {
            state.CurrentPower = this;
        }

        public Dictionary<Effect, Action<GameState, PowerCard>> Effects = new Dictionary<Effect, Action<GameState, PowerCard>>();
    }

    class SpellCard : BaseCard
    {
        public Action<GameState, SpellCard> Effect;

        public SpellCard(string name, int manaCost, string description) : base(name, manaCost, description)
        {
            // Nothing here for now
        }

        public override void Play(GameState state)
        {
            this.Effect(state, this);
        }
    }

    class Deck
    {
        private List<BaseCard> Cards;

    }
}
