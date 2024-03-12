using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography.X509Certificates;
using Test_Game;
using System.Media;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Channels;

namespace Test_Game;

class Program
{
        /// <summary>
        /// The main method is where the game runs, the derived files are referenced in external folders in a scoope similar to that of Program.cs. 
        /// This is a turn based game.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
        /*
        System.Console.WriteLine(Directory.GetCurrentDirectory().ToString() + @"\second.wav");
        SoundPlayer sound = new SoundPlayer(Directory.GetCurrentDirectory().ToString() + @"\second.wav");
        sound.Play();*/

        /*
        string[] coolbeans = { "BEANS", "Cool" };
        MessageBoxButtons button = MessageBoxButtons.YesNo;
        DialogResult result = MessageBox.Show("Good Stuff", "This is a caption", coolbeans);
        MessageBox.Show($"Yo, you chose {result.ToString()}", "The user's choice", MessageBoxButtons.OK);*/

        
        string input; //Input from the player 

        int required_credit = 1;
        int island_num = 1;
        int enemy_selector = 1;

        Player player = new Player();



        bool validator; //this checks if the user inputs something that is right.
        //Lightweight  Quadrupedal, Heavyweight Quadrupedal, Lightweight Bidpedal, Heavyweight Bipedal
        string[,] Enemy_Animals = { { "Fox", "Wolf", "Cheetah" }, { "Monkey", "Bird", "Lizard" }, /*Screw it, we going imaginary*/{ "Bodybuilder", "Cat Human", "Anthro" }, { "Bull", "Horse", "Lion" } };


        Random random = new Random();

        List<Weapon> weapon_list = new List<Weapon>();
        List<Weapon> player_weapon_list = new List<Weapon>();

        weapon_list.Add(new Weapon("Sword"));
        weapon_list.Add(new Weapon("Butcher Knife"));
        weapon_list.Add(new Weapon("Stones"));
        weapon_list.Add(new Weapon("Gun"));
        weapon_list.Add(new Weapon("Nail Gun"));
        weapon_list.Add(new Weapon("Sharpened Obsidian"));
        
        //Methods
        //The battle function will manage the battle aspect of the game.

        void Battle()
        {
            Enemy enemy = new Enemy(Enemy_Animals[random.NextInt64(0, enemy_selector), random.NextInt64(0, 3)]);

            Thread.Sleep(1000);
            string turn = (player.speed > enemy.speed)? "player": "enemy";
            enemy.stunned_turns = 0;
            Console.WriteLine($"You trek through the landscape, you find a {enemy.name}!");
            Thread.Sleep(1000);

            

            while (player.current_health > 0 && enemy.current_health > 0)
            {
                int i = 0; //We will need certain instances where i will carry over.
                int o;

                Weapon weapon_focus;                

                if (turn.Equals("player")){
                    i = 0; 
                    o = 0;
                    Console.WriteLine("-------------------------------------------");
                    Thread.Sleep(1000);
                    Console.WriteLine("It's your turn!");

                    do {
                        Console.WriteLine($"Health: {player.current_health}/{player.overall_health}");
                        Console.WriteLine("What weapon will you use?");
                        
                        foreach(Weapon weapon in player_weapon_list.ToArray())
                        {
                            Console.WriteLine($"   · {weapon.name}: {weapon.accuracy}% chance of hitting. {weapon.stun_chance}% chance of stunning.");
                        }
                        if (enemy.chance_waste == 100){
                            Console.WriteLine("   · Execute: Guaranteed fatal move.");
                        }

                        Console.Write("Weapon: ");
                        input = Console.ReadLine();
                        
                        validator = false;

                        foreach(Weapon weapon in player_weapon_list.ToArray()){
                            if (weapon.name.ToLower().Equals(input.ToLower()) || (input.ToLower().Equals("execute") && enemy.chance_waste == 100)) {
                                validator = true;
                                break;
                                
                            }
                        } 

                        if (!validator) {
                            Console.WriteLine("You didn't select a weapon");
                            Console.WriteLine("-------------------------------------------");
                        }
                    } while(!validator);

                    validator = false;
                    Thread.Sleep(750);
                    weapon_focus = new Weapon("sword");
                    
                    if (!input.ToLower().Equals("execute")){
                        weapon_focus = new Weapon(input.ToString());
                    }
                    else{
                        input = "";
                        string[] methods = new string[] {"Behead", "Puncture Chest", "Joust", "Batter"};
                        string[] method_desc = new string[] {"Take the head and go home.", "A strike to the heart at the end will do it.", "A sharp object through the brain will do.", "Beat them to death!"};
                        validator = false;
                        while (!validator)
                        {
                            Console.WriteLine("-------------------------------------\nExecution Method: \n-------------------------------------");
                            foreach(string exec in methods){
                                Console.WriteLine($"   · {exec}: {method_desc[i]}");
                                ++i;
                            }
                            i = 0;
                            Console.WriteLine("-------------------------------------");
                            Console.Write("Your method: ");
                            input = Console.ReadLine();

                            foreach(string exec in methods){
                                if (input.ToLower().Equals(exec.ToLower())){
                                    
                                    enemy.current_health = -1;
                                    Console.WriteLine("-------------------------------------");
                                    switch(input.ToLower()){
                                        case "behead":
                                            Console.WriteLine($"{player.name} beheaded the {enemy.name}...");
                                            break;
                                        case "puncture chest":
                                            Console.WriteLine($"{player.name} pierced the {enemy.name}'s heart...");
                                            break;
                                        case "joust":    
                                            Console.WriteLine($"{player.name} ran something through the {enemy.name}'s brain...");
                                            break;
                                        case "batter":
                                            Console.WriteLine($"{player.name} beats the {enemy.name} to death...");
                                            break;
                                    }
                                    
                                    validator = true;
                                    break;
                                }
                                
                            }
                            if (validator){
                                break;
                            }
                            else {
                                Console.WriteLine("-------------------------------------\nThere's no going back...");
                            }
                        }

                        if (validator){
                            break;
                        }    
                    }
                    
                    Thread.Sleep(1000);
                    validator = false; //This is so that 'while' isn't skipped.
                    do {
                        Console.WriteLine("-------------------------------------------");
                        
                        foreach(string limb in enemy.limb_names){ //This lists all limbs currently available.
                            if (i < 4) //made since enemy.limb_health_set is only 4 elements long, and name is 5 bodies long.
                            {
                                if (enemy.limb_health_set[i] > 0) { Console.WriteLine($"   · {limb.ToString()}"); };
                            }
                            else {
                                Console.WriteLine($"   · {limb.ToString()}"); 
                            }
                            ++i;
                        }

                        i = 0;                        

                        Console.WriteLine("-------------------------------------------");
                        Console.Write("Commit move to: "); 
                        
                        input = Console.ReadLine();

                        foreach(string limb in enemy.limb_names)
                        {
                            if (input.ToString().ToLower().Equals(limb.ToString().ToLower())) { //If input equals the limb name
                                if (i < 4){
                                    if (enemy.limb_health_set[i] > 0)
                                    {
                                        validator = true;
                                    }
                                    break;
                                }
                                else {
                                    validator = true;
                                    break;
                                }
                            }
                            ++i; //i = 4 if body
                        }
                        if (validator){
                            break;
                        }
                        else {
                            if (i < 4){
                                if (enemy.limb_health_set[i] <= 0){
                                    Console.WriteLine($"Action cannot be perfomed on the {enemy.limb_names[i]}");
                                }
                            }
                            else {
                                Console.WriteLine($"Action cannot be perfomed on the {input}");
                            }
                            i = 0;
                        }                    
                    }while(!validator);
                    
                    o=i;
                    
                    Console.WriteLine("-------------------------------------------");
                    Thread.Sleep(1000);
                    
                    if (random.NextInt64(0, 100) < weapon_focus.accuracy || enemy.stunned_turns > 0){
                        int dmg = (int)Math.Floor(enemy.overall_health*((float)(weapon_focus.dmg)/100));
                        if (o < 4){
                            dmg = (dmg > enemy.limb_health_set[o])? enemy.limb_health_set[o] : dmg;

                            enemy.limb_health_set[o] -= dmg;
                            enemy.current_health -= dmg;
                        }
                        else {
                            enemy.current_health -= dmg;
                        }
                        
                        if (o < 4){
                            Console.WriteLine($"{player.name} dealt {dmg} damage to the {enemy.name}'s {enemy.limb_names[o]}.");
                            
                            if (enemy.limb_health_set[o] > 0){
                                Console.WriteLine($"Health of the {enemy.name}'s {enemy.limb_names[o]} is now {enemy.limb_health_set[o]}/{enemy.limb_health_set_ref[o]}.");
                            }

                            else{ //A little maiming function that takes speed.
                                int speed_damage = (int)Math.Floor(enemy.speed * ((double)enemy.limb_speed_distribution[o]/100));
                                Thread.Sleep(500);
                                Console.WriteLine($"You severed the {enemy.name}'s {enemy.limb_names[o]}!");
                                enemy.current_speed -= speed_damage;
                                enemy.chance_waste += 25;
                            }

                        }
                        else {
                            Console.WriteLine($"You dealt {dmg} damage to the {enemy.name}!");
                        }

                        if (random.NextInt64(0,100) <= weapon_focus.stun_chance){
                            Thread.Sleep(500);
                            if (enemy.stunned_turns < 2){
                                Console.WriteLine($"{player.name} stunned the {enemy.name}!");
                            }
                            else {
                                Console.WriteLine($"{player.name} stunned the {enemy.name} harder!");
                            }
                            enemy.stunned_turns += 2;
                        }

                        Thread.Sleep(1000);


                    }
                    else {
                        Thread.Sleep(1000);
                        Console.WriteLine($"The {weapon_focus.name} missed!");
                    }
                    Console.WriteLine("-------------------------------------------");

                    enemy.stunned_turns -= (enemy.stunned_turns == 1)? 1 : 0 ;
                    
                    turn = "enemy";
                }
                else {
                    Thread.Sleep(1000);
                    if (enemy.stunned_turns == 0){
                        if (enemy.chance_waste != 0){
                            if (random.NextInt64(0, 100) <= enemy.chance_waste){
                                Console.WriteLine($"The {enemy.name} pleads for its life! You are yielded the turn.");
                                turn = "player";
                            }
                        }
                        if (!turn.Equals("player")){
                            int dmg = 0; 

                            List<string> available_attacks = new List<string>{};

                            i = 0;

                            int front_limbs = 0;
                            int back_limbs = 0;

                            foreach(int limb_health in enemy.limb_health_set){
                                if (limb_health > 0){
                                    if (i < 2){
                                        ++front_limbs;
                                    }
                                    else{
                                        ++back_limbs;
                                    }
                                }
                                ++i;
                            }

                            i = 0;

                            foreach( int[] part_requirement in enemy.attacks_limb_required)
                            {
                                if (front_limbs >= part_requirement[0] && back_limbs >= part_requirement[1]){
                                    available_attacks.Add(enemy.attacks[i]);
                                }    
                                ++i;
                            }

                            if (available_attacks.Count != 0){
                                string move = available_attacks[random.Next(0, available_attacks.Count)];
                                
                                for (int j=0; j<enemy.attacks.Length; ++j){
                                    if (enemy.attacks[j].Equals(move)){
                                        dmg = (int)((double)(player.overall_health * ((double)enemy.current_speed/enemy.speed) * ((float)enemy.attacks_damage[j]/100) * ((island_num > 1)? 1 + (0.05 * island_num): 1)) + (int)random.NextInt64(0, 100));
                                        break;
                                    }
                                }
                                player.current_health -= dmg;

                                Console.WriteLine($"The {enemy.name} used {move}! Dealing {dmg} damage!");
                            }
                            else {
                                dmg = (int)Math.Floor((double)enemy.current_health/50);
                                
                                Console.WriteLine($"The {enemy.name} uses Disabled Charge! Dealing {dmg} damage to the player and {dmg/10} points of damage to itself!");

                                player.current_health -= dmg;
                                enemy.current_health -= (int)Math.Floor((float)dmg/10);

                            }
                            
                            //We will make it so that we get all available actions the enemy can make so that we can get something random

                        }
                    }
                    else {
                        
                        enemy.stunned_turns -= (enemy.stunned_turns > 1)? 1 : 0 ;
                        switch (enemy.stunned_turns)
                        {
                            case 1:
                                Console.WriteLine($"The {enemy.name} is stunned for this turn, they'll be in action next turn! All attacks are guaranteed to hit this turn!");
                                break;
                            case 2:
                                Console.WriteLine($"The {enemy.name} is stunned for this turn and the next turn. All attacks are guaranteed to hit this turn!");
                                break;
                            default:
                                Console.WriteLine($"The {enemy.name} is stunned for this turn and {enemy.stunned_turns - 1} sebsequent turns. All attacks are guaranteed to hit this turn!");
                                break;
                        }

                        
                        
                    }
                    turn = "player";
                }

            }

            if (enemy.current_health > 0)
                {
                    ++required_credit;
                    Console.WriteLine("You were defeated! We're taking you back...");
                    player.current_health = player.overall_health;
                }
                else if (player.current_health > 0){
                    --required_credit;
                    player.current_health = player.overall_health;
                    Console.WriteLine($"You defeated the {enemy.name}!");
                }
            Console.WriteLine("-----------------------------------------------------");

        }
        //We first define all of our stuff here.

        
        // -------------------------------------
        
        Console.WriteLine("------------------------\n- The Ben Shapiro Show -\n------------------------");
        Console.Write("Welcome person! What is your name called? ");

        player.name = Console.ReadLine(); //We get a name from the user
        Console.WriteLine("-------------------------------");
        Console.WriteLine($"Welcome {player.name}!");
        input = "";
        while (!input.ToLower().Equals("yes"))
        {

            Console.Write("Do you agree to our EULA, \"Do not harm.\", and join our alliance to defend the world? ");
            input = Console.ReadLine();

            if (!input.ToLower().Equals("yes"))
            {
                Console.WriteLine("No is not an option.\n-----------------------------------------------------");
            }
        }
        Console.WriteLine("-----------------------------------------------------\nIntroduction\n------------------------------------------------");
        Console.WriteLine($"You are {player.name}, you are on an island. We are part of a sleeper cell called AXOS freeing people one person at a time. In order to seek the salvation that is imperative to survival, you must go through 4 islands, it won't be easy, but it is better than living here.\n------------------------------------------------");
        Console.Write("What do you think? ");
        Console.ReadLine();
        Console.WriteLine("------------------------------------------------\nDoesn't matter, we must get going.");



        while (weapon_list.Count > 2)
        {
            Console.WriteLine("--------------------\nWeapon List\n--------------------");
            foreach (Weapon weapon in weapon_list)
            {
                Console.WriteLine($"{weapon.name} - {weapon.desc}\n----------------------------------");
            }

            Console.Write("Select thy weapon: ");
            input = Console.ReadLine();

            /* //Vestigial
            if (input.Equals("skip")){
                break;
            }
            */


            validator = false;
            string temp = "";



            foreach (Weapon weapon in weapon_list.ToArray())
            {
                if (weapon.name.ToLower().Equals(input.ToLower()))
                {

                    temp = weapon.name;
                    player_weapon_list.Add(weapon);
                    weapon_list.RemoveAt(weapon_list.IndexOf(weapon));
                    validator = true;

                }

            }



            if (!validator)
            {
                Console.WriteLine("You didn't pick one!");
            }
            else
            {
                Console.WriteLine($"You picked the {temp}!");
                //break;
            }

        }

        Console.WriteLine("-----------------------------------------------------");
        Console.WriteLine("Congratulations! You have armed yourself with the neccessary tools to survive.\nWe however can't let you go straight to island 4, monsters over there are really powerful! You'll need to show development of your skills before we allow you to go to the next island.");
        Console.WriteLine("-----------------------------------------------------");



        while (island_num < 9)
        {
            input = "";
            Console.WriteLine($"You are on island: {island_num}/4.");
            while (!input.ToLower().Equals("adventure") && !input.ToLower().Equals("depart") &&
                    !input.ToLower().Equals("checkself"))
            {

                Console.WriteLine($"What would you like to do? \n[Adventure, CheckSelf, Depart]");
                if (required_credit > 0)
                {
                    if (required_credit > 1){Console.WriteLine($"You need to win {required_credit} more battles to leave.");}
                    else {Console.WriteLine($"You need to win one more victory!"); }
                }
                else
                {
                    Console.WriteLine("You are clear to leave!");
                }

                Console.Write("Action: ");
                input = Console.ReadLine();
                Console.WriteLine("-----------------------------------------------------");

                if (!input.ToLower().Equals("adventure") && !input.ToLower().Equals("depart") &&
                    !input.ToLower().Equals("checkself"))
                {
                    Console.WriteLine("You didn't pick an option!\n-----------------------------------------------------");
                }
            }

            switch (input.ToLower())
            {
                case "checkself":
                    Console.WriteLine("Player Stats-----------------------------------------");
                    Console.WriteLine("-----------------------------------------------------");

                    Console.WriteLine($"{player.name}\n-----------------------------\nHealth: {player.overall_health}\nMass: {player.mass}\nSpeed: {player.speed}\n-----------------------------------------------------");
                    input = "";
                    break;
                case "depart":
                    if (required_credit > 0)
                    {
                        if (required_credit == 1)
                        {
                            Console.WriteLine($"You need to achieve one more victory to move on to the next island!");
                        }
                        else
                        {
                            Console.WriteLine($"You need to win {required_credit} more battles to move to the next island!");
                            Console.WriteLine("-----------------------------------------------------");
                        }
                    }

                    else
                    {
                        island_num++;
                        if (island_num < 5)
                        {
                            enemy_selector = island_num;
                            switch(island_num){
                                case 2:
                                    required_credit = 1;
                                    break;
                                case 3:
                                case 4:
                                    required_credit = 2;
                                    break;
                                default:
                                    Console.WriteLine("No value matching for island_num!");
                                    break;
                            }
                        }

                        else
                        {
                            Console.WriteLine($"{player.name} escaped the shores! They went on to massacre and commit war crimes on all kinds of animals!\n--------------------------\n\n--------------------------\n--- My Condolences -------\n--------------------------");
                            System.Environment.Exit(0);
                        }

                    }

                    break;
                case "adventure":
                    Battle();
                    break;

            }

        }
    }
}