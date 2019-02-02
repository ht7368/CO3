﻿using System;
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
                DescriptionData = "MINIONS GAIN +3/+0 WHEN ATTACKING",
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
                            foreach (MinionCard mm in s.AllOnboardMinions())
                            {
                                mm.Attack += 1;
                                mm.Health += 1;
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
                ArtData = Properties.Resources.ScrollWeak,
                SpellEffectData = (s, m) =>
                {
                    switch (m.Targeted.AsCard())
                    {
                        case MinionCard minion:
                            minion.Health -= 4;
                            return;
                        case HeroCard hero:
                            hero.Owner.Health -= 4;
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
                ArtData = Properties.Resources.ScrollMagic,
                SpellEffectData = (s, m) =>
                {
                    switch (m.Targeted.AsCard())
                    {
                        case MinionCard minion:
                            minion.Health -= 7;
                            return;
                        case HeroCard hero:
                            hero.Owner.Health -= 7;
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
                ArtData = Properties.Resources.ScrollStrong,
                SpellEffectData = (s, m) =>
                {
                    switch (m.Targeted.AsCard())
                    {
                        case MinionCard minion:
                            minion.Health -= 10;
                            return;
                        case HeroCard hero:
                            hero.Owner.Health -= 10;
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
                DescriptionData = "DRAW A CARD WHEN MINION DIES.",
                // Art,
                MinionEffectData = new EffectData<MinionCard>()
                {{
                        Effect.MinionKilled,
                        (s, m) =>
                        {
                            m.Owner.DrawCard();
                        }
                }}
            },
            new CardBuilder()
            {
                DeckID = 18,
                TypeID = CardBuilder.CardType.Minion,

                NameData = "INSECT SWARM",
                ManaCostData = 4,
                AttackData = 1,
                HealthData = 2,
                DescriptionData = "SUMMON 2 1/2 'INSECT SWARM'",
                ArtData = Properties.Resources.InsectSingle,
                MinionOnPlayData = (s, m) =>
                {
                    var Owner = m.Selected.AsCardT<MinionCard>().Owner;
                    Owner.Board.Add(CardFromID(18).Build(s, Owner) as MinionCard);
                    Owner.Board[Owner.Board.Count - 1].OnBoard = true;
                    Owner.Board.Add(CardFromID(18).Build(s, Owner) as MinionCard);
                    Owner.Board[Owner.Board.Count - 1].OnBoard = true;
                }
            },
            new CardBuilder()
            {
                DeckID = 19,
                TypeID = CardBuilder.CardType.Minion,

                NameData = "INSECT QUEEN",
                ManaCostData = 6,
                AttackData = 3,
                HealthData = 4,
                DescriptionData = "SUMMON A 1/2 'INSECT SWARM' ON OWNER'S TURN START",
                ArtData = Properties.Resources.InsectDouble,
                MinionEffectData = new EffectData<MinionCard>
                {{
                        Effect.TurnStart, (s, m) =>
                        {
                            if (m.Owner == s.ActivePlayer)
                            {
                                m.Owner.Board.Add(CardFromID(18).Build(s, m.Owner) as MinionCard);
                                m.Owner.Board[m.Owner.Board.Count - 1].OnBoard = true;
                            }
                        }
                }}
            },
            new CardBuilder()
            {
                DeckID = 20,
                TypeID = CardBuilder.CardType.Minion,

                NameData = "INSECT INFEST",
                ManaCostData = 10,
                AttackData = 4,
                HealthData = 8,
                DescriptionData = "SUMMON A 3/4 'INSECT QUEEN' ON OWNER'S TURN END",
                ArtData = Properties.Resources.InsectTriple,
                MinionEffectData = new EffectData<MinionCard>
                {{
                        Effect.TurnEnd, (s, m) =>
                        {
                            if (m.Owner == s.ActivePlayer)
                            {
                                m.Owner.Board.Add(CardFromID(19).Build(s, m.Owner) as MinionCard);
                                m.Owner.Board[m.Owner.Board.Count - 1].OnBoard = true;
                            }
                        }
                }},
            },
            new CardBuilder()
            {
                DeckID = 21,
                TypeID = CardBuilder.CardType.Power,

                NameData = "INSECT HIVE",
                ManaCostData = 4,
                DescriptionData = "SUMMON 1/2 'INSECT SWARM' FOR BOTH PLAYERS ON CARD DRAW",
                ArtData = Properties.Resources.InsectHive,
                PowerEffectData = new EffectData<PowerCard>
                {{
                        Effect.CardDrawn, (s, m) =>
                        {
                            m.Owner.Board.Add(CardFromID(18).Build(s, m.Owner) as MinionCard);
                            m.Owner.Board[m.Owner.Board.Count - 1].OnBoard = true;
                            BasePlayer Other = s.PlayerOne;
                            if (m.Owner == s.PlayerOne)
                                Other = s.PlayerTwo;
                            Other.Board.Add(CardFromID(18).Build(s, Other) as MinionCard);
                            Other.Board[Other.Board.Count - 1].OnBoard = true;
                        }
                }},
            },
            new CardBuilder()
            {
                DeckID = 22,
                TypeID = CardBuilder.CardType.Spell,

                NameData = "INSECT PLAGUE",
                ManaCostData = 5,
                DescriptionData = "ALL INSECTS GAIN +1/+2",
                TargetedData = false,
                ArtData = Properties.Resources.InsectQuad,
                SpellEffectData = (s, m) =>
                {
                    foreach (MinionCard mn in m.Selected.AsCard().Owner.Board)
                        if (mn.Name.Contains("INSECT"))
                        {
                            mn.Attack += 1;
                            mn.Health += 2;
                        }
                },
            },
            new CardBuilder()
            {
                DeckID = 23,
                TypeID = CardBuilder.CardType.Spell,

                NameData = "MUTATION",
                ManaCostData = 12,
                DescriptionData = "DOUBLE YOUR MINION STATS",
                TargetedData = false,
                ArtData = Properties.Resources.Evolution,
                SpellEffectData = (s, m) =>
                {
                    foreach (MinionCard mn in m.Selected.AsCard().Owner.Board)
                    {
                        mn.Health *= 2;
                        mn.Attack *= 2;
                    }
                }
            }
        }
        // Keep these sorted by mana cost
        .OrderBy(x => x.ManaCostData).ToList();
    }
}
