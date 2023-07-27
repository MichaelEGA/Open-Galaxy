
[System.Serializable]
public class MenuItem
{
    public string ParentMenu;
    public string Name;
    public string Description;
    public string Function;
    public string Type;
    public string Variable;
}

[System.Serializable]
public class MenuItems
{
    public MenuItem[] menuData;
}

