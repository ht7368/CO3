using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards
{
    public static partial class Cards
    {
        public static List<CardBuilder> Collection = new List<CardBuilder>()
        {
            new CardBuilder()
            {
                DeckID = 0,
                TypeID = CardBuilder.CardType.Minion,

                NameData = "TESTING DUMMY",
                ManaCostData = 0,
                AttackData = 0,
                HealthData = 4,
                DescriptionData = "",
                ArtData = Properties.Resources.TargetDummy,
            },
            new CardBuilder()
            {
                DeckID = 1,
                TypeID = CardBuilder.CardType.Minion,

                NameData = "RUNT",
                ManaCostData = 2,
                AttackData = 2,
                HealthData = 2,
                DescriptionData = "",
                //ArtData = Properties.Resources.DuoDemon,
            },
            new CardBuilder()
            {
                DeckID = 2,
                TypeID = CardBuilder.CardType.Minion,

                NameData = "BOAR",
                ManaCostData = 3,
                AttackData = 3,
                HealthData = 3,
                DescriptionData = "",
                //ArtData = Properties.Resources.Treant,
            },
            new CardBuilder()
            {
                DeckID = 3,
                TypeID = CardBuilder.CardType.Minion,

                NameData = "TREANT",
                ManaCostData = 4,
                AttackData = 4,
                HealthData = 4,
                DescriptionData = "",
                ArtData = Properties.Resources.Treant,
            },
            new CardBuilder()
            {
                DeckID = 4,
                TypeID = CardBuilder.CardType.Minion,

                NameData = "HARPY",
                ManaCostData = 5,
                AttackData = 5,
                HealthData = 5,
                DescriptionData = "",
                //ArtData = Properties.Resources.Treant,
            },
            new CardBuilder()
            {
                DeckID = 5,
                TypeID = CardBuilder.CardType.Minion,

                NameData = "TROLL",
                ManaCostData = 6,
                AttackData = 6,
                HealthData = 6,
                DescriptionData = "",
                //ArtData = Properties.Resources.Treant,
            },
            new CardBuilder()
            {
                DeckID = 6,
                TypeID = CardBuilder.CardType.Minion,

                NameData = "OGRE",
                ManaCostData = 7,
                AttackData = 8,
                HealthData = 8,
                DescriptionData = "",
                //ArtData = Properties.Resources.Treant,
            },
            new CardBuilder()
            {
                DeckID = 7,
                TypeID = CardBuilder.CardType.Minion,

                NameData = "GIANT",
                ManaCostData = 9,
                AttackData = 12,
                HealthData = 12,
                DescriptionData = "",
                //ArtData = Properties.Resources.Treant,
            },
            new CardBuilder()
            {
                DeckID = 8,
                TypeID = CardBuilder.CardType.Power,

                NameData = "CLEAR SKY",
                ManaCostData = 2,
                DescriptionData = "NO EFFECT",
                ArtData = Properties.Resources.ClearSky,
            },
            new CardBuilder()
            {
                DeckID = 9,
                TypeID = CardBuilder.CardType.Spell,

                NameData = "CLEAN SHOT",
                ManaCostData = 7,
                DescriptionData = "REMOVE A MINION",
                TargetedData = true,
                ArtData = Properties.Resources.Vanquish,
                SpellEffectData = (s, m) =>
                {
                    var Target = IdGenerator.GetById(m.Targeted) as MinionCard;
                    Target.Health = 0;
                },
            },
            new CardBuilder()
            {
                DeckID = 10,
                TypeID = CardBuilder.CardType.Power,

                NameData = "ARENA",
                ManaCostData = 7,
                DescriptionData = "MINIONS GAIN +3 ATK ATTACKING",
                ArtData = Properties.Resources.GrandArena,
                PowerEffectData = new EffectData<PowerCard>()
                {{
                        Effect.MinionAttacking, (s, m) => { s.LastMove.Targeted.AsCardT<MinionCard>().Attack += 3; }
                }},
            },
            new CardBuilder()
            {
                DeckID = 11,
                TypeID = CardBuilder.CardType.Power,

                NameData = "DARK CAVE",
                ManaCostData = 6,
                DescriptionData = "ALL MINIONS +1/+1 WHEN CARD PLAYED",
                ArtData = Properties.Resources.WhispersMadness,
                PowerEffectData = new EffectData<PowerCard>
                {{
                        Effect.CardPlayed, (s, m) =>
                        {
                            foreach (MinionCard c in s.AllOnboardMinions())
                            {
                                    (c as MinionCard).Attack += 1;
                                    (c as MinionCard).Health += 1;
                            }
                        }
                }},
            },
            new CardBuilder()
            {
                DeckID = 12,
                TypeID = CardBuilder.CardType.Spell,

                NameData = "VORTEX",
                ManaCostData = 12,
                DescriptionData = "REMOVE ALL MINIONS",
                TargetedData = false,
                ArtData = Properties.Resources.Vortex,
                SpellEffectData = (s, m) =>
                {
                    foreach (MinionCard mn in s.AllOnboardMinions())
                        mn.Health = 0;
                },
            },
            new CardBuilder()
            {
                DeckID = 13,
                TypeID = CardBuilder.CardType.Spell,

                NameData = "LESSER BOLT",
                ManaCostData = 4,
                DescriptionData = "DEAL 4 DAMAGE",
                TargetedData = true,
                // ArtData = Properties.Resources.a,
                SpellEffectData = (s, m) =>
                {
                    switch (m.Targeted.AsCard())
                    {
                        case MinionCard minion:
                            minion.Health -= 4;
                            return;
                        case HeroCard hero:
                            s.InactivePlayer.Health -= 4;
                            return;
                        default: return;
                    }
                }
            },
            new CardBuilder()
            {
                DeckID = 14,
                TypeID = CardBuilder.CardType.Spell,

                NameData = "MAGIC BOLT",
                ManaCostData = 7,
                DescriptionData = "DEAL 7 DAMAGE",
                TargetedData = true,
                // ArtData = Properties.Resources.a,
                SpellEffectData = (s, m) =>
                {
                    switch (m.Targeted.AsCard())
                    {
                        case MinionCard minion:
                            minion.Health -= 7;
                            return;
                        case HeroCard hero:
                            s.PlayerTwo.Health -= 7;
                            return;
                        default: return;
                    }
                }
            },
            new CardBuilder()
            {
                DeckID = 15,
                TypeID = CardBuilder.CardType.Spell,

                NameData = "GREATER BOLT",
                ManaCostData = 11,
                DescriptionData = "DEAL 10 DAMAGE",
                TargetedData = true,
                // ArtData = Properties.Resources.a,
                SpellEffectData = (s, m) =>
                {
                    switch (m.Targeted.AsCard())
                    {
                        case MinionCard minion:
                            minion.Health -= 10;
                            return;
                        case HeroCard hero:
                            s.InactivePlayer.Health -= 10;
                            return;
                        default: return;
                    }
                }
            },
            new CardBuilder()
            {
                DeckID = 16,
                TypeID = CardBuilder.CardType.Spell,

                NameData = "UPSIZE",
                ManaCostData = 4,
                DescriptionData = "ALL MINIONS +1/+1",
                TargetedData = false,
                // Art
                SpellEffectData = (s, m) =>
                {
                    foreach (MinionCard mn in s.AllOnboardMinions())
                    {
                        mn.Health += 1;
                        mn.Attack += 1;
                    }
                }
            },
            new CardBuilder()
            {
                DeckID = 17,
                TypeID = CardBuilder.CardType.Minion,

                NameData = "LICH",
                ManaCostData = 5,
                AttackData = 4,
                HealthData = 2,
                DescriptionData = "DRAW CARD WHEN MINION DIES.",
                // Art,
                MinionEffectData = new EffectData<MinionCard>()
                { {
                        Effect.MinionKilled,
                        (s, m) =>
                        {
                            m.Owner.DrawCard();
                        }
                } }
            },
        }
        // Keep these sorted by mana cost
        .OrderBy(x => x.ManaCostData).ToList();
    }
}
