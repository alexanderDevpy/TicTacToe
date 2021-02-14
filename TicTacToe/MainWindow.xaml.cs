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

            stats.Add("X", -1);
            stats.Add("O", 1);
            stats.Add("Tie", 0);


        }




        #endregion

        private void NewGame()
        {
            Results = new MarkType[9];

            for (var i = 0; i < Results.Length; i++)
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

            if (Results[index] != MarkType.Free)
            {
                return;
            }

            Results[index] = MarkType.Cross;

            button.Content = "X";





            string check = CheckForWinner(Results, "X");


            if (check == "X" || check == "O" || check == "Tie")
            {
                GameEnded = true;
            }


            if (!GameEnded)
            {
                
                PcTurn(Results);
            }



            check = CheckForWinner(Results, "O");


            if (check == "X" || check == "O" || check == "Tie")
            {
                GameEnded = true;
            }

        }

        private void PcTurn(MarkType[] board)
        {


            var bestscore = -1000;
            int[] bestMove = new int[2];
            bool isbest = false;
            for (var i = 0; i < board.Length; i++)
            {

                if (board[i] == MarkType.Free)
                {
                    double temp = i / 3;
                    int row = Convert.ToInt32(Math.Floor(temp));
                    int col = 3;

                    board[i] = MarkType.Nought;


                    if (new[] { 0, 3, 6 }.Contains(i))
                    {
                        col = 0;
                    }
                    else if (new[] { 1, 4, 7 }.Contains(i))
                    {
                        col = 1;
                    }
                    else if (new[] { 2, 5, 8 }.Contains(i))
                    {
                        col = 2;
                    }


                    
                    var score = MinMax(board, 0, false,"O");
                    board[i] = MarkType.Free;
                    Console.WriteLine(score);
                    if (score > bestscore)
                    {

                        bestscore = score;
                        isbest = true;

                        bestMove[0] = col;
                        bestMove[1] = row;


                    }




                }

            }


            if (isbest)
            {
                Container.Children.Cast<Button>().ToList().ForEach(button =>
                {
                    if (button.Name == $"Button{bestMove[0]}_{bestMove[1]}")
                    {

                        var index = bestMove[0] + (bestMove[1] * 3);
                        Results[index] = MarkType.Nought;
                        button.Content = "O";
                        button.Foreground = Brushes.Red;
                        Player1Turn = true;
                    }

                });

            }




        }

        private int MinMax(MarkType[] board, int depth, bool ismax,string player)
        {
            string result = CheckForWinner(board, player);




            if (stats.ContainsKey(result))
            {
                int score = stats[result];

                return score;
            }


            if (ismax)
            {
                Player1Turn = false;
                var bestscore = -1000;
                for (var i = 0; i < board.Length; i++)
                {

                    if (board[i] == MarkType.Free)
                    {

                        board[i] = MarkType.Nought;





                        var score = MinMax(board, depth + 1, false,"O");
                        board[i] = MarkType.Free;

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
                for (var i = 0; i < board.Length; i++)
                {

                    if (board[i] == MarkType.Free)
                    {


                        board[i] = MarkType.Cross;
                        var score = MinMax(board, depth + 1, true,"X");
                        board[i] = MarkType.Free;

                        if (score < bestscore)
                        {
                            bestscore = score;




                        }




                    }

                }
                return bestscore;
            }
        }

        private string CheckForWinner(MarkType[] board, string player)
        {

            string winer = player;



            #region Horizontal Wins

            // Check for horizontal wins
            //
            //  - Row 0
            //
            if (board[0] != MarkType.Free && (board[0] & board[1] & board[2]) == board[0])
            {
                // Game ends


                // Highlight winning cells in green
                Button0_0.Background = Button1_0.Background = Button2_0.Background = Brushes.Green;
                return winer;
            }
            //
            //  - Row 1
            //
            if (board[3] != MarkType.Free && (board[3] & board[4] & board[5]) == board[3])
            {
                // Game ends


                // Highlight winning cells in green
                Button0_1.Background = Button1_1.Background = Button2_1.Background = Brushes.Green;
                return winer;
            }
            //
            //  - Row 2
            //
            if (board[6] != MarkType.Free && (board[6] & board[7] & board[8]) == board[6])
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
            if (board[0] != MarkType.Free && (board[0] & board[3] & board[6]) == board[0])
            {
                // Game ends


                // Highlight winning cells in green
                Button0_0.Background = Button0_1.Background = Button0_2.Background = Brushes.Green;
                return winer;
            }
            //
            //  - Column 1
            //
            if (board[1] != MarkType.Free && (board[1] & board[4] & board[7]) == board[1])
            {
                // Game ends


                // Highlight winning cells in green
                Button1_0.Background = Button1_1.Background = Button1_2.Background = Brushes.Green;
                return winer;
            }
            //
            //  - Column 2
            //
            if (board[2] != MarkType.Free && (board[2] & board[5] & board[8]) == board[2])
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
            if (board[0] != MarkType.Free && (board[0] & board[4] & board[8]) == board[0])
            {
                // Game ends


                // Highlight winning cells in green
                Button0_0.Background = Button1_1.Background = Button2_2.Background = Brushes.Green;
                return winer;
            }
            //
            //  - Top Right Bottom Left
            //
            if (board[2] != MarkType.Free && (board[2] & board[4] & board[6]) == board[2])
            {
                // Game ends


                // Highlight winning cells in green
                Button2_0.Background = Button1_1.Background = Button0_2.Background = Brushes.Green;
                return winer;
            }

            #endregion

            #region No Winners

            // Check for no winner and full board
            if (!board.Any(f => f == MarkType.Free))
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
