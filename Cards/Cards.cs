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
        public BasePlayer Owner;

        public System.Drawing.Image Art { get; set; }

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
        public bool CanAttack = false;

        public MinionCard(GameState game) : base(game)
        {

        }

        // This method is called from the gamestate and passes in itself
        public override void Play()
        {
            // A "move" can mean different things for a minion card 
            // If it's on board, it is attacking another minion
            if (OnBoard)
            {
                CanAttack = false;
                Game.BroadcastEffect(Effect.MinionAttacking);
                if (Game.LastMove.Targeted.IsCardT<HeroCard>())
                {
                    Game.InactivePlayer.Health -= this.Attack;
                    return;
                }
                MinionCard Attacker = Game.LastMove.Targeted.AsCardT<MinionCard>();
                this.Health -= Attacker.Attack;
                Attacker.Health -= this.Attack;
            }
            // But otherwise, it is being played
            else
            {
                Game.BroadcastEffect(Effect.CardPlayed);
                OnBoard = true;
                Game.ActivePlayer.Board.Add(this);
                Game.ActivePlayer.Mana -= this.ManaCost;
                if (Game.ActivePlayer.Mana < 0)
                    Game.ActivePlayer.Mana = 0;
            }
            CanAttack = false;
        }

        public override bool IsPlayable(Move potentialMove)
        {
            // Initial codes are reserved for special actions
            if (potentialMove.Selected < GameState.NUM_RESERVED_CODES)
                return false;
            if (OnBoard)
            {
                // Move is valid on board if:
                // Attack target exists,
                // the target is a minion that has been played
                // And the card is capable of attacking
                if (potentialMove.Targeted < GameState.NUM_RESERVED_CODES)
                    return false;
                if (Game.InactivePlayer.PlayerCard == potentialMove.Targeted.AsCard())
                    return CanAttack;
                if (potentialMove.Targeted.IsCardT<HeroCard>())
                    return CanAttack;
                if (potentialMove.Targeted.AsCard() is MinionCard m)
                    return CanAttack && m.OnBoard && m.Owner == Game.PlayerTwo;
                return false;
            }
            else
            {
                // Move is valid in hand if it is in the correct hand and has mana
                if (!Game.ActivePlayer.Hand.Contains(this))
                    return false;
                return ManaCost <= Game.ActivePlayer.Mana;
            }
        }
    }

    public class PowerCard : BaseCard
    {
        // Dictionary of effect: thing to call when it happens
        public EffectData<PowerCard> Effects = new EffectData<PowerCard>();

        public PowerCard(GameState game) : base(game)
        {
            // Nothing here for now
        }

        public override void Play()
        {
            Game.BroadcastEffect(Effect.CardPlayed);
            Game.CurrentPower = this;
            Game.ActivePlayer.Mana -= this.ManaCost;
            if (Game.ActivePlayer.Mana < 0)
                Game.ActivePlayer.Mana = 0;
        }

        public override bool IsPlayable(Move potentialMove)
        {
            if (potentialMove.Selected < GameState.NUM_RESERVED_CODES)
                return false;
            if (!Game.ActivePlayer.Hand.Contains(this))
                return false;
            if (ManaCost > Game.ActivePlayer.Mana)
                return false;
            return true;
        }
    }

    public class SpellCard : BaseCard
    {
        public bool IsTargeted;
        // Will be called when the spell is played
        public Action<GameState, Move> SpellEffect;

        public SpellCard(GameState game) : base(game)
        {
            // Nothing here for now
        }

        public override void Play()
        {
            Game.ActivePlayer.Mana -= this.ManaCost;
            Game.BroadcastEffect(Effect.CardPlayed);
            this.SpellEffect(Game, Game.LastMove);
        }

        public override bool IsPlayable(Move potentialMove)
        {
            if (potentialMove.Selected < GameState.NUM_RESERVED_CODES)
                return false;
            if (!Game.ActivePlayer.Hand.Contains(this))
                return false;
            if (ManaCost > Game.ActivePlayer.Mana)
                return false;
            if (!IsTargeted)
                return true;
            if (potentialMove.Targeted.IsCardT<HeroCard>())
                return true;
            else if (potentialMove.Targeted.AsCard() is MinionCard m)
                return m.OnBoard;
            return false;
        }
    }

    public class HeroCard : BaseCard
    {
        public HeroCard(GameState game) : base(game)
        {

        }

        public override void Play()
        {

        }

        public override bool IsPlayable(Move potentialMove)
        {
            return false;
        }
    }
}
