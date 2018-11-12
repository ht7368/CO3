using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cards
{
    class Cards
    {
        public static List<BaseCard> CardDB = new List<BaseCard>
        {
            // Testing Dummy
            new MinionCard(name: "Testing Dummy", manaCost: 1, attack: 0, health: 1, description:
                "Card dies at end of turn.")
            {
                /*Effects = new EffectData<MinionCard>
                {
                    { Effect.TurnEnd, (p) =>  { p.CardPlayed.Health = 0; } },
                }*/
            },

            new PowerCard(name: "Clear Weather", manaCost: 3, description:
                "No effect. Can overwrite other powers.")
            {
                Effects = new EffectData<PowerCard> { /* None */ }
            },

            new SpellCard(name: "Vanquish", manaCost: 5, description:
                "Destroy a target minion.") 
            {
                Effect = (s, m) => {  }
            },
        };
    }
}
