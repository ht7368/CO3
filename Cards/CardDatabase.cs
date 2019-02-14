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
                DescriptionData = "+3/+3 FOR EACH 'BOAR' ON BOARD",
                ArtData = Properties.Resources.Runt,
                MinionOnPlayData = (s, m) =>
                {
                    foreach (MinionCard minion in s.ActivePlayer.Board)
                        if (minion.Name == "BOAR")
                        {
                            MinionCard Self = m.Selected.AsCardT<MinionCard>();
                            Self.Health += 3;
                            Self.Attack += 3;
                        }
                },
            },
            new CardBuilder()
            {
                DeckID = 2,
                TypeID = CardBuilder.CardType.Minion,

                NameData = "BOAR",
                ManaCostData = 3,
                AttackData = 3,
                HealthData = 3,
                DescriptionData = "A 'RUNT' CAN ATTACK AGAIN AFTER THIS ATTACKS",
                ArtData = Properties.Resources.Boar,
                MinionEffectData = new EffectData<MinionCard>()
                {{
                        Effect.MinionAttacking, (s, m) =>
                        {
                            foreach (MinionCard minion in m.Owner.Board)
                                if (minion.Name == "RUNT")
                                    minion.CanAttack = true;
                        }
                }}
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
                ArtData = Properties.Resources.Harpy,
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
                ArtData = Properties.Resources.Troll,
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
                ArtData = Properties.Resources.Ogre,
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
                ArtData = Properties.Resources.Giant,
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
                        Effect.MinionAttacking, (s, m) =>
                        {
                            s.LastMove.Selected.AsCardT<MinionCard>().Attack += 3;
                        }
                }},
            },
            new CardBuilder()
            {
                DeckID = 11,
                TypeID = CardBuilder.CardType.Power,

                NameData = "DARK CAVE",
                ManaCostData = 5,
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
                ManaCostData = 11,
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
                ManaCostData = 3,
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
                ManaCostData = 6,
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
                ManaCostData = 9,
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
                ArtData = Properties.Resources.Upsize,
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
                ArtData = Properties.Resources.Lich,
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
                HealthData = 1,
                DescriptionData = "SUMMON 2 1/1 'INSECT SWARM'",
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
                DescriptionData = "SUMMON A 1/1 'INSECT SWARM' ON OWNER'S TURN START",
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
                ManaCostData = 11,
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
                ManaCostData = 3,
                DescriptionData = "SUMMON 1/1 'INSECT SWARM' AT PLAYER TURN END",
                ArtData = Properties.Resources.InsectHive,
                PowerEffectData = new EffectData<PowerCard>
                {{
                        Effect.TurnEnd, (s, m) =>
                        {
                            s.ActivePlayer.Board.Add(CardFromID(18).Build(s, s.ActivePlayer) as MinionCard);
                            s.ActivePlayer.Board[s.ActivePlayer.Board.Count - 1].OnBoard = true;
                        }
                }},
            },
            new CardBuilder()
            {
                DeckID = 22,
                TypeID = CardBuilder.CardType.Spell,

                NameData = "INSECT PLAGUE",
                ManaCostData = 5,
                DescriptionData = "ALL INSECTS GAIN +2/+2",
                TargetedData = false,
                ArtData = Properties.Resources.InsectQuad,
                SpellEffectData = (s, m) =>
                {
                    foreach (MinionCard mn in m.Selected.AsCard().Owner.Board)
                        if (mn.Name.Contains("INSECT"))
                        {
                            mn.Attack += 2;
                            mn.Health += 2;
                        }
                },
            },
            new CardBuilder()
            {
                DeckID = 23,
                TypeID = CardBuilder.CardType.Spell,

                NameData = "MUTATION",
                ManaCostData = 8,
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
            },
            new CardBuilder()
            {
                DeckID = 24,
                TypeID = CardBuilder.CardType.Minion,

                NameData = "ZOMBIE",
                ManaCostData = 5,
                AttackData = 4,
                HealthData = 4,
                DescriptionData = "DRAW A CARD WHEN PLAYED",
                ArtData = Properties.Resources.DeadRising,
                MinionOnPlayData = (s, m) =>
                {
                    m.Targeted.AsCardT<MinionCard>().Owner.DrawCard();
                },
            },
            new CardBuilder()
            {
                DeckID = 25,
                TypeID = CardBuilder.CardType.Spell,

                NameData = "STORM",
                ManaCostData = 5,
                DescriptionData = "DEAL 4 DAMAGE TO ALL MINIONS",
                TargetedData = false,
                ArtData = Properties.Resources.Gust,
                SpellEffectData = (s, m) =>
                {
                    foreach (MinionCard mm in s.AllOnboardMinions())
                        mm.Health -= 4;
                }
            },
            new CardBuilder()
            {
                DeckID = 26,
                TypeID = CardBuilder.CardType.Spell,

                NameData = "FLOOD",
                ManaCostData = 8,
                DescriptionData = "DEAL 6 DAMAGE TO ALL MINIONS",
                TargetedData = false,
                ArtData = Properties.Resources.Tsunami,
                SpellEffectData = (s, m) =>
                {
                    foreach (MinionCard mm in s.AllOnboardMinions())
                        mm.Health =- 6;
                }
            },
            new CardBuilder()
            {
                DeckID = 27,
                TypeID = CardBuilder.CardType.Minion,

                NameData = "MAGI",
                ManaCostData = 8,
                AttackData = 5,
                HealthData = 1,
                DescriptionData = "OWNER'S SPELLS ARE CAST TWICE",
                ArtData = Properties.Resources.Magi,
                MinionEffectData = new EffectData<MinionCard>()
                {{
                        Effect.CardPlayed, (s, c) =>
                        {
                            if (s.LastMove.Selected.AsCard() is SpellCard sp && sp.Owner == c.Owner)
                                sp.SpellEffect(s, s.LastMove);
                        }
                }}
            },
            new CardBuilder()
            {
                DeckID = 28,
                TypeID = CardBuilder.CardType.Power,

                NameData = "ASCENT",
                ManaCostData = 6,
                DescriptionData = "GAIN 1 MANA WHEN CARD PLAYED; GAIN 2 AT TURN START",
                ArtData = Properties.Resources.Ascent,
                PowerEffectData = new EffectData<PowerCard>()
                {{
                        Effect.CardPlayed, (s, c) =>
                        {
                            s.ActivePlayer.Mana += 1;
                        }
                }, {
                        Effect.TurnStart, (s, c) =>
                        {
                            s.ActivePlayer.Mana += 2;
                        }
                }}
            },
            new CardBuilder()
            {
                DeckID = 29,
                TypeID = CardBuilder.CardType.Spell,

                NameData = "INNOVATION",
                ManaCostData = 0,
                DescriptionData = "GAIN 2 MANA",
                ArtData = Properties.Resources.Innovation,
                TargetedData = false,
                SpellEffectData = (s, m) =>
                {
                    s.ActivePlayer.Mana += 2;
                }
            },
            new CardBuilder()
            {
                DeckID = 30,
                TypeID = CardBuilder.CardType.Power,

                ManaCostData = 6,
                NameData = "ECLIPSE",
                DescriptionData = "50% TO DEAL 1 DAMAGE TO ALL MINIONS TURN START AND END",
                ArtData = Properties.Resources.Eclipse,
                PowerEffectData = new EffectData<PowerCard>()
                {{
                        Effect.TurnEnd, (s, c) =>
                        {
                            if (s.RNG.Next(0, 2) == 1)
                                foreach (MinionCard m in s.AllOnboardMinions())
                                    m.Health -= 1;
                        }
                }, {
                        Effect.TurnStart, (s, c) =>
                        {
                            if (s.RNG.Next(0, 2) == 1)
                                foreach (MinionCard m in s.AllOnboardMinions())
                                    m.Health -= 1;
                        }
                }},
            },
            new CardBuilder()
            {
                DeckID = 31,
                TypeID = CardBuilder.CardType.Spell, 
                
                ManaCostData = 5,
                NameData = "INTELLECT",
                DescriptionData = "DRAW 3 CARDS",
                TargetedData = false,
                ArtData = Properties.Resources.Intelligence,
                SpellEffectData = (s, m) =>
                {
                    s.ActivePlayer.DrawCard();
                    s.ActivePlayer.DrawCard();
                    s.ActivePlayer.DrawCard();
                }
            },
            new CardBuilder()
            {
                DeckID = 32,
                TypeID = CardBuilder.CardType.Spell,
                
                ManaCostData = 3,
                NameData = "ASSAULT",
                DescriptionData = "DEAL 7 DAMAGE TO ENEMY'S HERO",
                TargetedData = false,
                ArtData = Properties.Resources.Hunted,
                SpellEffectData = (s, m) =>
                {
                    s.InactivePlayer.Health -= 7;
                }
            },
            new CardBuilder()
            {
                DeckID = 33,
                TypeID = CardBuilder.CardType.Spell,

                NameData = "DOWNSIZE",
                ManaCostData = 5,
                DescriptionData = "ALL MINIONS -2/-2",
                TargetedData = false,
                ArtData = Properties.Resources.Downsize,
                SpellEffectData = (s, m) =>
                {
                    foreach (MinionCard mm in s.AllOnboardMinions())
                    {
                        mm.Health -= 2;
                        mm.Attack -= 2;
                    }
                }
            }
        }
        // Keep these sorted by mana cost
        .OrderBy(x => x.ManaCostData).ToList();
    }
}
