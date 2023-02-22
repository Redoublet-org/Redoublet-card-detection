using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace object_detection_backend
{
    /// <summary>
    /// Represents a singular detected card
    /// </summary>
    internal class Detection
    {
        internal Card Card;
        internal BoundingBox BoundingBox;
        internal double Confidence; 

        public Detection(string s)
        {
            string[] args = s.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            Card = new Card(args[0].TrimEnd(':'));
            Confidence = double.Parse(args[1].TrimEnd('%')) / 100;
            BoundingBox = new BoundingBox(s);
        }

        public Detection(Card card, BoundingBox boundingBox)
        {
            Card = card;
            BoundingBox = boundingBox;
        }

        public override string ToString()
        {
            return $"{Card} {Confidence * 100}% at [{BoundingBox}]";
        }
    }

    internal class BoundingBox
    {
        internal int LeftX { get; }
        internal int TopY { get; }
        internal int Width { get; }
        internal int Height { get; }

        /// <summary>
        /// Takes string of form "left_x: xxx top_y: xxx width: xxx height: xxx"
        /// </summary>
        /// <param name="s"></param>
        public BoundingBox(string s)
        {
            string[] args = s.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            int start = Array.IndexOf(args, "(left_x:");
            LeftX  = int.Parse(args[start + 1]);
            TopY   = int.Parse(args[start + 3]);
            Width  = int.Parse(args[start + 5]);
            Height = int.Parse(args[start + 7].TrimEnd(new char[] {')', '\r', '\n'}));
            Console.WriteLine();
        }

        public BoundingBox(int leftX, int topY, int width, int height)
        {
            LeftX = leftX;
            TopY = topY;
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"x: {LeftX}, y: {TopY}, w: {Width}, h: {Height}";
        }
    }
}
