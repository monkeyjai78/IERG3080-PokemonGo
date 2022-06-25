using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
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

namespace Project2
{

    public partial class Manage : Window
    {
        Bag bag;
        MainWindow map;
        List<Grid> pokemonGrids = new List<Grid>();
        List<Rectangle> imageBoxs = new List<Rectangle>();
        List<TextBlock> textBlocks = new List<TextBlock>();
        List<RadioButton> selectButtons = new List<RadioButton>();
        List<String> status = new List<String>();

        public Manage(Bag bag, MainWindow map)
        {
            InitializeComponent();
            this.bag = bag;
            this.map = map;
            ManageSetUp();
            MessageBox.Show("Home Sweet Home!\nHere you can manage your pokemons!\nClick the buttons to do actions!\nType in the text box to change pokemon's name! ");
        }
        private void ManageSetUp() {
            //add UI component into lists for processing 
            imageBoxs.Add(Pokemon_Rec0);
            imageBoxs.Add(Pokemon_Rec1);
            imageBoxs.Add(Pokemon_Rec2);
            imageBoxs.Add(Pokemon_Rec3);
            imageBoxs.Add(Pokemon_Rec4);
            imageBoxs.Add(Pokemon_Rec5);
            textBlocks.Add(Pokemon_Status0);
            textBlocks.Add(Pokemon_Status1);
            textBlocks.Add(Pokemon_Status2);
            textBlocks.Add(Pokemon_Status3);
            textBlocks.Add(Pokemon_Status4);
            textBlocks.Add(Pokemon_Status5);
            selectButtons.Add(Select0);
            selectButtons.Add(Select1);
            selectButtons.Add(Select2);
            selectButtons.Add(Select3);
            selectButtons.Add(Select4);
            selectButtons.Add(Select5);
            pokemonGrids.Add(Grid0);
            pokemonGrids.Add(Grid1);
            pokemonGrids.Add(Grid2);
            pokemonGrids.Add(Grid3);
            pokemonGrids.Add(Grid4);
            pokemonGrids.Add(Grid5);

            Money.Text = "Money: " + bag.money;
            selectButtons[bag.selected].IsChecked=true; //Check the button of the selected pokemon

            int i = 0;
            foreach (Pokemon x in bag.item)
            {
                updateStatus(i); 
                updateImage(i);
                showUI(i);
                i++;
            } 
        }
        private void updateStatus(int i) //To update status when action is committed, index of that grid (pokemon) as input
        {
            textBlocks[i].Text = "Name:\n" + bag.item[i].nickname
                                     + "\nHP: " + bag.item[i].health + "/" + bag.item[i].MaxHealth
                                     + "\nAttack: " + bag.item[i].Attack
                                     + "\nLevel: " + bag.item[i].Level
                                     + "\nExp: " + bag.item[i].Exp
                                     + "\nType: " + bag.item[i].pokemons[bag.item[i].current].type;
            Money.Text = "Money: " + bag.money;
        }
        private void updateImage(int i) //To update image after evolving
        {
            ImageBrush PokemonImage = new ImageBrush();
            PokemonImage.ImageSource = new BitmapImage(new Uri(bag.item[i].pokemons[bag.item[i].current].path));
            imageBoxs[i].Fill = PokemonImage;
        }
        private void showUI(int i) //To make the component of one pokemon grid visible, index of that grid (pokemon) as input
        {
            pokemonGrids[i].Visibility = Visibility.Visible;
        }

        private void End_Click(object sender, RoutedEventArgs e) //When button back to map clicked, end this window
        {
            Manage_End();
        }
        private void Manage_End() //method to end this window and show the map back, trigger by clicking button
        {
            map.Show();
            this.Close();
        }


        /// 
        /// In below are all buttons for actions, only first button will be commented since the others are the same
        /// 
        //////////////////////////Pokemon no.1 buttons//////////////////////////
        private void Nickname0_TextChanged(object sender, TextChangedEventArgs e) //When text changed change the pokemon's nickname
        {
            bag.item[0].nickname = Nickname0.Text;
            updateStatus(0);
        }
        private void Evolve0_Click(object sender, RoutedEventArgs e) //When evolve button clicked, run evolve
        {
            bag.item[0].evolve();
            updateStatus(0);
            updateImage(0);
        }

        private void Powerup0_Click(object sender, RoutedEventArgs e) //When powerup button clicked, run powerup
        {
            bag.item[0].powerup();
            updateStatus(0);
        }

        private void Sell0_Click(object sender, RoutedEventArgs e) //When sell button clicked, run sell
        {
            if (bag.Sell(0))
                pokemonGrids[0].Visibility = Visibility.Hidden;
            Money.Text = "Money: " + bag.money;
        }
        private void Heal0_Click(object sender, RoutedEventArgs e) //When sell button clicked, check heal
        {
            int dif = bag.item[0].MaxHealth - bag.item[0].health; //calculate the difference of MaxHP and current HP
            if (dif != 0)
            {
                // One dollar one HP
                //Double confirm player
                MessageBoxResult Result = MessageBox.Show("You have $" + bag.money + "\nYou need $" + dif + " to heal", "Are you sure you want to heal?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes && bag.money>=dif) //if confirm then heal and check enough money or not and deduct money
                {
                    MessageBox.Show("Your pokemon has healed");
                    bag.item[0].health = bag.item[0].MaxHealth;
                    bag.money -= dif;
                }
                else if(Result == MessageBoxResult.Yes && bag.money <= dif)
                {
                    MessageBox.Show("You don't have enough money!");
                }
                updateStatus(0);
            }
        }
        private void Select0_Checked(object sender, EventArgs e) //When radio button of select checked ,change the pokemon selected to this pokemon
        {
            if (bag.selected != 0 )
            {
                selectButtons[bag.selected].IsChecked = false;
                selectButtons[0].IsChecked = true;
                bag.selected = 0;
            }
            
        }
        //////////////////////////Pokemon no.2 buttons//////////////////////////
        private void Nickname1_TextChanged(object sender, TextChangedEventArgs e)
        {
           bag.item[1].nickname = Nickname1.Text;
            updateStatus(1);
        }

        private void Evolve1_Click(object sender, RoutedEventArgs e)
        {
            bag.item[1].evolve();
            updateStatus(1);
            updateImage(1);
        }

        private void Powerup1_Click(object sender, RoutedEventArgs e)
        {
            bag.item[1].powerup();
            updateStatus(1);
        }

        private void Sell1_Click(object sender, RoutedEventArgs e)
        {
            if (bag.Sell(1))
                pokemonGrids[1].Visibility = Visibility.Hidden;
            Money.Text = "Money: " + bag.money;
        }
        private void Heal1_Click(object sender, RoutedEventArgs e)
        {
            int dif = bag.item[1].MaxHealth - bag.item[1].health;
            if (dif != 0)
            {
                MessageBoxResult Result = MessageBox.Show("You have $" + bag.money + "\nYou need $" + dif + " to heal", "Are you sure you want to heal?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Your pokemon has healed");
                    bag.item[1].health = bag.item[1].MaxHealth;
                    bag.money -= dif;
                }
                updateStatus(1);
            }
        }
        private void Select1_Checked(object sender, EventArgs e) { 
            if (bag.selected != 1)
            {
                selectButtons[bag.selected].IsChecked = false;
                selectButtons[1].IsChecked = true;
                bag.selected = 1;
            }            
        }
        //////////////////////////Pokemon no.3 buttons//////////////////////////
        private void Nickname2_TextChanged(object sender, TextChangedEventArgs e)
        {
           bag.item[2].nickname = Nickname2.Text;
           updateStatus(2);
        }

        private void Evolve2_Click(object sender, RoutedEventArgs e)
        {
            bag.item[2].evolve();
            updateStatus(2);
            updateImage(2);
        }

        private void Powerup2_Click(object sender, RoutedEventArgs e)
        {
           bag.item[2].powerup();
           updateStatus(2);
        }

        private void Sell2_Click(object sender, RoutedEventArgs e)
        {
            if (bag.Sell(2))
                pokemonGrids[2].Visibility = Visibility.Hidden;
            Money.Text = "Money: " + bag.money;
        }
        private void Heal2_Click(object sender, RoutedEventArgs e)
        {
            int dif = bag.item[2].MaxHealth - bag.item[2].health;
            if (dif != 0)
            {
                MessageBoxResult Result = MessageBox.Show("You have $" + bag.money + "\nYou need $" + dif + " to heal", "Are you sure you want to heal?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Your pokemon has healed");
                    bag.item[2].health = bag.item[2].MaxHealth;
                    bag.money -= dif;
                }
                updateStatus(2);
            }
        }
        private void Select2_Checked(object sender, EventArgs e)
        {
            if (bag.selected != 2)
            {
                selectButtons[bag.selected].IsChecked = false;
                selectButtons[2].IsChecked = true;
                bag.selected = 2;
            }
        }
        //////////////////////////Pokemon no.4 buttons//////////////////////////
        private void Nickname3_TextChanged(object sender, TextChangedEventArgs e)
        {
            bag.item[3].nickname = Nickname3.Text;
            updateStatus(3);
        }

        private void Evolve3_Click(object sender, RoutedEventArgs e)
        {
            bag.item[3].evolve();
            updateStatus(3);
            updateImage(3);
        }

        private void Powerup3_Click(object sender, RoutedEventArgs e)
        {
            bag.item[3].powerup();
            updateStatus(3);
        }

        private void Sell3_Click(object sender, RoutedEventArgs e)
        {
            if (bag.Sell(3))
                pokemonGrids[3].Visibility = Visibility.Hidden;
            Money.Text = "Money: " + bag.money;
        }
        private void Heal3_Click(object sender, RoutedEventArgs e)
        {
            int dif = bag.item[3].MaxHealth - bag.item[3].health;
            if (dif != 0)
            {
                MessageBoxResult Result = MessageBox.Show("You have $" + bag.money + "\nYou need $" + dif + " to heal", "Are you sure you want to heal?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Your pokemon has healed");
                    bag.item[3].health = bag.item[3].MaxHealth;
                    bag.money -= dif;
                }
                updateStatus(3);
            }
        }
        private void Select3_Checked(object sender, EventArgs e)
        {
            if (bag.selected != 3)
            {
                selectButtons[bag.selected].IsChecked = false;
                selectButtons[3].IsChecked = true;
                bag.selected = 3;
            }
                
        }
        //////////////////////////Pokemon no.5 buttons//////////////////////////
        private void Nickname4_TextChanged(object sender, TextChangedEventArgs e)
        {
            bag.item[4].nickname = Nickname4.Text;
        }

        private void Evolve4_Click(object sender, RoutedEventArgs e)
        {
            bag.item[4].evolve();
            updateStatus(4);
            updateImage(4);
        }

        private void Powerup4_Click(object sender, RoutedEventArgs e)
        {
            bag.item[4].powerup();
            updateStatus(4);
        }

        private void Sell4_Click(object sender, RoutedEventArgs e)
        {
            if (bag.Sell(4))
                pokemonGrids[4].Visibility = Visibility.Hidden;
            Money.Text = "Money: " + bag.money;
        }
        private void Heal4_Click(object sender, RoutedEventArgs e)
        {
            int dif = bag.item[4].MaxHealth - bag.item[4].health;
            if (dif != 0)
            {
                MessageBoxResult Result = MessageBox.Show("You have $" + bag.money + "\nYou need $" + dif + " to heal", "Are you sure you want to heal?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Your pokemon has healed");
                    bag.item[4].health = bag.item[4].MaxHealth;
                    bag.money -= dif;
                }
            }
        }
        private void Select4_Checked(object sender, EventArgs e)
        {
            if (bag.selected != 4)
            {
                selectButtons[bag.selected].IsChecked = false;
                selectButtons[4].IsChecked = true;
                bag.selected = 4;
            }
                
        }
        //////////////////////////Pokemon no.6 buttons//////////////////////////
        private void Nickname5_TextChanged(object sender, TextChangedEventArgs e)
        {
            bag.item[5].nickname = Nickname5.Text;
            updateStatus(5);
        }

        private void Evolve5_Click(object sender, RoutedEventArgs e)
        {
            bag.item[5].evolve();
            updateStatus(5);
            updateImage(5);
        }

        private void Powerup5_Click(object sender, RoutedEventArgs e)
        {
            bag.item[5].powerup();
            updateStatus(5);
        }

        private void Sell5_Click(object sender, RoutedEventArgs e)
        {
            if(bag.Sell(5))
                pokemonGrids[5].Visibility = Visibility.Hidden;
            Money.Text = "Money: " + bag.money;
        }
        private void Heal5_Click(object sender, RoutedEventArgs e)
        {
            int dif = bag.item[5].MaxHealth - bag.item[5].health;
            if (dif != 0)
            {
                MessageBoxResult Result = MessageBox.Show("You have $" + bag.money + "\nYou need $" + dif + " to heal", "Are you sure you want to heal?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Your pokemon has healed");
                    bag.item[5].health = bag.item[5].MaxHealth;
                    bag.money -= dif;
                }
                updateStatus(5);
            }
        }
        private void Select5_Checked(object sender, EventArgs e)
        {
            if (bag.selected != 5)
            {
                selectButtons[bag.selected].IsChecked = false;
                selectButtons[5].IsChecked = true;
                bag.selected = 5;
            }
                
        }


    }
}
