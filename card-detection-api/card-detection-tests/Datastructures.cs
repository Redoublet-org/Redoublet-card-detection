using Redoublet.Backend.Models;
using System.Windows.Markup;

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

    public class DetectionTests
    {
        [Theory]
        [InlineData("As: 99% (left_x:  179   top_y:  172   width:   65   height:  101)", 0.99)]
        [InlineData("2d: 90% (left_x:  269   top_y:  121   width:   45   height:  104)", 0.90)]
        [InlineData("3s: 99% (left_x:  365   top_y:   94   width:   43   height:  111)", 0.99)]
        [InlineData("5c: 84% (left_x:  408   top_y:  424   width:   60   height:   90)", 0.84)]
        [InlineData("4h: 82% (left_x:  450   top_y:   90   width:   39   height:   98)", 0.82)]
        public void ParseConfidenceFromLines(string line, double confidence)
        {
            Detection detection = new Detection(line);

            Assert.Equal(detection.Confidence, confidence);
        }

        [Theory]
        [InlineData("As: 99% (left_x:  179   top_y:  172   width:   65   height:  101)", 179, 172, 65, 101)]
        [InlineData("2d: 90% (left_x:  269   top_y:  121   width:   45   height:  104)", 269, 121, 45, 104)]
        [InlineData("3s: 99% (left_x:  365   top_y:   94   width:   43   height:  111)", 365, 94, 43, 111)]
        [InlineData("5c: 84% (left_x:  408   top_y:  424   width:   60   height:   90)", 408, 424, 60, 90)]
        [InlineData("4h: 82% (left_x:  450   top_y:   90   width:   39   height:   98)", 450, 90, 39, 98)]
        public void ParseBoundingBoxFromLines(string line, int x, int y, int w, int h)
        {
            BoundingBox boundingBox = new BoundingBox(line);

            Assert.Equal(boundingBox.LeftX, x);
            Assert.Equal(boundingBox.TopY, y);
            Assert.Equal(boundingBox.Width, w);
            Assert.Equal(boundingBox.Height, h);
        }

        [Fact]
        public void ParseBoundingBoxNoStart()
        {
            string line = "As: 99% (l 179   top_y:  172   width:   65   height:  101)";

            Assert.Throws<ArgumentException>(() => new BoundingBox(line));
        }

        [Fact]
        public void ParseBoundingBoxIncorrectNumDim()
        {
            string line = "As: 99% (l 179   top_y:  172";

            Assert.Throws<ArgumentException>(() => new BoundingBox(line));
        }

        [Fact]
        public void ParseBoundingBoxIncorrectDimFormat()
        {
            string line = "4h: 82% (left_x:  s50   top_y:   9d0   width:   f39   height:   9s)";

            Assert.Throws<ArgumentException>(() => new BoundingBox(line));
        }
    }
}
