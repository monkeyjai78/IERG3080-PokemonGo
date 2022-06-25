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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window //The map
    {   
        int speed = 10; //Player movement speed
        //bool for direction
        bool moveUp; 
        bool moveDown;
        bool moveLeft;
        bool moveRight;
        public bool NotInGame=true; //Check in minigame or not to prevent re trigger the minigame
        public Bag bag; 
        //Series of pokemon
        public SeriesDictionary water;
        public SeriesDictionary fire;
        public SeriesDictionary grass;
        //All kind of pokemons
        List<SeriesDictionary> allPokemon ;
        

        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer dispatcherTimer = new DispatcherTimer(); //Timer for check event and movement
            dispatcherTimer.Tick += Time_tick; 
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(20); 
            dispatcherTimer.Start();

            DispatcherTimer miniTimer = new DispatcherTimer(); //Timer for random minigame trigger area
            miniTimer.Tick += miniGame_Area_Time_Tick;
            miniTimer.Interval = TimeSpan.FromMilliseconds(1000);
            miniTimer.Start();
            Pokemons_Initialize(); //Initailize all exisiting pokemon
            gameStart();
        }

        private void gameStart()
        {
            Start startPage = new Start(bag, allPokemon, this);
            
            this.Hide();
            startPage.Show();

        }

        private void Pokemons_Initialize()
        {
            //Pokemons initialization
            allPokemon = new List<SeriesDictionary>();
            Random rnd = new Random();
            Dictionary squirtle = new Dictionary("Squirtle", "water", "pack://application:,,,/images/squirtle.png");
            Dictionary blastoise = new Dictionary("Blastoise", "water", "pack://application:,,,/images/blastoise.png");
            Dictionary charmander = new Dictionary("Charmander", "fire", "pack://application:,,,/images/charmander.png");
            Dictionary charizard = new Dictionary("Charizard", "fire", "pack://application:,,,/images/charizard.png");
            Dictionary bulbasaur = new Dictionary("Bulbasaur", "grass", "pack://application:,,,/images/bulbasaur.png");
            Dictionary venusaur = new Dictionary("Venusaur", "grass", "pack://application:,,,/images/venusaur.png");
            Dictionary[] waters = new Dictionary[2] { squirtle, blastoise };
            Dictionary[] fires = new Dictionary[2] { charmander, charizard };
            Dictionary[] grasses = new Dictionary[2] { bulbasaur, venusaur };
            SeriesDictionary water = new SeriesDictionary(waters, 0);
            SeriesDictionary fire = new SeriesDictionary(fires, 0);
            SeriesDictionary grass = new SeriesDictionary(grasses, 0);

            allPokemon.Add(fire);
            allPokemon.Add(water);
            allPokemon.Add(grass);
            
            //Initialize bag
            bag = new Bag();
        }


        private void Canvas_KeyDown(object sender, KeyEventArgs e) //Trigger when keyboard key is pressed
        {
            if (e.Key == Key.S)
            {
                moveDown = true; 
            }
            else if (e.Key == Key.W)
            {
                moveUp = true;
            }
            else if (e.Key == Key.A)
            {
                moveLeft = true;
            }
            else if (e.Key == Key.D)
            {
                moveRight = true;
            }
        }

        private void Canvas_KeyUp(object sender, KeyEventArgs e) //Trigger when keyboard key is released
        {
            if (e.Key == Key.S)
            {
                moveDown = false;
            }
            else if (e.Key == Key.W)
            {
                moveUp = false;
            }
            else if (e.Key == Key.A)
            {
                moveLeft = false;
            }
            else if (e.Key == Key.D)
            {
                moveRight = false;
            }
        }
        private void miniGame_Area_Time_Tick(object sender, EventArgs e) //Randomly set miniGame trigger area for every second
        {
            Random rnd = new Random();
            Canvas.SetLeft(MiniGame, rnd.Next(32, 650));
            Canvas.SetTop(MiniGame, rnd.Next(290, 450));
        }
        private void Time_tick(object sender, EventArgs e) //Check player movement and check any event triggered 
        {
            //To set the player position
            if (moveUp && Canvas.GetTop(player) > 0)
            {
                Canvas.SetTop(player, Canvas.GetTop(player) - speed);
                
            }
            if (moveDown && Canvas.GetTop(player) + (player.Height * 2) < Application.Current.MainWindow.Height)
            {
                Canvas.SetTop(player, Canvas.GetTop(player) + speed);
            }
            if (moveLeft && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - speed);
 
            }
            if (moveRight && Canvas.GetLeft(player) + (player.Width * 2) < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + speed);
            }
            Check_Event();
            
        }

        private void Check_Event()
        {
            //If player go into the gym area then check the condition open gym window
            if ( Canvas.GetTop(player) >= (Canvas.GetTop(gym)) &&
                (Canvas.GetLeft(player) + player.Width) >= (Canvas.GetLeft(gym)) &&
                 (Canvas.GetTop(player) - player.Height)<= (Canvas.GetTop(gym)+gym.Height)
                 )
            {
                //Move player out of the area of the gym
                Canvas.SetLeft(player, 517);
                Canvas.SetTop(player, 88);
                Movement_reset();
                if (bag.selected == -1) //select=-1 means there is no pokemon
                {
                    MessageBox.Show("Your don't have any pokemon, go catch one!");
                    
                }
                else if (bag.item[bag.selected].health <= 0) //If the selected pokemon is <= 0 then cannot battle
                {
                    MessageBox.Show("Your pokemon has no HP, please dont abuse pokemon...");
                }
                else //Open gym window
                {
                    MessageBox.Show("Welcome to the gym! ^ . ^");
                    this.Hide();
                    Gym gymPage = new Gym(bag.item[bag.selected], randomPokemon(), bag, this);
                    gymPage.Show();
                }
                

            }

            //If player get into the random minigame (catch wild pokemon) area then check the condition open minigame window
            if (Canvas.GetTop(player) >= (Canvas.GetTop(MiniGame)) &&
                (Canvas.GetLeft(player) + player.Width) >= (Canvas.GetLeft(MiniGame)) &&
                 (Canvas.GetTop(player) - player.Height) <= (Canvas.GetTop(MiniGame) + MiniGame.Height) &&
                 (Canvas.GetLeft(player) <= (Canvas.GetLeft(MiniGame) + MiniGame.Width)) && 
                 NotInGame //To make sure not in minigame prevent minigame re trigger
                )
            {
                MiniGame miniGamePage = new MiniGame(randomPokemon(), this, bag);
                MessageBox.Show("You have found a wild pokemon!\n Get ready to catch it!");
                NotInGame = false;
                this.Hide();
                Movement_reset();
                miniGamePage.Show();
            }

            //If player go into the home (pokemon management) area then check the condition and open manage window
            if (Canvas.GetTop(player) >= (Canvas.GetTop(manage)) &&
                (Canvas.GetLeft(player) + player.Width) >= (Canvas.GetLeft(manage)) &&
                 (Canvas.GetTop(player) - player.Height) <= (Canvas.GetTop(manage) + manage.Height) &&
                 (Canvas.GetLeft(player) <= (Canvas.GetLeft(manage) + manage.Width))
                )
            {
                if (Canvas.GetTop(player) > Canvas.GetTop(manage) + manage.Height) //To check which side the player get into home and Move player out of the area of the home
                {   //bottom side of the home
                    Canvas.SetLeft(player, 181);
                    Canvas.SetTop(player, 272);
                }
                else //right side of home
                {
                    Canvas.SetLeft(player, 360);
                    Canvas.SetTop(player, 128);
                }
                Movement_reset();
                if (bag.selected == -1) //Check if player own any pokemons
                {
                    MessageBox.Show("Your don't have any pokemon, go catch one!");
                }
                else //Open manage window
                {
                    Manage managePage = new Manage(bag, this);
                    this.Hide();
                    managePage.Show();
                }
                


            }
        }

        private void Movement_reset() //To reset movement to false since when entering the area the key is pressed down
        {
            moveRight = false;
            moveLeft = false;
            moveUp = false;
            moveDown = false;
        }

        public Pokemon randomPokemon() //A function to ranndom generate a pokemon
        {
            Random rnd = new Random();
            Pokemon pokemon = new Pokemon(rnd.Next(10, 20), rnd.Next(10, 20), rnd.Next(5, 10), allPokemon[rnd.Next(0, allPokemon.Count)]);
            return pokemon;
        }

    }




    //Pokemon class
    public class Dictionary
    {
        public string name;
        public string type;
        public string path;
        public Dictionary(string name, string type, string path)
        {
            this.name = name;
            this.type = type;
            this.path = path;
        }
    }

    public class SeriesDictionary
    {
        public Dictionary[] pokemons;
        public int current;
        public SeriesDictionary(Dictionary[] pokemons, int current)
        {
            this.pokemons = pokemons;
            this.current = current;
        }
        public SeriesDictionary(SeriesDictionary pokemons)
        {
            this.pokemons = pokemons.pokemons;
            this.current = pokemons.current;
        }
    }

    public class Pokemon : SeriesDictionary
    {
        public string nickname;
        public int health;
        private int maxHealth;
        private int mp;
        public int exp;
        private int attack;
        private int level;
        private List<Skill> skills;

        public int Mp
        {
            get { return mp; }
        }
        public int Level
        {
            get { return level; }
        }
        public int MaxHealth 
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }
        public int Attack
        {
            get { return attack; }
            set { attack = value; }
        }
        public int Exp
        {
            get { return exp; }
            
        }

        public string normalAttack(Pokemon enermy) //For battle use, input the enemy Pokemon into this method
        {
            int realDamage = this.attack;
            string effect = "";
            if (typeCheck(enermy) == 1) //According the type counter to calculate the damage
            {
                realDamage = realDamage * 2;
                effect = "Attack very effective!";
            }
            if (typeCheck(enermy) == -1)
            {
                realDamage = realDamage / 2;
                effect = "Attack not very effective...";
            }   

            enermy.health = enermy.health - realDamage;

            //Attack result return
            string statement = nickname + " used normal attack and do " + realDamage + " hp damages!\n" + effect; 
            return statement;
        }

        private int typeCheck(Pokemon enermy) //Check the type counter, input the enemy Pokemon into this method
        {
            if (this.pokemons[current].type == "water" && enermy.pokemons[current].type == "fire")
                return 1;
            if (this.pokemons[current].type == "fire" && enermy.pokemons[current].type == "grass")
                return 1;
            if (this.pokemons[current].type == "grass" && enermy.pokemons[current].type == "water")
                return 1;
            if (this.pokemons[current].type == "fire" && enermy.pokemons[current].type == "water")
                return -1;
            if (this.pokemons[current].type == "grass" && enermy.pokemons[current].type == "fire")
                return -1;
            if (this.pokemons[current].type == "water" && enermy.pokemons[current].type == "grass")
                return -1;
            return 0;
        }

        public void skillAttack(Skill skill, Pokemon enermy) //Not implemented
        {
            skill.attack(enermy, typeCheck(enermy) == 1);
            this.mp = this.mp - skill.Neededmp;
        }

        public Pokemon(int health, int mp, int attack, Dictionary[] pokemons, int current) : base(pokemons, current)
        {
            this.health = health;
            this.mp = mp;
            this.attack = attack;
            this.maxHealth = health;
            this.exp = 0;
            Random rnd = new Random();
            this.level = rnd.Next(1, 50);
            this.nickname = pokemons[current].name;
        }
        public Pokemon(int health, int mp, int attack, SeriesDictionary pokemon) : base(pokemon)
        {
            this.health = health;
            this.mp = mp;
            this.attack = attack;
            this.maxHealth = health;
            this.exp = 0;
            Random rnd = new Random();
            this.level = rnd.Next(1, 50);
            this.nickname = pokemons[current].name;
        }
        public void Rename(string new_name) //For renaming the pokemon, string as input
        {
            nickname = new_name;
            MessageBox.Show("Pokemon has rename into " + new_name + " !");
        }
        public void evolve() //Pokemon evolve!!!!
        {
            if ( current < pokemons.Length - 1  && level >= 99) //To check the condition of evolving
            {
                current = current + 1;
                MessageBox.Show(nickname + " has evolve into " + pokemons[current].name + " !");
                attack = Convert.ToInt32(attack * 1.5); //To boost attack
                health = Convert.ToInt32(health * 1.5); //To boost Health
                level = 1;
                exp = 0;
            }
            else
            {
                MessageBox.Show("Seems pokemon is not ready for evolve....\n(Has it level up to 99? Is it evolve already?)");
            }
        }
        public void addSkill(Skill skill) //Not implemented
        {
            skills.Add(skill);
        }
        public void powerup() //To level up by consuming exp
        {
            if (exp >= 100 && level<99)
            {
                level = level + 1;
                MessageBox.Show("Your pokemon has level up!");
                attack += Convert.ToInt32(level / 20 + 1); //To boost attack
                health += Convert.ToInt32(level / 10 + 1); //To boost Health
                exp = exp - 100;
            }
            else if(level>=99) //Max level is 99
            {
                MessageBox.Show("You have reached Max Level!");
            }
            else if (exp < 100) // To check enough EXP
            {
                MessageBox.Show("You does not have enough EXP!");
            }
        }
        
    }
    public class Skill //Not implemented
    {
        private string name;
        private int neededmp;
        private int damage;
        public int Neededmp
        {
            get { return neededmp; }
        }
        public string attack(Pokemon enermy, Boolean typeCounter)
        {
            int realDamage = this.damage;
            if (typeCounter)
                realDamage = realDamage * 2;

            enermy.health = enermy.health - realDamage;
            string statement = "used skill " + name + " and do " + realDamage + " hp damages";
            return statement;
        }
        public Skill(string name, int neededmp, int damage)
        {
            this.name = name;
            this.neededmp = neededmp;
            this.damage = damage;
        }
    }

    public class Bag //Player inventory to store pokemons
    {
        public List<Pokemon> item; //List of player's pokemon
        public int selected; //if select=-1 means no pokemon
        public int money; //Player's money
        
        public Bag()
        {
            selected = 0;
            money = 100;
            item = new List<Pokemon>();
        }

   

        public void Add(Pokemon pokemon) //Add pokemon to player's bag, Pokemon as input
        {
            if (item.Count >= 6) //At most 6 pokemons
            {
                MessageBox.Show("Your bag is full! Cannot add pokemon!");
            }
            else 
            {
                item.Add(pokemon);
                MessageBox.Show(pokemon.pokemons[pokemon.current].name + " added to bag!");
            }
            if (selected == -1)
            {
                selected = 0;
            }
    }

        public bool Sell(int x) //For selling the pokemon, index of the pokemon as input
        {
            int worth = item[x].Level * (item[x].current + 1); //Calculate how much pokemon worth accoring level and state of evolve

            //To double check if player wants to sell the pokemon or not. Pokemon beg player to not sell it
            MessageBoxResult Result = MessageBox.Show("This pokemon worth $ "+worth +"\n"+item[x].nickname +" said: Don't leave me! I will miss you....", "Are you sure you want to sell?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (x <= selected) //when the pokemon going to sell is selected or index smaller than selected, selected -1
            {
                selected -= 1; 
            }
            if (Result == MessageBoxResult.Yes) 
            {
                //Pokemon is sad to say goodbye
                MessageBox.Show("Your pokemon has sold!\n" + item[x].nickname + " said: It's time to say goodbye...");
                item.RemoveAt(x) ;
                money += worth; //add money
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public void Check() //For debugging
        {
            foreach(Pokemon x in item)
            {
                MessageBox.Show("Pokemon: "+ x.nickname);
            }
        }
        
    }




    public class CatchPokemonEngine
    {
        private Pokemon pokemon;

        //put your project part 1 here
    }
}


