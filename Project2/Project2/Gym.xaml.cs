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
    /// <summary>
    /// Gym.xaml 的互動邏輯
    /// </summary>
    public partial class Gym : Window
    {

        private Pokemon player;
        private Pokemon enemy;
        private Bag bag;
        public string enemyHealthStatus="Health: ";
        public string playerHealthStatus = "Health: ";
        public MainWindow map;
        private int exp;

        public Gym(Pokemon player, Pokemon enemy, Bag bag, MainWindow map)
        {
            InitializeComponent();
            this.player = player;
            this.enemy = enemy;  
            this.bag = bag;
            this.map = map;
            setUp();
            
            
        }
        private void setUp()
        {
            Random rnd = new Random();
            //To set enemy's strength according to player's pokemons strength
            enemy.MaxHealth = rnd.Next(Convert.ToInt32(player.health *0.7) , Convert.ToInt32(player.health*1.5));
            enemy.health = enemy.MaxHealth;
            enemy.Attack = rnd.Next(Convert.ToInt32(player.Attack *0.7), Convert.ToInt32(player.Attack*1.5));
            enemy.current = rnd.Next(0, player.current);
            //Add pokemon image
            ImageBrush EnemyImage = new ImageBrush();
            EnemyImage.ImageSource = new BitmapImage(new Uri(enemy.pokemons[enemy.current].path));
            Enemy_Rec.Fill = EnemyImage;
            ImageBrush PlayerImage = new ImageBrush();
            PlayerImage.ImageSource = new BitmapImage(new Uri(player.pokemons[player.current].path));
            Player_Rec.Fill = PlayerImage;


            //Player and enemy health initaillize
            playerHealthStatus = player.nickname+"\nHealth: " + player.health + "/" + player.MaxHealth;
            PlayerHP.Text = playerHealthStatus;
            enemyHealthStatus = enemy.nickname+"\nHealth: " + enemy.health + "/" + enemy.MaxHealth;
            EnemyHP.Text = enemyHealthStatus;

            exp = enemy.health;
        }
       
        private void End_Click(object sender, RoutedEventArgs e) //When end button is clicked, end the gym battle. Trigger by clicking the button
        {
            Gym_End();
        }
        private void Attack_Click(object sender, RoutedEventArgs e) //When attack button clicked, player attack first and enemy fight back. Trigger by clicking the button
        {
            MessageBox.Show(player.normalAttack(enemy));
            enemyHealthStatus = enemy.nickname+"\nHealth: " + enemy.health + "/" + enemy.MaxHealth;
            EnemyHP.Text = enemyHealthStatus;

            if (!Check())
            {
                MessageBox.Show(enemy.normalAttack(player));
                playerHealthStatus = player.nickname + "\nHealth: " + player.health + "/" + player.MaxHealth;
                PlayerHP.Text = playerHealthStatus;
                Check();
            }
            
        }
        private bool Check() //A method to check any side has lower or equal to zero health
        {
            if (enemy.health <= 0) //if enemy health <=0 then player win
            {
                MessageBox.Show("You have won! Your pokemon have gain " + exp + " exp!");
                player.exp += exp;
                Gym_End();
                return true;
            }
            if (player.health <= 0) //if player health <=0 then player lose
            {
                MessageBox.Show("You have lost! Try better next time!");
                Gym_End();
                return true;
            }
            return false;
        }
        private void Gym_End() //method to end the gym page
        {
            map.Show();
            this.Close();
        }

    }
}
