using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NodeTypes
{
    #region generic nodes

    public static void Draw_TestNode(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "Test Node", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        NodeFunctions.DrawInputField(node, "Event Type", "none", 7, 5, -25, 12.5f, 90, 5f);

        List<string> options = new List<string>();
        options.Add("x-wing");
        options.Add("y-wing");
        options.Add("a-wing");

        NodeFunctions.DrawDropDownMenu(node, options, "Ship Type", "none", 7, 5, -40, 12.5f, 90, 5f);

        NodeFunctions.DrawNodeLink(node, 87.5f, -62.5f, 10, 10, "male");
    }

    public static void Draw_NodeNotAvaible(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "node not avaible", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        NodeFunctions.DrawText(node, "This node does not exist yet. Check again in later versions to see if it has been added.", 5, 5, drop, 20, 90);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_CustomNode(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "custom_node", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawInputField(node, "Event Type", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "data1", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "data2", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data3 = NodeFunctions.DrawInputField(node, "data3", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "data4", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "data5", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data6 = NodeFunctions.DrawInputField(node, "data6", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data7 = NodeFunctions.DrawInputField(node, "data7", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data8 = NodeFunctions.DrawInputField(node, "data8", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data9 = NodeFunctions.DrawInputField(node, "data9", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data10 = NodeFunctions.DrawInputField(node, "data10", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data11 = NodeFunctions.DrawInputField(node, "data11", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data12 = NodeFunctions.DrawInputField(node, "data12", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data13 = NodeFunctions.DrawInputField(node, "data13", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data14 = NodeFunctions.DrawInputField(node, "data14", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data15 = NodeFunctions.DrawInputField(node, "data15", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event 1", 7, 5);

        drop -= 15;

        node.nextEvent2 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event 2", 7, 5);

        drop -= 15;

        node.nextEvent3 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event 3", 7, 5);

        drop -= 15;

        node.nextEvent4 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event 4", 7, 5);

        drop -= 15 + 5;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    #endregion

    #region pre load event nodes

    public static void Draw_LoadScene(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "loadscene", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "loadscene", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        NodeFunctions.DrawText(node, "Leave this as 'none' if you want the game to randomly select a location each time.", 5, 5, drop, 20, 90);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PreLoad_LoadAsteroids(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadasteroids", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadasteroids", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "number", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        NodeFunctions.DrawText(node, "Leave this as 'none' or '0' if you want the asteroid number to be set by the location seed.", 5, 5, drop, 20, 90);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PreLoad_LoadPlanet(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadplanet", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadplanet", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        NodeFunctions.DrawText(node, "This node loads the planet set at the location set in the 'Load Scene' node.", 5, 5, drop, 20, 90);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PreLoad_LoadTiles(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadtiles", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadtiles", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "tile type", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x distance", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y distance", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "tile size", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "seed", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PreLoad_LoadMultipleShipsOnGround(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadmultipleshipsonground", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadmultipleshipsonground", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("tiefighter");
        options.Add("tieinterceptor");
        options.Add("tieadvanced");
        options.Add("tiebomber");
        options.Add("assaultgunboat");
        options.Add("xwing");
        options.Add("ywing");
        options.Add("awing");
        options.Add("dx9shuttle");
        options.Add("lambdashuttle");
        options.Add("z95headhunter");
        options.Add("escapepod");
        options.Add("bulkfreighter");
        options.Add("container");
        options.Add("corelliancorvette");
        options.Add("gr75transport");
        options.Add("stardestroyer");
        options.Add("superstardestroyer");
        options.Add("homeone");
        options.Add("nebulonbfrigate");
        options.Add("xq1station");
        options.Add("dsturrettall");
        options.Add("dsturretshort");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options, "ship type", "tiefighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "ship name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("imperial");
        options2.Add("rebel");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options2, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "number", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "no. per length", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data6 = NodeFunctions.DrawInputField(node, "variance", "10", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data7 = NodeFunctions.DrawInputField(node, "length", "1000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data8 = NodeFunctions.DrawInputField(node, "width", "1000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data9 = NodeFunctions.DrawInputField(node, "height", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("true");
        options3.Add("false");

        node.data10 = NodeFunctions.DrawDropDownMenu(node, options3, "randomise", "true", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options4 = new List<string>();
        options4.Add("true");
        options4.Add("false");

        node.data11 = NodeFunctions.DrawDropDownMenu(node, options4, "always load", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data12 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PreLoad_LoadSingleShip(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadsingleship", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadsingleship", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("tiefighter");
        options.Add("tieinterceptor");
        options.Add("tieadvanced");
        options.Add("tiebomber");
        options.Add("assaultgunboat");
        options.Add("xwing");
        options.Add("ywing");
        options.Add("awing");
        options.Add("dx9shuttle");
        options.Add("lambdashuttle");
        options.Add("z95headhunter");
        options.Add("escapepod");
        options.Add("bulkfreighter");
        options.Add("container");
        options.Add("corelliancorvette");
        options.Add("gr75transport");
        options.Add("stardestroyer");
        options.Add("superstardestroyer");
        options.Add("homeone");
        options.Add("nebulonbfrigate");
        options.Add("xq1station");
        options.Add("dsturrettall");
        options.Add("dsturretshort");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options, "ship type", "tiefighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options1 = new List<string>();
        options1.Add("true");
        options1.Add("false");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options1, "is AI", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("true");
        options2.Add("false");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options2, "randomise", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("imperial");
        options3.Add("rebel");

        node.data4 = NodeFunctions.DrawDropDownMenu(node, options3, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data6 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PreLoad_LoadMultipleShips(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadmultipleships", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadmultipleships", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("tiefighter");
        options.Add("tieinterceptor");
        options.Add("tieadvanced");
        options.Add("tiebomber");
        options.Add("assaultgunboat");
        options.Add("xwing");
        options.Add("ywing");
        options.Add("awing");
        options.Add("dx9shuttle");
        options.Add("lambdashuttle");
        options.Add("z95headhunter");
        options.Add("escapepod");
        options.Add("bulkfreighter");
        options.Add("container");
        options.Add("corelliancorvette");
        options.Add("gr75transport");
        options.Add("stardestroyer");
        options.Add("superstardestroyer");
        options.Add("homeone");
        options.Add("nebulonbfrigate");
        options.Add("xq1station");
        options.Add("dsturrettall");
        options.Add("dsturretshort");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options, "ship type", "tiefighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "number", "1", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "groups of", "4", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "ship distance", "50", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data6 = NodeFunctions.DrawInputField(node, "group distance", "250", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data8 = NodeFunctions.DrawInputField(node, "variance", "10", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("true");
        options2.Add("false");

        node.data9 = NodeFunctions.DrawDropDownMenu(node, options2, "randomise", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data10 = NodeFunctions.DrawInputField(node, "name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("true");
        options3.Add("false");

        node.data11 = NodeFunctions.DrawDropDownMenu(node, options3, "include player", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data12 = NodeFunctions.DrawInputField(node, "player no.", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data13 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PreLoad_LoadMultipleShipsByType(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadmultipleshipsbytype", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadmultipleshipsbytype", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("fighter");
        options.Add("bomber");
        options.Add("capital");
        options.Add("station");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options, "ship type", "fighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options1 = new List<string>();
        options1.Add("imperial");
        options1.Add("rebel");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options1, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "number", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "groups of", "4", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data6 = NodeFunctions.DrawInputField(node, "ship distance", "50", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data7 = NodeFunctions.DrawInputField(node, "group distance", "250", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data9 = NodeFunctions.DrawInputField(node, "variance", "10", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("true");
        options2.Add("false");

        node.data10 = NodeFunctions.DrawDropDownMenu(node, options2, "randomise", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data11 = NodeFunctions.DrawInputField(node, "name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("true");
        options3.Add("false");

        node.data12 = NodeFunctions.DrawDropDownMenu(node, options3, "include player", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data13 = NodeFunctions.DrawInputField(node, "player no", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data14 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PreLoad_SetSkybox(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_setskybox", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_setskybox", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("space");
        options2.Add("sky");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options2, "mode", "space", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    #endregion

    #region event nodes

    public static void Draw_StartMission(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "startmission", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "startmission", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_ChangeMusicVolume(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "changemusicvolume", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "changemusicvolume", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "volume", "100", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_ClearAIOverride(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "clearaioverride", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "clearaioverride", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_DisplayLargeMessageThenExit(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "displaylargemessagethenexit", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "displaylargemessagethenexit", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        float multiplySize = 2;

        node.data1 = NodeFunctions.DrawInputFieldLarge(node, "Message", "none", 7, 5, drop, 12.5f * multiplySize, 90);

        drop -= 15 * multiplySize;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_DialogueBox(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "dialoguebox", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "dialoguebox", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        float multiplySize = 5;

        node.data1 = NodeFunctions.DrawInputFieldLarge(node, "message", "none", 7, 5, drop, 12.5f * multiplySize, 90);

        drop -= 15 * multiplySize;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_DisplayLargeMessage(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "displaylargemessage", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "displaylargemessage", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        float multiplySize = 2;

        node.data1 = NodeFunctions.DrawInputFieldLarge(node, "Message", "none", 7, 5, drop, 12.5f * multiplySize, 90);

        drop -= 15 * multiplySize;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_DisplayLocation(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "displaylocation", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "displaylocation", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "text before", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "text after", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        NodeFunctions.DrawText(node, "Leave the inputs as 'none' if you dont want text to show before or after the location name.", 5, 5, drop, 20, 90);

        drop -= 15 +5;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_DisplayMissionBriefing(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "displaymissionbriefing", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "displaymissionbriefing", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        float multiplySize = 5;

        node.data1 = NodeFunctions.DrawInputFieldLarge(node, "Briefing", "none", 7, 5, drop, 12.5f * multiplySize, 90);

        drop -= 15 * multiplySize;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_IfTypeOfShipIsActive(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "iftypeofshipisactive", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "iftypeofshipisactive", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options1 = new List<string>();
        options1.Add("imperial");
        options1.Add("rebel");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options1, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "True", 7, 5);

        drop -= 15;

        node.nextEvent2 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "False", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_IfShipIsActive(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "ifshipisactive", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "ifshipisactive", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "True", 7, 5);

        drop -= 15;

        node.nextEvent2 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "False", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_IfShipHasBeenScanned(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "ifshiphasbeenscanned", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "ifshiphasbeenscanned", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "True", 7, 5);

        drop -= 15;

        node.nextEvent2 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "False", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_IfShipHasntBeenScanned(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "ifshiphasntbeenscanned", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "ifshiphasntbeenscanned", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "True", 7, 5);

        drop -= 15;

        node.nextEvent2 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "False", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_LoadShip(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "loadsingleship", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "loadsingleship", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("tiefighter");
        options.Add("tieinterceptor");
        options.Add("tieadvanced");
        options.Add("tiebomber");
        options.Add("assaultgunboat");
        options.Add("xwing");
        options.Add("ywing");
        options.Add("awing");
        options.Add("dx9shuttle");
        options.Add("lambdashuttle");
        options.Add("z95headhunter");
        options.Add("escapepod");
        options.Add("bulkfreighter");
        options.Add("container");
        options.Add("corelliancorvette");
        options.Add("gr75transport");
        options.Add("stardestroyer");
        options.Add("superstardestroyer");
        options.Add("homeone");
        options.Add("nebulonbfrigate");
        options.Add("xq1station");
        options.Add("dsturrettall");
        options.Add("dsturretshort");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options, "ship type", "tiefighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options1 = new List<string>();
        options1.Add("true");
        options1.Add("false");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options1, "is AI", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("true");
        options2.Add("false");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options2, "randomise", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("imperial");
        options3.Add("rebel");

        node.data4 = NodeFunctions.DrawDropDownMenu(node, options3, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data6 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_LoadSingleShipAtDistanceAndAngleFromPlayer(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "loadsingleshipatdistanceandanglefromplayer", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "loadsingleshipatdistanceandanglefromplayer", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "xAngle", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "yAngle", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "zAngle", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("tiefighter");
        options.Add("tieinterceptor");
        options.Add("tieadvanced");
        options.Add("tiebomber");
        options.Add("assaultgunboat");
        options.Add("xwing");
        options.Add("ywing");
        options.Add("awing");
        options.Add("dx9shuttle");
        options.Add("lambdashuttle");
        options.Add("z95headhunter");
        options.Add("escapepod");
        options.Add("bulkfreighter");
        options.Add("container");
        options.Add("corelliancorvette");
        options.Add("gr75transport");
        options.Add("stardestroyer");
        options.Add("superstardestroyer");
        options.Add("homeone");
        options.Add("nebulonbfrigate");
        options.Add("xq1station");
        options.Add("dsturrettall");
        options.Add("dsturretshort");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options, "ship type", "tiefighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("true");
        options2.Add("false");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options2, "randomise", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("imperial");
        options3.Add("rebel");

        node.data4 = NodeFunctions.DrawDropDownMenu(node, options3, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "distance", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data6 = NodeFunctions.DrawInputField(node, "name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data6 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_LoadMultipleShipsOnGround(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "loadmultipleshipsonground", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "loadmultipleshipsonground", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("tiefighter");
        options.Add("tieinterceptor");
        options.Add("tieadvanced");
        options.Add("tiebomber");
        options.Add("assaultgunboat");
        options.Add("xwing");
        options.Add("ywing");
        options.Add("awing");
        options.Add("dx9shuttle");
        options.Add("lambdashuttle");
        options.Add("z95headhunter");
        options.Add("escapepod");
        options.Add("bulkfreighter");
        options.Add("container");
        options.Add("corelliancorvette");
        options.Add("gr75transport");
        options.Add("stardestroyer");
        options.Add("superstardestroyer");
        options.Add("homeone");
        options.Add("nebulonbfrigate");
        options.Add("xq1station");
        options.Add("dsturrettall");
        options.Add("dsturretshort");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options, "ship type", "tiefighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "ship name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("imperial");
        options2.Add("rebel");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options2, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "number", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "no. per length", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data6 = NodeFunctions.DrawInputField(node, "variance", "10", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data7 = NodeFunctions.DrawInputField(node, "length", "1000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data8 = NodeFunctions.DrawInputField(node, "width", "1000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data9 = NodeFunctions.DrawInputField(node, "height", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("true");
        options3.Add("false");

        node.data10 = NodeFunctions.DrawDropDownMenu(node, options3, "randomise", "true", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;


        List<string> options4 = new List<string>();
        options4.Add("true");
        options4.Add("false");

        node.data11 = NodeFunctions.DrawDropDownMenu(node, options4, "always load", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data12 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_LoadMultipleShips(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "loadmultipleships", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "loadmultipleships", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("tiefighter");
        options.Add("tieinterceptor");
        options.Add("tieadvanced");
        options.Add("tiebomber");
        options.Add("assaultgunboat");
        options.Add("xwing");
        options.Add("ywing");
        options.Add("awing");
        options.Add("dx9shuttle");
        options.Add("lambdashuttle");
        options.Add("z95headhunter");
        options.Add("escapepod");
        options.Add("bulkfreighter");
        options.Add("container");
        options.Add("corelliancorvette");
        options.Add("gr75transport");
        options.Add("stardestroyer");
        options.Add("superstardestroyer");
        options.Add("homeone");
        options.Add("nebulonbfrigate");
        options.Add("xq1station");
        options.Add("dsturrettall");
        options.Add("dsturretshort");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options, "ship type", "tiefighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "number", "1", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "groups of", "4", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "ship distance", "50", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data6 = NodeFunctions.DrawInputField(node, "group distance", "250", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data8 = NodeFunctions.DrawInputField(node, "variance", "10", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("true");
        options2.Add("false");

        node.data9 = NodeFunctions.DrawDropDownMenu(node, options2, "randomise", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data10 = NodeFunctions.DrawInputField(node, "name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("true");
        options3.Add("false");

        node.data11 = NodeFunctions.DrawDropDownMenu(node, options3, "include player", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data12 = NodeFunctions.DrawInputField(node, "player no.", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data13 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_LoadMultipleShipsByType(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "loadmultipleshipsbytype", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "loadmultipleshipsbytype", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("fighter");
        options.Add("bomber");
        options.Add("capital");
        options.Add("station");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options, "ship type", "fighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options1 = new List<string>();
        options1.Add("imperial");
        options1.Add("rebel");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options1, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "number", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "groups of", "4", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data6 = NodeFunctions.DrawInputField(node, "ship distance", "50", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data7 = NodeFunctions.DrawInputField(node, "group distance", "250", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data9 = NodeFunctions.DrawInputField(node, "variance", "10", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("true");
        options2.Add("false");

        node.data10 = NodeFunctions.DrawDropDownMenu(node, options2, "randomise", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data11 = NodeFunctions.DrawInputField(node, "name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("true");
        options3.Add("false");

        node.data12 = NodeFunctions.DrawDropDownMenu(node, options3, "include player", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data13 = NodeFunctions.DrawInputField(node, "player no", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data14 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_LockMainShipWeapons(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "lockmainshipweapons", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "lockmainshipweapons", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("true");
        options.Add("false");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options, "lock weapons", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_Message(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "message", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "message" + "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        float multiplySize = 5;

        node.data1 = NodeFunctions.DrawInputFieldLarge(node, "Message", "none", 7, 5, drop, 12.5f * multiplySize, 90);

        drop -= 15 * multiplySize;

        node.data2 = NodeFunctions.DrawInputField(node, "audio file", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("true");
        options.Add("false");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options, "internal file", "true", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_MoveToWayPoint(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "movetowaypoint", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "movetowaypoint", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PlayMusicType(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "playmusictype", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "playmusictype", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("action");
        options.Add("tension");
        options.Add("theme");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options, "music type", "tension", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetAIOverride(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setaioverride", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setaioverride", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("none");
        options.Add("MoveToWaypoint");
        options.Add("Patrol");
        options.Add("Stationary");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options, "override", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetDontAttackLargeShips(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setdontattacklargeships", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setdontattacklargeships", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("true");
        options.Add("false");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options, "dont attack", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetShipAllegiance(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setshipallegiance", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setshipallegiance", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("imperial");
        options.Add("rebel");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetShipTarget(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setshiptarget", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setshiptarget", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "target", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetShipTargetToClosestEnemy(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setshiptargettoclosestenemy", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setshiptargettoclosestenemy", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetShipToInvincible(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setshiptoinvincible", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setshiptoinvincible", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("true");
        options.Add("false");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options, "invincible", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetWeaponsLock(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setweaponslock", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setweaponslock", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("true");
        options.Add("false");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options, "locked", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_ShipsHullIsLessThan(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "shipshullislessthan", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "shipshullislessthan", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "is less than", "50", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "True", 7, 5);

        drop -= 15;

        node.nextEvent2 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "False", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_ShipIsLessThanDistanceToWaypoint(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "shipislessthandistancetowaypoint", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "shipislessthandistancetowaypoint", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "is less than", "50", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "True", 7, 5);

        drop -= 15;

        node.nextEvent2 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "False", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    #endregion

}
