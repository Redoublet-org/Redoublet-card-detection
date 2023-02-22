using object_detection_backend;
using System.Diagnostics;
using System.Drawing;

namespace card_detection_api
{ 
    public class Darknet
    {
        public static Process process;

        public static void Initialize()
        {
            string darknet_folder_path = "C:\\Users\\thoma\\Desktop\\darknet";
            string relative_executable = "darknet.exe";
            string relative_data = "build\\darknet\\x64\\data\\obj.data";
            string relative_config = "cfg\\yolov4-obj.cfg";
            string relative_weights = "backup\\yolov4-obj_9000.weights";

            string executable_path = Path.Combine(darknet_folder_path, relative_executable);
            string data_path = Path.Combine(darknet_folder_path, relative_data);
            string config_path = Path.Combine(darknet_folder_path, relative_config);
            string weights_path = Path.Combine(darknet_folder_path, relative_weights);

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

            process = Process.Start(startInfo);

            Queue<string> images = new Queue<string>();
            images.Enqueue("C:\\Users\\thoma\\Desktop\\AcetoFive.JPG");
            images.Enqueue("C:\\Users\\thoma\\Desktop\\Skatblatt_02.jpg");
            images.Enqueue("C:\\Users\\thoma\\Desktop\\AcetoFive.JPG");

            AwaitInputState();
        }

        /// <summary>
        /// Awaits darknet startup untill images can be entered
        /// </summary>
        /// <param name="proc">The running darknet process</param>
        public static void AwaitInputState()
        {
            string outp = "";
            while (!process.HasExited)
            {
                int x = process.StandardOutput.Read();
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
        public static DetectionResult ProcessImage(string img)
        {
            if (process == null) throw new InvalidOperationException("Darknet process is not yet running. Did you initialize?");

            process.StandardInput.WriteLine(img);
            process.StandardInput.Flush();
            process.StandardOutput.DiscardBufferedData();

            string line = "";

            DetectionResult detectionResult = new();
            detectionResult.FileName = img;

            while (true)
            {
                char f = (char)process.StandardOutput.Read();
                line += f;
                if (line == "Enter Image Path: ")
                {
                    return detectionResult;
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
