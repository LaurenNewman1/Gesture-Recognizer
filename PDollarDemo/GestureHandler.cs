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

        public static Gesture[] ReadEvent(string filename)
        {
            List<Gesture> gestures = new List<Gesture>();
            List<Point> points = new List<Point>();
            string[] lines = File.ReadAllLines(filename);
            int currStrokeIndex = 0;
            foreach (string line in lines)
            {
                if (line.Contains(","))
                {
                    points.Add(new Point(
                        float.Parse(line.Substring(0, line.IndexOf(','))),
                        float.Parse(line.Substring(line.IndexOf(' ') + 1, line.Length - line.IndexOf(','))),
                        currStrokeIndex
                    ));
                }
                else if (line.Equals("MOUSUP"))
                {
                    currStrokeIndex++;
                }
                else if (line.Equals("RECOGNIZE"))
                {
                    gestures.Add(new Gesture(points.ToArray(), "unidentified gesture"));
                    points.Clear();
                }
            }
            return gestures.ToArray();
        }

        public static Gesture ReadGesture(string fileName)
        {
            List<Point> points = new List<Point>();
            string[] alllines = File.ReadAllLines(fileName);
            string name = alllines[0];
            int currStrokeIndex = 0;
            string[] lines = SubArray(alllines, 1, alllines.Length - 1);
            foreach (string line in lines)
            {
                if (line.Contains(","))
                {
                    points.Add(new Point(
                        float.Parse(line.Substring(0, line.IndexOf(','))),
                        float.Parse(line.Substring(line.IndexOf(' ') + 1, line.Length - line.IndexOf(','))),
                        currStrokeIndex
                    ));
                }
                else if (line.Equals("END"))
                {
                    currStrokeIndex++;
                }
            }
            return new Gesture(points.ToArray(), name);
        }

        public static string formatGestureName(string filename)
        {
            if (filename.Contains("."))
            {
                int i = filename.IndexOf('.');
                filename = filename.Substring(0, i);
            }
            while (filename.Contains("/"))
            {
                int length = filename.Length;
                int i = filename.IndexOf('/');
                filename = filename.Substring(i + 1, length - i - 1);
            }
            while (filename.Contains("\\"))
            {
                int length = filename.Length;
                int i = filename.IndexOf('\\');
                filename = filename.Substring(i + 1, length - i - 1);
            }
            return filename;
        }

        public static string[] SubArray(string[] array, int offset, int length)
        {
            string[] result = new string[length];
            Array.Copy(array, offset, result, 0, length);
            return result;
        }
    }
}
