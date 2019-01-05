using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cards
{
    class Cards
    {
        // (Subject to change?) A collection of all cards. More specifically,
        // functions that take a GameState to return a card.
        public static List<Func<GameState, BaseCard>> CardDB = new List<Func<GameState, BaseCard>>
        {
            // Testing Dummy
            (GameState g) => new MinionCard(g)
            {
                Name = "TESTING DUMMY",
                ManaCost = 1,
                Attack = 0,
                Health = 3,
                Description = "DIES ON TURN END",
                OnBoard = false,
                Art = Properties.Resources.DeadRising,
                /*Effects = new EffectData<MinionCard>
                {
                    { Effect.TurnEnd, (p) =>  { p.CardPlayed.Health = 0; } },
                }*/
            },

            (GameState g) => new PowerCard(g)
            {
                Name = "CLEAR WEATHER",
                ManaCost = 5,
                Description = "NO EFFECT",
                Art = Properties.Resources.ClearSky,
                Effects = new EffectData<PowerCard> { /* None */ }
            },

            (GameState g) => new SpellCard(g)
            {
                Name = "VANQUISH",
                ManaCost = 5,
                Description = "DESTROY A MINION",
                isTargeted = true,
                Art = Properties.Resources.Vanquish,
                SpellEffect = (s, m) => 
                {
                    var Target = IdGenerator.GetById(m.Targeted) as MinionCard;
                    Target.Health = 0;
                }
            },

            (GameState g) => new PowerCard(g)
            {
                Name = "GRAND ARENA",
                ManaCost = 6,
                Description = "MINIONS GAIN +3 ATK BEFORE ATTACKING",
                Art = Properties.Resources.GrandArena,
                Effects = new EffectData<PowerCard>
                {
                    {
                        Effect.MinionAttacking, (GameState s) => (s.LastMove.Selected.AsCard() as MinionCard).Attack += 3
                    }
                }
            },

            (GameState g) => new MinionCard(g)
            {
                Name = "FIGHTING DUMMY",
                ManaCost = 2,
                Attack = 2,
                Health = 4,
                Description = "",
                OnBoard = false,
            },

            (GameState g) => new PowerCard(g)
            {
                Name = "WHISPERS OF POWER",
                ManaCost = 4,
                Description = "ALL MINIONS +1/+1 WHEN CARD PLAYED",
                Art = Properties.Resources.WhispersMadness,
                Effects = new EffectData<PowerCard>
                {
                    {
                        Effect.CardPlayed, (GameState s) =>
                        {
                            foreach (BaseCard c in s.AllCards())
                                if (c is MinionCard)
                                {
                                    (c as MinionCard).Attack += 1;
                                    (c as MinionCard).Health += 1;
                                }
                        }
                    }
                }
            }
        };
    }
}
