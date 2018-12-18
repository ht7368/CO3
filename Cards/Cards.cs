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

        public abstract bool IsPlayable(Move potentialMove);
    }

    public class MinionCard : BaseCard
    {
        public EffectData<MinionCard> Effects = new EffectData<MinionCard>();
        // Health and attack values need to be modifyable
        public int Attack;
        public int Health;
        public bool OnBoard = false;
        public bool CanAttack = true;

        public MinionCard(GameState game) : base(game)
        {

        }

        // This method is called from the gamestate and passes in itself
        public override void Play()
        {
            if (OnBoard)
            {
                Game.BroadcastEffect(Effect.MinionAttacking);
                MinionCard Attacker = Game.LastMove.Targeted.AsCardT<MinionCard>();
                this.Health -= Attacker.Attack;
                Attacker.Health -= this.Attack;
            }
            else
            {
                OnBoard = true;
                Game.ActivePlayer.Board.Add(this);
                var a = Game.ActivePlayer.Board;
                Game.BroadcastEffect(Effect.CardPlayed);
                return;
            }
        }

        public override bool IsPlayable(Move potentialMove)
        {
            if (OnBoard)
                return CanAttack;
            if (ManaCost > Game.ActivePlayer.Mana)
                return false;
            if (potentialMove.Targeted == 0 || !potentialMove.Targeted.IsCardT<MinionCard>())
                return false;
            MinionCard CombatTarget = potentialMove.Targeted.AsCardT<MinionCard>();
            return CombatTarget.OnBoard;
            
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

        public override bool IsPlayable(Move potentialMove)
        {
            if (ManaCost > Game.ActivePlayer.Mana)
                return false;
            return true;
        }
    }

    public class SpellCard : BaseCard
    {
        public bool isTargeted;
        public Action<GameState, Move> SpellEffect;

        public SpellCard(GameState game) : base(game)
        {
            // Nothing here for now r
        }

        public override void Play()
        {
            Game.BroadcastEffect(Effect.CardPlayed);
            this.SpellEffect(Game, Game.LastMove);
        }

        public override bool IsPlayable(Move potentialMove)
        {
            if (ManaCost > Game.ActivePlayer.Mana)
                return false;
            if (!isTargeted)
                return true;
            if (isTargeted && !potentialMove.Targeted.IsCardT<MinionCard>())
                return false;
            MinionCard SpellTarget = potentialMove.Targeted.AsCardT<MinionCard>();
            return SpellTarget.OnBoard;
        }
    }
}
