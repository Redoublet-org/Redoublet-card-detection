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
    public class Detection
    {
        public Card Card { get; }
        public BoundingBox BoundingBox { get; }
        public double Confidence { get; }

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

    public class BoundingBox
    {
        public int LeftX { get; }
        public int TopY { get; }
        public int Width { get; }
        public int Height { get; }

        /// <summary>
        /// Takes string of form "left_x: xxx top_y: xxx width: xxx height: xxx"
        /// </summary>
        /// <param name="s"></param>
        public BoundingBox(string s)
        {
            string[] args = s.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            int start = Array.IndexOf(args, "(left_x:");
            if (start == -1) throw new ArgumentException("Input did not contain '(left_x:'. Could not parse bounding box.");
            if (start + 7 >= args.Length) throw new ArgumentException("Input does not contain enough fields to parse bounding box");

            try
            {
                LeftX  = int.Parse(args[start + 1]);
                TopY   = int.Parse(args[start + 3]);
                Width  = int.Parse(args[start + 5]);
                Height = int.Parse(args[start + 7].TrimEnd(new char[] {')', '\r', '\n'}));
            }
            catch {
                throw new ArgumentException("Bounding box dimensions were not in integer format.");
            }
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
