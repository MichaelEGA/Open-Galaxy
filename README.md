# Open-Galaxy

Gameplay
![image](https://github.com/user-attachments/assets/3df4d5a6-63a2-4bcc-b111-9e28017c1d3f)

![image](https://github.com/user-attachments/assets/dcf59720-56d5-49b3-bca9-33d6d1688397)

Mission Editor
![Screenshot (668)](https://github.com/user-attachments/assets/82905984-8da3-47fb-b885-5ab8783ffb1c)

Open Galaxy is a X-Wing and Tie Fighter style space sim designed to be a platform for custom missions made by the community. 

**Main Features**

  - X-Wing and Tie Fighter style ship combat
  - Missions Events
  - AI Tagging
  - Dynamic Cockpits
  - Easy to use mission editor
  - External mission loading
  - Accurate Star Wars galaxy i.e the star locations accurately represent the galaxy
  - Switch between different asset sets
  - 14 Inbuilt Missions
  - Keyboard and Mouse Controls

**Controls**  
  - Mouse Steers
  - A-D roll left and right
  - W-S speeds up and slows down
  - TAB toggles weapons if more than one type is available
  - CAPS toggles weapon linking (i.e. fire one laser at a time, or two at time, or all at once)
  - R selects next target
  - F selects closest enemy target if available
  - T selects next enemy target if available
  - G selects the target directly ahead of the player
  - Up, down, left, and right control energy management
  - Left Mouse fires weapons
  - Right Mouse matches speed
  - Scan a target by selecting it and then flying close

**Latest Development Release:** 4.3.07

  - https://github.com/MichaelEGA/Open-Galaxy/releases/download/v.4.3.07/Open.Galaxy.4.3.07.zip
    
**Latest Milestone Release:** 4.0.11

  - https://github.com/MichaelEGA/Open-Galaxy/releases/download/v.4.0.11/Open.Galaxy.4.0.11.zip

**Roadmap**

  - Roadmap: https://docs.google.com/spreadsheets/d/14mWjYlATWQYKEfD6AG4MwC_ppTzG-aDlsI2yR2h7D54/edit?usp=sharing

**Wiki and Mission Editor Tutorials**

  - https://github.com/MichaelEGA/Open-Galaxy/wiki

**All Open Galaxy resources are free to use in your own project**
  - Open Galaxy Assets: https://sketchfab.com/michael.evan.allison/models
  - First Strike Assets: https://www.moddb.com/mods/first-strike/news/public-release-of-first-strike-models
  - Original Trilogy Mode Assets: https://www.indiedb.com/downloads/original-trilogy-assets

**Licence**  

The game is open source and can be forked, modified, or replicated (Apache 2.0) but models, music, and icons remain the property of the respective creators and must be properly accredited.

**Changelog**

30/11/24 - 4.3.07 (Unity 6000.0.15f1)
  - Added: load next mission node/function
  - Added: select ship in front
  - Added: activate waypoint marker node/function
  - Added: systems level display on hud for player ship
  - Updated: remade audio in x-wing mission 6 and 7
  - Updated: remade x-wing mission 3
  - Updated: modify all missions to use radio distortion function
  - Updated: make disabled ship spin slightly while slowing down to a complete halt
  - Updated: different resolutions that match monitor aspect ratio
  - Updated: fade in and out next mission window
  - Updated: link all relevant missions together
  - Updated: increase rotation speed on stationary largeships
  - Updated: Add a hyperspace jump at end of X-Wing mission 4
  - Fixed: X-Wing mission 4 doesn't finish
  - Updated: increasing turn speed on GR75 transport
  - Added: add new function: clear objective
  - Updated: double size of open mission window in editor
  - Fixed: loading information should scroll not run of edge of screen
  - Updated: hyperspace at the end of x-wing mission 4
  - Fixed: cockpit movement should be low when speed is low
  - Fixed: shortcut commands stop working when the editor is run more than once
  - Fixed: fix polygon error on assault gunboat cockpit
  - Fixed: mesh colliders restored on asteroids
  - Updated: make planet further away in TF mission 5 and 6
  - Updated: add 'node copied' and 'node pasted' and 'node deleted' messages to editor
  - Fixed: xq1 station should be invincible in tie fighter mission 1
  - Added: created systems recharge for player craft
  - Fixed: overlapping audio files on X-Wing misison 6
  - Fixed: x-wing mission 7, next mission screen loads too quickly
  - Fixed: y-wings should only have two torpedo tubes
  - Fixed: assault gunboats should only have two torpedo tubes
  - Updated: update internal mission files with external missions
  - Fixed: right UI boxes should not overlap with radar
  - Fixed: tie fighter mission 1: buoy should be invincible
  - Fixed: systems number doesn't change colour on hud

08/11/24 - 4.0.11 (Unity 6000.0.15f1)
  - Added: Sulon Star Model
  - Added: mc80cc
  - Added: Escape Pod Cockpit
  - Added: Cloud Car Cockpit
  - Added: new node set waypoint to ship location
  - Added: new node ifobjectiveisactive
  - Added: new node/function: cannotbedisabled
  - Added: new node/function: display a hint
  - Added: x-wing mission 7
  - Added: slow cockpit sway
  - Added: sound radio distortion system
  - Added: x-wing mission 8
  - Updated: x-wing mission descriptions
  - Updated: tie fighter mission descriptions
  - Fixed: Tie Fighter mission 5 x-wings don't launch in third wave
  - Fixed: unresolved node functions null error in mission editor
  - Fixed: incorrect position of waypoint
  - Fixed: smallship not being marked as disabled
  - Fixed: mission 5 interceptors don't launch from SD
  - Fixed: ships not launching from correct hangar position
  - Removed: terrains
  - Added: add audio distortion option
  - Updated: post processing
  - Fixed: sentinel wings don't rotate
  - Fixed: sentinel wings on radar model in wrong position
  - Added: cloud city as background city option
  - Fixed: tie fighter mission 5 doesn't finish
  - Fixed: disabled ship should deselect target to make room for more ai attackers
  - Removed: wing position from ai control and make it automatic i.e. hyperspace/docking wings closed, otherwise open
  - Fixed: newly loaded ships can be heard while in hyperspace
  - Fixed: ship rolls one way and then the other when destroyed
  - Fixed: dual lasers on three laser ships doesn't work properly
  - Fixed: make build version use high fidelity by default
  - Fixed: change lower fidelity versions to include drive glows
  - Removed: all lod system code - largeship
  - Removed: environments
  - Removed: set galaxy location function
  - Removed: randomised explosions
  - Fixed: x-wing mission 6 freighter should exit more quickly
  - Fixed: x-wing mission 3 slight delay one of the ship destroyed notification messages
  - Fixed: x-wing mission 4 move red 2 so he is not destroyed straight away
  - Fixed: make fogger invincible in Tie Fighter mission 5
  - Fixed: remove dialogue box node, function, and assets
  - Fixed: ships not turning around at edge of game area
  - Fixed: make skybox options load dynamically in editor
  - Fixed: red bar behind hint box
  - Fixed: prevent x-wings attacking buoy in X-Wing mission 7
  - Removed: terrain image option from editor
  - Removed: controller code
  - Removed: controller information from menu
  - Updated: tutorials to remove terrain tutorial
  - Removed: menu mouse-over button sound
  - Updated: asteroid node and function to be less confusing
  - Fixed: no cursor on save name for mission editor
  - Updated: rename first x-wing campaign to A New Ally
  - Updated: rename Tie Fighter campaign to Aftermath of Hoth
  - Added: new campaign Imperial Assault
  - Added: 'none' option to music tracks in editor
  - Fixed: time should be in minutes
  - Updated: write a new description for A New Ally
  - Updated: write a new description for Aftermath of Hoth
  - Updated: write a new descrition for Imperial Assault
  - Updated: copy data from custom mission file back into internal folder

26/07/24 - v3.5.01 (Unity 6000.0.15f1)
  - Added: Radar objects for new star destroyers
  - Added: Turrets to new star destroyers
  - Updated: Universalised turret selection
  - Added: New star destroyer models
  - Added: Drive glows for galactic conquest assets
  - Added: Drive glows to Original Trilogy Asset Ships
  - Added: New snow speeder cockpit
  - Added: Drive glows for all First Strike ship assets

26/07/24 - v3.0.0 (Unity 6000.0.2f1)
  - Added: Latest mission files
  - Added: Hangars to nbf and xq1 station
  - Fixed: Direction cursor fades more quickly
  - Updated: Credits
  - Added: Open Galaxy as an asset set
  - Added: Shinyman Planets
  - Updated: Sentinel cockpit missing face
  - Added: Sentinel Class Shuttle Cockpit
  - Updated: lambda class shuttle cockpit
  - Updated: lambda class shuttle cockpit
  - Added: Load Single Ship on Ground Node
  - Added: base lambda shuttle mesh

16/07/24 - v2.6.82 (Unity 6000.0.2f1)
  - Added: Base falcon cockpit
  - Added: Assault Gunboat Cockpit
  - Added: Base AGB Cockpit mesh
  - Added: New B-Wing Cockpit
  - Updated: Added base mesh for new b-wing cockpit
  - Added: z95 cockpit
  - Updated Y-Wing Cockpit
  - Updated cockpits
  - Updated: Y-Wing cockpit model and textures
  - Added: finished new y-wing cockpit
  - Removed: Old broken particle thruster system
  - Added: E-Wing Cockpit
  - Finished: Tie Phantom Cockpit
  - Added: Phantom Cockpit
  - Updated: Tie Cockpit Scale
  - Updated: X-Wing cockpit complete
  - Updated: tie cockpit finished
  - Updated: Tie Fighter Cockpit
  - Updated: Cockpits, new version of unity
  - Update xwing_c.png
  - Updated: Cockpits for x-wing and tie fighters
  - Updated: x-wing and tie fighter cockpits
  - Updated: settings

28/05/24 - v2.5.37 (Unity 6000.0.2f1)
  - Added: Laat GC Cockpit
  - Added: Galactic Conquest LAAT remove community asset ships
  - Added: Ability to change quality settings in the menu
  - Added: Systems rating to ship class
  - Fixed: Settings load error
  - Added: dummy makers for target gauges when empty
  - Added: if ship has been disabled node
  - Fixed: Can now select first item in dropdown
  - Added: Automatic version update
  - Added: Coroutine enders
  - Added: display only selected nodes in editor
  - Added: Wing rotations, hangars, and ion to all relevant ships
  - Added: Turret loading from json
  - Updated: Base code for wing rotations finished
  - Added: Base code for finding movable wings on ship
  - Added: ability to change torpedo type and number in editor
  - Updated: minor changes
  - Updated: Numerous systems updated
  - Added: Systems Target Hud Icon, Changed firerate
  - Minor changes
  - Updated: clean up audio node type
  - Added: Two new tracks, automatic track listing
  - Added: ion canon explosion
  - Updated: Basic ion cannon system implemented
  - Updated: laser particle effects
  - Updated: Finished base code for ion cannons

06/05/24 - v.2.2.10 (Unity 6000.0.0f1)
  - Fixed: drag still not releasing breaking editor
  - Updated: updated to unity 6
  - Updated: removed unity splash screen
  - Updated: Enabled draw submissions through the GPU

05/05/24 - v.2.2.06 (Unity 2023.2.14f1)
  - Fixed: game-stopping error caused by removal of old node type

05/05/24 - v.2.2.05 (Unity 2023.2.14f1)
  - Updated: Asteroids now spin and move
  - Fixed: Drag not releasing in editor
  - Added: Different formations for formation flying
  - Updated: Formation flying to be more dynamic
  - Added: Base code for formation flying
  - Fixed: bugs in ai targeting system
  - Added: load multiple ships from hangar function

29/04/24 - v.2.0.92 (Unity 2023.2.14f1)
  - Added: Targeting control to smallship AI
  - Updated: All missions now use the new AI tagging system
  - Added: Node descriptions for add ai tag nodes
  - Fixed: AI not selecting torpedoes as active weapon
  - Added: Node ai tagging system fully implemented
  - Updated: AI Tagging System added to mission editor

22/04/24 - v.2.0.0 (Unity 2023.2.14f1)
  - Updated: numerous little changes
  - Fixed: Dragging and Deselect work properly now
  - Fixed: Caret displaying behind node again after unity engine update
  - Update: scale in editor now works correctly
  - Added: Exit hyperspace sound to all ships
  - Updated: fogwall now works correctly
  - Fixed: Windows no longer load on top of menu bars
  - Fixed: Skybox not reseting on change location
  - Update SceneFunctions.cs
  - Fixed: X-Wing Cockpit canopy offcenter
  - Fixed: mutlipleshipsnode doesn't include player by default
  - Updated: Added new hud reticle
  - Fixed: Null error when mission audio is not found
  - Updated: Asteroids now have mesh colliders
  - Fixed: Removed duplicate cockpit camera
  - Updated: Asteroid names
  - Fixed: Ships with 1 or 3 lasers don't fire correctly
  - Update: can display on side view without error
  - Fixed: Mission 'editor' conflict with unity editor
  - Updated: file restructure part 3
  - Updated: file restructure part 2
  - Updated: file restructure part 1
  - Updated: File references in several scripts
  - Updated: File Address references in several scripts
  - Updated: Get Address Function
  - Added: Get Address Function

07/04/24 - v.1.8.05 (Unity 2022.3.12f1)
  - Added: target locking now holds tone on confirmed lock
  - Updated: Added docking points to all ships
  - Updated: Move exit mission function to mission functions
  - Added: Outline to mission editor windows
  - Fixed: AI Ships now steer correctly when upside down
  - Updated: Distance to waypoint now works with large ships
  - Updated: Smallship and Largeship now exit docking on different vectors
  - Fixed: Minor docking errors
  - Updated: Set way point now affects LargeShips
  - Updated: Upgraded docking system complete
  - Updated: Docking now checks whether the dock is being used before running
  - Updated: Docking alignment now more accurate
  - Updated: Docking now connects docking point to docking point
  - Updated: Docking systems now fully implemented
  - Updated: Basic docking procedures are now functioning and can be call…
  - Added: Base code for docking
  - Added: Node and Window borders/highlights

25/03/24 - v1.6.54 (Unity 2022.3.12f1)
  - Fixed: A minor mission breaking bug that prevented smallships from hypering-in in the correct position

25/03/24 - v1.6.53 (Unity 2022.3.12f1)
  - Fixed: Starfield is now reset when exiting the game during hyperspace
  - Fixed: AI firing when target is not forward
  - Fixed: Colour on some turrets not changing from red
  - Added: Hyperspace shake & in game cursor fade
  - Added: Copy and Paste functions to editor
  - Added: Move multiple selected nodes at once
  - Added: Delete selected nodes with delete key
  - Added: Export Node Selection Function to editor
  - Updated: Editor controls
  - Updated: Select nodes function now fully works
  - Updated: Selection box code
  - Added: Dragbox selection to editor
  - Added: Ability to select hud colour
  - Added: Ability to choose different types of asteroids
  - Added: New asteroid loading system
  - Added: Ship explosion type to ship class
  - Updated: Display location tool
  - Updated: Load ships on ground is now correctly positioned
  - Added: Initial code for display location tool
  - Updated: Numerous minor changes
  - Update NodeDescriptions.cs
  - Added: set galaxy location node
  - Removed: Libnoise credit from menu
  - Removed: Libnoise
  - Added: reorganised and added new music tracks
  - Removed: Vwing
  - Updated: Tie Cockpit texture
  - Added: Change skybox node
  - Added: New Terrain loading system
  - Added: First strike ships
  - Added: Galactic Conquest Ships
  - Added: New planet loading system
  - Added: Additional Assets Ships
  - Updated: Dropbox size in editor
  - Added: Single run preload option on load single ship
  - Added: Toggle between different ship sets
  - Added: Dynamic campaign loading to main menu
  - Fixed: Incorrect rotation data applied in node
  - Fixed: Player ship wrongly being cleared from object list on hyperspace
  - Fixed: Hyperspace exit velocity bug
  - Added: Control lock node/function 
  - Fixed: Numerous hyperspace bugs

31/01/24 - MILESTONE RELEASE: v1.0.0 (Unity 2022.3.12f1)
  - Added: Four new missions
  - Updated: Reduced cost of looking for target
  - Added: Target request system to reduce cost of ai looking for targets
  - Fixed: Minor layer error
  - Fixed: largeships being loaded on player layer
  - Added: Two more missions
  - Fixed: Numerous small errors
  - Fixed: Error where ships targeted friendly
  - Updated: Mission Briefing Screen
  - Fixed: Dynamic cockpit not moving correctly
  - Fixed: Controller now works correctly in the game
  - Fixed: Minor hud errors
  - Updated: Hud functions are now more independant
  - Fixed: A heap of random bugs and error
  - Updated: Laser color is now a variable in load ship nodes
  - Removed: Explore mode
  - Removed: Tiling code and death star tiles

04/01/24 - v0.9.41 (Unity 2022.3.12f1)
  - NOTE: This is the only and final release of the game with the 'explore' mode. The feature is no longer in development and will be removed from future releases. But I made this release to preserve the code and to give an example of what it would have been like.
  - Updated: Final update for explore mode
  - Updated: More powerful region data categories
  - Updated: Mission Briefing now plays audio
  - Added: AI now checks if line of fire is clear of friendlies
  - Updated: Hyperspace activation sequence
  - Added: Ignore collision function when creating smallships
  - Added: display hyperdrive state on hud
  - Added: hyperspace location selection brace
  - Updated: GC lambda textures and reposition cockpit
  - Added: Procedural Position loading without clashes
  - Updated: Ship loading in explore has started
  - Added: Load location profile data
  - Updated: Improved planet rotation ability
  - Updated: Hyperspace tunnel with reflection probe
  - Fixed: Ship being destroyed during hyperspace
  - Added: Change location for explore
  - Added: Get Locations Function
  - Updated: Now possible to run explore mode
  - Updated: Base explore mode code
  - Added: Initial code for explore mode
  - Added: Lock ship controls function
  - Added: Starfield stretch function for hyperspace
  - Updated: Change location function
  - Added: Change Location Function/Node
  - Added: Hyperspace Shader
  - Updated: Hud Selection Brace

04/12/23 - v.0.9.1 (Unity 2022.3.12f1)
  - Updated: Hud Updated
  - Updated: Minor Hud modifications
  - Removed: Defunct missions and menu options
  - Fixed: Node dragging in editor plus other things
  - Added: Nav Buoy
  - Fixed: Next target not moving on from current selection
  - Added: Mission Objectives Function
  - Updated: Multiple editor nodes
  - Updated: music system
  - Added: new play track node
  - Updated: Audio now runs through audio mixer
  - Updated: Cargo and Allegiance Types
  - Updated: All mission functions take mission event variable
  - Updated: Node functions renamed and reorganised
  - Added: Multi track events
  - Added: Hyperspace in and out function
  - Updated: overhauled load ship functions
  - Added: New loading patterns for ships
  - Added: Scan ship cargo function
  - Updated: Clean up mission editor
  - Updated: Save function now correctly saves over old file
  - Updated: Added all event descriptions to mission editor
  - Updated: Add Event Window with scrollable description
  - Added: Reload main menu function
  - Added: About window to mission editor
  - Added: Open Web Links Function
  - Fixed: base data not loading when new node created
  - Fixed: Data is correctly parsed when saving
  - Updated: Save function now closes menu when run
  - Added: Bar indicating the file that is being edited
  - Added; Added scrollbar to window scrollview
  - Added: Close function to editor windows
  - Updated: Add Mission Event Node
  - Updated: Menu buttons now work
  - Updated: Mission Editor menus now close
  - Added: Large message box
  - Added: Drop down menus
  - Added: Information bar at bottom of mission editor
  - Added: editor scale restrictions
  - Added: Zoom indicator in editor
  - Fixed: Editor caret now displays correctly
  - Added: increase size of some input fields
  - Fixed: Node titles overflowing
  - Added: new mission function 'ifshipisactive'
  - Fixed: editor data not loading and saving
  - Updated: Missions now load in mission editor
  - Added: Load Mission Node to Editor
  - Added: node links connect when loading mission in editor
  - Added: Save function to mission editor
  - Added: Save menu in mission editor
  - Added: Finished adding nodes to mission editor
  - Added: More mission function nodes to mission editor
  - Added: Two new node functions to mission editor
  - Added: All preload functions to mission editor
  - Updated: Added mission events list to add mission event function
  - Updated: Node links now fully functional
  - Added: Mission event data to node and node data saving
  - Updated: Node link scaling and placement
  - Added: Draw line function to visually connect nodes
  - Added: Node links to mission editor
  - Updated: Add node function now works correctly
  - Added: New Mission Editor Menu "Add Node"
  - Added: Menu nodes to mission editor
  - Added: New Main Menu Node to Node System
  - Added: Draw button function to node system
  - Added: Drop down menu to node draw functions
  - Updated: Correctly implemented node dragging in editor
  - Added: Initial Code for Dragging Nodes
  - Added: Added basic node loading functionality to mission editor
  - Added: Exit button to mission editor
  - Added: Run editor from main menu
  - Added: Basic Code for Mission Editor

01/10/23 - v0.7.6 (Unity 2022.2.9f1)
  - Fixed: Menu doesn't load when custom mission folder doesn't exist
  - Updated: General Code Cleanup
  - Added: External Mission Loading
  - Updated: Improved SmallShip AI target selection
  - Updated: Reduced cost of looking for target
  - Fixed: Bombers not attacking capital ships
  - Fixed: Event loop at the beginning of mission three
  - Added: Optional mission briefing screen event

24/09/23 - v.0.6.61 (Unity 2022.2.9f1)
  - Added: Don't Attack Large Ships Function to Mission Functions
  - Fixed: Smallships targetting largeships before smallships
  - Updated: A-Wing cockpit lightmap texture added
  - Fixed: Ship now spins in a consistent direction
  - Updated: Turret placement code is complete

18/09/23 - v.0.6.5 (Unity 2022.2.9f1)
  - Updated: Misc improvements
  - Updated: Radar prefabs
  - Added: Basic Ground Turret Code
  - Fixed: UI Images, String Parse Bug, and Explode on collision bug
  - Added: Ships now spin before blowing up
  - Added: Set game area radius, Updated: set camera to cockpit pos not s…
  - Fixed: More loading issues
  - Added: More cockpit prefabs
  - Updated: Capital ship speed is now displayed in Hud
  - Updated SmallShipAIFunctions.cs
  - Fixed: Lasers no longer fire when ship is loaded
  - Fixed: ships firing when no enemy is present
  - Updated: More checks to prevent loading errors from incorrect settings

10/09/23 - v.0.6.3 (Unity 2022.2.9f1)
  - Updated: Cleaned up some mission functions
  - Fixed: Missions that broke with capital ship support
  - Updated: Minor modifications to prepare for node-based mission editor
  - Fixed: Enumeration error in avoid collision
  - Added: Two new in development missions
  - Added: Ability to change skybox
  - Added: Ability to set rotation of ship when loading
  - Updated: Scene now unloads tiles when mission is over
  - Fixed: Various reported loading problems

03/09/23 - v.0.6.2 (Unity 2022.2.9f1)
  - Updated: Death Star test mission with new messages and enemies
  - Added: Tile Loading to mission functions. 
  - Added: Death Star Mission - Our Moment of Triumph
  - Updated: death star tiles and tiling code
  - Added: Code for generating tiles
  - Updated: FS and GC cockpit models
  - Updated: First Strike X-Wing cockpit is now slightly more accurate
  - Fixed: Torpedoes not exploding 
  - Added: high resolution cockpit textures for FS
  - Updated: GC X-Wing cockpit improved

27/08/23 - v.0.5.7 (Unity 2022.2.9f1)
  - Added: Galactic Conquest Cockpits, Bomber attack pattern, dynamic thruster placement
  - Added: Turret explodes on destruction + some general cleaning up
  - Added: Cockpit rotation for lighting effects
  - Added: Toggle Cockpit Sets, Torpedoes damage cap ships
  - Fixed: turret tracking bud
  - Added: Turrets can be destroyed

21/08/23 - v.0.5.21 (Unity 2022.2.9f1)
  - WARNING: This version may temporarily break some missions like 'Assault on Empress Station' 
  - Fixed: Upside Down Turret Rotation Fixed, and Capital Ships are Destroyed
  - Added: All turret prefabs set up, initial code for upside down turrets
  - Fixed: capital ship jitter, turret firerates and damage, capital ship …
  - Added: Begun to add code to toggle cockpit sets
  - Added: Capital ships now deal and take laser damage
  - Fixed: laser color bug
  - Added: Capital ship turrets now fire lasers
  - Added: Implemented variable values for different turrets
  - Added: rotation restrictions on turrets
  - Updated: turret rotation improvements

13/08/23 - v.0.5.2 (Unity 2022.2.9f1)
  - Added: capital ship turret loading
  - Added: "In Development" mission category
  - Updated: Correctly Implemented Code for Avoiding Collisions with large objects
  - Fixed: bug that causes stationary "capital ships" (i.e. stations) to move
  - Added: Rudimentary capital ship support complete
  - Added: Created base functions for capital ships

08/08/23 - v.0.5.1 (Unity 2022.2.9f1)
  - Added: real cockpits, removed hud shake and hud glass
  - Added: dynamic head movement
  - Added: cockpit shake

05/08/23 - v.0.5.01 (Unity 2022.2.9f1)
  - Updated: aes_b1.wav
  - Fixed: torpedoes locking on instantly
  - Fixed: target representation not showing

02/08/23 - v.0.5.0 (Unity 2022.2.9f1)
  - Updated: cursor and fixed loading placement problems
  - Updated: all three training missions and the menu script
  - Updated: Minor changes to several missions
  - Added: the ladder imperial game mode
  - Added: the ladder rebel game mode
  - Added: dynamic messages to title and load screens

30/07/23 - v.0.49.1 (Unity 2022.2.9f1)
  - Added: new mission functions: set target, set ai override mode
  - Added: invert options and planet heightmap resolution options
  - Added: external loading and saving of game settings from the persistent data file
  - Added: Made separate loading pathways for standard audio and mission audio
  - Added: new mission - Corrans Nightmare Part 2

27/07/23 - v0.49.0
  - First Commit
