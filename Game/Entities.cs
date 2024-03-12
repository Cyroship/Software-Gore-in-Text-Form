using System;
using System.Windows;
using Test_Game;
namespace Test_Game;

public class Player {
    Random rand = new Random();
    public Player() {
        mass = rand.Next(70, 90);
        overall_health = (int)Math.Floor((double)mass * 500 + rand.Next(0, 100));
        current_health = overall_health;
        speed = rand.Next(60,80);
    }

    public string name{get; set;}
    public int mass{get; set;}
    public int overall_health{get; set;}
    public int current_health{get; set;}
    public int speed{get; set;}
}
public class Enemy{
    Random rand = new Random();

    public string name{get; private set;}
    public string animal{get; private set;} 
    public string weight{get; private set;} 
    public string type{get; private set;}
    public string[] limb_names{get; private set;} 
    public string[] attacks {get; private set;}
    public int[] attacks_damage{get; private set;}
    public int[] limb_speed_distribution{get; private set;}
    public int[] limb_health_set{get; private set;}
    public int[] limb_health_set_ref{get; private set;}
    public int[][] attacks_limb_required {get; private set;} //This will reference

    public int overall_health{get; set;}
    public int mass{get; set;}
    public int current_health{get; set;}
    public int speed{get; set;}
    public int current_speed{get; set;}
    public int chance_waste = 0; 
    public int stunned_turns{get; set;}
    public Enemy(string aName){
        name = aName;
        switch (aName){
            case "Fox": 
            case "Wolf": 
            case "Cheetah":
                type = "Quadrupedal";
                weight = "light";
                break;
            case "Bull": 
            case "Horse": 
            case "Lion":
                type = "Quadrupedal";
                weight = "heavy";
                break;
            case "Monkey": 
            case "Bird": 
            case "Lizard":
                type = "Bipedal";
                weight = "light";
                break;
            case "Bodybuilder": 
            case "Cat Human": 
            case "Anthro":
                type = "Bipedal";
                weight = "heavy";
                break;      
            default:
                type = "Unknown";
                weight ="Undefined";
                break;

        }    

        switch (weight){
            case "light":
                mass = rand.Next(20,60);
                break;
            case "heavy":
                mass = rand.Next(100,150);
                break;
            default:
                break;
        }

        overall_health =(int) Math.Floor((double)(mass/5)*500 + rand.NextInt64(1, 100));        
        current_health = overall_health;

        switch (type){
            //Torso does generic damage, however accuracy is generally increased.
            case "Quadrupedal":
                speed = rand.Next(90, 150);
                current_speed = speed;
                
                limb_speed_distribution = new int[]{15, 15, 35, 35};
                limb_health_set = new int[]{(int)Math.Floor((float)overall_health/6), (int)Math.Floor((float)overall_health/6), (int)Math.Floor((float)overall_health/6), (int)Math.Floor((float)overall_health/6)};
                limb_health_set_ref = new int[] {(int)Math.Floor((float)overall_health/6), (int)Math.Floor((float)overall_health/6), (int)Math.Floor((float)overall_health/6), (int)Math.Floor((float)overall_health/6)};

                attacks = new string[]{"Scratch", "Bite", "Hindleg kick", "Charge"};
                attacks_damage = new int[]{6, 8, 13, 15}; //damage is proportional to player health
                attacks_limb_required = new int[][]{new int[2]{1, 0}, new int[2]{1, 0}, new int[2]{1,1}, new int[2]{1,1}}; //This is reference the limbs needed to perform the move
                // the 3rd (or 2nd in 0-based) element: 0 if limb is optional, 1 requires the right amount of limbs to be present

                limb_names = new string[]{"Front Right Leg", "Front Left Leg", "Rear Left Leg", "Rear Right Leg", "Body"};

                break;
            case "Bipedal":
                speed = rand.Next(30, 50);
                current_speed = speed;
                
                limb_speed_distribution = new int[]{25, 25, 25, 25};
                limb_health_set = new int[]{overall_health/6,overall_health/6,overall_health/6,overall_health/6};
                limb_health_set_ref = new int[]{overall_health/6,overall_health/6,overall_health/6,overall_health/6};

                attacks = new string[]{"Scratch", "Bite", "Roundhouse Kick", "Charge"};
                attacks_damage = new int[]{3, 5, 16, 15}; //damage is proportional to player health
                attacks_limb_required = new int[][]{new int[2]{1, 0}, new int[2]{1, 0}, new int[2]{0,1}, new int[2]{0,1}}; //This is reference the limbs needed to perform the move
                // the 3rd (or 2nd in 0-based) element: 0 if limb is optional, 1 requires the right amount of limbs to be present
                
                limb_names = new string[]{"Left Arm", "Right Arm", "Left Leg", "Right Leg", "Body"};
                break;
            default:
                break;
        }

        
    }    

}
