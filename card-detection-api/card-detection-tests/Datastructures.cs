using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Primitives;
using object_detection_backend;

namespace card_detection_tests
{
    public class CardTests
    {
        [Fact]
        public void ParseSingleCard()
        {
            string cardString = "Ac";
            Value cardValue = Value.Ace;
            Suit cardSuit = Suit.Clubs;

            Card read = new Card(cardString);

            Assert.Equal(cardValue, read.Value);
            Assert.Equal(cardSuit, read.Suit);
        }

        [Theory]
        [MemberData(nameof(AllCards))]
        public void ParseAllCards(string cardString, Value cardValue, Suit cardSuit)
        {
            Card read = new Card(cardString);

            Assert.Equal(cardValue, read.Value);
            Assert.Equal(cardSuit, read.Suit);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("-1")]
        [InlineData("11")]
        [InlineData("1234532")]
        [InlineData("Ghfdjs")]
        [InlineData("KQJA")]
        public void ParseOutOfRangeValue(string value)
        {
            string suitString = "c";
            string cardString = value + suitString;

            Assert.Throws<ArgumentException>(() => new Card(cardString));
        }

        public static IEnumerable<object[]> AllCards
        {
            get
            {
                string[] valueString = new string[] { "A", "K", "Q", "J", "10", "9", "8", "7", "6", "5", "4", "3", "2" };
                string[] suitString  = new string[] { "c", "d", "h", "s" };
                Value[] valueEnum    = new Value[] { Value.Ace, Value.King, Value.Queen, Value.Jack, Value.Ten, Value.Nine, Value.Eight,
                                                  Value.Seven, Value.Six, Value.Five, Value.Four, Value.Three, Value.Two };
                Suit[] suitEnum      = new Suit[] { Suit.Clubs, Suit.Diamonds, Suit.Hearts, Suit.Spades };

                List<object[]> combinations = new(52);

                for (int v = 0; v < valueString.Length; v++)
                {
                    for (int s = 0; s < suitEnum.Length; s++)
                    {
                        combinations.Add(new object[] { valueString[v] + suitString[s], valueEnum[v], suitEnum[s] });
                    }
                }

                return combinations;
            }
        }
    }
}
