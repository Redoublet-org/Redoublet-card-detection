namespace object_detection_backend
{
    internal class DetectionResult
    {
        public List<Detection> Detections;
        public double Milliseconds;
        public string FileName;

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
