using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cards
{
    public class CardBuilder
    {
        public enum CardType
        {
            Minion,
            Spell,
            Power
        }

        // ID used to determine card put in deck;
        public int DeckID;
        public CardType TypeID;

        public int ManaCostData;
        public string NameData;
        public string DescriptionData;
        public System.Drawing.Image ArtData;

        public int? AttackData;
        public int? HealthData;
        public bool? TargetedData;
        public EffectData<MinionCard> MinionEffectData;
        public EffectData<PowerCard> PowerEffectData;
        public Action<GameState, Move> MinionOnPlayData;
        public Action<GameState, Move> SpellEffectData;

        public BaseCard Build(GameState g, BasePlayer owner) 
        {
            Action<GameState, Move> EmptyFunc = (_g, _s) => { return; };
            if (TypeID == CardType.Minion)
                return new MinionCard(g)
                {
                    Owner = owner,
                    ManaCost = ManaCostData,
                    Name = NameData,
                    Description = DescriptionData,
                    Art = ArtData,
                    Attack = AttackData.Value,
                    Health = HealthData.Value,
                    Effects = MinionEffectData ?? new EffectData<MinionCard>(),
                    OnPlay = MinionOnPlayData,
                    OnBoard = false,
                    CanAttack = false,
                };
            else if (TypeID == CardType.Spell)
                return new SpellCard(g)
                {
                    Owner = owner,
                    ManaCost = ManaCostData,
                    Name = NameData,
                    Description = DescriptionData,
                    Art = ArtData,
                    SpellEffect = SpellEffectData ?? EmptyFunc,
                    IsTargeted = TargetedData.Value,
                };
            else if (TypeID == CardType.Power)
                return new PowerCard(g)
                {
                    Owner = owner,
                    ManaCost = ManaCostData,
                    Name = NameData,
                    Description = DescriptionData,
                    Art = ArtData,
                    Effects = PowerEffectData ?? new EffectData<PowerCard>(),
                };
            // Can never happen
            return null;
        }

        public override string ToString()
        {
            return NameData;
        }
    }

    public static partial class Cards
    {
        public static CardBuilder CardFromID(byte ID)
        {
            foreach (CardBuilder c in Collection)
                if (c.DeckID == ID)
                    return c;
            return null;
        }

        public static bool ValidateDeck(CardLabel[] cards, out string why)
        {
            // Check for too many duplicates
            var Dict = new Dictionary<int, int>();
            foreach (CardBuilder c in cards.Select(x => x.Card))
                if (c == null)
                {
                    why = "One or more card slots is unassigned.";
                    return false;
                }
                else if (Dict.ContainsKey(c.DeckID))
                    Dict[c.DeckID] += 1;
                else
                    Dict.Add(c.DeckID, 1);
            foreach (var v in Dict.Values)
                if (v > 2)
                {
                    why = "Too many copies of one card. (maximum: 2)";
                    return false;
                }
            if (cards.Length != 25)
            {
                why = "Incorrect number of cards. (must have: 25)";
                return false;
            }
            why = "";
            return true;
        }

        // (Subject to change?) A collection of all cards. More specifically,
        // functions that take a GameState to return a card.
    }
}
