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


        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            NewGame();  




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

            Results[index] = Player1Turn ? MarkType.Cross : MarkType.Nought;

            button.Content = Player1Turn ? "X" : "0";

            if (!Player1Turn)
                button.Foreground = Brushes.Red;

            Player1Turn ^= true;

            CheckForWinner();

        }

        private void CheckForWinner()
        {
            
        }
    }
}
