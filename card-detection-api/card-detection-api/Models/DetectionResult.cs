namespace Redoublet.Backend.Models
{
    public class DetectionResult
    {
        public List<Detection> Detections { get; }
        public double Milliseconds { get; set; }
        public string FileName { get; set; }

        public DetectionResult()
        {
            Detections = new List<Detection>();
            Milliseconds = -1;
        }

        public DetectionResult(List<Detection> detections, double milliseconds)
        {
            Detections = detections;
            Milliseconds = milliseconds;
        }

        public override string ToString()
        {
            return $" {Detections.Count} detections in {Milliseconds}ms";
        }
    }
}
