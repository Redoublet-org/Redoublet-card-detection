using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics.Metrics;

namespace object_detection_backend
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string darknet_folder_path = "C:\\Users\\thoma\\Desktop\\darknet";
            string relative_executable = "darknet.exe";
            string relative_data       = "build\\darknet\\x64\\data\\obj.data";
            string relative_config     = "cfg\\yolov4-obj.cfg";
            string relative_weights    = "backup\\yolov4-obj_9000.weights";

            string executable_path = Path.Combine(darknet_folder_path, relative_executable);
            string data_path       = Path.Combine(darknet_folder_path, relative_data);
            string config_path     = Path.Combine(darknet_folder_path, relative_config);
            string weights_path    = Path.Combine(darknet_folder_path, relative_weights);

            string[] arguments = new string[]
            {
                "detector", "test",
                data_path, config_path, weights_path,
                "-dont_show", // Geen image preview
                "-ext_output" // Extra informatie over hitboxes
            };

            ProcessStartInfo startInfo = new();
            startInfo.Arguments = String.Join(" ", arguments);
            startInfo.WorkingDirectory = darknet_folder_path;
            startInfo.FileName = executable_path;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;

            var proc = Process.Start(startInfo);

            Queue<string> images = new Queue<string>();
            images.Enqueue("C:\\Users\\thoma\\Desktop\\AcetoFive.JPG");
            images.Enqueue("C:\\Users\\thoma\\Desktop\\Skatblatt_02.jpg");
            images.Enqueue("C:\\Users\\thoma\\Desktop\\AcetoFive.JPG");

            AwaitInputState(proc);
            while (images.Count > 0)
            {
                ProcessNextImage(proc, images);
            }
        }
        
        /// <summary>
        /// Awaits darknet startup untill images can be entered
        /// </summary>
        /// <param name="proc">The running darknet process</param>
        static void AwaitInputState(Process proc)
        {
            string outp = "";
            while (!proc.HasExited)
            {
                int x = proc.StandardOutput.Read();
                //int c = proc.StandardError.Read();
                //Console.Write(((char)c).ToString());

                if (outp.EndsWith("\n") || outp.EndsWith(":"))
                {
                    Console.Write(outp);

                    if (outp == "Enter Image Path:")
                    {
                        return;
                    }

                    outp = ((char)x).ToString();


                }
                else outp += ((char)x).ToString();

            }
        }

        /// <summary>
        /// Processes the next image in the Queue. Should only be called if the process is in an
        /// input accepting state. Waiting untill this function returns true will ensure that process is in the 
        /// correct state.
        /// </summary>
        /// <param name="proc">The process containing the running darknet application</param>
        /// <param name="images">Queue of images to work through</param>
        /// <returns>True if the next image was analysed. False if no more images were available.s</returns>
        static bool ProcessNextImage(Process proc, Queue<string> images)
        {
            if (images.Count == 0) return false;

            string img = images.Dequeue();

            proc.StandardInput.WriteLine(img);
            proc.StandardInput.Flush();
            proc.StandardOutput.DiscardBufferedData();

            string line = "";

            DetectionResult detectionResult = new();
            detectionResult.FileName = img;

            while (true)
            {
                char f = (char)proc.StandardOutput.Read();
                line += f;
                if (line == "Enter Image Path: ")
                {
                    return true;
                }

                if (line.EndsWith("\n"))
                {
                    Console.Write(line);
                    if (line.Contains("left_x"))
                    {
                        detectionResult.Detections.Add(new Detection(line));
                    }
                    line = "";
                }
            }
        }
    }
}