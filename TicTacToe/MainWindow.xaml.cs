using System;
using System.Collections.Generic;
using System.Linq;
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

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        #region Private Members

        private MarkType[] Results;

        private bool Player1Turn;

        private bool GameEnded;

        Dictionary<string, int> stats = new Dictionary<string, int>();

        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            NewGame();

            stats.Add("X", -10);
            stats.Add("O", 10);
            stats.Add("Tie", 0);


        }




        #endregion

        private void NewGame()
        {
            Results = new MarkType[9];

            for (var i=0; i< Results.Length; i++)
            {
                Results[i] = MarkType.Free;
            }
                


            Player1Turn = true;

            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.Content = string.Empty;
                button.Background = Brushes.White;
                button.Foreground = Brushes.Blue;
            });

            GameEnded = false;
            

        }

       
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (GameEnded)
            {
                NewGame();
                return;
            }



            var button = (Button)sender;
            var column = Grid.GetColumn(button);
            var row = Grid.GetRow(button);

            var index = column + (row * 3);

            if(Results[index] != MarkType.Free )
            {
                return;
            }

            Results[index] = MarkType.Cross; 

            button.Content = "X";





           string check = CheckForWinner();


            if (check=="X" || check=="O" || check == "Tie")
            {
                GameEnded = true;
            }


            if (!GameEnded)
            {
                Player1Turn = false;
                PcTurn();
            }



            check = CheckForWinner();


            if (check == "X" || check == "O" || check == "Tie")
            {
                GameEnded = true;
            }

        }

        private void PcTurn()
        {
            

            var bestscore = -1000;
            int[] bestMove = new int[2] ;
            bool isbest = false;
            for (var i = 0; i < Results.Length; i++)
            {

                if (Results[i]== MarkType.Free)
                {
                    double temp = i / 3;
                    int row =  Convert.ToInt32(Math.Floor(temp));
                    int col;
                    Results[i] = MarkType.Nought;


                    if (new[] { 0, 3, 6 }.Contains(i))
                    {
                        col = 0;
                    } else if (new[] { 1, 4, 7 }.Contains(i))
                    {
                        col = 1;
                     }else
                    {
                        col = 2;
                    }
                    

                    var score = MinMax(0,false);
                    Results[i] = MarkType.Free;
                    Console.WriteLine(score);
                    if (score > bestscore)
                    {
                        bestscore = score;
                        isbest = true;
                        bestMove[0] = col;
                        bestMove[1] = row;
                        bestMove.ToList().ForEach(x => Console.WriteLine(x.ToString()));

                    }


                    

                }
                 
            }


            if (isbest)
            {
                Container.Children.Cast<Button>().ToList().ForEach(button =>
                {
                    if (button.Name == $"Button{bestMove[0]}_{bestMove[1]}")
                    {
                        Console.WriteLine($"is best {bestMove}");
                        var index = bestMove[0] + (bestMove[1] * 3);
                        Results[index] = MarkType.Nought;
                        button.Content = "O";
                        button.Foreground = Brushes.Red;
                        Player1Turn = true;
                    }

                });

            }
            
            


        }

        private int MinMax(int depth, bool ismax)
        {
            string result = CheckForWinner();
            
            if ( stats.ContainsKey(result))
            {
                int score = stats[result];
                
                return score;
            }


            if (ismax)
            {
                Player1Turn = false;
                var bestscore = -1000;
                for (var i = 0; i < Results.Length; i++)
                {

                    if (Results[i] == MarkType.Free)
                    {
                        double temp = i / 3;
                        int row = Convert.ToInt32(Math.Floor(temp));
                        int col;
                        Results[i] = MarkType.Nought;


                        if (new[] { 0, 3, 6 }.Contains(i))
                        {
                            col = 0;
                        }
                        else if (new[] { 1, 4, 7 }.Contains(i))
                        {
                            col = 1;
                        }
                        else
                        {
                            col = 2;
                        }


                        var score = MinMax(depth+1, false);
                        Results[i] = MarkType.Free;

                        if (score > bestscore)
                        {
                            bestscore = score;
                            
                            


                        }




                    }

                }
                return bestscore;
            }
            else
            {
                Player1Turn = true;
                var bestscore = 1000;
                for (var i = 0; i < Results.Length; i++)
                {

                    if (Results[i] == MarkType.Free)
                    {
                        double temp = i / 3;
                        int row = Convert.ToInt32(Math.Floor(temp));
                        int col;
                        Results[i] = MarkType.Cross;


                        if (new[] { 0, 3, 6 }.Contains(i))
                        {
                            col = 0;
                        }
                        else if (new[] { 1, 4, 7 }.Contains(i))
                        {
                            col = 1;
                        }
                        else
                        {
                            col = 2;
                        }


                        var score = MinMax(depth + 1, true);
                        Results[i] = MarkType.Free;

                        if (score < bestscore)
                        {
                            bestscore = score;




                        }




                    }

                }
                return bestscore;
            }
        }

        private string CheckForWinner()
        {
            
            string winer = Player1Turn?"X":"O";



            #region Horizontal Wins

            // Check for horizontal wins
            //
            //  - Row 0
            //
            if (Results[0] != MarkType.Free && (Results[0] & Results[1] & Results[2]) == Results[0])
            {
                // Game ends
                

                // Highlight winning cells in green
                Button0_0.Background = Button1_0.Background = Button2_0.Background = Brushes.Green;
                return winer;
            }
            //
            //  - Row 1
            //
            if (Results[3] != MarkType.Free && (Results[3] & Results[4] & Results[5]) == Results[3])
            {
                // Game ends
                

                // Highlight winning cells in green
                Button0_1.Background = Button1_1.Background = Button2_1.Background = Brushes.Green;
                return winer;
            }
            //
            //  - Row 2
            //
            if (Results[6] != MarkType.Free && (Results[6] & Results[7] & Results[8]) == Results[6])
            {
                // Game ends
                

                // Highlight winning cells in green
                Button0_2.Background = Button1_2.Background = Button2_2.Background = Brushes.Green;
                return winer;
            }

            #endregion

            #region Vertical Wins

            // Check for vertical wins
            //
            //  - Column 0
            //
            if (Results[0] != MarkType.Free && (Results[0] & Results[3] & Results[6]) == Results[0])
            {
                // Game ends
                

                // Highlight winning cells in green
                Button0_0.Background = Button0_1.Background = Button0_2.Background = Brushes.Green;
                return winer;
            }
            //
            //  - Column 1
            //
            if (Results[1] != MarkType.Free && (Results[1] & Results[4] & Results[7]) == Results[1])
            {
                // Game ends
                

                // Highlight winning cells in green
                Button1_0.Background = Button1_1.Background = Button1_2.Background = Brushes.Green;
                return winer;
            }
            //
            //  - Column 2
            //
            if (Results[2] != MarkType.Free && (Results[2] & Results[5] & Results[8]) == Results[2])
            {
                // Game ends
                

                // Highlight winning cells in green
                Button2_0.Background = Button2_1.Background = Button2_2.Background = Brushes.Green;
                return winer;
            }

            #endregion

            #region Diagonal Wins

            // Check for diagonal wins
            //
            //  - Top Left Bottom Right
            //
            if (Results[0] != MarkType.Free && (Results[0] & Results[4] & Results[8]) == Results[0])
            {
                // Game ends
                

                // Highlight winning cells in green
                Button0_0.Background = Button1_1.Background = Button2_2.Background = Brushes.Green;
                return winer;
            }
            //
            //  - Top Right Bottom Left
            //
            if (Results[2] != MarkType.Free && (Results[2] & Results[4] & Results[6]) == Results[2])
            {
                // Game ends
                

                // Highlight winning cells in green
                Button2_0.Background = Button1_1.Background = Button0_2.Background = Brushes.Green;
                return winer;
            }

            #endregion

            #region No Winners

            // Check for no winner and full board
            if (!Results.Any(f => f == MarkType.Free))
            {
                // Game ended
                

                // Turn all cells orange
                Container.Children.Cast<Button>().ToList().ForEach(button =>
                {
                    button.Background = Brushes.Orange;
                });
                return "Tie";
            }

            #endregion


            return "nothing";
        }
    }
}
