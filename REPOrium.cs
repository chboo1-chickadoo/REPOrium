using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;

namespace REPOrium
{
    [BepInPlugin("chboo1.repo.reporium", "REPOrium", "0.0.1")]
    public class REPOrium : BaseUnityPlugin
    {
        public static BepInEx.Logging.ManualLogSource logger;
        private static Harmony harmony = null;

        public static bool godmode = false;
        public static bool fly = false;
        public static bool freecam = false;
        public static bool infiniteRange = false;
        public static bool infiniteStrength = false;
        public static bool infiniteStamina = false;
        public static bool trueInfiniteStrength = false;
        public static bool nodeath = false;
        public static bool weakEnemy = false;
        public static bool batteryDrain = true;
        public static bool verySqueakyHammer = false;
        public static bool noTarget = false;
        public static bool invisible = false;
        public static bool quiet = false;
        public static bool infiniteMoney = false;
        public static bool doTraps = true;
        public static bool itemDamage = true;

        public static bool testHallway = false;

        public static int previousMoney = -1;
        public static float previousGrabStrength = -1;
        public static float previousGrabRange = -1;

        public static int annoyingCount = 0;

        public static Level savedLevel = null;
        public static Dictionary<string, UnityEngine.Object> spawnerKeys;
        public static Dictionary<string, EnemySetup> enemySpawnerKeys;
        public static bool spawning = false;
        public void Awake()
        {
            logger = BepInEx.Logging.Logger.CreateLogSource("chboo1.repo.reporium");
            logger.LogInfo("Loading REPORIUM...");
            harmony = new Harmony("chboo1.repo.reporium");
            spawnerKeys = new Dictionary<string, UnityEngine.Object>();

            spawnerKeys.Add("diamond", Resources.Load("valuables/01 tiny/Valuable Diamond")); // I'm so sorry
            spawnerKeys.Add("bracelet", Resources.Load("valuables/01 tiny/Valuable Emerald Bracelet"));
            spawnerKeys.Add("goblet", Resources.Load("valuables/01 tiny/Valuable Goblet"));
            spawnerKeys.Add("ocarina", Resources.Load("valuables/01 tiny/Valuable Ocarina"));
            spawnerKeys.Add("watch", Resources.Load("valuables/01 tiny/Valuable Pocket Watch"));
            spawnerKeys.Add("pocketwatch", Resources.Load("valuables/01 tiny/Valuable Pocket Watch"));
            spawnerKeys.Add("mug", Resources.Load("valuables/01 tiny/Valuable Uranium Mug"));
            spawnerKeys.Add("uraniummug", Resources.Load("valuables/01 tiny/Valuable Uranium Mug"));
            logger.LogInfo("Tiny items loaded!");
            spawnerKeys.Add("bonsai", Resources.Load("valuables/02 small/Valuable Arctic Bonsai"));
            spawnerKeys.Add("hdd", Resources.Load("valuables/02 small/Valuable Arctic HDD"));
            spawnerKeys.Add("drive", Resources.Load("valuables/02 small/Valuable Arctic HDD"));
            spawnerKeys.Add("harddrive", Resources.Load("valuables/02 small/Valuable Arctic HDD"));
            spawnerKeys.Add("book", Resources.Load("valuables/02 small/Valuable Chomp Book"));
            spawnerKeys.Add("chompbook", Resources.Load("valuables/02 small/Valuable Chomp Book"));
            spawnerKeys.Add("crown", Resources.Load("valuables/02 small/Valuable Crown"));
            spawnerKeys.Add("doll", Resources.Load("valuables/02 small/Valuable Doll"));
            spawnerKeys.Add("frog", Resources.Load("valuables/02 small/Valuable Frog"));
            spawnerKeys.Add("gembox", Resources.Load("valuables/02 small/Valuable Gem Box"));
            spawnerKeys.Add("globe", Resources.Load("valuables/02 small/Valuable Globe"));
            spawnerKeys.Add("potion", Resources.Load("valuables/02 small/Valuable Love Potion"));
            spawnerKeys.Add("lovepotion", Resources.Load("valuables/02 small/Valuable Love Potion"));
            spawnerKeys.Add("money", Resources.Load("valuables/02 small/Valuable Money"));
            spawnerKeys.Add("musicbox", Resources.Load("valuables/02 small/Valuable Music Box"));
            spawnerKeys.Add("monkey", Resources.Load("valuables/02 small/Valuable Toy Monkey"));
            spawnerKeys.Add("plate", Resources.Load("valuables/02 small/Valuable Uranium Plate"));
            spawnerKeys.Add("uraniumplate", Resources.Load("valuables/02 small/Valuable Uranium Plate"));
            spawnerKeys.Add("smallvase", Resources.Load("valuables/02 small/Valuable Vase Small"));
            logger.LogInfo("Small items loaded!");
            spawnerKeys.Add("3dprinter", Resources.Load("valuables/03 medium/Valuable Arctic 3D Printer"));
            spawnerKeys.Add("printer", Resources.Load("valuables/03 medium/Valuable Arctic 3D Printer"));
            spawnerKeys.Add("laptop", Resources.Load("valuables/03 medium/Valuable Arctic Laptop"));
            spawnerKeys.Add("propane", Resources.Load("valuables/03 medium/Valuable Arctic Propane Tank"));
            spawnerKeys.Add("propanetank", Resources.Load("valuables/03 medium/Valuable Arctic Propane Tank"));
            spawnerKeys.Add("samplepack", Resources.Load("valuables/03 medium/Valuable Arctic Sample Six Pack"));
            spawnerKeys.Add("sample", Resources.Load("valuables/03 medium/Valuable Arctic Sample"));
            spawnerKeys.Add("bottle", Resources.Load("valuables/03 medium/Valuable Bottle"));
            spawnerKeys.Add("clown", Resources.Load("valuables/03 medium/Valuable Clown"));
            spawnerKeys.Add("computer", Resources.Load("valuables/03 medium/Valuable Computer"));
            spawnerKeys.Add("fan", Resources.Load("valuables/03 medium/Valuable Fan"));
            spawnerKeys.Add("gramophone", Resources.Load("valuables/03 medium/Valuable Gramophone"));
            spawnerKeys.Add("table", Resources.Load("valuables/03 medium/Valuable Marble Table"));
            spawnerKeys.Add("marbletable", Resources.Load("valuables/03 medium/Valuable Marble Table"));
            spawnerKeys.Add("radio", Resources.Load("valuables/03 medium/Valuable Radio"));
            spawnerKeys.Add("ship", Resources.Load("valuables/03 medium/Valuable Ship in a bottle"));
            spawnerKeys.Add("shipbottle", Resources.Load("valuables/03 medium/Valuable Ship in a bottle"));
            spawnerKeys.Add("bottleship", Resources.Load("valuables/03 medium/Valuable Ship in a bottle"));
            spawnerKeys.Add("shipinabottle", Resources.Load("valuables/03 medium/Valuable Valuable Ship in a bottle"));
            spawnerKeys.Add("trophy", Resources.Load("valuables/03 medium/Valuable Trophy"));
            spawnerKeys.Add("vase", Resources.Load("valuables/03 medium/Valuable Vase"));
            spawnerKeys.Add("mediumvase", Resources.Load("valuables/03 medium/Valuable Vase"));
            spawnerKeys.Add("goblin", Resources.Load("valuables/03 medium/Valuable Wizard Goblin Head"));
            spawnerKeys.Add("goblinhead", Resources.Load("valuables/03 medium/Valuable Wizard Goblin Head"));
            spawnerKeys.Add("wizardcrystal", Resources.Load("valuables/03 medium/Valuable Wizard Power Crystal"));
            spawnerKeys.Add("timeglass", Resources.Load("valuables/03 medium/Valuable Wizard Time Glass"));
            spawnerKeys.Add("hourglass", Resources.Load("valuables/03 medium/Valuable Wizard Time Glass"));
            logger.LogInfo("Medium items loaded!");
            spawnerKeys.Add("barrel", Resources.Load("valuables/04 big/Valuable Arctic Barrel"));
            spawnerKeys.Add("samplebig", Resources.Load("valuables/04 big/Valuable Arctic Big Sample"));
            spawnerKeys.Add("bigsample", Resources.Load("valuables/04 big/Valuable Arctic Big Sample"));
            spawnerKeys.Add("leg", Resources.Load("valuables/04 big/Valuable Arctic Creature Leg"));
            spawnerKeys.Add("flamethrower", Resources.Load("valuables/04 big/Valuable Arctic Flamethrower"));
            spawnerKeys.Add("guitar", Resources.Load("valuables/04 big/Valuable Arctic Guitar"));
            spawnerKeys.Add("cooler", Resources.Load("valuables/04 big/Valuable Arctic Sample Cooler"));
            spawnerKeys.Add("samplecooler", Resources.Load("valuables/04 big/Valuable Arctic Sample Cooler"));
            spawnerKeys.Add("diamonddisplay", Resources.Load("valuables/04 big/Valuable Diamond Display"));
            spawnerKeys.Add("saw", Resources.Load("valuables/04 big/Valuable Ice Saw"));
            spawnerKeys.Add("icesaw", Resources.Load("valuables/04 big/Valuable Ice Saw"));
            spawnerKeys.Add("bigdoll", Resources.Load("valuables/04 big/Valuable Scream Doll"));
            spawnerKeys.Add("screamdoll", Resources.Load("valuables/04 big/Valuable Scream Doll"));
            spawnerKeys.Add("tv", Resources.Load("valuables/04 big/Valuable Television"));
            spawnerKeys.Add("television", Resources.Load("valuables/04 big/Valuable Television"));
            spawnerKeys.Add("bigvase", Resources.Load("valuables/04 big/Valuable Vase Big"));
            spawnerKeys.Add("vasebig", Resources.Load("valuables/04 big/Valuable Vase big"));
            spawnerKeys.Add("cube", Resources.Load("valuables/04 big/Valuable Wizard Cube of Knowledge"));
            spawnerKeys.Add("wizardcube", Resources.Load("valuables/04 big/Valuable Wizard Cube of Knowledge"));
            spawnerKeys.Add("cubeofknowledge", Resources.Load("valuables/04 big/Valuable Wizard Cube of Knowledge"));
            spawnerKeys.Add("bigpotion", Resources.Load("valuables/04 big/Valuable Wizard Master Potion"));
            spawnerKeys.Add("masterpotion", Resources.Load("valuables/04 big/Valuable Wizard Master Potion"));
            logger.LogInfo("Big items loaded!");
            spawnerKeys.Add("crate", Resources.Load("valuables/05 wide/Valuable Animal Crate"));
            spawnerKeys.Add("animalcrate", Resources.Load("valuables/05 wide/Valuable Animal Crate"));
            spawnerKeys.Add("skeleton", Resources.Load("valuables/05 wide/Valuable Arctic Ice Block"));
            spawnerKeys.Add("ice", Resources.Load("valuables/05 wide/Valuable Arctic Ice Block"));
            spawnerKeys.Add("iceblock", Resources.Load("valuables/05 wide/Valuable Arctic Ice Block"));
            spawnerKeys.Add("dino", Resources.Load("valuables/05 wide/Valuable Dinosaur"));
            spawnerKeys.Add("dinosaur", Resources.Load("valuables/05 wide/Valuable Dinosaur"));
            spawnerKeys.Add("piano", Resources.Load("valuables/05 wide/Valuable Piano"));
            spawnerKeys.Add("griffin", Resources.Load("valuables/05 wide/Valuable Wizard Griffin Statue"));
            spawnerKeys.Add("griffinstatue", Resources.Load("valuables/05 wide/Valuable Wizard Griffin Statue"));
            logger.LogInfo("Wide items loaded!");
            spawnerKeys.Add("sciencestation", Resources.Load("valuables/06 tall/Valuable Arctic Science Station"));
            spawnerKeys.Add("harp", Resources.Load("valuables/06 tall/Valuable Harp"));
            spawnerKeys.Add("painting", Resources.Load("valuables/06 tall/Valuable Painting"));
            spawnerKeys.Add("staff", Resources.Load("valuables/06 tall/Valuable Wizard Dumgolfs Staff"));
            spawnerKeys.Add("greatsword", Resources.Load("valuables/06 tall/Valuable Wizard Sword"));
            spawnerKeys.Add("wizardsword", Resources.Load("valuables/06 tall/Valuable Wizard Sword"));
            logger.LogInfo("Tall items loaded!");
            spawnerKeys.Add("server", Resources.Load("valuables/07 very tall/Valuable Arctic Server Rack"));
            spawnerKeys.Add("servers", Resources.Load("valuables/07 very tall/Valuable Arctic Server Rack"));
            spawnerKeys.Add("serverrack", Resources.Load("valuables/07 very tall/Valuable Arctic Server Rack"));
            spawnerKeys.Add("statue", Resources.Load("valuables/07 very tall/Valuable Golden Statue"));
            spawnerKeys.Add("goldstatue", Resources.Load("valuables/07 very tall/Valuable Golden Statue"));
            spawnerKeys.Add("goldenstatue", Resources.Load("valuables/07 very tall/Valuable Golden Statue"));
            spawnerKeys.Add("clock", Resources.Load("valuables/07 very tall/Valuable Grandfather Clock"));
            spawnerKeys.Add("grandfatherclock", Resources.Load("valuables/07 very tall/Valuable Grandfather Clock"));
            spawnerKeys.Add("broom", Resources.Load("valuables/07 very tall/Valuable Wizard Broom"));
            logger.LogInfo("Very tall items loaded!");
            spawnerKeys.Add("bigcart", Resources.Load("items/Cart Big"));
            spawnerKeys.Add("cart", Resources.Load("items/Item Cart Medium"));
            spawnerKeys.Add("smallcart", Resources.Load("items/Item Cart Small"));
            spawnerKeys.Add("pocketcart", Resources.Load("items/Item Cart Small"));
            spawnerKeys.Add("rechargedrone", Resources.Load("items/Item Drone Battery"));
            spawnerKeys.Add("featherdrone", Resources.Load("items/Item Drone Feather"));
            spawnerKeys.Add("indestructibledrone", Resources.Load("items/Item Drone Indestructible"));
            spawnerKeys.Add("rolldrone", Resources.Load("items/Item Drone Torque"));
            spawnerKeys.Add("zerogravitydrone", Resources.Load("items/Item Drone Zero Gravity"));
            spawnerKeys.Add("extracttracker", Resources.Load("items/Item Extraction Tracker"));
            spawnerKeys.Add("extractiontracker", Resources.Load("items/Item Extraction Tracker"));
            spawnerKeys.Add("ducttapegrenade", Resources.Load("items/Item Grenade Duct Taped"));
            spawnerKeys.Add("ducttapedgrenade", Resources.Load("items/Item Grenade Duct Taped"));
            spawnerKeys.Add("explosivegrenade", Resources.Load("items/Item Grenade Explosive"));
            spawnerKeys.Add("humangrenade", Resources.Load("items/Item Grenade Human"));
            spawnerKeys.Add("shockwavegrenade", Resources.Load("items/Item Grenade Shockwave"));
            spawnerKeys.Add("stungrenade", Resources.Load("items/Item Grenade Stun"));
            spawnerKeys.Add("handgun", Resources.Load("items/Item Gun Handgun"));
            spawnerKeys.Add("pistol", Resources.Load("items/Item Gun Handgun"));
            spawnerKeys.Add("gun", Resources.Load("items/Item Gun Handgun"));
            spawnerKeys.Add("shotgun", Resources.Load("items/Item Gun Shotgun"));
            spawnerKeys.Add("tranq", Resources.Load("items/Item Gun Tranq"));
            spawnerKeys.Add("tranqgun", Resources.Load("items/Item Gun Tranq"));
            spawnerKeys.Add("largehealth", Resources.Load("items/Item Health Pack Large"));
            spawnerKeys.Add("largehealthpack", Resources.Load("items/Item Health Pack Large"));
            spawnerKeys.Add("mediumhealth", Resources.Load("items/Item Health Pack Medium"));
            spawnerKeys.Add("mediumhealthpack", Resources.Load("items/Item Health Pack Medium"));
            spawnerKeys.Add("smallhealth", Resources.Load("items/Item Health Pack Small"));
            spawnerKeys.Add("smallhealthpack", Resources.Load("items/Item Health Pack Small"));
            spawnerKeys.Add("bat", Resources.Load("items/Item Melee Baseball Bat"));
            spawnerKeys.Add("baseballbat", Resources.Load("items/Item Melee Baseball Bat"));
            spawnerKeys.Add("pan", Resources.Load("items/Item Melee Frying Pan"));
            spawnerKeys.Add("fryingpan", Resources.Load("items/Item Melee Frying Pan"));
            spawnerKeys.Add("squeakyhammer", Resources.Load("items/Item Melee Inflatable Hammer"));
            spawnerKeys.Add("inflatablehammer", Resources.Load("items/Item Melee Inflatable Hammer"));
            spawnerKeys.Add("sledgehammer", Resources.Load("items/Item Melee Sledge Hammer"));
            spawnerKeys.Add("sword", Resources.Load("items/Item Melee Sword"));
            spawnerKeys.Add("explosivemine", Resources.Load("items/Item Mine Explosive"));
            spawnerKeys.Add("shockwavemine", Resources.Load("items/Item Mine Shockwave"));
            spawnerKeys.Add("stunmine", Resources.Load("items/Item Mine Stun"));
            spawnerKeys.Add("zerogravityorb", Resources.Load("items/Item Orb Zero Gravity"));
            spawnerKeys.Add("powercrystal", Resources.Load("items/Item Power Crystal"));
            spawnerKeys.Add("crystal", Resources.Load("items/Item Power Crystal"));
            spawnerKeys.Add("rubberduck", Resources.Load("items/Item Rubber Duck"));
            spawnerKeys.Add("valuabletracker", Resources.Load("items/Item Valuable Tracker"));
            spawnerKeys.Add("scraptracker", Resources.Load("items/Item Valuable Tracker"));
            logger.LogInfo("Shop items loaded!");
            spawnerKeys.Add("strengthupgrade", Resources.Load("items/Item Upgrade Player Grab Strength"));
            spawnerKeys.Add("rangeupgrade", Resources.Load("items/Item Upgrade Player Grab Range"));
            spawnerKeys.Add("jumpupgrade", Resources.Load("items/Item Upgrade Player Extra Jump"));
            spawnerKeys.Add("extrajumpupgrade", Resources.Load("items/Item Upgrade Player Extra Jump"));
            spawnerKeys.Add("throwupgrade", Resources.Load("items/Item Upgrade Player Grab Throw"));
            spawnerKeys.Add("staminaupgrade", Resources.Load("items/Item Upgrade Player Energy"));
            spawnerKeys.Add("healthupgrade", Resources.Load("items/Item Upgrade Player Health"));
            spawnerKeys.Add("speedupgrade", Resources.Load("items/Item Upgrade Player Sprint Speed"));
            spawnerKeys.Add("sprintspeedupgrade", Resources.Load("items/Item Upgrade Player Sprint Speed"));
            spawnerKeys.Add("tumbleupgrade", Resources.Load("items/Item Upgrade Player Tumble Launch"));
            spawnerKeys.Add("launchupgrade", Resources.Load("items/Item Upgrade Player Tumble Launch"));
            spawnerKeys.Add("tumblelaunchupgrade", Resources.Load("items/Item Upgrade Player Tumble Launch"));
            spawnerKeys.Add("playercountupgrade", Resources.Load("items/Item Upgrade Map Player Count"));
            spawnerKeys.Add("mapplayercountupgrade", Resources.Load("items/Item Upgrade Map Player Count"));
            logger.LogInfo("Upgrades loaded!");
            /*
            These just don't work. They're ScriptableObject s, which unity refuses to spawn with a position and rotation. I can't be bothered to make it work. 

            spawnerKeys.Add("healdrone", Resources.Load("items/removed items/Item Drone Heal"));
            spawnerKeys.Add("rechargeorb", Resources.Load("items/removed items/Item Orb Battery"));
            spawnerKeys.Add("featherorb", Resources.Load("items/removed items/Item Orb Feather"));
            spawnerKeys.Add("healorb", Resources.Load("items/removed items/Item Orb Heal"));
            spawnerKeys.Add("indestructibleorb", Resources.Load("items/removed items/Item Orb Indestructible"));
            spawnerKeys.Add("magnetorb", Resources.Load("items/removed items/Item Orb Magnet"));
            spawnerKeys.Add("rollorb", Resources.Load("items/removed items/Item Orb Torque"));
            */

            AddPrefix(typeof(ChatManager), "MessageSend");
            AddPrefix(typeof(PlayerHealth), "Update");
            AddPrefix(typeof(ExtractionPoint), "Update");
            AddPrefix(typeof(EnemyVision), "VisionTrigger");
            AddPrefix(typeof(EnemyDirector), "SetInvestigate");
            AddPrefix(typeof(PhysGrabber), "Update");
            AddPrefix(typeof(RunManager), "Update");
            AddPrefix(typeof(PlayerHealth), "Hurt");
            AddPrefix(typeof(LevelGenerator), "Generate");
            AddPrefix(typeof(ChatManager), "Update");
            AddPrefix(typeof(PhysGrabObjectImpactDetector), "Break");
            AddPostfix(typeof(ItemBattery), "Update");
            AddPrefix(typeof(EnemyHealth), "Hurt");
            AddPrefix(typeof(ItemMeleeInflatableHammer), "OnHit");
            AddPrefix(typeof(EnemyParent), "Despawn");

            AddPrefix(typeof(Enemy), "Update");
            AddPrefix(typeof(EnemyAnimal), "Update");
            AddPrefix(typeof(EnemyBangDirector), "Update");
            AddPrefix(typeof(EnemyBeamer), "Update");
            AddPrefix(typeof(EnemyBowtie), "Update");
            AddPrefix(typeof(EnemyDuck), "Update");
            AddPrefix(typeof(EnemyGnomeDirector), "Update");
            AddPrefix(typeof(EnemyHidden), "Update");
            AddPrefix(typeof(EnemySlowMouth), "Update");
            AddPrefix(typeof(EnemySlowWalker), "Update");
            AddPrefix(typeof(EnemyThinMan), "Update");
            AddPrefix(typeof(EnemyValuableThrower), "Update");
            AddPrefix(typeof(EnemyCeilingEye), "Update");
            AddPrefix(typeof(EnemyRobe), "Update");
            AddPrefix(typeof(EnemyFloater), "Update");
            AddPrefix(typeof(EnemyUpscream), "Update");
            AddPrefix(typeof(EnemyTumbler), "Update");
            AddPrefix(typeof(EnemyRunner), "Update");
        }

        public static void AddPostfix(Type type, string funcname)
        {
            MethodInfo og = AccessTools.Method(type, funcname);
            MethodInfo postfix = AccessTools.Method(typeof(REPOrium), type.Name + "_" + funcname + "_Postfix");
            if (og == null || postfix == null)
            {
                logger.LogInfo("Cannot find original function or postfix!");
                return;
            }
            harmony.Patch(og, null, new HarmonyMethod(postfix));
        }
        public static void AddPrefix(Type type, string funcname)
        {
            MethodInfo og = AccessTools.Method(type, funcname);
            MethodInfo prefix = AccessTools.Method(typeof(REPOrium), type.Name + "_" + funcname + "_Prefix");
            if (og == null || prefix == null)
            {
                logger.LogInfo("Cannot find original function or prefix!");
                return;
            }
            harmony.Patch(og, new HarmonyMethod(prefix));
        }
        public static bool ParseChat(ref string text)
        {
            if (!SemiFunc.IsMasterClientOrSingleplayer())
            {
                return true;
            }
            string[] words = text.ToLower().Split(' ');
            string command = words[0];
            switch (command)
            {
                case "!god": // Done
                    if (words.Length > 1)
                    {
                        if (words[1] == "true" || words[1] == "enable" || words[1] == "enabled" || words[1] == "on")
                        {
                            godmode = true;
                        }
                        else if (words[1] == "false" || words[1] == "disable" || words[1] == "disabled" || words[1] == "off")
                        {
                            godmode = false;
                        }
                    }
                    else
                    {
                        godmode = !godmode; 
                    }
                    logger.LogInfo("Set godmode to " + godmode);
                    return false;
                case "!fly":
                    if (words.Length > 1)
                    {
                        if (words[1] == "true" || words[1] == "enable" || words[1] == "enabled" || words[1] == "on")
                        {
                            fly = true;
                        }
                        else if (words[1] == "false" || words[1] == "disable" || words[1] == "disabled" || words[1] == "off")
                        {
                            fly = false;
                        }
                    }
                    else
                    {
                        fly = !fly;
                    }
                    return false;
                case "!freecam":
                    if (words.Length > 1)
                    {
                        if (words[1] == "true" || words[1] == "enable" || words[1] == "enabled" || words[1] == "on")
                        {
                            freecam = true;
                        }
                        else if (words[1] == "false" || words[1] == "disable" || words[1] == "disabled" || words[1] == "off")
                        {
                            freecam = false;
                        }
                    }
                    else
                    {
                        freecam = !freecam;
                    }
                    return false;
                case "!range": // Done
                    if (words.Length > 1)
                    {
                        if (words[1] == "true" || words[1] == "enable" || words[1] == "enabled" || words[1] == "on")
                        {
                            infiniteRange = true;
                        }
                        else if (words[1] == "false" || words[1] == "disable" || words[1] == "disabled" || words[1] == "off")
                        {
                            infiniteRange = false;
                        }
                    }
                    else
                    {
                        infiniteRange = !infiniteRange;
                    }
                    logger.LogInfo("Set infiniteRange to " + infiniteRange);
                    return false;
                case "!strength": // Done
                    if (words.Length > 1)
                    {
                        if (words[1] == "true" || words[1] == "enable" || words[1] == "enabled" || words[1] == "on")
                        {
                            infiniteStrength = true;
                        }
                        else if (words[1] == "false" || words[1] == "disable" || words[1] == "disabled" || words[1] == "off")
                        {
                            infiniteStrength = false;
                        }
                    }
                    else
                    {
                        infiniteStrength = !infiniteStrength;
                    }
                    return false;
                case "!toomuchstrength": // :)
                    if (!trueInfiniteStrength)
                    {
                        text = "Only if you say please.";
                        if (words.Length > 1)
                        {
                            if (words[1] == "please")
                            {
                                text = "But do you mean it? Say please twice, then.";
                                if (words[2] == "please")
                                {
                                    text = "Ok, but are you worthy? Say my name.";
                                }
                            }
                            if (words[1] == "chboo1")
                            {
                                text = "Correct, but I still don't know. What's the best item in Lethal Company?";
                            }
                            if (words[1] == "hairdryer")
                            {
                                text = "Ok, I'll do it. Just say \"doit\"";
                            }
                            if (words[1] == "doit")
                            {
                                trueInfiniteStrength = true;
                            }
                        }
                    }
                    else
                    {
                        switch (annoyingCount)
                        {
                            case 0:
                                text = "I'm not turning it off.";
                                break;
                            case 1:
                                text = "I said I'm not turning it off.";
                                break;
                            case 2:
                                text = "You made your choice already.";
                                break;
                            case 3:
                                text = "You need to live with it.";
                                break;
                            case 4:
                                text = "Stop it.";
                                break;
                            case 5:
                                text = "I mean it.";
                                break;
                            case 6:
                                text = "One more chance.";
                                break;
                            case 7:
                                UnityEngine.Application.Quit();
                                break;
                        }
                        annoyingCount++;
                    }
                    return true;
                case "!stamina": // Done
                    if (words.Length > 1)
                    {
                        if (words[1] == "true" || words[1] == "enable" || words[1] == "enabled" || words[1] == "on")
                        {
                            infiniteStamina = true;
                        }
                        else if (words[1] == "false" || words[1] == "disable" || words[1] == "disabled" || words[1] == "off")
                        {
                            infiniteStamina = false;
                        }
                    }
                    else
                    {
                        infiniteStamina = !infiniteStamina;
                    }
                    return false;
                case "!nodeath": // Done
                    if (words.Length > 1)
                    {
                        if (words[1] == "true" || words[1] == "enable" || words[1] == "enabled" || words[1] == "on")
                        {
                            nodeath = true;
                        }
                        else if (words[1] == "false" || words[1] == "disable" || words[1] == "disabled" || words[1] == "off")
                        {
                            nodeath = false;
                        }
                    }
                    else
                    {
                        nodeath = !nodeath;
                    }
                    return false;
                case "!weakenemies": // Done
                    if (words.Length > 1)
                    {
                        if (words[1] == "true" || words[1] == "enable" || words[1] == "enabled" || words[1] == "on")
                        {
                            weakEnemy = true;
                        }
                        else if (words[1] == "false" || words[1] == "disable" || words[1] == "disabled" || words[1] == "off")
                        {
                            weakEnemy = false;
                        }
                    }
                    else
                    {
                        weakEnemy = !weakEnemy;
                    }
                    return false;
                case "!battery": // Done
                    if (words.Length > 1)
                    {
                        if (words[1] == "true" || words[1] == "enable" || words[1] == "enabled" || words[1] == "on")
                        {
                            batteryDrain = false;
                        }
                        else if (words[1] == "false" || words[1] == "disable" || words[1] == "disabled" || words[1] == "off")
                        {
                            batteryDrain = true;
                        }
                    }
                    else
                    {
                        batteryDrain = !batteryDrain;
                    }
                    batteryDrain = !batteryDrain;
                    return false;
                case "!boomhammer": // Done :)
                    if (words.Length > 1)
                    {
                        if (words[1] == "true" || words[1] == "enable" || words[1] == "enabled" || words[1] == "on")
                        {
                            verySqueakyHammer = true;
                        }
                        else if (words[1] == "false" || words[1] == "disable" || words[1] == "disabled" || words[1] == "off")
                        {
                            verySqueakyHammer = false;
                        }
                    }
                    else
                    {
                        verySqueakyHammer = !verySqueakyHammer;
                    }
                    return false;
                case "!notarget": // Done
                    if (words.Length > 1)
                    {
                        if (words[1] == "true" || words[1] == "enable" || words[1] == "enabled" || words[1] == "on")
                        {
                            noTarget = true;
                        }
                        else if (words[1] == "false" || words[1] == "disable" || words[1] == "disabled" || words[1] == "off")
                        {
                            noTarget = false;
                        }
                    }
                    else
                    {
                        noTarget = !noTarget;
                    }
                    return false;
                case "!invisible": // Done
                    if (words.Length > 1)
                    {
                        if (words[1] == "true" || words[1] == "enable" || words[1] == "enabled" || words[1] == "on")
                        {
                            invisible = true;
                        }
                        else if (words[1] == "false" || words[1] == "disable" || words[1] == "disabled" || words[1] == "off")
                        {
                            invisible = false;
                        }
                    }
                    else
                    {
                        invisible = !invisible;
                    }
                    return false;
                case "!quiet": // Done
                    if (words.Length > 1)
                    {
                        if (words[1] == "true" || words[1] == "enable" || words[1] == "enabled" || words[1] == "on")
                        {
                            quiet = true;
                        }
                        else if (words[1] == "false" || words[1] == "disable" || words[1] == "disabled" || words[1] == "off")
                        {
                            quiet = false;
                        }
                    }
                    else
                    {
                        quiet = !quiet;
                    }
                    return false;
                case "!infinitemoney": // Done
                    if (words.Length > 1)
                    {
                        if (words[1] == "true" || words[1] == "enable" || words[1] == "enabled" || words[1] == "on")
                        {
                            infiniteMoney = true;
                        }
                        else if (words[1] == "false" || words[1] == "disable" || words[1] == "disabled" || words[1] == "off")
                        {
                            infiniteMoney = false;
                        }
                    }
                    else
                    {
                        infiniteMoney = !infiniteMoney;
                    }
                    return false;
                case "!setmap": // Done
                    if (words.Length > 1)
                    {
                        string level = words[1];
                        if (words[1] == "manor")
                        {
                            level = "Manor";
                        }
                        else if (words[1] == "station" || words[1] == "arctic")
                        {
                            level = "Arctic";
                        }
                        else if (words[1] == "school" || words[1] == "academy" || words[1] == "wizard")
                        {
                            level = "Wizard";
                        }
                        foreach (Level levelfile in RunManager.instance.levels)
                        {
                            if (levelfile.ResourcePath == level)
                            {
                                savedLevel = levelfile;
                                break;
                            }
                        }
                    }
                    return false;
                case "!setlevel": // Done
                    int newLevel = 0;
                    if (words.Length > 1 && int.TryParse(words[1], out newLevel))
                    {
                        RunManager.instance.levelsCompleted = newLevel - 1; // -1 because levels completed, not level at
                    }
                    return false;
                case "!nobreak": // Done
                    if (words.Length > 1)
                    {
                        if (words[1] == "true" || words[1] == "enable" || words[1] == "enabled" || words[1] == "on")
                        {
                            itemDamage = false;
                        }
                        else if (words[1] == "false" || words[1] == "disable" || words[1] == "disabled" || words[1] == "off")
                        {
                            itemDamage = true;
                        }
                    }
                    else
                    {
                        itemDamage = !itemDamage;
                    }
                    return false;
                case "!notraps":
                    if (words.Length > 1)
                    {
                        if (words[1] == "true" || words[1] == "enable" || words[1] == "enabled" || words[1] == "on")
                        {
                            doTraps = false;
                        }
                        else if (words[1] == "false" || words[1] == "disable" || words[1] == "disabled" || words[1] == "off")
                        {
                            doTraps = true;
                        }
                    }
                    else
                    {
                        doTraps = !doTraps;
                    }
                    return false;
                case "!spawn":{ // Done
                    if (words.Length <= 1)
                    {
                        text = "Must provide a thing to spawn!";
                        return true;
                    }
                    int amount = 1;
                    if (PlayerController.instance == null)
                    {
                        text = "No player controller exists! Either you're not yet in a game or something is broken!";
                        return true;
                    }
                    if (words.Length > 2)
                    {
                        if (!int.TryParse(words[2], out amount) || amount <= 0)
                        {
                            text = "Second argument must be a non-zero positive integer; the amount to spawn! Using 1.";
                            amount = 1;
                        }
                    }
                    if (words.Length > 1)
                    {
                        if (words[1] == "enemy")
                        {
                            EnemySetup esetup = null;
                            try
                            {
                                    esetup = enemySpawnerKeys[words[2]];
                            }
                            catch (KeyNotFoundException)
                            {
                                if (words[2] == "dredge")
                                {
                                    text = "It's called 'trudge' you dingus";
                                    return true;
                                }
                                esetup = null;
                            }
                            if (esetup == null)
                            {
                                text = "Invalid or null enemy to spawn!";
                                return true;
                            }
                            spawning = true;
                            AccessTools.Field(typeof(LevelGenerator), "EnemiesSpawned").SetValue(LevelGenerator.Instance, -1); // If you don't do this, every enemy that spawns will duplicate players in every enemy's list.
                            if (esetup.spawnObjects.Count == 1)
                            {
                                    for (int i = 0; i < amount; i++)
                                    {
                                        UnityEngine.GameObject go = UnityEngine.GameObject.Instantiate(esetup.spawnObjects[0], PlayerController.instance.transform.position, Quaternion.identity);
                                        EnemyParent eparent = go.GetComponent<EnemyParent>();
                                        if (eparent != null)
                                        {
                                            AccessTools.Field(typeof(EnemyParent), "SetupDone").SetValue(eparent, true);
                                            go.GetComponentInChildren<Enemy>().EnemyTeleported(PlayerController.instance.transform.position);
                                            EnemyDirector.instance.FirstSpawnPointAdd(eparent);
                                            EnemyDirector.instance.enemiesSpawned.Add(eparent);
                                            foreach (PlayerAvatar player in GameDirector.instance.PlayerList)
                                            {
                                                ((Enemy)AccessTools.Field(typeof(EnemyParent), "Enemy").GetValue(eparent)).PlayerAdded(player.photonView.ViewID);
                                            }
                                        }

                                    }
                            }
                            else if (esetup.spawnObjects.Count > 1)
                            {
                                    UnityEngine.GameObject.Instantiate(esetup.spawnObjects[0], PlayerController.instance.transform.position, Quaternion.identity);
                                    for (int i = 0; i < amount; i++)
                                    {
                                        UnityEngine.GameObject go = UnityEngine.GameObject.Instantiate(esetup.spawnObjects[1], PlayerController.instance.transform.position, Quaternion.identity);
                                        EnemyParent eparent = go.GetComponent<EnemyParent>();
                                        if (eparent != null)
                                        {
                                            AccessTools.Field(typeof(EnemyParent), "SetupDone").SetValue(eparent, true);
                                            go.GetComponentInChildren<Enemy>().EnemyTeleported(PlayerController.instance.transform.position);
                                            EnemyDirector.instance.FirstSpawnPointAdd(eparent);
                                            EnemyDirector.instance.enemiesSpawned.Add(eparent);
                                            foreach (PlayerAvatar player in GameDirector.instance.PlayerList)
                                            {
                                                ((Enemy)AccessTools.Field(typeof(EnemyParent), "Enemy").GetValue(eparent)).PlayerAdded(player.photonView.ViewID);
                                            }
                                    }

                                    }
                            }
                        }
                        if (words[1] == "item")
                        {
                            UnityEngine.Object spawnObject = null;
                            try
                            {
                                    spawnObject = spawnerKeys[words[2]];
                            }
                            catch (KeyNotFoundException)
                            {
                                    spawnObject = null;
                            }
                            if (spawnObject == null)
                            {
                                text = "Invalid or null object to spawn!";
                                return true;
                            }
                            for (int i = 0; i < amount; i++)
                            {
                                UnityEngine.GameObject.Instantiate(spawnObject, PlayerController.instance.transform.position + new Vector3(amount < 50 ? (i * 0.2f) : 0, 0, 0), PlayerController.instance.transform.rotation);
                            }

                        }
                    }
                    return false;
                    }
                case "!upgrade":
                    if (words.Length < 3 && (words.Length < 2 || words[1] != "playercount"))
                    {
                        return true;
                    }
                    switch (words[1])
                    {
                        case "strength":{
                            if (int.TryParse(words[2], out int num))
                            {
                                StatsManager.instance.playerUpgradeStrength[SemiFunc.PlayerGetSteamID(PlayerAvatar.instance)] = num;
                                PlayerAvatar.instance.physGrabber.grabStrength = 1f + 0.2f * num;
                            }
                            return false;}
                        case "range":{
                            if (int.TryParse(words[2], out int num))
                            {
                                StatsManager.instance.playerUpgradeRange[SemiFunc.PlayerGetSteamID(PlayerAvatar.instance)] = num;
                                PlayerAvatar.instance.physGrabber.grabRange = 2.5f + 1f * num;
                            }
                            return false;
                            }
                        case "health":{
                            if (int.TryParse(words[2], out int num))
                            {
                                AccessTools.Field(typeof(PlayerHealth), "health").SetValue(PlayerAvatar.instance.playerHealth, (num - StatsManager.instance.playerUpgradeHealth[SemiFunc.PlayerGetSteamID(PlayerAvatar.instance)]) * 20 + (int)AccessTools.Field(typeof(PlayerHealth), "health").GetValue(PlayerAvatar.instance.playerHealth));
                                AccessTools.Field(typeof(PlayerHealth), "maxHealth").SetValue(PlayerAvatar.instance.playerHealth, 100 + 20 * num);
                                StatsManager.instance.playerUpgradeHealth[SemiFunc.PlayerGetSteamID(PlayerAvatar.instance)] = num;
                            }
                            return false;}
                        case "stamina":{
                            if (int.TryParse(words[2], out int num))
                            {
                                StatsManager.instance.playerUpgradeStamina[SemiFunc.PlayerGetSteamID(PlayerAvatar.instance)] = num;
                                PlayerController.instance.EnergyStart = 40 + 10 * num;

                            }
                            return false;}
                        case "speed":{
                            if (int.TryParse(words[2], out int num))
                            {
                                StatsManager.instance.playerUpgradeSpeed[SemiFunc.PlayerGetSteamID(PlayerAvatar.instance)] = num;
                                PlayerController.instance.SprintSpeed = 5f + num;
                            }
                            return false;}
                        case "playercount":{
                            if (int.TryParse(words[2], out int num))
                            {
                                StatsManager.instance.playerUpgradeMapPlayerCount[SemiFunc.PlayerGetSteamID(PlayerAvatar.instance)] = num;
                            }
                            else
                            {
                                if (words.Length > 2)
                                {
                                    if (words[2] == "true" || words[2] == "enable" || words[2] == "enabled" || words[2] == "on")
                                    {
                                        StatsManager.instance.playerUpgradeMapPlayerCount[SemiFunc.PlayerGetSteamID(PlayerAvatar.instance)] = 1;
                                    }
                                    else if (words[2] == "false" || words[2] == "disable" || words[2] == "disabled" || words[2] == "off")
                                    {
                                        StatsManager.instance.playerUpgradeMapPlayerCount[SemiFunc.PlayerGetSteamID(PlayerAvatar.instance)] = 0;
                                    }
                                }
                                else
                                {
                                    StatsManager.instance.playerUpgradeMapPlayerCount[SemiFunc.PlayerGetSteamID(PlayerAvatar.instance)] = StatsManager.instance.playerUpgradeMapPlayerCount[SemiFunc.PlayerGetSteamID(PlayerAvatar.instance)] != 0 ? 0 : 1;
                                }
                            }
                            AccessTools.Field(typeof(PlayerAvatar), "upgradeMapPlayerCount").SetValue(PlayerAvatar.instance, StatsManager.instance.playerUpgradeMapPlayerCount[SemiFunc.PlayerGetSteamID(PlayerAvatar.instance)]);
                            return false;}
                        case "jump":{
                            if (int.TryParse(words[2], out int num))
                            {
                                StatsManager.instance.playerUpgradeExtraJump[SemiFunc.PlayerGetSteamID(PlayerAvatar.instance)] = num;
                                AccessTools.Field(typeof(PlayerController), "JumpExtra").SetValue(PlayerController.instance, num);
                            }
                            return false;}
                        case "launch":{
                            if (int.TryParse(words[2], out int num))
                            {
                                StatsManager.instance.playerUpgradeLaunch[SemiFunc.PlayerGetSteamID(PlayerAvatar.instance)] = num;
                                AccessTools.Field(typeof(PlayerTumble), "tumbleLaunch").SetValue(PlayerAvatar.instance.tumble, num);
                            }
                            return false;}
                        case "throw":{
                            if (int.TryParse(words[2], out int num))
                            {
                                StatsManager.instance.playerUpgradeThrow[SemiFunc.PlayerGetSteamID(PlayerAvatar.instance)] = num;
                                PlayerAvatar.instance.physGrabber.throwStrength = 0.3f * num;
                            }
                            return false;}
                        default:
                            text = "Invalid upgrade!";
                            return true;
                    }
                /*
                case "!testhallway": // Works
                    if (words.Length > 1)
                    {
                        if (words[1] == "true" || words[1] == "enable" || words[1] == "enabled" || words[1] == "on")
                        {
                            testHallway = true;
                        }
                        else if (words[1] == "false" || words[1] == "disable" || words[1] == "disabled" || words[1] == "off")
                        {
                            testHallway = false;
                        }
                    }
                    else
                    {
                        testHallway = !testHallway;
                    }
                    return false;
                */
            }
            return true;
        }
        public static bool PhysGrabObjectImpactDetector_Break_Prefix(bool ___isEnemy)
        {
            if (itemDamage || ___isEnemy)
            {
                return true;
            }
            return false;
        }
        public static void PlayerHealth_Update_Prefix(ref bool ___godMode, PlayerAvatar ___playerAvatar)
        {
            if ((bool)AccessTools.Field(typeof(PlayerAvatar), "isLocal").GetValue(___playerAvatar))
            {
                ___godMode = godmode;
            }
        }

        public static bool PlayerHealth_Hurt_Prefix(ref int damage, PlayerAvatar ___playerAvatar, int ___health)
        {
            if ((bool)AccessTools.Field(typeof(PlayerAvatar), "isLocal").GetValue(___playerAvatar) && godmode)
            {
                return false;
            }
            if ((bool)AccessTools.Field(typeof(PlayerAvatar), "isLocal").GetValue(___playerAvatar) && nodeath)
            {
                damage = Math.Min(damage, ___health - 1);
                if (damage == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static void ExtractionPoint_Update_Prefix(ExtractionPoint __instance, bool ___isShop)
        {
            if (___isShop)
            {
                if (infiniteMoney)
                {
                    if (previousMoney < 0)
                    {
                        previousMoney = StatsManager.instance.runStats["currency"];
                    }
                    StatsManager.instance.runStats["currency"] = 9999;
                }
                else
                {
                    if (previousMoney >= 0)
                    {
                        StatsManager.instance.runStats["currency"] = previousMoney;
                    }
                    previousMoney = -1;
                }
            }
        }

        public static bool ChatManager_MessageSend_Prefix(ChatManager __instance, string ___chatMessage)
        {
            if (___chatMessage != "")
            {
                return ParseChat(ref ___chatMessage);
            }
            return true;
        }


        public static bool EnemyParent_Despawn_Prefix(float ___SpawnedTimeMin, float ___SpawnedTimeMax, ref float ___SpawnedTimer)
        {
            if (spawning)
            {
                spawning = false;
                ___SpawnedTimer = UnityEngine.Random.Range(___SpawnedTimeMin, ___SpawnedTimeMax);
                return false;
            }
            return true;
        }


        public static bool ChatManager_Update_Prefix(ChatManager __instance, PlayerAvatar ___playerAvatar, List<ChatManager.PossessMessageBatch> ___possessBatchQueue, ChatManager.PossessMessageBatch ___currentBatch)
        {
            AccessTools.Method(typeof(ChatManager), "PossessionActive").Invoke(__instance, null);
            if ((bool)___playerAvatar && (bool)AccessTools.Field(typeof(PlayerAvatar), "isDisabled").GetValue(___playerAvatar) && (___possessBatchQueue.Count > 0 || ___currentBatch != null))
            {
                AccessTools.Method(typeof(ChatManager), "InterruptCurrentPossessBatch").Invoke(__instance, null);
            }
            if (!LevelGenerator.Instance.Generated)
            {
                AccessTools.Method(typeof(ChatManager), "NewLevelResets").Invoke(__instance, null);
                return false;
            }
            AccessTools.Method(typeof(ChatManager), "ImportantFetches").Invoke(__instance, null);
            AccessTools.Method(typeof(ChatManager), "PossessChatCustomLogic").Invoke(__instance, null);
            if (!(bool)AccessTools.Field(typeof(ChatManager), "textMeshFetched").GetValue(__instance) || !(bool)AccessTools.Field(typeof(ChatManager), "localPlayerAvatarFetched").GetValue(__instance))
            {
                return false;
            }
            switch ((ChatManager.ChatState)AccessTools.Field(typeof(ChatManager), "chatState").GetValue(__instance))
            {
                case ChatManager.ChatState.Inactive:
                    AccessTools.Method(typeof(ChatManager), "StateInactive").Invoke(__instance, null);
                    break;
                case ChatManager.ChatState.Active:
                    AccessTools.Method(typeof(ChatManager), "StateActive").Invoke(__instance, null);
                    break;
                case ChatManager.ChatState.Possessed:
                    AccessTools.Method(typeof(ChatManager), "StatePossessed").Invoke(__instance, null);
                    break;
                case ChatManager.ChatState.Send:
                    AccessTools.Method(typeof(ChatManager), "StateSend").Invoke(__instance, null);
                    break;
            }
            AccessTools.Method(typeof(ChatManager), "PossessChatCustomLogic").Invoke(__instance, null);
            FieldInfo spamTimer = AccessTools.Field(typeof(ChatManager), "spamTimer");
            if ((float)spamTimer.GetValue(__instance) > 0f)
            {
                spamTimer.SetValue(__instance, (float)spamTimer.GetValue(__instance) + Time.deltaTime);
            }
            if (SemiFunc.FPSImpulse15() && (bool)AccessTools.Field(typeof(ChatManager), "betrayalActive").GetValue(__instance) && (bool)AccessTools.Field(typeof(RoomVolumeCheck), "inTruck").GetValue(PlayerController.instance.playerAvatarScript.RoomVolumeCheck))
            {
                __instance.PossessCancelSelfDestruction();
            }
            return false;
        }

        public static bool EnemyVision_VisionTrigger_Prefix(int playerID, PlayerAvatar player, EnemyVision __instance)
        {
            if (invisible || noTarget)
            {
                __instance.VisionTriggered[playerID] = false;
                __instance.VisionsTriggered[playerID] = 0;
                return false;
            }
            return true;
        }
        public static bool EnemyDirector_SetInvestigate_Prefix()
        {
            if (quiet || noTarget)
            {
                return false;
            }
            return true;
        }

        public static void PhysGrabber_Update_Prefix(PhysGrabber __instance)
        {
            if (infiniteRange)
            {
                if (previousGrabRange < 0)
                {
                    previousGrabRange = __instance.grabRange;
                }
                __instance.grabRange = 9999f;
                __instance.maxDistanceFromPlayer = 9999f;
            }
            else
            {
                if (previousGrabRange >= 0)
                {
                    __instance.grabRange = previousGrabRange;
                    previousGrabRange = -1f;
                    __instance.maxDistanceFromPlayer = 6f;
                }
            }
            if (infiniteStrength)
            {
                if (previousGrabStrength < 0)
                {
                    previousGrabStrength = __instance.grabStrength;
                }
                __instance.grabStrength = trueInfiniteStrength? 999f : 8f;
            }
            else
            {
                if (previousGrabStrength >= 0)
                {
                    __instance.grabStrength = previousGrabStrength;
                    previousGrabStrength = -1f;
                }
            }
        }


        public static void ItemBattery_Update_Postfix(ItemBattery __instance, ref int ___batteryLifeInt)
        {
            if (!batteryDrain)
            {
                __instance.batteryLife = 100f;
                ___batteryLifeInt = 6;
            }
        }

        public static bool LevelGenerator_Generate_Prefix(ref System.Collections.IEnumerator __result)
        {
            if (enemySpawnerKeys == null)
            {
                enemySpawnerKeys = new Dictionary<string, EnemySetup> ();
                foreach (EnemySetup esetup in EnemyDirector.instance.enemiesDifficulty1)
                {
                    if (esetup.name == "Enemy - Ceiling Eye")
                    {
                        enemySpawnerKeys.Add("peeper", esetup);
                    }
                    if (esetup.name == "Enemy - Duck")
                    {
                        enemySpawnerKeys.Add("duck", esetup);
                        enemySpawnerKeys.Add("apexpredator", esetup);
                    }
                    if (esetup.name == "Enemy - Gnome")
                    {
                        enemySpawnerKeys.Add("gnome", esetup);
                    }
                    if (esetup.name == "Enemy - Slow Mouth")
                    {
                        enemySpawnerKeys.Add("spewer", esetup);
                    }
                    if (esetup.name == "Enemy - Thin Man")
                    {
                        enemySpawnerKeys.Add("shadowchild", esetup);
                    }
                }
                foreach (EnemySetup esetup in EnemyDirector.instance.enemiesDifficulty2)
                {
                    if (esetup.name == "Enemy - Animal")
                    {
                        enemySpawnerKeys.Add("animal", esetup);
                    }
                    if (esetup.name == "Enemy - Bang")
                    {
                        enemySpawnerKeys.Add("banger", esetup);
                    }
                    if (esetup.name == "Enemy - Bowtie")
                    {
                        enemySpawnerKeys.Add("bowtie", esetup);
                    }
                    if (esetup.name == "Enemy - Floater")
                    {
                        enemySpawnerKeys.Add("mentalist", esetup);
                    }
                    if (esetup.name == "Enemy - Hidden")
                    {
                        enemySpawnerKeys.Add("hidden", esetup);
                    }
                    if (esetup.name == "Enemy - Tumbler")
                    {
                        enemySpawnerKeys.Add("chef", esetup);
                    }
                    if (esetup.name == "Enemy - Upscream")
                    {
                        enemySpawnerKeys.Add("upscream", esetup);
                    }
                    if (esetup.name == "Enemy - Valuable Thrower")
                    {
                        enemySpawnerKeys.Add("rugrat", esetup);
                    }
                }
                foreach (EnemySetup esetup in EnemyDirector.instance.enemiesDifficulty3)
                {
                    if (esetup.name == "Enemy - Beamer")
                    {
                        enemySpawnerKeys.Add("clown", esetup);
                    }
                    if (esetup.name == "Enemy - Head")
                    {
                        enemySpawnerKeys.Add("headman", esetup);
                    }
                    if (esetup.name == "Enemy - Robe")
                    {
                        enemySpawnerKeys.Add("robe", esetup);
                    }
                    if (esetup.name == "Enemy - Runner")
                    {
                        enemySpawnerKeys.Add("reaper", esetup);
                    }
                    if (esetup.name == "Enemy - Slow Walker")
                    {
                        enemySpawnerKeys.Add("trudge", esetup);
                    }
                    if (esetup.name == "Enemy - Hunter")
                    {
                        enemySpawnerKeys.Add("huntsman", esetup);
                    }
                }
            }
            if (RunManager.instance.levelCurrent == RunManager.instance.levelMainMenu || RunManager.instance.levelCurrent == RunManager.instance.levelLobbyMenu || RunManager.instance.levelCurrent == RunManager.instance.levelLobby || RunManager.instance.levelCurrent == RunManager.instance.levelRecording || RunManager.instance.levelCurrent == RunManager.instance.levelTutorial || RunManager.instance.levelCurrent == RunManager.instance.levelArena || RunManager.instance.levelCurrent == RunManager.instance.levelShop)
            {
                logger.LogInfo("Skipping");
                return true;
            }
            if (savedLevel != null)
            {
                RunManager.instance.levelCurrent = savedLevel;
                savedLevel = null;
            }
            if (testHallway)
            {
                __result = testHallwayGeneration();
                return false;
            }
            return true;
        }

        public static System.Collections.IEnumerator testHallwayGeneration()
        {
            logger.LogInfo("Executing alternate generation.");
            AccessTools.Field(typeof(LevelGenerator), "AllPlayersReady").SetValue(LevelGenerator.Instance, true);
            LevelGenerator.Instance.Level = RunManager.instance.levelCurrent;
            AccessTools.Field(typeof(RunManager), "levelPrevious").SetValue(RunManager.instance, LevelGenerator.Instance.Level);
            LevelGenerator.Instance.State = LevelGenerator.LevelState.Tiles;
            FieldInfo levelgrid = AccessTools.Field(typeof(LevelGenerator), "LevelGrid");
            LevelGenerator.Tile[,] grid = new LevelGenerator.Tile[1, 100];
            LevelGenerator.Instance.LevelWidth = 1;
            LevelGenerator.Instance.LevelHeight = 100;
            for (int j = 0; j < 100; j++)
            {
                grid[0, j] = new LevelGenerator.Tile { x = 0, y = j, active = false };
            }
            AccessTools.Field(typeof(LevelGenerator), "ModuleAmount").SetValue(LevelGenerator.Instance, 5 + RunManager.instance.levelsCompleted);
            AccessTools.Field(typeof(LevelGenerator), "ExtractionAmount").SetValue(LevelGenerator.Instance, (RunManager.instance.levelsCompleted) >= 5 ? 3 : ((RunManager.instance.levelsCompleted >= 3) ? 2 : ((RunManager.instance.levelsCompleted >= 1) ? 1 : 0)));
            grid[0, 0].active = true;
            grid[0, 0].first = true;
            yield return null;
            for (int j = 0; j < 100; j++)
            {
                grid[0, j].active = true;
                grid[0, j].type = Module.Type.Passage;
            }
            yield return null;
            levelgrid.SetValue(LevelGenerator.Instance, grid);
            LevelGenerator.Instance.State = LevelGenerator.LevelState.StartRoom;
            GameObject startRoomInstance = UnityEngine.Object.Instantiate(LevelGenerator.Instance.Level.StartRooms[UnityEngine.Random.Range(0, LevelGenerator.Instance.Level.StartRooms.Count)]);
            startRoomInstance.transform.parent = LevelGenerator.Instance.LevelParent.transform;
            FieldInfo waitingCoroutine = AccessTools.Field(typeof(LevelGenerator), "waitingForSubCoroutine");
            while ((bool)waitingCoroutine.GetValue(LevelGenerator.Instance))
            {
                yield return null;
            }
            LevelGenerator.Instance.State = LevelGenerator.LevelState.ModuleGeneration;
            yield return null;
            for (int j = 0; j < 100; j++)
            {
                GameObject spawnedModule = UnityEngine.Object.Instantiate(LevelGenerator.Instance.Level.ModulesPassage1[0], new Vector3(0, 0f, (float)j * 15 + 7.5f), Quaternion.Euler(Vector3.zero));
                spawnedModule.transform.parent = LevelGenerator.Instance.LevelParent.transform;
                Module spawnedModuleRealModule = spawnedModule.GetComponent<Module>();
                AccessTools.Field(typeof(Module), "GridX").SetValue(spawnedModuleRealModule, 0);
                AccessTools.Field(typeof(Module), "GridY").SetValue(spawnedModuleRealModule, j);
                AccessTools.Field(typeof(Module), "First").SetValue(spawnedModuleRealModule, grid[0, j].first);
                if (j != 99)
                {
                    AccessTools.Field(typeof(Module), "ConnectingTop").SetValue(spawnedModuleRealModule, true);
                }
                AccessTools.Field(typeof(Module), "ConnectingBottom").SetValue(spawnedModuleRealModule, true);
                if (j%10 == 0)
                {
                    yield return null;
                }
            }
            yield return null;
            LevelGenerator.Instance.StartCoroutine("GenerateBlockObjects");
            while ((bool)waitingCoroutine.GetValue(LevelGenerator.Instance))
            {
                yield return null;
            }
            FieldInfo modulesSpawnedCount = AccessTools.Field(typeof(LevelGenerator), "ModulesSpawned");
            while ((int)modulesSpawnedCount.GetValue(LevelGenerator.Instance) < 100)
            {
                LevelGenerator.Instance.State = LevelGenerator.LevelState.ModuleSpawnLocal;
                yield return new WaitForSeconds(0.1f);
            }
            EnvironmentDirector.Instance.Setup();
            logger.LogInfo(LevelGenerator.Instance);
            logger.LogInfo(LevelGenerator.Instance.Level);
            PostProcessing.Instance.Setup();
            LevelMusic.instance.Setup();
            ConstantMusic.instance.Setup();
            LevelGenerator.Instance.ItemSetup();
            LevelGenerator.Instance.NavMeshSetup();
            LevelGenerator.Instance.PlayerSpawn();
            LevelGenerator.Instance.State = LevelGenerator.LevelState.Done;
            AccessTools.Method(typeof(LevelGenerator), "GenerateDone").Invoke(LevelGenerator.Instance, new object[] { });
            SessionManager.instance.CrownPlayer();
        }

        public static void RunManager_Update_Prefix()
        {
            if (infiniteStamina && PlayerController.instance != null)
            {
                PlayerController.instance.EnergyCurrent = PlayerController.instance.EnergyStart;
            }
        }


        public static void EnemyHealth_Hurt_Prefix(ref int _damage, int ___healthCurrent)
        {
            if (weakEnemy)
            {
                _damage = ___healthCurrent;
            }
        }


        public static bool ItemMeleeInflatableHammer_OnHit_Prefix(ItemMeleeInflatableHammer __instance, PhotonView ___photonView)
        {
            if (verySqueakyHammer)
            {
                if (SemiFunc.IsMultiplayer())
                {
                    ___photonView.RPC("ExplosionRPC", RpcTarget.All);
                }
                else
                {
                    __instance.ExplosionRPC();
                }
                return false;
            }
            return true;
        }

        public static void Enemy_Update_Prefix(ref PlayerAvatar ___TargetPlayerAvatar) {if (noTarget) { ___TargetPlayerAvatar = null; } }
        public static void EnemyAnimal_Update_Prefix(ref PlayerAvatar ___playerTarget) { if (noTarget) { ___playerTarget = null; } }
        public static void EnemyBangDirector_Update_Prefix(ref PlayerAvatar ___playerTarget) { if (noTarget) { ___playerTarget = null; } }
        public static void EnemyBeamer_Update_Prefix(ref PlayerAvatar ___playerTarget) { if (noTarget) { ___playerTarget = null; } }
        public static void EnemyBowtie_Update_Prefix(ref PlayerAvatar ___playerTarget) { if (noTarget) { ___playerTarget = null; } }
        public static void EnemyDuck_Update_Prefix(ref PlayerAvatar ___playerTarget) { if (noTarget) { ___playerTarget = null; } }
        public static void EnemyGnomeDirector_Update_Prefix(ref PlayerAvatar ___playerTarget) { if (noTarget) { ___playerTarget = null; } }
        public static void EnemyHidden_Update_Prefix(ref PlayerAvatar ___playerTarget) { if (noTarget) { ___playerTarget = null; } }
        public static void EnemySlowMouth_Update_Prefix(ref PlayerAvatar ___playerTarget) { if (noTarget) { ___playerTarget = null; } }
        public static void EnemyThinMan_Update_Prefix(ref PlayerAvatar ___playerTarget) { if (noTarget) { ___playerTarget = null; } }
        public static void EnemyValuableThrower_Update_Prefix(ref PlayerAvatar ___playerTarget) { if (noTarget) { ___playerTarget = null; } }
        public static void EnemyRobe_Update_Prefix(ref PlayerAvatar ___targetPlayer) { if (noTarget) { ___targetPlayer = null; } }
        public static void EnemyCeilingEye_Update_Prefix(ref PlayerAvatar ___targetPlayer) { if (noTarget) { ___targetPlayer = null; } }
        public static void EnemyFloater_Update_Prefix(ref PlayerAvatar ___targetPlayer) { if (noTarget) { ___targetPlayer = null; } }
        public static void EnemyUpscream_Update_Prefix(ref PlayerAvatar ___targetPlayer) { if (noTarget) { ___targetPlayer = null; } }
        public static void EnemySlowWalker_Update_Prefix(ref PlayerAvatar ___targetPlayer) { if (noTarget) { ___targetPlayer = null; } }
        public static void EnemyTumbler_Update_Prefix(ref PlayerAvatar ___targetPlayer) { if (noTarget) { ___targetPlayer = null; } }
        public static void EnemyRunner_Update_Prefix(ref PlayerAvatar ___targetPlayer) { if (noTarget) { ___targetPlayer = null; } }
    }
}
