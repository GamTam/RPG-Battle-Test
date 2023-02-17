using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public static class Globals
{
    public static MusicManager MusicManager;
    public static SoundManager SoundManager;

    public static PlayerController Player;

    public static List<sItem> Items;

    public static bool BeginSceneLoad;

    public static Dictionary<string, ArrayList> LoadTSV(string file) {
        
        Dictionary<string, ArrayList> dictionary = new Dictionary<string, ArrayList>();
        ArrayList list = new ArrayList();

        using (var reader = new StreamReader(Application.dataPath + "/files/" + file + ".tsv")) {
            while (!reader.EndOfStream)
            {
                list = new ArrayList();
                var line = reader.ReadLine();
                if (line == null) continue;
                string[] values = line.Split('	');
                for (int i=1; i < values.Length; i++) {
                    list.Add(values[i]);
                }
                if (values[0] != "") dictionary.Add(values[0], list);
            }
        }

        return dictionary;
    }

    public static int DamageFormula(int atk, int def, out bool crit, int luck = 500)
    {
        int threshold = (int) ((luck / 400f) * 100f);
        int chance = GetRandomNumber(0, 101);

        crit = chance < threshold;
        
        return Mathf.Max(Mathf.RoundToInt((float) ((atk * 4 - def * 2) * GetRandomNumber(0.7f, 1.2f))), 1);
    }
    
    public static bool IsAnimationPlaying(Animator anim, string stateName, int animLayer=0)
    {
        if (anim.GetCurrentAnimatorStateInfo(animLayer).IsName(stateName) &&
            anim.GetCurrentAnimatorStateInfo(animLayer).normalizedTime < 1.0f)
            return true;
        
        return false;
    }
    
    public static int Mod(int x, int m) {
        int r = x%m;
        return r<0 ? r+m : r;
    }
    
    public static String NumberToChar(int num, bool capital) {
        Char c = (Char)((capital ? 65 : 97) + (num - 1));

        return c.ToString();
    }
    
    public static int GetRandomNumber(int minimum, int maximum)
    { 
        Random random = new Random();
        return random.Next(minimum, maximum);
    }
    
    public static double GetRandomNumber(double minimum, double maximum)
    { 
        Random random = new Random();
        return random.NextDouble() * (maximum - minimum) + minimum;
    }
    
    public static string RemoveRichText(string str)
    {
     
        str = RemoveRichTextDynamicTag(str, "color");
     
        str = RemoveRichTextTag(str, "b");
        str = RemoveRichTextTag(str, "i");
        
        str = RemoveRichTextDynamicTag(str, "align");
        str = RemoveRichTextDynamicTag(str, "size");
        str = RemoveRichTextDynamicTag(str, "cspace");
        str = RemoveRichTextDynamicTag(str, "font");
        str = RemoveRichTextDynamicTag(str, "indent");
        str = RemoveRichTextDynamicTag(str, "line-height");
        str = RemoveRichTextDynamicTag(str, "line-indent");
        str = RemoveRichTextDynamicTag(str, "link");
        str = RemoveRichTextDynamicTag(str, "margin");
        str = RemoveRichTextDynamicTag(str, "margin-left");
        str = RemoveRichTextDynamicTag(str, "margin-right");
        str = RemoveRichTextDynamicTag(str, "mark");
        str = RemoveRichTextDynamicTag(str, "mspace");
        str = RemoveRichTextDynamicTag(str, "noparse");
        str = RemoveRichTextDynamicTag(str, "nobr");
        str = RemoveRichTextDynamicTag(str, "page");
        str = RemoveRichTextDynamicTag(str, "pos");
        str = RemoveRichTextDynamicTag(str, "space");
        str = RemoveRichTextDynamicTag(str, "sprite index");
        str = RemoveRichTextDynamicTag(str, "sprite name");
        str = RemoveRichTextDynamicTag(str, "sprite");
        str = RemoveRichTextDynamicTag(str, "style");
        str = RemoveRichTextDynamicTag(str, "voffset");
        str = RemoveRichTextDynamicTag(str, "width");
     
        str = RemoveRichTextTag(str, "u");
        str = RemoveRichTextTag(str, "s");
        str = RemoveRichTextTag(str, "sup");
        str = RemoveRichTextTag(str, "sub");
        str = RemoveRichTextTag(str, "allcaps");
        str = RemoveRichTextTag(str, "smallcaps");
        str = RemoveRichTextTag(str, "uppercase");
        
        return str;
     
    }
    
    private static string RemoveRichTextDynamicTag (string str, string tag)
    {
        int index = -1;
        while (true)
        {
            index = str.IndexOf($"<{tag}=");
            //Debug.Log($"{{{index}}} - <noparse>{input}");
            if (index != -1)
            {
                int endIndex = str.Substring(index, str.Length - index).IndexOf('>');
                if (endIndex > 0)
                    str = str.Remove(index, endIndex + 1);
                continue;
            }
            str = RemoveRichTextTag(str, tag, false);
            return str;
        }
    }
    private static string RemoveRichTextTag (string str, string tag, bool isStart = true)
    {
        while (true)
        {
            int index = str.IndexOf(isStart ? $"<{tag}>" : $"</{tag}>");
            if (index != -1)
            {
                str = str.Remove(index, 2 + tag.Length + (!isStart).GetHashCode());
                continue;
            }
            if (isStart)
                str = RemoveRichTextTag(str, tag, false);
            return str;
        }
    }

    public static IEnumerator LoadScene(string scene, bool additive) {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation;
        if (additive) asyncOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        else asyncOperation = SceneManager.LoadSceneAsync(scene);

        asyncOperation.completed += (AsyncOperation o) =>
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
        };
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                //Wait to you press the space key to activate the Scene
                if (BeginSceneLoad)
                {
                    asyncOperation.allowSceneActivation = true;
                }
            }

            yield return null;
        }

        BeginSceneLoad = false;
    }
}

[Serializable]
public struct sItem
{
    public AttackSO Item;
    public int Count;
}