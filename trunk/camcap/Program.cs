using System;
using System.IO;
using System.Drawing;
using System.Security;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//requires AForge vision library: https://code.google.com/p/aforge/
//most of the credit for this goes to: http://www.codeproject.com/Questions/456777/Capturing-image-from-a-webcam

namespace info.arace.andrew.camcap {

    class Program {
        private static void PrintUsage() {
            Console.Out.WriteLine("Usage: ");
            Console.Out.WriteLine("camcap [-f filename] [-s 640x480] [-d 1000] [-l logfile]");
            Console.Out.WriteLine();
            Console.Out.WriteLine("{0,-10}captures image to filename.jpg, overwriting existing", "   -f");
            Console.Out.WriteLine("{0,-10}if filename is path ending in \\, uses default naming", "", System.DateTime.Now);
            Console.Out.WriteLine("{0,-10}default capture file name is {1:yyyyMMddHmmss}.jpg", "", System.DateTime.Now);
            Console.Out.WriteLine();
            Console.Out.WriteLine("{0,-10}specify capture size. default is 640x480", "   -s");
            Console.Out.WriteLine();
            Console.Out.WriteLine("{0,-10}specify delay before capture, in milliseconds.", "   -d");
            Console.Out.WriteLine("{0,-10}some cameras (laptop bezel cams for example) have long ", "");
            Console.Out.WriteLine("{0,-10}startup times. if you are capturing all-black images, set", "");
            Console.Out.WriteLine("{0,-10}this to a higher value. default is 4000 = 4 seconds", "");
            Console.Out.WriteLine("{0,-10}", "");
            Console.Out.WriteLine();
            Console.Out.WriteLine("{0,-10}log results to logfile.log", "   -l");
        }



        [STAThread]
        static int Main(string[] args) {
            if (args.Length % 2 != 0 || (args.Length == 1 && args[0] == "/?")) {
                PrintUsage();
                return 0;
            }

            string logfilename = null;
            try {
                string fileName = String.Format("{0:yyyyMMddHmmss}", System.DateTime.Now);
                int delay = 4000;
                int width = 640;
                int height = 480;

                for (int i = 0; i < args.Length; i = i + 2) {
                    string sw = args[i].Replace("-", "").ToLower();
                    string op = args[i + 1];

                    switch (sw) {
                        case ("f"):
                            fileName = op;
                            break;
                        case ("s"):
                            string[] ops = op.Split("x".ToCharArray());
                            if (ops == null || ops.Length != 2) {
                                Console.WriteLine("size format WIDTHxHEIGHT.");
                                PrintUsage();
                                return 0;
                            }
                            width = Convert.ToInt32(ops[0]);
                            height = Convert.ToInt32(ops[1]);
                            break;
                        case ("d"):
                            delay = Convert.ToInt32(op);
                            break;
                        case ("l"):
                            logfilename = op;
                            break;
                        default:
                            Console.Out.WriteLine("Unknown option {0}", sw);
                            PrintUsage();
                            return 0;
                    }
                }

                Webcam camera = new Webcam(new Size(width, height), 30);
                Image captured_image = null;
                int counter = 0;

                //remove any extensions from target file name
                int lastdot = -1;
                lastdot = fileName.LastIndexOf(".");
                if (lastdot != -1 && !fileName.Substring(lastdot).Contains("\\"))
                    fileName = fileName.Substring(0, lastdot);

                if (fileName.EndsWith("\\")) {
                    fileName = fileName + String.Format("{0:yyyyMMddHmmss}", System.DateTime.Now);
                }

                //Assign proper extension
                if (!fileName.EndsWith(".jpg", true, null))
                    fileName = fileName + ".jpg";

                if (logfilename != null && !logfilename.EndsWith(".log", true, null))
                    logfilename = logfilename + ".log";

                string curr_dir = Environment.CurrentDirectory;

                //modify filenames to show absolute file paths
                if (!fileName.Contains("\\"))
                    fileName = curr_dir + "\\" + fileName;

                if (logfilename != null && !logfilename.Contains("\\"))
                    logfilename = curr_dir + "\\" + logfilename;

                //start the camera
                camera.Start();
                //start listening for windows messages
                Application.DoEvents();

                /* Try capturing the image from the webcam 100 times
                 * with sleeping 10 milliseconds before each try.
                */
                do {
                    Thread.Sleep(delay);
                    captured_image = camera.currentImage;
                    counter++;
                } while (captured_image == null && counter <= 100);

                camera.Stop();

                if (captured_image == null) {
                    throw new Exception("Device time-out");
                }
                else {
                    using (FileStream fs = new FileStream(fileName, FileMode.Create)) {
                        captured_image.Save(fs, ImageFormat.Jpeg);
                        Console.WriteLine("Image saved to " + fileName);
                        WriteLog("Image stored at " + fileName, logfilename);
                    }
                }
            }
            catch (FileNotFoundException fnfex) {
                WriteLog(fnfex.ToString(), logfilename);
                return 1;
            }
            catch (ArgumentException aex) {
                Console.WriteLine("Argument Exception");
                WriteLog(aex.ToString(), logfilename);
                return 2;
            }
            catch (Exception ex) {
                WriteLog(ex.ToString(), logfilename);
                return 7;
            }
            finally {
                Application.Exit();
            }
            return 0;
        }


        //method to write the log file
        private static void WriteLog(string message, string logfilepath) {
            if (string.IsNullOrEmpty(logfilepath))
                return;

            try {
                using (StreamWriter writer = File.AppendText(logfilepath)) {
                    writer.WriteLine(message);
                }
            }
            catch (FileNotFoundException) {
                Console.WriteLine("Error code 1 : Please specify a valid log file-path");
            }
            catch (ArgumentException) {
                Console.WriteLine("Error code 2 : Invalid log path");
            }
            catch (NotSupportedException) {
                Console.WriteLine("Error code 3 : Log file-path refers to a non-file device");
            }
            catch (SecurityException) {
                Console.WriteLine("Error code 4 : Log file-path permission denied");
            }
            catch (DirectoryNotFoundException) {
                Console.WriteLine("Error code 5 : Log file-path directory not found");
            }
            catch (PathTooLongException) {
                Console.WriteLine("Error code 6 : Log file-path is too long");
            }
            catch (Exception ex) {
                Console.WriteLine("Error code 7 : " + ex.Message);
            }
        }
    }
}
