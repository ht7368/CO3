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
                Name = "Testing Dummy",
                ManaCost = 1,
                Attack = 0,
                Health = 3,
                Description = "Testing Dummy dies at the end of its turn.",
                OnBoard = false,
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
                SpellEffect = (s, m) => 
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
            },

            (GameState g) => new MinionCard(g)
            {
                Name = "Fighting Dummy",
                ManaCost = 2,
                Attack = 1,
                Health = 4,
                Description = "",
                OnBoard = true,
            },

            (GameState g) => new PowerCard(g)
            {
                Name = "Whispers of Power",
                ManaCost = 4,
                Description = "Whenever you play a card, give all minions +1/+1",
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
