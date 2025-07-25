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
        else if (name == "preload_loadmultiplepropsonground")
        {
            description =
            "Pre-Load Multiple Props on Ground \n " +
            "\n " +
            "This node loads multiple props on the ground \n" +
            "\n " +
            "Extra Information \n" +
            "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order when loading a location. \n" +
            "- If you write 'random' or 'randomise' for the cargo the game will automatically randomise the ships cargo using preset list \n" +
            "- For pattern rectanglehorizontal or retangleVertical the shipsPerLine value must be atleast two. If it is less than this the game will automatically change it to 2. \n" +
            "- Pattern: treepositions places the props in the arrangement of a forest. \n" +
            "- Pattern: buildingpositions places the props in the arrangement of a town or city. \n";
        }
        else if (name == "preload_loadmultipleships")
        {
            description =
            "Pre-Load Multiple Ships \n " +
            "\n " +
            "This node loads multiple ships of the same type \n" +
            "\n " +
            "Extra Information \n" +
            "- The name of the ship will automatically be inumerated i.e. Alpha will become Alpha01, the next ship Alpha02 and so on \n" +
            "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order when loading a location. \n" +
            "- If you write 'random' or 'randomise' for the cargo the game will automatically randomise the ships cargo using preset list \n" +
            "- For pattern rectanglehorizontal or retangleVertical the shipsPerLine value must be atleast two. If it is less than this the game will automatically change it to 2. \n" +
            "- Pattern: rectanglehorizontal uses with and length. Height is ignored. \n" +
            "- Pattern: rectanglevertical uses width and height. Length is ignored. \n" +
            "- Pattern: arrowhorizontal uses with and length. Height is ignored. \n" +
            "- Pattern: arrowhorizontalinverted uses with and length. Height is ignored. \n" +
            "- Pattern: linehorizontallongways uses length. Width and height are ignored. \n" +
            "- Pattern: linehorizontalsideways uses width. Height and length are ignored. \n" +
            "- Pattern: linevertical uses height. Width and length are ignored. \n" +
            "- Pattern: randominsidecube uses width, length, height. \n";
        }
        else if (name == "preload_loadmultipleshipsonground")
        {
            description =
            "Pre-Load Multiple Ships on Ground \n " +
            "\n " +
            "This loads multiple ships on ground, usually turrets, but can also be used to place normal ships as well \n" +
            "\n " +
            "Extra Information \n" +
            "- The name of the ship will automatically be inumerated i.e. Alpha will become Alpha01, the next ship Alpha02 and so on \n" +
            "- For this node to work you need to also use the load terrain event node. \n" +
            "- You can force the function to load without hitting a tile by using the 'if raycast fails still load function'. \n" +
            "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order when loading a location. \n";
        }
        else if (name == "preload_loadsingleproponground")
        {
            description =
           "Pre-Load Load Single Prop On Ground \n " +
           "\n " +
           "This node loads a single prop on the ground \n" +
           "\n " +
           "Extra Information \n" +
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
        else if (name == "preload_loadsingleshiponground")
        {
            description =
           "Pre-Load Single Ship on Ground \n " +
           "\n " +
           "This node loads a single ship on the ground\n" +
           "\n " +
           "Extra Information \n" +
           "- For this node to work you need to also use the load terrain event node. \n" +
           "- You can force the function to load without hitting a tile by using the 'if raycast fails still load function'. \n" +
           "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order when loading a location. \n";
        }
        else if (name == "preload_loadterrain")
        {
            description =
           "Pre-Load Terrain \n " +
           "\n " +
           "This node loads terrain\n" +
           "\n " +
           "Extra Information \n" +
           "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order when loading a location. \n";
        }
        else if (name == "preload_setfogdistanceandcolor")
        {
            description =
           "Pre-Load Set Fog Distance and Color \n " +
           "\n " +
           "Sets the fog distance and color # i.e. The default distance is 30000-40000 and the default color us black #000000. The settings in the nodes are the optimal settings for a planetary environment. \n" +
           "\n " +
           "Extra Information \n" +
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
        else if (name == "preload_setlighting")
        {
            description =
           "Pre-Load Set Lighting \n " +
           "\n " +
           "Sets the colour of the lighting and also turns on and off and toggles the intensity of the sun" +
           "\n " +
           "Extra Information \n" +
           "- Preload events do not need to be linked to any other events. OG will search for and run them in the correct order when loading a location. \n";
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
            "Add AI Tag to Large Ship \n " +
            "\n " +
            "This node adds an AI Tag (command) to a large ship. It does not work with small ships." +
            "\n " +
           "Extra Information \n" +
           "- If your not sure if a ship is a largeship or smallship check the wiki. /n" +
           "- If you change the targetting tags the ships target will be set to null and it will search for a new ship, so if you want to set the manually, set it after you have run the tag. /n" +
           "- The following tags are added to the largeship when first loaded: 'nospeed', 'fireweapons', 'norotation'. \n" +
           "- A large ship is any ship that cannot be controlled by the player i.e.  bulk freighters, corvettes, and stardestroyers are large ships but the tie fighter, x-wing, millenium falcon, and shuttles are not. \n" +
           "- This function will affect any ship whose name contains the designated string. \n";
        }
        else if (name == "addaitagtosmallship")
        {
            description =
            "Add AI Tag to Small Ship \n " +
            "\n " +
            "This node adds an AI Tag (command) to a small ship. It does not work with large ships." +
            "\n " +
           "Extra Information \n" +
           "- If your not sure if a ship is a smallship or largeship check the wiki. /n" +
           "- If you change the targetting tags the ships target will be set to null and it will search for a new ship, so if you want to set the manually, set it after you have run the tag. /n" +
           "- The following tags are added to the smallship when first loaded: 'threequarterspeed', 'singlelaser', 'lowaccuracy', 'chasewithdraw', 'resetenergylevels', 'targetallprefsmall'. \n" +
           "- A small ship is any ship that can (theoretically) be controlled by the player i.e. tie fighter, x-wing, millenium falcon, and shuttles are small ships but the bulk freighters, corvettes, and stardestroyers are not. \n" +
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
           "- If the location is left as 'none' the node will run regardless of location. \n";
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
        else if (name == "activatewaypointmarker")
        {
            description =
            "Activate Waypoint Marker \n " +
            "\n " +
            "This activates and deactivates the waypoint marker so the waypoint is visible to the player" +
            "\n " +
           "Extra Information \n" +
           "- This function only works for the players waypoint. \n";
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
           "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "changelocationfade")
        {
            description =
            "Change Location \n " +
            "\n " +
            "This node unloads the current location and loads a new one while using a simple fade in and out" +
            "\n " +
           "Extra Information \n" +
           "- You can only move to locations you have created in the mission using the 'create location' node. The game will abort changing location if it can't the location.  \n" +
           "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "exitmission")
        {
            description =
           "Exit Mission \n " +
           "\n " +
           "This node exits the mission and returns the player to the main menu. \n" +
           "\n " +
           "Extra Information \n" +
           "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "exitanddisplaynextmissionmenu")
        {
            description =
           "Exit and Display Next Mission Menu \n " +
           "\n " +
           "This node exits the mission and then displays the next mission menu. \n" +
           "\n " +
           "Extra Information \n" +
           "- If the nextmission is left as 'none' when the next mission node is pressed it will simply return to the main menu with an error message. \n";
        }
        else if (name == "deactivateship")
        {
            description =
           "Deactivate Ship \n " +
           "\n " +
           "This node deactivates a ship so that is is no longer part of the scene. \n" +
           "\n " +
           "Extra Information \n" +
           "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "displayhint")
        {
            description =
            "Display Hint \n " +
            "\n " +
            "This node displays a hint in the middle bottom of the screen, like 'HINT: Press CAPS to change your weapons' or similar for a short period of time. \n" +
            "\n " +
           "Extra Information \n" +
            "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "displaytitle")
        {
            description =
            "Display Large Messgage \n " +
            "\n " +
            "This node displays a large message in the middle of the screen, like 'MISSION COMPLETE' or similar for a short period of time. \n" +
            "\n " +
           "Extra Information \n" +
            "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "displaymessage")
        {
            description =
            "Display Message \n " +
            "\n " +
            "This node sends an in game message to the message queue, it will play when the messages in front of the message in the queue have played i.e. there will be a delay \n" +
            "\n " +
            "Extra Information \n" +
            "- You can link an audio file (.wav) to run at the same time the message is sent. Simply make a folder in the custom missions folder. Name the folder 'YOURMISSIONNAME_audio'. Paste your audio file in the folder, type the name of the audio file (without the .wav extension) in the designated area on the node, and select the 'External Audio' option. \n" +
            "- You can also access several inbuilt audio files. Use the names listed below and select the option 'Internal Audio'. \n" +
            "- Internal Audio Files: beep01_toggle, beep02_targetlock, beep03_weaponchange,  r2d2, r5" +
           "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "displayimmediate")
        {
            description =
            "Display Message Immediate \n " +
            "\n " +
            "This node sends an in game message immediately regardless of whether a message is currently playing or not i.e. there will be no delay \n" +
            "\n " +
            "Extra Information \n" +
            "- You can link an audio file (.wav) to run at the same time the message is sent. Simply make a folder in the custom missions folder. Name the folder 'YOURMISSIONNAME_audio'. Paste your audio file in the folder, type the name of the audio file (without the .wav extension) in the designated area on the node, and select the 'External Audio' option. \n" +
            "- You can also access several inbuilt audio files. Use the names listed below and select the option 'Internal Audio'. \n" +
            "- Internal Audio Files: beep01_toggle, beep02_targetlock, beep03_weaponchange,  r2d2, r5" +
           "- If the location is left as 'none' the node will run regardless of location. \n";
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
            "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "fadeinfromcolour")
        {
            description =
            "Fade in from colour \n " +
            "\n " +
            "This node fades the screen in from a block colour \n" +
            "\n ";
        }
        else if (name == "fadetocolour")
        {
            description =
            "Fade to colour \n " +
            "\n " +
            "This node fades the screen to a block colour \n" +
            "\n ";
        }
        else if (name == "ifobjectiveisactive")
        {
            description =
            "If Objective is Active \n " +
            "\n " +
            "This node checks to see whether an objective is active or not. \n" +
            "\n " +
            "Extra Information \n" +
            "- This node will return a result if the current list of objectives contains the given text.  \n" +
            "- This string is case sensitive. If you write 'destroy tie fighters' and you are looking for 'DESTROY TIE FIGHTERS' the function wont find it. \n" +
            "- This is a branching node. You can intiate a different set of events depending on whether the answer is yes or no. \n" +
            "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "ifkeyboardisactive")
        {
            description =
            "If Keyboard is Active \n " +
            "\n " +
            "This node checks to see whether the player is using the keyboard or not. \n" +
            "\n " +
            "Extra Information \n";
        }
        else if (name == "ifnumberofshipsislessthan")
        {
            description =
            "If Number of Ships is Less Than \n " +
            "\n " +
            "This node checks to see the number of ships in the scene is less than the given amount. \n" +
            "\n " +
            "Extra Information \n" +
            "- You can choose whether the node counts only smallships or only largeships or everyship in the scene. \n";
        }
        else if (name == "ifnumberofshipsofallegianceislessthan")
        {
            description =
            "If Number of Ships of Allegiance is Less Than \n " +
            "\n " +
            "This node checks to see the number of ships of a particular allegiance in the scene is less than the given amount. \n" +
            "\n " +
            "Extra Information \n" +
            "- You can choose whether the node counts only smallships or only largeships or everyship in the scene. \n";
        }
        else if (name == "ifnumberofshipswithnameislessthan")
        {
            description =
            "If Number of Ships With Name is Less Than \n " +
            "\n " +
            "This node checks to see the number of ships which contain a given name in the scene is less than the given amount. \n" +
            "\n " +
            "Extra Information \n" +
            "- You can choose whether the node counts only smallships or only largeships or everyship in the scene. \n";
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
             "- This node will return a result on the first ship that returns a positive result. For example, if you have two ships one named 'Container A' and the other 'Container B' and you simply write 'Container' the node will return a result on the first ship that returns true. \n" +
            "- If the location is left as 'none' the node will run regardless of location. \n";
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
             "- This node will return a result on the first ship that returns a positive result. For example, if you have two ships one named 'Container A' and the other 'Container B' and you simply write 'Container' the node will return a result on the first ship that returns true. \n" +
             "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "ifshipislessthandistancetopointinspace")
        {
            description =
             "If Ship Is Less Than Distance to Point in Space \n " +
             "\n " +
             "This function checks the distance between the designated ship and a point in space, it then returns true or false according to whether the ship's distance is less or more than the designated amount. \n" +
             "\n " +
             "Extra Information \n" +
             "- This is a branching node. You can intiate a different set of events depending on whether the answer is yes or no. \n" +
             "- This node will return a result on the first ship that returns a positive result. For example, if you have two ships one named 'Container A' and the other 'Container B' and you simply write 'Container' the node will return a result on the first ship that returns true. \n" +
             "- If the location is left as 'none' the node will run regardless of location. \n";
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
             "- This node will return a result on the first ship that returns a positive result. For example, if you have two ships one named 'Container A' and the other 'Container B' and you simply write 'Container' the node will return a result on the first ship that returns true. \n" +
             "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "ifshipisactive")
        {
            description =
            "If Ship is Active \n " +
            "\n " +
            "This node checks to see whether a particular ship is active or not. \n" +
            "\n " +
            "Extra Information \n" +
            "- This node will return a result on the first ship that returns a positive result. For example, if you have two ships one named 'Container A' and the other 'Container B' and you simply write 'Container' the node will return a result on the first ship that returns true. \n" +
            "- This string is case sensitive. If you write 'container' and you are looking for 'Container' the function wont find it. \n" +
            "- This is a branching node. You can intiate a different set of events depending on whether the answer is yes or no. \n" +
            "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "ifshiphasbeendisabled")
        {
            description =
            "If Ship has been disabled \n " +
            "\n " +
            "This node checks to see whether a particular ship has been disabled or not. \n" +
            "\n " +
            "Extra Information \n" +
            "- This node will return a result on the first ship that returns a positive result. For example, if you have two ships one named 'Container A' and the other 'Container B' and you simply write 'Container' the node will return a result on the first ship that returns true. \n" +
            "- This string is case sensitive. If you write 'container' and you are looking for 'Container' the function wont find it. \n" +
            "- This is a branching node. You can intiate a different set of events depending on whether the answer is yes or no. \n" +
            "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "ifshiphasntbeendisabled")
        {
            description =
            "If Ship hasnt been disabled \n " +
            "\n " +
            "This node checks to see whether a particular ship hasn't been disabled or not. \n" +
            "\n " +
            "Extra Information \n" +
            "- This node will return a result on the first ship that returns a positive result. For example, if you have two ships one named 'Container A' and the other 'Container B' and you simply write 'Container' the node will return a result on the first ship that returns true. \n" +
            "- This node is useful for checking if a player has finished scanning a group of ships/containers with a similar name. \n" +
            "- This string is case sensitive. If you write 'container' and you are looking for 'Container' the function wont find it. \n" +
            "- This is a branching node. You can intiate a different set of events depending on whether the answer is yes or no. \n" +
            "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "ifshiphasbeenscanned")
        {
            description =
            "If Ship has been scanned \n " +
            "\n " +
            "This node checks to see whether a particular ship has been scanned or not. \n" +
            "\n " +
            "Extra Information \n" +
            "- This node will return a result on the first ship that returns a positive result. For example, if you have two ships one named 'Container A' and the other 'Container B' and you simply write 'Container' the node will return a result on the first ship that returns true. \n" +
            "- This string is case sensitive. If you write 'container' and you are looking for 'Container' the function wont find it. \n" +
            "- This is a branching node. You can intiate a different set of events depending on whether the answer is yes or no. \n" +
            "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "ifshiphasntbeenscanned")
        {
            description =
            "If Ship hasn't been scanned \n " +
            "\n " +
            "This node checks to see whether a ship hasn't been scanned or not. \n" +
            "\n " +
            "Extra Information \n" +
            "- This node will return a result on the first ship that returns a positive result. For example, if you have two ships one named 'Container A' and the other 'Container B' and you simply write 'Container' the node will return a result on the first ship that returns true. \n" +
            "- This node is useful for checking if a player has finished scanning a group of ships/containers with a similar name. \n" +
            "- This string is case sensitive. If you write 'container' and you are looking for 'Container' the function wont find it. \n" +
            "- This is a branching node. You can intiate a different set of events depending on whether the answer is yes or no. \n" +
            "- If the location is left as 'none' the node will run regardless of location. \n";
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
            "- This node will return a result on the first ship that returns a positive result. For example, if you have two ships one named 'Container A' and the other 'Container B' and you simply write 'Container' the node will return a result on the first ship that returns true. \n" +
            "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "ifshipsshieldsarelessthan")
        {
            description =
             "If Ships Shields are Less Than \n " +
             "\n " +
             "This function checks the shields of the designated ship and returns true or false according to whether the ship's shields are less or more than the designated amount. \n" +
             "\n " +
             "Extra Information \n" +
             "- This is a branching node. You can intiate a different set of events depending on whether the answer is yes or no. \n" +
             "- This node will return a result on the first ship that returns a positive result. For example, if you have two ships one named 'Container A' and the other 'Container B' and you simply write 'Container' the node will return a result on the first ship that returns true. \n" +
             "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "ifshipssystemsarelessthan")
        {
            description =
             "If Ships Systesm are Less Than \n " +
             "\n " +
             "This function checks the systems of the designated ship and returns true or false according to whether the ship's sytems are less or more than the designated amount. \n" +
             "\n " +
             "Extra Information \n" +
             "- This is a branching node. You can intiate a different set of events depending on whether the answer is yes or no. \n" +
             "- This node will return a result on the first ship that returns a positive result. For example, if you have two ships one named 'Container A' and the other 'Container B' and you simply write 'Container' the node will return a result on the first ship that returns true. \n" +
             "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "loadmultipleships")
        {
            description =
            "Pre-Load Multiple Ships \n " +
            "\n " +
            "This node loads multiple ships of the same type \n" +
            "\n " +
            "Extra Information \n" +
            "- The name of the ship will automatically be inumerated i.e. Alpha will become Alpha01, the next ship Alpha02 and so on \n" +
            "- If you write 'random' or 'randomise' for the cargo the game will automatically randomise the ships cargo using preset list \n" +
            "- For pattern rectanglehorizontal or retangleVertical the shipsPerLine value must be atleast two. If it is less than this the game will automatically change it to 2. \n" +
            "- Pattern: rectanglehorizontal uses with and length. Height is ignored. \n" +
            "- Pattern: rectanglevertical uses width and height. Length is ignored. \n" +
            "- Pattern: arrowhorizontal uses with and length. Height is ignored. \n" +
            "- Pattern: arrowhorizontalinverted uses with and length. Height is ignored. \n" +
            "- Pattern: linehorizontallongways uses length. Width and height are ignored. \n" +
            "- Pattern: linehorizontalsideways uses width. Height and length are ignored. \n" +
            "- Pattern: linevertical uses height. Width and length are ignored. \n" +
            "- Pattern: randominsidecube uses width, length, height. \n" +
            "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "loadmultipleshipsfromhangar")
        {
            description =
            "Pre-Load Multiple Ships from a ships hangar \n " +
            "\n " +
            "This node loads multiple ships of the same type from the designated hangar \n" +
            "\n " +
            "Extra Information \n" +
            "- The name of the ship will automatically be inumerated i.e. Alpha will become Alpha01, the next ship Alpha02 and so on \n" +
            "- The ships will not load if the launching ship is not found OR the launching ship has no hangars. \n" +
            "- Some ships have multiple hangars. To select a particular hangar change the number of the hangar. \n" +
            "- The hangar numbers begin at 0 not 1. So if you want to load from hangar 2, you need to write 1, and if you want to load from hangar 1 you need to write 0, etc. \n" +
            "- If you write 'random' or 'randomise' for the cargo the game will automatically randomise the ships cargo using preset list \n" +
            "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "loadmultipleshipsonground")
        {
            description =
            "Load Multiple Ships on Ground \n " +
            "\n " +
            "This loads multiple ships on ground, usually turrets, but can also be used to place normal ships as well \n" +
            "\n " +
            "Extra Information \n" +
            "- The name of the ship will automatically be inumerated i.e. Alpha will become Alpha01, the next ship Alpha02 and so on \n" +
            "- If you write 'random' or 'randomise' for the cargo the game will automatically randomise the ships cargo using preset list \n" +
            "- For this node to work you need to also use the load terrain event node. \n" +
            "- You can force the function to load without hitting a tile by using the 'if raycast fails still load function'. \n" +
            "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "loadsingleship")
        {
            description =
            "Load Single Ship \n " +
            "\n " +
            "This node loads a single ship \n" +
            "\n " +
            "Extra Information \n" +
            "- If you write 'random' or 'randomise' for the cargo the game will automatically randomise the ships cargo using preset list \n" +
            "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "loadsingleshipfromhangar")
        {
            description =
            "Load Single Ship From Hangar \n " +
            "\n " +
            "This node loads a single ship from a hangar \n" +
            "\n " +
            "Extra Information \n" +
            "- If you write 'random' or 'randomise' for the cargo the game will automatically randomise the ships cargo using preset list \n" +
            "- If the location is left as 'none' the node will run regardless of location. \n";
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
            "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "loadsingleshiponground")
        {
            description =
            "Load Single Ship on Ground \n " +
            "\n " +
            "This node loads a single ship on the ground \n" +
            "\n " +
            "Extra Information \n" +
            "- For this node to work you need to also use the load terrain event node. \n" +
            "- You can force the function to load without hitting a tile by using the 'if raycast fails still load function'. \n" +
            "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "pausesequence")
        {
            description =
            "Pause Sequence \n " +
            "\n " +
            "This function pauses the event sequence for the designated amount of time. \n" +
            "\n " +
            "Extra Information \n" +
            "- This node is useful for when you want another function to only run after a set amount of time. i.e. waiting for a ship to be loaded before runing a check. \n";
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
           "- If the location is left as 'none' the node will run regardless of location. \n";
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
           "- If the location is left as 'none' the node will run regardless of location. \n";
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
             "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "setfollowtarget")
        {
            description =
            "Set Follow Target \n " +
            "\n " +
            "This function sets the designated ship for the ship to follow in formation. \n" +
            "\n " +
            "Extra Information \n" +
            "- This function will affect the first target ship it finds, and then any other ship (that is not the designated target ship) whose name contains the designated string will follow it. \n" +
            "- For example you can set the target ship to 'Alpha01' and then you can set the following ship to 'Alpha' and this will cause all ships with alpha in their name to follow alpha 1. \n" +
            "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "setobjective")
        {
            description =
           "Set Objective \n " +
           "\n " +
           "This function adds, cancels, completes an objective. It can also clear all objectives. \n" +
           "- If the location is left as 'none' the node will run regardless of location. \n";
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
           "- If the location is left as 'none' the node will run regardless of location. \n";
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
           "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "setshiplevels")
        {
            description =
            "Set Ship Levels \n " +
            "\n " +
            "This function allows you to manually modify the ships levels i.e. the hull, the shields, the systems, and wep \n" +
            "\n " +
            "Extra Information \n" +
            "- If you don't want to modify a level you can leave it as 'nochange'. \n" +
            "- The game will automatically reduce a level that is inputted above a ships rating. i.e. if a ships hull is rated to 100 and you input 110, the game will automatically reduce the inputed amount to 100. \n" +
            "- This node shouldn't be confused with the 'setshipstats' which actually changes the ships rating i.e. can increase or decrease the amount of damamge a ship can take. \n" +
            "- This node is particularly helpful if you wish to reactivate a ship that has been disabled. \n";
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
            "- If the location is left as 'none' the node will run regardless of location. \n";
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
           "- If the location is left as 'none' the node will run regardless of location. \n";
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
           "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "setshiptocannotbedisabled")
        {
            description =
             "Set Ship To Cannot Be Disabled \n " +
             "\n " +
             "This function sets the designated ship so that it cannot be disbaled by ion cannons. \n" +
             "\n " +
             "Extra Information \n" +
             "- A ship that is set to cannotbedisabled will still receive system damage until it only has 5 points of health left. \n" +
             "- This function will affect any ship whose name contains the designated string. \n" +
           "- If the location is left as 'none' the node will run regardless of location. \n";
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
           "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "settorpedoes")
        {
            description =
             "Set Torpedoes \n " +
             "\n " +
             "This function set the type and number of torpedoes a ship has. \n" +
             "\n " +
             "Extra Information \n" +
             "- This function will affect any ship whose name contains the designated string. \n" +
             "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "setwaypoint")
        {
            description =
            "Set Waypoint \n " +
            "\n " +
            "Allows you to set the position of the ships waypoint \n" +
            "\n " +
            "Extra Information \n" +
            "- Every ship has a waypoint. Setting its position can be useful for a number of purposes. \n" +
            "- You can check a ships distance to its waypoint using the 'ifshipislessthandistancetowaypoint' function. \n" +
            "- You can direct an ai ship to fly toward its waypoint by setting the ai override to 'movetowaypoint'. \n" +
            "- This function will affect any ship whose name contains the designated string. \n" +
           "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "setwaypointtoship")
        {
            description =
            "Set Waypoint To Ship \n " +
            "\n " +
            "Allows you to set the position of the ships waypoint to the position of another ship \n" +
            "\n " +
            "Extra Information \n" +
            "- Every ship has a waypoint. Setting its position can be useful for a number of purposes. \n" +
            "- You can check a ships distance to its waypoint using the 'ifshipislessthandistancetowaypoint' function. \n" +
            "- You can direct an ai ship to fly toward its waypoint by setting the ai override to 'movetowaypoint'. \n" +
            "- This function will affect any ship whose name contains the designated string. \n" +
           "- If the location is left as 'none' the node will run regardless of location. \n";
        }
        else if (name == "setweaponselectiononplayership")
        {
            description =
            "Set Weapon Selection On Player Ship \n " +
            "\n " +
            "This sets the weapon selection on the player ship and controls whether the player can change their weapons or not \n" +
            "\n " +
            "Extra Information \n" +
            "- If you want to set the weapons on a non-player smallship use the AddSmallShipAITag node \n";
        }

        return description;
    }
}
