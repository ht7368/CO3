using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards
{
    public abstract class BaseCard
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

        public abstract void Play(GameState currState, Move movePlayed);
    }

    public class MinionCard : BaseCard
    {
        public EffectData<MinionCard> Effects = new EffectData<MinionCard>();
        // Health and attack values need to be modifyable
        public int Attack;
        public int Health;

        public MinionCard(string name, int manaCost, int attack, int health, string description) : base(name, manaCost, description)
        {
            Attack = attack;
            Health = health;
        }

        public override void Play(GameState currState, Move movePlayed)
        {
            
        }
    }

    public class PowerCard : BaseCard
    {
        public EffectData<PowerCard> Effect = new EffectData<PowerCard>();

        public PowerCard(string name, int manaCost, string description) : base(name, manaCost, description)
        {
            // Nothing here for now
        }

        public override void Play(GameState currState, Move movePlayed)
        {
            currState.CurrentPower = this;
        }
    }

    public class SpellCard : BaseCard
    {
        public Action<Play<SpellCard>> Effect;

        public SpellCard(string name, int manaCost, string description) : base(name, manaCost, description)
        {
            // Nothing here for now
        }

        public override void Play(GameState currState, Move movePlayed)
        {
            this.Effect(new Play<SpellCard>(currState, movePlayed, this));
        }
    }
}
