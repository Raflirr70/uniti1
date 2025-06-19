using System.Collections.Generic;

[System.Serializable]
public class Item
{
    public string nama;
    public int jumlah;
}

[System.Serializable]
public class PlayerItem
{
    public List<Item> item = new List<Item>();
}
