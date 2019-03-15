using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cards
{
    public class CardBuilder
    {
        // See below
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

        // Nullable - these are only relevant for minions and are otherwise null
        public int? AttackData;
        public int? HealthData;
        public bool? TargetedData;
        // These are specific effects that are only used for certain types of cards
        // Expect them to be null otherwise
        public EffectData<MinionCard> MinionEffectData;
        public EffectData<PowerCard> PowerEffectData;
        public Action<GameState, Move> MinionOnPlayData;
        public Action<GameState, Move> SpellEffectData;

        public BaseCard Build(GameState g, BasePlayer owner) 
        {
            // Used to fill in when left blank in the builder
            Action<GameState, Move> EmptyFunc = (_g, _s) => { return; };
            // Every class has different properties set
            // We switch on TypeID to determine this
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

        public static bool ValidateDeck(IEnumerable<CardBuilder> cards, out string why)
        {
            // Check for too many duplicates
            // "why" out parameter used to give a reason the deck is invalid
            // Has to be set before returning from the function
            why = "";
            // Checking no cards are duplicate by associating each ID with a count
            // Also check for null cards
            var Dict = new Dictionary<int, int>();
            foreach (CardBuilder c in cards)
            {
                if (c == null)
                {
                    why = "One or more card slots is unassigned.";
                    return false;
                }
                if (Dict.ContainsKey(c.DeckID))
                {
                    Dict[c.DeckID] += 1;
                }
                else
                {
                    Dict.Add(c.DeckID, 1);
                }
            }
            // Perform other checks - correct no. total cards
            foreach (var v in Dict.Values)
            {
                if (v > 2)
                {
                    why = "Too many copies of one card. (maximum: 2)";
                    return false;
                }
            }
            if (cards.Count() != 25)
            {
                why = "Incorrect number of cards. (must have: 25)";
                return false;
            }
            return true;
        }
    }
}
