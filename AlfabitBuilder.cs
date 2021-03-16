using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AlfabitMessage
{
    [System.Serializable]
    public struct Letter
    {
        public int idx;
        public int idy;
        public string let;
    }

    public List<Letter> letters = new List<Letter>();
}

public class AlfabitBuilder : MonoBehaviour
{
    #region Singlton
    private static AlfabitBuilder instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private static bool isNullreference
    {
        get
        {
            return instance == null;
        }
    }

    public static AlfabitBuilder GetInstance
    {
        get
        {
            return instance;
        }
    }
    private void OnDestroy()
    {
        instance = null;
    }

    #endregion

    #region Private Field
    private float count = 885885;
    private string[,] field = new string[5, 5];
    List<string> keys = new List<string>();
    AlfabitMessage message = new AlfabitMessage();
    #endregion

    private void Start()
    {
        if (GLobalParametrs.playRoom.CreatorName.Equals(GLobalParametrs.Player.Name))
        {
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    field[i, j] = "";

            buildFields();
            Sync();
        }
    }

    #region Sincronize
    public void Sync()
    {
        CallMessage msg = new CallMessage();
        msg.from = GLobalParametrs.Player.Name;
        msg.to = GLobalParametrs.Anemy.Name;
        msg.comand = "alphabit";
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 5; j++) {
                AlfabitMessage.Letter let = new AlfabitMessage.Letter();
                let.idx = i;
                let.idy = j;
                let.let = field[i, j];
                message.letters.Add(let);
            }
        }
        msg.message = JsonUtility.ToJson(message);
        WEbSocketController.GetInstance.SendMessage(JsonUtility.ToJson(msg));

    }

    public void GetSyncAlphabit(string mess)
    {
        message = JsonUtility.FromJson<AlfabitMessage>(mess);
        for(int i = 0; i< message.letters.Count; i++)
        {
            field[message.letters[i].idx, message.letters[i].idy] = message.letters[i].let;
        }
    }
    #endregion
    #region Getters
    public string GetLetter(int x, int y)
    {
        if (isNullreference) return "";
        return field[x, y];
    }

    public void SetLetter(int x, int y)
    {
        field[x, y] = GenerateLetter(x,y);
    }
    #endregion
    #region Publc Methods
    public void buildFields()
    {

        /*field[0, 0] = FirstLetter();
        field[0, 1] = GenerateLetter(0, 1);*/

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++) {
                if (i == 0 && j == 0)
                    field[i, j] = FirstLetter();
                else
                    field[i, j] = GenerateLetter(i, j);
            }
        }
    }

    public void PrintResult()
    {
        for (int i = 0; i < 5; i++)
        {
            string line = "";
            for (int j = 0; j < 5; j++)
            {
                line += field[i, j] + " : ";
            }
            Debug.Log(line);
        }
    }

    public string GenerateLetter(int x, int y)
    {
        List<int> result = ChanceCollision(x, y);
        List<float> chance = new List<float>();
        float max = 0;
        string letter = "";
        for (int i = 0; i < result.Count; i++)
        {
            chance.Add((float)result[i] / (count / 100));
            max += chance[i];
        }
        float generate = Random.Range(0, max);
        float chan = 0;
        for (int i = 0; i < result.Count-1; i++)
        {
            if(generate > chan && generate< chan+ chance[i])
            {
                letter = keys[i];
                break;
            }
            else
            {
                chan += chance[i];
            }
        }

        if (isRepeat(letter, x + 1, y) || isRepeat(letter, x + 1, y + 1) || isRepeat(letter, x + 1, y - 1) || isRepeat(letter, x, y + 1)
            || isRepeat(letter, x, y - 1) || isRepeat(letter, x - 1, y) || isRepeat(letter, x - 1, y + 1) || isRepeat(letter, x - 1, y - 1))
            letter = GenerateLetter(x, y);

        return letter;
    }
    #endregion

    #region Private Methods
    string FirstLetter()
    {
        
        foreach (var val in GLobalParametrs.letters.let)
        {
            keys.Add(val.Key);
        }
        int n = Random.Range(0, keys.Count);
        return keys[n];
    }
    bool isRepeat(string s,int x, int y)
    {
        if (!isExist(x, y)) return false;
        return s.Equals(field[x, y]);
    }

    List<int> ChanceCollision(int x, int y)
    {
        //if (GLobalParametrs.letters.let.ContainsKey(field[x, y])) return null;
        List<List<int>> property = new List<List<int>>();
        List<int> result = new List<int>();
        if (isExist(x + 1, y)) property.Add(GLobalParametrs.letters.let[field[x+1,y]]);
        if (isExist(x + 1, y+1)) property.Add(GLobalParametrs.letters.let[field[x + 1, y+1]]);
        if (isExist(x + 1, y-1)) property.Add(GLobalParametrs.letters.let[field[x + 1, y-1]]);
        if (isExist(x - 1, y)) property.Add(GLobalParametrs.letters.let[field[x - 1, y]]);
        if (isExist(x - 1, y+1)) property.Add(GLobalParametrs.letters.let[field[x - 1, y+1]]);
        if (isExist(x - 1, y-1)) property.Add(GLobalParametrs.letters.let[field[x - 1, y-1]]);
        if (isExist(x, y+1)) property.Add(GLobalParametrs.letters.let[field[x, y + 1]]);
        if (isExist(x, y-1)) property.Add(GLobalParametrs.letters.let[field[x, y - 1]]);

        //Debug.Log(property.Count);

        if (property.Count == 1) return property[0];

        for (int i = 0; i< GLobalParametrs.letters.let.Count; i++)
        {
            result.Add(0);
            for(int j = 0; j<property.Count;j++)
            {
                result[i] += property[j][i];
            }
            if(result[i]>0)
                result[i] /= property.Count;
        }



        return result;

    }
    
    bool isExist(int x, int y)
    {
        if (x < 0 || y < 0 || x >= 5 || y >= 5 || string.IsNullOrEmpty(field[x, y])) return false;
        //Debug.Log(field[x, y] + " : " + x.ToString() + " : " + y.ToString());
        return true;
    }
    #endregion
}
