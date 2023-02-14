using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class Globals
{
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

    public static int DamageFormula(int atk, int def)
    {
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
    
    public static double GetRandomNumber(double minimum, double maximum)
    { 
        Random random = new Random();
        return random.NextDouble() * (maximum - minimum) + minimum;
    }
    
    public static string RemoveRichText(string input)
    {
     
        input = RemoveRichTextDynamicTag(input, "color");
     
        input = RemoveRichTextTag(input, "b");
        input = RemoveRichTextTag(input, "i");
     
     
        // TMP
        input = RemoveRichTextDynamicTag(input, "align");
        input = RemoveRichTextDynamicTag(input, "size");
        input = RemoveRichTextDynamicTag(input, "cspace");
        input = RemoveRichTextDynamicTag(input, "font");
        input = RemoveRichTextDynamicTag(input, "indent");
        input = RemoveRichTextDynamicTag(input, "line-height");
        input = RemoveRichTextDynamicTag(input, "line-indent");
        input = RemoveRichTextDynamicTag(input, "link");
        input = RemoveRichTextDynamicTag(input, "margin");
        input = RemoveRichTextDynamicTag(input, "margin-left");
        input = RemoveRichTextDynamicTag(input, "margin-right");
        input = RemoveRichTextDynamicTag(input, "mark");
        input = RemoveRichTextDynamicTag(input, "mspace");
        input = RemoveRichTextDynamicTag(input, "noparse");
        input = RemoveRichTextDynamicTag(input, "nobr");
        input = RemoveRichTextDynamicTag(input, "page");
        input = RemoveRichTextDynamicTag(input, "pos");
        input = RemoveRichTextDynamicTag(input, "space");
        input = RemoveRichTextDynamicTag(input, "sprite index");
        input = RemoveRichTextDynamicTag(input, "sprite name");
        input = RemoveRichTextDynamicTag(input, "sprite");
        input = RemoveRichTextDynamicTag(input, "style");
        input = RemoveRichTextDynamicTag(input, "voffset");
        input = RemoveRichTextDynamicTag(input, "width");
     
        input = RemoveRichTextTag(input, "u");
        input = RemoveRichTextTag(input, "s");
        input = RemoveRichTextTag(input, "sup");
        input = RemoveRichTextTag(input, "sub");
        input = RemoveRichTextTag(input, "allcaps");
        input = RemoveRichTextTag(input, "smallcaps");
        input = RemoveRichTextTag(input, "uppercase");
        // TMP end
     
     
        return input;
     
    }
    
    private static string RemoveRichTextDynamicTag (string input, string tag)
    {
        int index = -1;
        while (true)
        {
            index = input.IndexOf($"<{tag}=");
            //Debug.Log($"{{{index}}} - <noparse>{input}");
            if (index != -1)
            {
                int endIndex = input.Substring(index, input.Length - index).IndexOf('>');
                if (endIndex > 0)
                    input = input.Remove(index, endIndex + 1);
                continue;
            }
            input = RemoveRichTextTag(input, tag, false);
            return input;
        }
    }
    private static string RemoveRichTextTag (string input, string tag, bool isStart = true)
    {
        while (true)
        {
            int index = input.IndexOf(isStart ? $"<{tag}>" : $"</{tag}>");
            if (index != -1)
            {
                input = input.Remove(index, 2 + tag.Length + (!isStart).GetHashCode());
                continue;
            }
            if (isStart)
                input = RemoveRichTextTag(input, tag, false);
            return input;
        }
    }
 
}