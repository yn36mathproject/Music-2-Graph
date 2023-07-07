using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr AllocConsole();

        private List<char> ScalarAlpha = new List<char>{'D', 'C', 'B', 'A', 'G', 'F', 'E' };
        private List<char> Melody = new List<char> { 'o', 'o', 'o', 'o', 'o' };
        // Defining the Dictionary for notes
        Dictionary<char, double> noteValues = new Dictionary<char, double>
        {
            //{'E', 49.9},
            {'D', 41.6},
            {'C', 33.2},
            {'B', 29.1},
            {'A', 20.8},
            {'G', 12.4},
            {'F', 4.1},
            {'E', 0},
            {'o', 0}
        };
        private int CurCol = 0;

        public MainWindow()
        {
            AllocConsole();

            InitializeComponent();

            for (uint i = 0; i < 5 ; i++)
            {
                CreateNotesSelector();
            }
        }

        string CalculateFunction()
        {
            string GeneratedEquation = string.Empty;
            double PrevNoteVal = 0;

            for (int i = 0; i < Melody.Count; i++)
            {
                if (i == 0)
                {
                    GeneratedEquation += $"y = g(x, {noteValues[Melody[i]]}, 0)";
                    PrevNoteVal = noteValues[Melody[i]];
                }
                else
                {
                    if (Melody[i] == 'o')
                    {
                        continue;
                    }
                    
                    double CurNoteVal = noteValues[Melody[i]];
                    if (CurNoteVal < PrevNoteVal)
                    {
                        GeneratedEquation += $" - g(x, {Math.Round(PrevNoteVal - CurNoteVal, 2)}, {i})";
                    }
                    else if (CurNoteVal > PrevNoteVal)
                    {
                        GeneratedEquation += $" + g(x, {Math.Round(CurNoteVal - PrevNoteVal, 2)}, {i})";
                    }
                    PrevNoteVal = noteValues[Melody[i]];
                }
            }

            return GeneratedEquation;
        }

        void CreateNotesSelector()
        {
            var stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;
            stackPanel.MaxWidth = 50;
            int CurColIns = CurCol;

            for (int i = 0; i < 7; ++i)
            {
                var ScaledNote = new Button();
                ScaledNote.Background = new SolidColorBrush(Color.FromRgb(36, 36, 36));
                ScaledNote.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                ScaledNote.Opacity = 0.5;
                ScaledNote.BorderThickness = new System.Windows.Thickness(0);

                char NoteName = ScalarAlpha[i];

                ScaledNote.Click += (s, e) =>
                {
                    Console.WriteLine($"I: {i} CurColIns: {CurColIns} Note: {NoteName}");
                    
                    if (NoteName == Melody[CurColIns])
                    {
                        ScaledNote.Opacity = 0.5;
                        Melody[CurColIns] = 'o';

                        return;
                    }

                    Melody[CurColIns] = NoteName;
                    for (int n = 0; n < stackPanel.Children.Count; ++n)
                    {
                        if (stackPanel.Children[n].Opacity == 1)
                        {
                            stackPanel.Children[n].Opacity = 0.5;
                            ScaledNote.Opacity = 1;
                        }
                        else
                        {
                            ScaledNote.Opacity = 1;
                            Console.WriteLine("The button has been disabled.");
                        }
                    }
                    UpdateEquation();
                };

                ScaledNote.Width = 50;
                ScaledNote.Content = ScalarAlpha[i];
                stackPanel.Children.Add(ScaledNote);
            }
            NoteSelect.Children.Add(stackPanel);
            CurCol++;
        }

        void UpdateEquation()
        {
            for (int n = 0; n < Melody.Count; ++n)
            {
                Console.Write(n + ": " + Melody[n] + ' ');
            }
            Console.WriteLine();
            string GeneratedEquation = CalculateFunction();

            EquationsBox.Text = GeneratedEquation;
            Console.WriteLine(GeneratedEquation);
        }

        private void AddMoreNotes_Click(object sender, RoutedEventArgs e)
        {
            CreateNotesSelector();
            Melody.Add('o');
        }
    }
}
