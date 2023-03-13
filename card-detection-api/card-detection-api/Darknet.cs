using Redoublet.Backend.Models;
using System.Diagnostics;

namespace Redoublet.Backend
{ 
    public class Darknet
    {
        public static Process process;

        public static void Initialize()
        {
            string[] configLines = File.ReadAllLines(Path.GetFullPath("./darknetconfig.txt"));
            string darknet_path = configLines[0];
            string data_path    = configLines[1];
            string config_path  = configLines[2];
            string weights_path  = configLines[3];

            string[] arguments = new string[]
            {
                "detector", "test",
                data_path, config_path, weights_path,
                "-dont_show", // Geen image preview
                "-ext_output" // Extra informatie over hitboxes
            };


            ProcessStartInfo startInfo = new();
            startInfo.Arguments = String.Join(" ", arguments);

            startInfo.FileName = darknet_path;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;

            process = Process.Start(startInfo);

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
        /// Processes the image given by the file path. Should only be called if the process is in an
        /// input accepting state. Waiting untill this function returns true will ensure that process is in the 
        /// correct state.
        /// </summary>
        /// <param name="img">File path to image</param>
        /// <returns>DetectionResult of the given image</returns>
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
