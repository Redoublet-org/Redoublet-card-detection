using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace object_detection_backend
{
    public class Card
    {
        public Suit Suit { get; }
        public Value Value { get; }

        public Card(string value_suit)
        {
            Suit = suitFromChar(value_suit[^1]);
            Value = valueFromString(value_suit.Substring(0, value_suit.Length - 1));
        }

        public Card(Suit suit, Value value)
        {
            Suit = suit;
            Value = value;
        }

        public override string ToString()
        {
            return ValueString() + SuitString();
        }

        public string SuitString()
        {
            switch (Suit)
            {
                case Suit.Clubs:    return "c";
                case Suit.Diamonds: return "d";
                case Suit.Hearts:   return "h";
                case Suit.Spades:   return "s";
            }

            return "NULL SUIT";
        }

        public string ValueString()
        {
            if ((int)Value <= 10)
                return ((int)Value).ToString();
            
            switch(Value)
            {
                case Value.Ace:     return "A";
                case Value.King:    return "K";
                case Value.Queen:   return "Q";
                case Value.Jack:    return "J";
            }

            return "NULL VALUE";
        }

        private static Suit suitFromChar(char c)
        {
            switch(c)
            {
                case 'c': return Suit.Clubs;
                case 'd': return Suit.Diamonds;
                case 'h': return Suit.Hearts;
                case 's': return Suit.Spades;
                default: throw new ArgumentException("Suit char was not recognised; only c, d, h, and s are accepted");
            }
        }
        private static Value valueFromString(string s)
        {
            switch (s)
            {
                case "A": return Value.Ace;
                case "K": return Value.King;
                case "Q": return Value.Queen;
                case "J": return Value.Jack;
                default:
                    {
                        try
                        {
                            int val = int.Parse(s);
                            if (val < 2 || val > 10) throw new ArgumentException("Value was outside expected range");
                            return (Value)val;
                        }
                        catch (Exception e) { throw; }
                    }
            }
        }
    }

    public enum Suit
    {
        Clubs, Diamonds, Hearts, Spades
    }

    public enum Value
    {
        Two = 2, Three = 3, Four = 4, Five = 5, 
        Six = 6, Seven = 7, Eight = 8, Nine = 9, 
        Ten = 10, Jack = 11, Queen = 12, King = 13, 
        Ace = 14
    }
}
