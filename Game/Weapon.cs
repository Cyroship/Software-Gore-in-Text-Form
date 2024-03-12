using System;
using System.Security.Cryptography;
using Microsoft.VisualBasic.FileIO;

namespace Test_Game;



public class Weapon {
    Random random_num_class = new Random();
    public string name {get; set;}
    public string desc {get; set;}
    public int dmg {get; set;}
    public int stun_chance {get; set;}
    public int accuracy {get; set;} //stun_chance, 101 is guaranteed, 0 is no shot.
    
    private void set_stats(int aDmg, int aStun_chance, int aAccuracy)
    {
        dmg = aDmg;
        stun_chance = aStun_chance;
        accuracy = aAccuracy;
    }
    
    public Weapon(string weaponName){
         
        switch(weaponName.ToLower()){
            case "sword":
                name = weaponName;
                desc = "Standard Combat Tool.";
                set_stats(5, 0, 100); //dmg is out of 100, all damage dealt to the enemy will be proportional to their health!
                break;

            case "butcher knife":
                name = weaponName;
                desc = "Goodbye self and everything that finds itself on the other side of the blade.";
                set_stats(8, 0, 80);
                break;

            case "gun":
                name = weaponName;
                desc = "Capable of falling the most formidable opponents! Bullet spread and stuff kinda sucks tho.";
                set_stats(10, 0, 70);
                break;

            case "stones":
                name = weaponName; 
                desc = "Powerful in the right hands, you could stun them if you hit them! You look like a really good thrower!";
                set_stats(4, 80, 70);
                break;

            case "nail gun":
                name = weaponName;
                desc = "\"If you don't like the video I\'ll screw you to your bed; I'll do a Jesus yeah and screw your wrists to your bedframe\"\n\t~ Jay Swingler";
                set_stats(12, 0, 80);
                break;

            case "sharpened obsidian":
                name = weaponName;
                desc = "A delectable treat on one hand, then you touch it, and now you're without a finger!";
                set_stats(20, 0, 50);
                break;
        }
    } //Apparently you can use strings as a case possibility
    
}
/*
class SFstern: Weapon{
    public SFstern(string aName, string aDesc, int aDmg, int aWeapon_accuracy, int aStun_chance, double aLimb_efficiency) : base(aName, aDesc, aDmg, aWeapon_accuracy, aStun_chance, aLimb_efficiency)
    {
    }
} Okay got it now.

*/
