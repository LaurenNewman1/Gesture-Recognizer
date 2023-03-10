using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PDollarGestureRecognizer;

namespace PDollarDemo
{
    static class Program
    {
        static Gesture[] trainingSet;

        static void Main(string[] args)
        {
            trainingSet = GestureHandler.LoadTrainingSet();
            if (args.Length == 2 && args[0] == "-t")
            {
                try
                {
                    System.IO.File.Copy(args[1], "gesturefiles/" + GestureHandler.formatGestureName(args[1]) + ".txt", true);
                    trainingSet = GestureHandler.LoadTrainingSet();
                    Console.WriteLine(args[1] + " has been added to the gesture files.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed. Make sure the gesture file you want to add is in the main folder or that you include the relative path.");
                }
            }
            else if (args.Length == 1 && args[0] == "-r")
            {
                string[] files = System.IO.Directory.GetFiles("gesturefiles");
                foreach (string s in files)
                {
                    try
                    {
                        System.IO.File.Delete(s);
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }
                }
                trainingSet = GestureHandler.LoadTrainingSet();
                Console.WriteLine("Gesture files have been successfully cleared.");
            }
            else if (args.Length == 1)
            {
                try
                {
                    Gesture[] candidates = GestureHandler.ReadEvent(args[0]);
                    foreach (Gesture candidate in candidates)
                    {
                        string classification = PointCloudRecognizer.Classify(candidate, trainingSet);
                        Console.WriteLine(classification);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Failed. Make sure your event file is in the main folder or that you include the relative path.");
                }
            }
            else
            {
                Console.WriteLine("Help: Use one of the following formats");
                Console.WriteLine("pdollar –t <gesturefile>");
                Console.WriteLine("pdollar -r");
                Console.WriteLine("pdollar <eventstream>");
            }
        }
    }
}

