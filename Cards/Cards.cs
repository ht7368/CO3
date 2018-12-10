using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards
{
    public abstract class BaseCard
    {
        public int ManaCost { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public readonly uint Id;

        public GameState Game;

        public BaseCard(GameState game)
        {
            Game = game;
            Id = IdGenerator.NextId(this);
        }

        // Differs based on class
        public abstract void Play();
    }

    public class MinionCard : BaseCard
    {
        public EffectData<MinionCard> Effects = new EffectData<MinionCard>();
        // Health and attack values need to be modifyable
        public int Attack;
        public int Health;
        public bool OnBoard = false;

        public MinionCard(GameState game) : base(game)
        {
        }

        // This method is called from the gamestate and passes in itself
        public override void Play()
        {
            if (!OnBoard)
            {
                OnBoard = true;
                Game.ActivePlayer.Board.Add(this);
                var a = Game.ActivePlayer.Board;
                //currState.BroadcastEffect(Effect.CardPlayed);
                return;
            }
            
            //currState.BroadcastEffect(Effect.MinionAttacking);

        }
    }

    public class PowerCard : BaseCard
    {
        public EffectData<PowerCard> Effects = new EffectData<PowerCard>();

        public PowerCard(GameState game) : base(game)
        {
            // Nothing here for now
        }

        public override void Play()
        {
            Game.CurrentPower = this;
        }
    }

    public class SpellCard : BaseCard
    {
        public bool isTargeted;
        public Action<GameState, Move> Effect;

        public SpellCard(GameState game) : base(game)
        {
            // Nothing here for now r
        }

        public override void Play()
        {
            this.Effect(Game, Game.LastMove);
        }
    }
}
