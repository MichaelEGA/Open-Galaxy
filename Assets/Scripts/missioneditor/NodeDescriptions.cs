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
        else if (name == "campaigninformation")
        {
            description =
            "Campaign Information \n " +
            "\n " +
            "This is used to define the campaign information that will appear in the menu before the mission is loaded. \n" +
            "\n " +
            "Extra Information \n" +
            "- The mission will be included in a list of missions which share the same campaign name. \n" +
            "- This missions will be organised by their file names alphabetically and numerically \n" +
            "- This node does not need to be linked to any others. OG will automatically search for and run this node when making the game's menu. \n" +
            "- The campaign information should be the same for each mission, or there should be one mission with the information and the rest should says 'none' or blank. \n" +
            "- The image file is the image associated with the campaign. Make sure that this is either the same for all missions, or there is one mission with the file name and the rest say 'none' or a left blank. The image should be in the same folder as the custom mission files. \n";
        }
        else if (name == "createlocation")
        {
            description =
            "Create Location \n " +
            "\n " +
            "This creates a new location in the mission. At least one location is needed. \n" +
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
            "- The X,Y,Z values set the centre of the asteroid field. \n" +
            "- Number: the number of asteroids. \n" +
            "- Type: selects the type of asteroids/debris to load. \n" +
            "- Width, Height, Length set the bounds in which the asteroids will be loaded \n" +
            "- MinSize and Maxsize are the minimum and maximum possible scale of the generated asteroids. \n" +
            "- MinSize and Maxsize are the minimum and maximum possible scale of the generated asteroids. \n" +
            "- Preferene and Percentage: sets the percent to which the generator should prioritise a certain size of asteroid. This is use to create a variegated asteroid field with features rather than a uniform field. \n" +
            "- Seed: allows you to generate the same field each time, by typing in the same seed number and setting the same values. \n" +
            "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order when loading a location. \n";
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
           "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order when loading a location. \n";
        }
        else if (name == "preload_loadterrain")
        {
            description =
            "Pre-Load Planet \n " +
            "\n " +
            "This event tells the scene to load the terrain with the selected heightmap and material. \n" +
            "\n " +
            "Extra Information \n" +
            "- Remove this node if you want to load a scene without a terrain \n" +
            "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order when loading a location. \n";
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
            "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order when loading a location. \n";
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
           "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order when loading a location. \n";
        }
        else if (name == "preload_loadmultipleships")
        {
            description =
            "Pre-Load Multiple Ships \n " +
            "\n " +
            "This node loads multiple ships of the same type \n" +
            "\n " +
            "Extra Information \n" +
            "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order when loading a location. \n" +
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
        else if (name == "preload_setgalaxylocation")
        {
            description =
           "Pre-Load Set Galaxy Location \n " +
           "\n " +
           "This sets the moves the starfield camera to the designated location in the starfield \n" +
           "\n " +
           "Extra Information \n" +
           "- You can simply type in the name of the planet where you want scene to be i.e. 'Alderaan' and the game will automatically supply the correct location or you can manually enter the coordinates.  \n" +
           "- If you use the 'search' mode you don't need to enter any coordinates. Open Galaxy will autmotically supply the correct coordinates if the planet exists. \n" +
           "- The galaxy is 15000 units by width, 15000 units by length, and 15000 units by height. So the coordinates you enter will need to be smaller than this. \n" +
           "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order when loading a location. \n";
        }
        else if (name == "preload_sethudcolour")
        {
            description =
           "Pre-Load Set Hud Colour \n " +
           "\n " +
           "Sets the colour parts of the Hud using a html colour code beginning with # i.e. The default colour is #ff0000 (red). \n" +
           "\n " +
           "Extra Information \n" +
           "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order when loading a location. \n" ;
        }
        else if (name == "preload_setsceneradius")
        {
            description =
           "Pre-Load Set Scene Radius \n " +
           "\n " +
           "This sets how far players can fly from the center of the scene before they are turned around \n" +
           "\n " +
           "Extra Information \n" +
           "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order when loading a location. \n";
        }
        else if (name == "preload_setskybox")
        {
            description =
           "Pre-Load Set Skybox \n " +
           "\n " +
           "This sets the sky to the designated skybox and can toggle the star field on and off. \n" +
           "\n " +
           "Extra Information \n" +
           "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order when loading a location. \n";
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
        else if (name == "spliteventseries")
        {
            description =
           "Split Event Series \n " +
           "\n " +
           "This is splits an event series into two, three, or four new event series \n" +
           "\n " +
           "Extra Information \n" +
           "- This node is particulary helpful when you want several events to occur concurrently in response to a specific event. \n" +
           "- You can have as many event series as you want. \n";
        }
        else if (name == "addaitagtolargeship")
        {
            description =
            "Add AI Tag to Small Ship \n " +
            "\n " +
            "This node adds an AI Tag (command) to a large ship" +
            "\n " +
           "Extra Information \n" +
           "- A large ship is any ship that cannot be controlled by the player i.e.  bulk freighters, corvettes, and stardestroyers are large ships but the tie fighter, x-wing, millenium falcon, and shuttles are not. \n" +
           "- This function will affect any ship whose name contains the designated string. \n";
        }
        else if (name == "addaitagtosmallship")
        {
            description =
            "Add AI Tag to Small Ship \n " +
            "\n " +
            "This node adds an AI Tag (command) to a small ship" +
            "\n " +
           "Extra Information \n" +
           "- A small ship is any ship that can (theoretically) be controlled by the player i.e. the tie fighter, x-wing, millenium falcon, and shuttles are small ships but the bulk freighters, corvettes, and stardestroyers are not. \n" +
           "- This function will affect any ship whose name contains the designated string. \n";
        }
        else if (name == "activatehyperspace")
        {
            description =
            "Activate Hyperspace \n " +
            "\n " +
            "This node causes a ship or ships to jump to hyperspace" +
            "\n " +
           "Extra Information \n" +
           "- When activated the ship will jump out of the scene and be deactivated. \n" +
           "- This node should only be used on non-player ships \n" +
           "- For making the player jump to hyperspace use the 'change location' node. \n" +
           "- If you want the ship to 'return' you will need to load a new ship with the same name and parameters. \n" +
           "- This function will affect any ship whose name contains the designated string. \n" +
           "- If the location is left as 'none' the node will runn regardless of location. \n";
        }
        else if (name == "activatedocking")
        {
            description =
            "Activate Docking \n " +
            "\n " +
            "This node causes the designated ship to dock with the target ship" +
            "\n " +
           "Extra Information \n" +
           "- This function will affect the first ship whose name contains the designated string. \n" +
           "- Rotation speed is the speed with which the ship rotates into position. \n" +
           "- Movement speed is the speed with which the ship moves into position. \n" +
           "- When docking the while will always rotate BEFORE it moves into position \n" +
           "- When disengaging from docking only there is not rotation and the rotation speed value is not used. \n";
        }
        else if (name == "changelocation")
        {
            description =
            "Change Location \n " +
            "\n " +
            "This node unloads the current location and loads a new one while simulating a hyperspace jump" +
            "\n " +
           "Extra Information \n" +
           "- You can only jump to locations you have created in the mission using the 'create location' node. The game will abort changing location if it can't the location.  \n" +
           "- If the location is left as 'none' the node will runn regardless of location. \n";
        }
        else if (name == "exitmission")
        {
            description =
           "Display Large Mission Then Exit \n " +
           "\n " +
           "This node exits the mission and returns the player to the main menu. \n" +
           "\n " +
           "Extra Information \n" +
           "- If the location is left as 'none' the node will runn regardless of location. \n";
        }
        else if (name == "deactivateship")
        {
            description =
           "Deactivate Ship \n " +
           "\n " +
           "This node deactivates a ship so that is is no longer part of the scene. \n" +
           "\n " +
           "Extra Information \n" +
           "- If the location is left as 'none' the node will runn regardless of location. \n";
        }
        else if (name == "displaydialoguebox")
        {
            description =
           "Display Dialogue Box \n " +
           "\n " +
           "This node pauses the game and displays a dialogue box with a message. When the button on the dialogue box is pressed the box disappears and the game resumes. \n" +
           "\n " +
           "Extra Information \n" +
           "- If the location is left as 'none' the node will runn regardless of location. \n";
        }
        else if (name == "displaytitle")
        {
            description =
            "Display Large Messgage \n " +
            "\n " +
            "This node displays a large message in the middle of the screen, like 'MISSION COMPLETE' or similar for a short period of time. \n" +
            "\n " +
           "Extra Information \n" +
            "- If the location is left as 'none' the node will runn regardless of location. \n";
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
            "- Internal Audio Files: beep01_toggle, beep02_targetlock, beep03_weaponchange,  r2d2, r5" +
           "- If the location is left as 'none' the node will runn regardless of location. \n";
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
            "- WARNING: This node is under development. Expect it to change over time. \n" +
           "- If the location is left as 'none' the node will runn regardless of location. \n";
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
             "- This node will return a result on the first ship that contains the given text. For example, if you have two ships one named 'Container A' and the other 'Container B' and you simply write 'Container' the node will return a result on the first of the two ships it checks. \n" +
           "- If the location is left as 'none' the node will runn regardless of location. \n";
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
             "- This node will return a result on the first ship that contains the given text. For example, if you have two ships one named 'Container A' and the other 'Container B' and you simply write 'Container' the node will return a result on the first of the two ships it checks. \n" +
             "- If the location is left as 'none' the node will runn regardless of location. \n";
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
             "- This node will return a result on the first ship that contains the given text. For example, if you have two ships one named 'Container A' and the other 'Container B' and you simply write 'Container' the node will return a result on the first of the two ships it checks. \n" +
             "- If the location is left as 'none' the node will runn regardless of location. \n";
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
            "- This is a branching node. You can intiate a different set of events depending on whether the answer is yes or no. \n" +
            "- If the location is left as 'none' the node will runn regardless of location. \n";
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
            "- This is a branching node. You can intiate a different set of events depending on whether the answer is yes or no. \n" +
            "- If the location is left as 'none' the node will runn regardless of location. \n";
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
            "- This is a branching node. You can intiate a different set of events depending on whether the answer is yes or no. \n" +
            "- If the location is left as 'none' the node will runn regardless of location. \n";
        }
        else if (name == "ifshipofallegianceisactive")
        {
            description =
            "If Ship of Allegiance is Active \n " +
            "\n " +
            "This node checks to see whether there are any ships active of a particular allegiance i.e. whether there are any imperial ships left in the scene. \n" +
            "\n " +
            "Extra Information \n" +
            "- This is a branching node. You can intiate a different set of events depending on whether the answer is yes or no. \n" +
            "- WARNING: The name of this node may change to accurately represent the function it runs. \n" +
            "- If the location is left as 'none' the node will runn regardless of location. \n";
        }
        else if (name == "loadsingleship")
        {
            description =
            "Single Ship \n " +
            "\n " +
            "This node loads a single ship \n" +
            "\n " +
            "Extra Information \n" +
            "- If you write 'random' or 'randomise' for the cargo the game will automatically randomise the ships cargo using preset list \n" +
            "- If the location is left as 'none' the node will runn regardless of location. \n";
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
            "- This not is helpful if you want to deliberately load an enemy ship behind the player for example. \n" +
            "- If the location is left as 'none' the node will runn regardless of location. \n";
        }
        else if (name == "loadmultipleships")
        {
            description =
            "Pre-Load Multiple Ships \n " +
            "\n " +
            "This node loads multiple ships of the same type \n" +
            "\n " +
            "Extra Information \n" +
            "- If you write 'random' or 'randomise' for the cargo the game will automatically randomise the ships cargo using preset list \n" +
            "- Pattern: rectanglehorizontal uses with and length. Height is ignored. \n" +
            "- Pattern: rectanglevertical uses width and height. Length is ignored. \n" +
            "- Pattern: arrowhorizontal uses with and length. Height is ignored. \n" +
            "- Pattern: arrowhorizontalinverted uses with and length. Height is ignored. \n" +
            "- Pattern: linehorizontallongways uses length. Width and height are ignored. \n" +
            "- Pattern: linehorizontalsideways uses width. Height and length are ignored. \n" +
            "- Pattern: linevertical uses height. Width and length are ignored. \n" +
            "- Pattern: randominsidecube uses width, length, height. \n" +
            "- If the location is left as 'none' the node will runn regardless of location. \n";
        }
        else if (name == "loadmultipleshipsfromhangar")
        {
            description =
            "Pre-Load Multiple Ships from a ships hangar \n " +
            "\n " +
            "This node loads multiple ships of the same type from the designated hangar \n" +
            "\n " +
            "Extra Information \n" +
            "- The ships will not load if the launching ship is not found OR the launching ship has no hangars. \n" +
            "- Some ships have multiple hangars. To select a particular hangar change the number of the hangar. \n" +
            "- The hangar numbers begin at 0 not 1. So if you want to load from hangar 2, you need to write 1, and if you want to load from hangar 1 you need to write 0, etc. \n" +
            "- If you write 'random' or 'randomise' for the cargo the game will automatically randomise the ships cargo using preset list \n" +
            "- If the location is left as 'none' the node will runn regardless of location. \n";
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
            "- You can force the function to load without hitting a tile by using the 'if raycast fails still load function'. \n" +
            "- If the location is left as 'none' the node will runn regardless of location. \n";
        }
        else if (name == "pausesequence")
        {
            description =
            "Pause Sequence \n " +
            "\n " +
            "This function pauses the event sequence for the designated amount of time. \n" +
            "\n " +
            "Extra Information \n" +
            "- This node is useful for when you want another function to only run after a set amount of time. i.e. waiting for a ship to be loaded before running a check. \n";
        }
        else if (name == "playmusictrack")
        {
            description =
            "Play Music Track \n " +
            "\n " +
            "This function plays the selected track on repeat. \n" +
            "\n " +
            "Extra Information \n" +
            "- This node will fade out the current track while fading in the new track. \n" +
            "- If loop is set to false the track will play once and then stop, if set to true it will repeat. \n" +
            "- Select none if you want the music to fade out. \n" +
           "- If the location is left as 'none' the node will runn regardless of location. \n";
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
            "- This function will affect any ship whose name contains the designated string. \n" +
           "- If the location is left as 'none' the node will runn regardless of location. \n";
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
           "- If you write 'random' or 'randomise' the game will automatically randomise the ships cargo using preset list \n" +
           "- If the location is left as 'none' the node will runn regardless of location. \n";
        }
        else if (name == "setcontrollock")
        {
            description =
             "Set Control Lock \n " +
             "\n " +
             "This function locks the controls on the designated ship which prevents the ship from steering, changing current ship settings (i.e. speed), or firing weapons.  \n" +
             "\n " +
             "Extra Information \n" +
             "- This function will affect any ship whose name contains the designated string. \n" +
             "- If the location is left as 'none' the node will runn regardless of location. \n";
        }
        else if (name == "setobjective")
        {
            description =
           "Set Objective \n " +
           "\n " +
           "This function adds, cancels, completes an objective. It can also clear all objectives. \n" +
           "- If the location is left as 'none' the node will runn regardless of location. \n";
        }
        else if (name == "setdontattacklargeships")
        {
            description =
           "Set Dont Attack Large Ships \n " +
           "\n " +
           "This function tells the designated small ships to not attack large ships \n" +
           "\n " +
           "Extra Information \n" +
           "- This function will affect any ship whose name contains the designated string. \n" +
           "- If the location is left as 'none' the node will runn regardless of location. \n";
        }
        else if (name == "setshipallegiance")
        {
            description =
           "Set Ship Allegiance \n " +
           "\n " +
           "This function changes the designated ship to the designated allegiance \n" +
           "\n " +
           "Extra Information \n" +
           "- This function will affect any ship whose name contains the designated string. \n" +
           "- If the location is left as 'none' the node will runn regardless of location. \n";
        }
        else if (name == "setshipstats")
        {
            description =
            "Set Ship Stats\n " +
            "\n " +
            "This function allows you to modify the ships stats to make the ship faster, slower, more maneurability, etc \n" +
            "\n " +
            "Extra Information \n" +
            "- All values are percentages. \n" +
            "- Acceleration: How quickly the ship speeds up and slows down. \n" +
            "- Speed: How fast the ship can go. \n" +
            "- Maneuverability: How quickly the ship can turn. \n" +
            "- Hull: How much damage the ship can take. \n" +
            "- Shield: How strong the ships shields can be. \n" +
            "- Laser Fire Rating: How quickly the ship fires it lasers. \n" +
            "- Laser Power: How strong the individual lasers are. \n" +
            "- WEP: Power of the ships boost. \n" +
            "- If the location is left as 'none' the node will runn regardless of location. \n";
        }
        else if (name == "setshiptarget")
        {
            description =
            "Set Ship Target \n " +
            "\n " +
            "This function sets the designated ship to the designated target. \n" +
            "\n " +
            "Extra Information \n" +
            "- This function will affect any ship whose name contains the designated string. \n" +
           "- If the location is left as 'none' the node will runn regardless of location. \n";
        }
        else if (name == "setshiptargettoclosestenemy")
        {
            description =
            "Set Ship Target To Closest Enemy \n " +
            "\n " +
            "This function sets the designated ship target to the closest enemy target. \n" +
            "\n " +
            "Extra Information \n" +
            "- This function will affect any ship whose name contains the designated string. \n" +
           "- If the location is left as 'none' the node will runn regardless of location. \n";
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
             "- This function will affect any ship whose name contains the designated string. \n" +
           "- If the location is left as 'none' the node will runn regardless of location. \n";
        }

        return description;
    }
}
