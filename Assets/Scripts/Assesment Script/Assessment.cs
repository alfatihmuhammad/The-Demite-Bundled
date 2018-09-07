using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System;
using System.Linq;
using UnityEngine.Networking;
//using SQLite4Unity3d;

public class Assessment : MonoBehaviour
{

    //wordinput
    public Text FieldQuest;
    public Text TextBoxQuest;
    public Text KeyWord;
    public IDbConnection dbconn;
    private string connectionString;

    //wordinputdatamuse
    public Text FieldAnswer;
    public Text TextBoxAnswer;

    //result assesment
    public Text TextBoxAssesment;

    //public Text debugText;

    public GameObject DataPhonetic;
    private SpeechRecognizer AccesControl;
    bool onData;
    int test;

    //assaesment
    int MH; // manhattan distance
    double EC; // Euclidean Distance
    int distance; // levi distance
    int sylA;
    int sylB;
    int ambA;
    int ambB;
    int subK0A;
    int subK0B;
    int subK1A;
    int subK1B;
    int subK2A;
    int subK2B;
    string phone;
    string ipa;
    float sim;
    int c;

    // Use this for initialization
    void Start()
    {
        AccesControl = DataPhonetic.GetComponent<SpeechRecognizer>();
        DataService("wordlist.db");
        ConnectionDB();


        //string conn = "URI=file:" + connectionString;
        //dbconn = (IDbConnection)new SqliteConnection(conn);
        keyWord();
        phoneticResult();
        //StartCoroutine(getUnityWebRequest());
       
    }


    // Update is called once per frame
    void Update()
    {
        onData = AccesControl.ConData;
        CopyText();
        //StartCoroutine(getUnityWebRequest());



    }

    public void ConnectionDB()
    {


        if (Application.platform != RuntimePlatform.Android)
        {

            connectionString = Application.dataPath + "/wordlist.db";
        }
        else
        {

            connectionString = Application.persistentDataPath + "/wordlist.db";
            if (!File.Exists(connectionString))
            {
                WWW load = new WWW("jar:file://" + Application.dataPath + "!/assets/" + "wordlist.db");
                while (!load.isDone) { }

                File.WriteAllBytes(connectionString, load.bytes);
            }
        }

        string conn = "URI=file:" + connectionString;
        dbconn = (IDbConnection)new SqliteConnection(conn);

    }
    public void ManhattanDistance()
    {
        LevenshteinDistance(phone, ipa);

        int a = Math.Abs(sylA - sylB);
        int b = Math.Abs(distance);
        int c = Math.Abs(subK0A - subK0B);
        int d = Math.Abs(subK1A - subK1B);
        int e = Math.Abs(subK2A - subK2B);
        

        MH = a + b + c + d + e ;

        
        //Debug.Log(a);
        //Debug.Log(b);
        //Debug.Log(c);
        //Debug.Log(d);
        //Debug.Log(e);
        Debug.Log(MH);
    }

    public void EuclideanDistance()
    {
        LevenshteinDistance(phone, ipa);
        int a = (sylA - sylB);
        int b = (distance);
        int c = (subK0A - subK0B);
        int d = (subK1A - subK1B);
        int e = (subK2A - subK2A);
        



        EC = Math.Sqrt((a * a) + (b * b) + (c * c) + (d * d) + (e * e));

        Debug.Log(EC);

    }

    public int LevenshteinDistance(string a, string b)
    {
        distance = 0;
        a = a.ToLower();
        b = b.ToLower();
        c = 0;

        // swap to save some memory O(min(a,b)) instead of O(a)
        if (a.Length > b.Length)
        {
            string tmp = a;
            a = b;
            b = tmp;
        }

        c = b.Length;

        

        int[] row = new int[a.Length + 1];
        // init the row
        for (var i = 0; i <= a.Length; i++)
        {
            row[i] = i;
        }

        // fill in the rest
        for (var i = 1; i <= b.Length; i++)
        {
            var prev = i;
            for (var j = 1; j <= a.Length; j++)
            {
                var val = 0;
                if (b[i - 1] == a[j - 1])
                {
                    val = row[j - 1]; // match
                }
                else
                {
                    val = Mathf.Min(row[j - 1] + 1, // substitution
                                   prev + 1,     // insertion
                                   row[j] + 1);  // deletion
                }
                row[j - 1] = prev;
                prev = val;
            }
            distance = row[a.Length] = prev;

        }


        sim = 100 - ((distance * 100) / c); 
        
       

        return distance;
    }

    public void AssesmentWord(string userInput)
    {
        //dataquest
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlquery = string.Format("SELECT * FROM WordList WHERE Word=\"{0}\"", userInput);
        dbcmd.CommandText = sqlquery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            phone = reader.GetString(2);
            sylA = reader.GetInt32(8);
            ambA = reader.GetInt32(9);
            subK0A = reader.GetInt32(10);
            subK1A = reader.GetInt32(11);
            subK2A = reader.GetInt32(12);
        }
        dbconn.Close();
        //DataMuse

        //sylB = syllables;

    }


    public void DataService(string DatabaseName)
    {
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
        }
        var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);
        //dbconn = (IDbConnection)new SqliteConnection(conn);
        while (!loadDb.isDone) { }
        File.WriteAllBytes(filepath, loadDb.bytes);

    }





    public void phoneticResult()
    {
        string PhoneticValue = findPhonetic(FieldQuest.text);
        TextBoxQuest.text = PhoneticValue;
        Debug.Log(PhoneticValue);
    }

    public void keyWord()
    {
        dbconn.Open();
        string word = "";
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlquery = string.Format("SELECT * FROM WordList ORDER BY RANDOM() LIMIT 1");
        dbcmd.CommandText = sqlquery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            word = reader.GetString(1);

        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();

        KeyWord.text = word;
    }

    string findPhonetic(string userInput)
    {
        dbconn.Open();
        string word = "";
        string phoneticResult = "";
        string indoPhonetic = "";
        string totalSyllables = "";
        string ambiguityRate = "";
        string subKategori0 = "";
        string subKategori1 = "";
        string subKategori2 = "";
        string similarWord = "";
        string type = "";
        string result = "";
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlquery = string.Format("SELECT * FROM WordList WHERE Word=\"{0}\"", userInput);
        dbcmd.CommandText = sqlquery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            word = reader.GetString(1);
            phoneticResult = reader.GetString(2);
            indoPhonetic = reader.GetString(3);
            totalSyllables = reader.GetInt32(8).ToString();
            ambiguityRate = reader.GetInt32(9).ToString();
            subKategori0 = reader.GetInt32(10).ToString();
            subKategori1 = reader.GetInt32(11).ToString();
            subKategori2 = reader.GetInt32(12).ToString();
            similarWord = reader.GetString(13);
            type = reader.GetString(14);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();

        result += "Word: " + word + "\n";
        result += "Phonetics: " + phoneticResult + "\n";
        result += "Syllables: " + totalSyllables + "\n";
        result += "Ambiguity: " + ambiguityRate + "\n";
        result += "Sub Kategori 0: " + subKategori0 + "\n";
        result += "Sub Kategori 1: " + subKategori1 + "\n";
        result += "Sub Kategori 2: " + subKategori2 + "\n";
        result += "Tipe: " + type + "\n";
        result += "Similar Word: " + similarWord + "\n";
        //var conn = new SQLiteConnection(Path.GetFileName(filename));
        //var query = conn.Table<Noun>().Where(a => a.Word.Equals(userInput)).FirstOrDefault();
        ////var query = conn.Query<Noun>("select phonetics from noun where word = ?", userInput);
        ////var query = conn.Query<Noun>("select phonetics from noun where word = ?", userInput);
        //return query.ToString();
        return result;
    }

    //datamuse

    IEnumerator getUnityWebRequest()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://api.datamuse.com/words?sl=" + FieldAnswer.text + "&md=r&ipa=1");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            string jsonString = fixJson(www.downloadHandler.text);
            DataMuse[] wordData = JsonHelper.FromJson<DataMuse>(jsonString);
            //TextBoxAssesment.text = "";
            TextBoxAnswer.text = "";
            
            string simWords = "";
            DataMuse last = wordData.Last();
            List<string> similarWords = new List<string>();
            var countAmbiguity = -1;
            foreach (var item in wordData)
            {
                if (item.score > 90)
                {
                    countAmbiguity++;
                    if (!item.word.Equals(FieldAnswer.text))
                    {
                        if (item.Equals(last))
                        {
                            simWords += item.word;
                        }
                        else
                        {
                            simWords += item.word + ", ";
                        }
                    }
                }
            }

            string word = wordData[0].word;
            int score = wordData[0].score;
            sylB = wordData[0].numSyllables;
            ambB = countAmbiguity;
            string[] pronFull = wordData[0].tags[0].Split(':');
            string pron = pronFull[0];

            string[] ipaFull = wordData[0].tags[1].Split(':');
            ipa = ipaFull[1];
            subK0B = countSubKategori0(FieldAnswer.text);
            subK1B = countSubKategori1(FieldAnswer.text);
            subK2B = countSubKategori2(FieldAnswer.text);

            TextBoxAnswer.text += "Word: " + word;
            TextBoxAnswer.text += "\nIPA: " + ipa;
            TextBoxAnswer.text += "\nSyllables: " + sylB;
            TextBoxAnswer.text += "\nAmbiguity: " + countAmbiguity;
            TextBoxAnswer.text += "\nSub Kategori 0: " + subK0B;
            TextBoxAnswer.text += "\nSub Kategori 1: " + subK1B;
            TextBoxAnswer.text += "\nSub Kategori 2: " + subK2B;
            TextBoxAnswer.text += "\nSimilar Words: " + simWords;

            AssesmentWord(FieldQuest.text);
            ManhattanDistance();
            EuclideanDistance();
            LevenshteinDistance(phone, ipa);

            
            

            TextBoxAssesment.text = "";
            TextBoxAssesment.text += "Manhattan: " + MH;
            TextBoxAssesment.text += "\nEuclidean: " + EC;
            TextBoxAssesment.text += "\nLevenstein: " + distance;
            TextBoxAssesment.text += "\nSimilarity: " + sim +"%";
            keyWord();
            phoneticResult();
            AccesControl.ConData = false;

        }
    }

    public void CopyText()
    {
        if (onData == true)
        {

            StartCoroutine(getUnityWebRequest());


        }
    }

    string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }

    //flmnoqsx
    int countSubKategori0(string word)
    {
        var count = word.Count(x => x == 'f' || x == 'l' || x == 'm' || x == 'n' || x == 'o' || x == 'q' || x == 's' || x == 'x');
        return count;
    }

    //abcdegjkptvz
    int countSubKategori1(string word)
    {
        var count = word.Count(x => x == 'a' || x == 'b' || x == 'c' || x == 'd' || x == 'e' || x == 'g' || x == 'j' || x == 'k' || x == 'p' || x == 't' || x == 'v' || x == 'z');
        return count;
    }

    //hiruwy
    int countSubKategori2(string word)
    {
        var count = word.Count(x => x == 'h' || x == 'i' || x == 'r' || x == 'u' || x == 'w' || x == 'y');
        return count;
    }
}


[Serializable]
public class DataMuse
{
    public string word;
    public int score;
    public int numSyllables;
    public string[] tags;

    public static DataMuse CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<DataMuse>(jsonString);
    }

}

