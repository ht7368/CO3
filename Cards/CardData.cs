using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cards
{
    class Cards
    {
        public static List<Func<GameState, BaseCard>> CardDB = new List<Func<GameState, BaseCard>>
        {
            // Testing Dummy
            (GameState g) => new MinionCard(g)
            {
                Name = "Testing Dummy",
                ManaCost = 1,
                Attack = 0,
                Health = 2,
                Description = "Testing Dummy dies at the end of its turn.",
                /*Effects = new EffectData<MinionCard>
                {
                    { Effect.TurnEnd, (p) =>  { p.CardPlayed.Health = 0; } },
                }*/
            },

            (GameState g) => new PowerCard(g)
            {
                Name = "Clear Weather",
                ManaCost = 3,
                Description = "No effect.",
                Effects = new EffectData<PowerCard> { /* None */ }
            },

            (GameState g) => new SpellCard(g)
            {
                Name = "Vanquish",
                ManaCost = 5,
                Description = "Target a minion. Destroy it.",
                isTargeted = true,
                Effect = (s, m) => 
                {
                    var Target = IdGenerator.GetById(m.Targeted) as MinionCard;
                    Target.Health = 0;
                }
            },

            (GameState g) => new PowerCard(g)
            {
                Name = "The Grand Arena",
                ManaCost = 6,
                Description = "Before attacking, minions gain +3 attack.",
                Effects = new EffectData<PowerCard>
                {
                    {
                        Effect.MinionAttacking, (GameState s) => (s.LastMove.Selected.AsCard() as MinionCard).Attack += 3
                    }
                }
            }
        };
    }
}
