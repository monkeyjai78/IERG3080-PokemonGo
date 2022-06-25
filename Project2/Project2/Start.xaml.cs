using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Project2
{
    /// <summary>
    /// Window1.xaml 的互動邏輯
    /// </summary>
    public partial class Start : Window
    {
        Bag bag;
        List<SeriesDictionary> allPokemon;
        MainWindow map;

        public Start(Bag bag, List<SeriesDictionary> allPokemon, MainWindow map)
        {
            InitializeComponent();
            this.bag = bag;
            this.allPokemon = allPokemon;
            this.map = map;
            MessageBox.Show("Welcome to the pokemon ver.3080!\nUse WASD to move!\nBuilding on the right hand side is the GYM, go there to fight some pokemon to gain exp!\nBuilding on the left hand side is your home, you can manage pokemon there.\nIn the grass area will appear pokemon randomly, get ready to catch them!\nAnyways, pick your first pokemon and get ready for your adventure!");
        }
        //To choose which pokemon to start
        private void Fire_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You have choosen Charmander!");
            Random rnd = new Random();
            Pokemon pokemon = new Pokemon(rnd.Next(10, 20), rnd.Next(10, 20), rnd.Next(5, 10), allPokemon[0]);
            bag.Add(pokemon);
            map.Show();
            this.Close();
        }

        private void Water_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You have choosen Squirtle!");
            Random rnd = new Random();
            Pokemon pokemon = new Pokemon(rnd.Next(10, 20), rnd.Next(10, 20), rnd.Next(5, 10), allPokemon[1]);
            bag.Add(pokemon);
            map.Show();
            this.Close();
        }

        private void Grass_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You have choosen Bulbasaur!");
            Random rnd = new Random();
            Pokemon pokemon = new Pokemon(rnd.Next(10, 20), rnd.Next(10, 20), rnd.Next(5, 10), allPokemon[2]);
            bag.Add(pokemon);
            map.Show();
            this.Close();
        }
    }
}
