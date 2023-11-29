using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NodeDescriptions
{ 
  public static string GetNodeDescription(string name)
    {
        string description = "No description availible.";

        if (name == "custom_node")
        {
            description =
            "Custom Node \n " +
            "\n " +
            "Use this node to access a custom mission function that has no node. \n";
        }
        else if (name == "loadscene")
        {
            description =
            "Load Scene \n " +
            "\n " +
            "This node is essential. It tells OG that you want to load a scene in a particular location. \n" +
            "\n " +
            "Extra Information \n" +
            "- If the location is not found (or you leave the location as *none*) OG will load a random location \n" +
            "- Look for a list of all avaible locations linked on the OG Github page. \n" +
            "- This node does not need to be linked to any others. OG will automatically search for and run this event. \n";
        }
        else if (name == "preload_loadasteroids")
        {
            description =
            "Pre-Load Asteroids \n " +
            "\n " +
            "This node loads asteroids in the scene. \n" +
            "\n " +
            "Extra Information \n" +
            "- If you want the asteroid number to be set by the location seed leave number as 'none' or '0'. \n" +
            "- Remove the node if you dont want any asteroids in the scene \n" +
            "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order before the mission starts. \n";
        }
        else if (name == "preload_loadplanet")
        {
           description =
           "Pre-Load Planet \n " +
           "\n " +
           "This event tells the scene to load the planet that is at the location defined in the loadscene event. \n" +
           "\n " +
           "Extra Information \n" +
           "- Remove this node if you want to load a scene without a planet... i.e. Alderaan \n" +
           "- No data is need for this event. OG simply looks to see if it is there or not. \n" +
           "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order before the mission starts. \n";
        }
        else if (name == "preload_loadtiles")
        {
            description =
           "Pre-Load Tiles \n " +
           "\n " +
           "This event tells the scene to load tiles to make a planet or death star surface in the scene \n" +
           "\n " +
           "Extra Information \n" +
           "- The number of tiles is determined by the length, width, and tile size. i.e. Tiles that are 500 long, with a width and length of 1000 will generate 4 tiles, the same tiles with a width and length of 2000 will generate 12 and so on... \n" +
           "- Too many tiles in the scene will cause long load times and slow gameplay. \n" +
           "- If the tile seed is set to 'none' the tiles will load in a completely randomised order each time the mission is loaded. \n" +
           "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order before the mission starts. \n";
        }
        else if (name == "preload_loadmultipleshipsonground")
        {
            description =
            "Pre-Load Multiple Ships on Ground \n " +
            "\n " +
            "This loads multiple ships on ground, usually turrets, but can also be used to place normal ships as well \n" +
            "\n " +
            "Extra Information \n" +
            "- If you write 'random' or 'randomise' for the cargo the game will automatically randomise the ships cargo using preset list \n" +
            "- For this node to work you need to also use the load tiles event node. \n" +
            "- You can force the function to load without hitting a tile by using the 'if raycast fails still load function'. \n" +
            "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order before the mission starts. \n";
        }
        else if (name == "preload_loadsingleship")
        {
            description =
           "Pre-Load Single Ship \n " +
           "\n " +
           "This node loads a single ship \n" +
           "\n " +
           "Extra Information \n" +
           "- If you write 'random' or 'randomise' for the cargo the game will automatically randomise the ships cargo using preset list \n" +
           "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order before the mission starts. \n";
        }
        else if (name == "preload_loadmultipleships")
        {
            description =
            "Pre-Load Multiple Ships \n " +
            "\n " +
            "This node loads multiple ships of the same type \n" +
            "\n " +
            "Extra Information \n" +
            "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order before the mission starts. \n" +
            "- If you write 'random' or 'randomise' for the cargo the game will automatically randomise the ships cargo using preset list \n" +
            "- Pattern: rectanglehorizontal uses with and length. Height is ignored. \n" +
            "- Pattern: rectanglevertical uses width and height. Length is ignored. \n" +
            "- Pattern: arrowhorizontal uses with and length. Height is ignored. \n" +
            "- Pattern: arrowhorizontalinverted uses with and length. Height is ignored. \n" +
            "- Pattern: linehorizontallongways uses length. Width and height are ignored. \n" +
            "- Pattern: linehorizontalsideways uses width. Height and length are ignored. \n" +
            "- Pattern: linevertical uses height. Width and length are ignored. \n" +
            "- Pattern: randominsidecube uses width, length, height. \n";
        }
        else if (name == "preload_loadmultipleshipsbyclassandallegiance")
        {
            description =
            "Pre-Load Multiple Ships By Type \n " +
            "\n " +
            "This node loads multiple ships of the same class and allegiance. For example you can load 10 'imperial' (allegiance) 'fighters' (class). This will load a random selection of tie fighter, interceptors, and other ships in the fighter class. \n" +
            "\n " +
            "Extra Information \n" +
            "- Tie Bombers, Assault Gunboats, and Y-Wings are classed both as bombers and fighters. \n" +
            "- If you write 'random' or 'randomise' for the cargo the game will automatically randomise the ships cargo using preset list \n" +
            "- Pattern: rectanglehorizontal uses with and length. Height is ignored. \n" +
            "- Pattern: rectanglevertical uses width and height. Length is ignored. \n" +
            "- Pattern: arrowhorizontal uses with and length. Height is ignored. \n" +
            "- Pattern: arrowhorizontalinverted uses with and length. Height is ignored. \n" +
            "- Pattern: linehorizontallongways uses length. Width and height are ignored. \n" +
            "- Pattern: linehorizontalsideways uses width. Height and length are ignored. \n" +
            "- Pattern: linevertical uses height. Width and length are ignored. \n" +
            "- Pattern: randominsidecube uses width, length, height. \n" +
            "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order before the mission starts. \n";
        }
        else if (name == "preload_setskybox")
        {
            description =
           "Pre-Load Set Skybox \n " +
           "\n " +
           "This sets the skybox to space or sky. \n" +
           "\n " +
           "Extra Information \n" +
           "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order before the mission starts. \n";
        }
        else if (name == "starteventseries")
        {
            description =
           "Start Event Series \n " +
           "\n " +
           "This is the first node in an event series. OG looks for this node to tell it which event to run first. \n" +
           "\n " +
           "Extra Information \n" +
           "- All standard event nodes must be linked in a chain beginning with this a 'startevenseries' node to run. \n" +
           "- You can have as many event series as you want. \n" +
           "- This node is particulary helpful for creating different series of events for primary and secondary objectives. \n" +
           "- This node is not required for pre-event nodes. \n";
        }
        else if (name == "activatehyperspace")
        {
            description =
            "Activate Hyperspace \n " +
            "\n " +
            "This node causes a ship or ships to jump to hyperspace" +
            "\n " +
           "Extra Information \n" +
            "- This function will affect any ship whose name contains the designated string. \n";
        }
        else if (name == "clearaioverride")
        {
            description =
           "Clear AI Override \n " +
           "\n " +
           "This is node clears all ai overrides on the selected craft \n" +
           "\n " +
           "Extra Information \n" +
           "- An ai override forces a ship to deviate from its standard behaviour to perform only one task. \n" +
           "- For a ship to return to its standard behavious the AI override must be cleared. \n" +
           "- AI overrides inlcude: 'MoveToWayPoint', 'Stationary', 'Patrol'. \n" +
           "- This function will affect any ship whose name contains the designated string. \n";
        }
        else if (name == "exitmission")
        {
            description =
           "Display Large Mission Then Exit \n " +
           "\n " +
           "This node exits the mission and returns the player to the main menu. \n";
        }
        else if (name == "displaydialoguebox")
        {
            description =
           "Display Dialogue Box \n " +
           "\n " +
           "This node pauses the game and displays a dialogue box with a message. When the button on the dialogue box is pressed the box disappears and the game resumes. \n";
        }
        else if (name == "displaylargemessage")
        {
            description =
            "Display Large Messgage \n " +
            "\n " +
            "This node displays a large message in the middle of the screen, like 'MISSION COMPLETE' or similar for a short period of time. \n";
        }
        else if (name == "displaylocation")
        {
            description =
            "Display Location \n " +
            "\n " +
            "This node displays the current location to the player as a Large Message for a short period of time. \n" +
            "\n " +
            "Extra Information \n" +
            "- You can add text before or after the location like 'BATTLE OF' or similar. \n" +
            "- This node is particulary helpful when you use a radomised location, but want to display it to the player. \n";
        }
        else if (name == "displaymessage")
        {
            description =
            "Display Message \n " +
            "\n " +
            "This node sends an in game message i.e. 'Rouge 01: I'm starting my attack run now.' \n" +
            "\n " +
            "Extra Information \n" +
            "- You can link an audio file (.wav) to run at the same time the message is sent. Simply make a folder in the custom missions folder. Name the folder 'YOURMISSIONNAME_audio'. Paste your audio file in the folder, type the name of the audio file (without the .wav extension) in the designated area on the node, and select the 'External Audio' option. \n" +
            "- You can also access several inbuilt audio files. Use the names listed below and select the option 'Internal Audio'. \n" +
            "- Internal Audio Files: beep01_toggle, beep02_targetlock, beep03_weaponchange, engine01_tiefighter, engine02_xwing, explosion01_xwing, explosion02_protontorpedo, impact01_laserhitshield, impact02_unshieldedhit, impact03_crash, r2d2, r5, shaking01, Turbolaser, turbolaser_ac, turbolaser_ac_3p, turbolaser_af, turbolaser_af_3p, weapon01_tiefighterlaser, weapon02_xwinglaser, weapon03_protontorpedo ";
        }
        else if (name == "displaymissionbriefing")
        {
            description =
            "Display Mission Brieding \n " +
            "\n " +
            "This node pauses the game and displays a mission briefing screen with a message. \n" +
            "\n " +
            "Extra Information \n" +
            "- At the moment you can only add a message to the briefing screen. \n" +
            "- WARNING: This node is under development. Expect it to change over time. \n";
        }
        else if (name == "ifshipshullislessthan")
        {
            description =
             "If Ships Hull is Less Than \n " +
             "\n " +
             "This function checks the hull of the designated ship and returns true or false according to whether the ship hull is less or more than the designated amount. \n" +
             "\n " +
             "Extra Information \n" +
             "- This is a branching node. You can intiate a different set of events depending on whether the answer is yes or no. \n" +
             "- This node will return a result on the first ship that contains the given text. For example, if you have two ships one named 'Container A' and the other 'Container B' and you simply write 'Container' the node will return a result on the first of the two ships it checks. \n";
        }
        else if (name == "ifshipislessthandistancetoothership")
        {
            description =
             "If Ship Is Less Than Distance to Other Ship \n " +
             "\n " +
             "This function checks the distance between two ships and returns true or false according to whether the ship's distance is less or more than the designated amount. \n" +
             "\n " +
             "Extra Information \n" +
             "- This is a branching node. You can intiate a different set of events depending on whether the answer is yes or no. \n" +
             "- This node will return a result on the first ship that contains the given text. For example, if you have two ships one named 'Container A' and the other 'Container B' and you simply write 'Container' the node will return a result on the first of the two ships it checks. \n";
        }
        else if (name == "ifshipislessthandistancetowaypoint")
        {
            description =
             "If Ship Is Less Than Distance to Waypoint \n " +
             "\n " +
             "This function checks the distance to waypoint of the designated ship and returns true or false according to whether the ship's distance is less or more than the designated amount. \n" +
             "\n " +
             "Extra Information \n" +
             "- This is a branching node. You can intiate a different set of events depending on whether the answer is yes or no. \n" +
             "- This node will return a result on the first ship that contains the given text. For example, if you have two ships one named 'Container A' and the other 'Container B' and you simply write 'Container' the node will return a result on the first of the two ships it checks. \n";
        }
        else if (name == "ifshipisactive")
        {
            description =
            "If Ship is Active \n " +
            "\n " +
            "This node checks to see whether a particular ship is active or not. \n" +
            "\n " +
            "Extra Information \n" +
            "- This node will return a result on the first ship that contains the given text. For example, if you have two ships one named 'Container A' and the other 'Container B' and you simply write 'Container' the node will return a result on the first of the two ships it checks. \n" +
            "- This string is case sensitive. If you write 'container' and you are looking for 'Container' the function wont find it. \n" +
            "- This is a branching node. You can intiate a different set of events depending on whether the answer is yes or no. \n";
        }
        else if (name == "ifshiphasbeenscanned")
        {
            description =
            "If Ship has been scanned \n " +
            "\n " +
            "This node checks to see whether a particular ship has been scanned or not. \n" +
            "\n " +
            "Extra Information \n" +
            "- This node will return a result on the first ship that contains the given text. For example, if you have two ships one named 'Container A' and the other 'Container B' and you simply write 'Container' the node will return a result on the first of the two ships it checks. \n" +
            "- This string is case sensitive. If you write 'container' and you are looking for 'Container' the function wont find it. \n" +
            "- This is a branching node. You can intiate a different set of events depending on whether the answer is yes or no. \n";
        }
        else if (name == "ifshiphasntbeenscanned")
        {
            description =
            "If Ship hasn't been scanned \n " +
            "\n " +
            "This node checks to see whether a ship hasn't been scanned or not. \n" +
            "\n " +
            "Extra Information \n" +
            "- This node will return a result on the first ship it finds that hasn't been scanned. For example if you type 'Container', the node will check all ships that have 'Container' in the name. If it finds one that hasn't been scanned it returns true. \n" +
            "- This node is useful for checking if a player has finished scanning a group of ships/containers with a similar name. \n" +
            "- This string is case sensitive. If you write 'container' and you are looking for 'Container' the function wont find it. \n" +
            "- This is a branching node. You can intiate a different set of events depending on whether the answer is yes or no. \n";
        }
        else if (name == "iftypeofshipisactive")
        {
            description =
            "If Type of Ship is Active \n " +
            "\n " +
            "This node checks to see whether there are any ships active of a particular allegiance i.e. whether there are any imperial ships left in the scene. \n" +
            "\n " +
            "Extra Information \n" +
            "- This is a branching node. You can intiate a different set of events depending on whether the answer is yes or no. \n" +
            "- WARNING: The name of this node may change to accurately represent the function it runs. \n";
        }
        else if (name == "loadsingleship")
        {
            description =
            "Single Ship \n " +
            "\n " +
            "This node loads a single ship \n" +
            "\n " +
            "Extra Information \n" +
            "- If you write 'random' or 'randomise' for the cargo the game will automatically randomise the ships cargo using preset list \n";
        }
        else if (name == "loadsingleshipatdistanceandanglefromplayer")
        {
            description =
            "Load Single Ship at Distance and Angle From Player \n " +
            "\n " +
            "This node loads a single ship at the designated distance and angle from the player. \n" +
            "\n " +
            "Extra Information \n" +
            "- If you write 'random' or 'randomise' for the cargo the game will automatically randomise the ships cargo using preset list \n" +
            "- The angles are euler i.e. 360 degrees beginning at the from of the ship. \n" +
            "- This not is helpful if you want to deliberately load an enemy ship behind the player for example. \n";
        }
        else if (name == "loadmultipleshipsonground")
        {
            description =
            "Load Multiple Ships on Ground \n " +
            "\n " +
            "This loads multiple ships on ground, usually turrets, but can also be used to place normal ships as well \n" +
            "\n " +
            "Extra Information \n" +
            "- If you write 'random' or 'randomise' for the cargo the game will automatically randomise the ships cargo using preset list \n" +
            "- For this node to work you need to also use the load tiles event node. \n" +
            "- You can force the function to load without hitting a tile by using the 'if raycast fails still load function'. \n";
        }
        else if (name == "loadmultipleships")
        {
            description =
            "Pre-Load Multiple Ships \n " +
            "\n " +
            "This node loads multiple ships of the same type \n" +
            "\n " +
            "Extra Information \n" +
            "- Tie Bombers, Assault Gunboats, and Y-Wings are classed both as bombers and fighters. \n" +
            "- If you write 'random' or 'randomise' for the cargo the game will automatically randomise the ships cargo using preset list \n" +
            "- Pattern: rectanglehorizontal uses with and length. Height is ignored. \n" +
            "- Pattern: rectanglevertical uses width and height. Length is ignored. \n" +
            "- Pattern: arrowhorizontal uses with and length. Height is ignored. \n" +
            "- Pattern: arrowhorizontalinverted uses with and length. Height is ignored. \n" +
            "- Pattern: linehorizontallongways uses length. Width and height are ignored. \n" +
            "- Pattern: linehorizontalsideways uses width. Height and length are ignored. \n" +
            "- Pattern: linevertical uses height. Width and length are ignored. \n" +
            "- Pattern: randominsidecube uses width, length, height. \n";
        }
        else if (name == "loadmultipleshipsbyclassandallegiance")
        {
            description =
            "Pre-Load Multiple Ships By Type \n " +
            "\n " +
            "This node loads multiple ships of the same class and allegiance. For example you can load 10 'imperial' (allegiance) 'fighters' (class). This will load a random selection of tie fighter, interceptors, and other ships in the fighter class. \n" +
            "\n " +
            "Extra Information \n" +
            "- Tie Bombers, Assault Gunboats, and Y-Wings are classed both as bombers and fighters. \n" +
            "- If you write 'random' or 'randomise' for the cargo the game will automatically randomise the ships cargo using preset list \n" +
            "- Pattern: rectanglehorizontal uses with and length. Height is ignored. \n" +
            "- Pattern: rectanglevertical uses width and height. Length is ignored. \n" +
            "- Pattern: arrowhorizontal uses with and length. Height is ignored. \n" +
            "- Pattern: arrowhorizontalinverted uses with and length. Height is ignored. \n" +
            "- Pattern: linehorizontallongways uses length. Width and height are ignored. \n" +
            "- Pattern: linehorizontalsideways uses width. Height and length are ignored. \n" +
            "- Pattern: linevertical uses height. Width and length are ignored. \n" +
            "- Pattern: randominsidecube uses width, length, height. \n" +
            "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order before the mission starts. \n";
        }
        else if (name == "setwaypoint")
        {
            description =
            "Set Waypoint \n " +
            "\n " +
            "Allows you to set the position of the ships waypoint \n" +
            "\n " +
            "Extra Information \n" +
            "- Every ship has a waypoint. Settings its position can be useful for a number of purposes. \n" +
            "- You can check a ships distance to its waypoint using the 'ifshipislessthandistancetowaypoint' function. \n" +
            "- You can direct an ai ship to fly toward its waypoint by setting the ai override to 'movetowaypoint'. \n" +
            "- This function will affect any ship whose name contains the designated string. \n";
        }
        else if (name == "setaioverride")
        {
            description =
           "Set AI Override \n " +
           "\n " +
           "This function sets an AI override on the designated ship. \n" +
           "\n " +
           "Extra Information \n" +
           "- This function will affect any ship whose name contains the designated string. \n" +
           "- This is an ai override. An ai override forces a ship to deviate from its standard behaviour to perform only one task (move to waypoint). For a ship to return to its standard behavious the AI override must be cleared using the clearaioverride function. \n" +
           "- AI overrides inlcude: 'MoveToWayPoint', 'Stationary', 'Patrol'. \n";
        }
        else if (name == "setcargo")
        {
            description =
           "Set Cargo \n " +
           "\n " +
           "This function sets the ships cargo. \n" +
           "\n " +
           "Extra Information \n" +
           "- This function will affect any ship whose name contains the designated string. \n" +
           "- This function is particulary useful when making group of ships where you want one ship to have a specific cargo. \n" +
           "- If you write 'random' or 'randomise' the game will automatically randomise the ships cargo using preset list \n";
        }
        else if (name == "setdontattacklargeships")
        {
            description =
           "Set Dont Attack Large Ships \n " +
           "\n " +
           "This function tells the designated small ships to not attack large ships \n" +
           "\n " +
           "Extra Information \n" +
           "- This function will affect any ship whose name contains the designated string. \n";
        }
        else if (name == "setmusictrack")
        {
            description =
            "Set Music Track \n " +
            "\n " +
            "This function plays the selected track on repeat. \n" +
            "\n " +
            "Extra Information \n" +
            "- This node will fade out the current track while fading in the new track. \n";
        }
        else if (name == "setshipallegiance")
        {
            description =
           "Set Ship Allegiance \n " +
           "\n " +
           "This function changes the designated ship to the designated allegiance \n" +
           "\n " +
           "Extra Information \n" +
           "- This function will affect any ship whose name contains the designated string. \n";
        }
        else if (name == "setshiptarget")
        {
            description =
            "Set Ship Target \n " +
            "\n " +
            "This function sets the designated ship to the designated target. \n" +
            "\n " +
            "Extra Information \n" +
            "- This function will affect any ship whose name contains the designated string. \n";
        }
        else if (name == "setshiptargettoclosestenemy")
        {
            description =
            "Set Ship Target To Closest Enemy \n " +
            "\n " +
            "This function sets the designated ship target to the closest enemy target. \n" +
            "\n " +
            "Extra Information \n" +
            "- This function will affect any ship whose name contains the designated string. \n";
        }
        else if (name == "setshiptoinvincible")
        {
            description =
             "Set Ship To Invincible \n " +
             "\n " +
             "This function sets the designated ship to invincible. \n" +
             "\n " +
             "Extra Information \n" +
             "- A ship that is set to invincible will still receive damage until it only has 5 points of health left. \n" +
             "- This function will affect any ship whose name contains the designated string. \n";
        }
        else if (name == "setweaponslock")
        {
            description =
             "Set Weapon Lock \n " +
             "\n " +
             "This function locks the weapons on the designated ship which prevents the ship from firing its weapons. \n" +
             "\n " +
             "Extra Information \n" +
             "- This function will affect any ship whose name contains the designated string. \n";
        }

        return description;
    }
}
