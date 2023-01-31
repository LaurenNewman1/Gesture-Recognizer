using PDollarGestureRecognizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace PDollarDemo
{
    public class GestureHandler
    {
        public static Gesture[] LoadTrainingSet()
        {
            List<Gesture> gestures = new List<Gesture>();
            string[] gestureFiles = Directory.GetFiles("gesturefiles", "*.txt");
            foreach (string file in gestureFiles)
                gestures.Add(ReadGesture(file));
            return gestures.ToArray();
        }

        public static Gesture ReadEvent(string filename)
        {
            List<Point> points = new List<Point>();
            string[] lines = File.ReadAllLines(filename);
            int currStrokeIndex = 0;
            foreach (string line in lines)
            {
                if (line.Contains(","))
                {
                    points.Add(new Point(
                        float.Parse(line.Substring(0, line.IndexOf(','))),
                        float.Parse(line.Substring(line.IndexOf(' ') + 1, line.Length)),
                        currStrokeIndex
                    ));
                }
                else if (line.Equals("MOUSUP"))
                {
                    currStrokeIndex++;
                }
            }
            return new Gesture(points.ToArray(), "unidentified gesture");
        }

        public static Gesture ReadGesture(string fileName)
        {
            List<Point> points = new List<Point>();
            string[] lines = File.ReadAllLines(fileName);
            int currStrokeIndex = 0;
            foreach (string line in lines)
            {
                if (line.Contains(","))
                {
                    points.Add(new Point(
                        float.Parse(line.Substring(0, line.IndexOf(','))),
                        float.Parse(line.Substring(line.IndexOf(' ') + 1, line.Length)),
                        currStrokeIndex
                    ));
                }
                else if (line.Equals("END"))
                {
                    currStrokeIndex++;
                }
            }
            return new Gesture(points.ToArray(), fileName.Substring(0, fileName.Length - 4));
        }
    }
}
