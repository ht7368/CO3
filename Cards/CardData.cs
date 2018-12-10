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
                Description = "Card dies at the end of the turn.",
                /*Effects = new EffectData<MinionCard>
                {
                    { Effect.TurnEnd, (p) =>  { p.CardPlayed.Health = 0; } },
                }*/
            },

            (GameState g) => new PowerCard(g)
            {
                Name = "Clear Weather",
                ManaCost = 3,
                Description = "This power has no effect.",
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
        };
    }
}
