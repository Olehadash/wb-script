using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[System.Serializable]
public struct PlayerModel
{
    public string Name;
    public int Life;
    public int Coins;
}

[System.Serializable]
public class WordsModel
{
    [System.Serializable]
    public struct Word
    {
        public int id;
        public string word;
        public string eng;
        public string ger;
    }

    public List<Word> words;
    public Dictionary<string, string> dict = new Dictionary<string, string>();
}

[System.Serializable]
public class LettersModel
{
    [System.Serializable]
    public struct Letter
    {
        public string id;
        public int l1;
        public int l2;
        public int l3;
        public int l4;
        public int l5;
        public int l6;
        public int l7;
        public int l8;
        public int l9;
        public int l10;
        public int l11;
        public int l12;
        public int l13; 
        public int l14;
        public int l15;
        public int l16;
        public int l17;
        public int l18;
        public int l19;
        public int l20;
        public int l21;
        public int l22;
        public int l23;
        public int l24;
        public int l25;
        public int l26;
        public int l27;
        public int l28;
        public int l29;
        public int l30;
        public int l31;
        public int l32;

    }

    public Letter[] letters;

    public Dictionary<string, List<int>> let = new Dictionary<string, List<int>>();

    public void SetLeters()
    {
        for(int i =0; i<letters.Length; i++)
        {
            List<int> num = new List<int>();
            num.Add(letters[i].l1);
            num.Add(letters[i].l2);
            num.Add(letters[i].l3);
            num.Add(letters[i].l4);
            num.Add(letters[i].l5);
            num.Add(letters[i].l6);
            num.Add(letters[i].l7);
            num.Add(letters[i].l8);
            num.Add(letters[i].l9);
            num.Add(letters[i].l10);
            num.Add(letters[i].l11);
            num.Add(letters[i].l12);
            num.Add(letters[i].l13);
            num.Add(letters[i].l14);
            num.Add(letters[i].l15);
            num.Add(letters[i].l16);
            num.Add(letters[i].l17);
            num.Add(letters[i].l18);
            num.Add(letters[i].l19);
            num.Add(letters[i].l20);
            num.Add(letters[i].l21);
            num.Add(letters[i].l22);
            num.Add(letters[i].l23);
            num.Add(letters[i].l24);
            num.Add(letters[i].l25);
            num.Add(letters[i].l26);
            num.Add(letters[i].l27);
            num.Add(letters[i].l28);
            num.Add(letters[i].l29);
            num.Add(letters[i].l30);
            num.Add(letters[i].l31);
            num.Add(letters[i].l32);
            let.Add(letters[i].id, num);
        }
    }
}

[System.Serializable]
public class GameMessage
{
    [System.Serializable]
    public struct Message
    {
        public int x;
        public int y;
    }

    public List<Message> message = new List<Message>();
}

[System.Serializable]
public class RoomsModel
{
    [System.Serializable]
    public struct Room
    {
        public int id;
        public string Name;
        public string CreatorName;
        public string ConnectorName;
        public int isShown;
    }

    public List<Room> rooms = new List<Room>();
}

class GLobalParametrs
{
    public static WordsModel dictionary;
    public static LettersModel letters;
    public static PlayerModel Player = new PlayerModel();
    public static PlayerModel Anemy = new PlayerModel();
    public static RoomsModel rooms = new RoomsModel();
    public static RoomsModel.Room playRoom;
    public static string Word = "";
    public static string Translate = "";
    public static bool Pause = false;
    public static bool Presed = false;
    public static Turn turn = Turn.None;
    public static List<ButtonViewController> butContainer = new List<ButtonViewController>();
    public static GameMessage gmessage = new GameMessage();
    
}
