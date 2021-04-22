using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class FileReader
{
    private const string LineSplitRe = @"\r\n|\n\r|\n|\r";

    public static List<string> ReadNote(string fileName)
    {
        var t = Resources.Load<TextAsset>(fileName).text;
        var rawData = Regex.Split(t, LineSplitRe);
        var index = Array.IndexOf(rawData, "[Note]"); //비트맵 파일에서 [Note]로 시작하는 부분의 줄 번호를 찾는다
        var data = new List<string>(); //임시로 데이터를 내보낼 변수를 선언한다

        for (var i = 0; i < rawData.Length - index; i++) //비트맵 파일의 끝까지 반복문을 시작한다
        {
            //위 확인결과를 통해 정상적인 데이터라면 rawData 리스트에 추가한다
            try
            {
                if (rawData[index + i + 1] == "")
                    return data;

                if (rawData[index + i + 1] == " ")
                    return data;

                data.Add(rawData[index + i + 1]);
            }
            catch
            {
                // 예외처리하기 귀찮음
            }
        }

        return data;
    }

    public static List<string> ReadEvent(string fileName)
    {
        var t = Resources.Load<TextAsset>(fileName).text;
        var rawData = Regex.Split(t, LineSplitRe);
        var index = Array.IndexOf(rawData, "[Events]");
        var data = new List<string>();

        for (var i = 0; i < rawData.Length - index; i++)
        {
            try
            {
                if (rawData[index + i + 1] == "")
                    return data;

                if (rawData[index + i + 1] == " ")
                    return data;

                data.Add(rawData[index + i + 1]);
            }
            catch
            {
                // 예외처리하기 귀찮음
            }
        }

        return data;
    }

    public static List<string> ReadLevelInfo(string fileName)
    {
        var t = Resources.Load<TextAsset>(fileName).text;
        var rawData = Regex.Split(t, LineSplitRe);
        var index = Array.IndexOf(rawData, "[Level]"); //비트맵 파일에서 [Level]로 시작하는 부분의 줄 번호를 찾는다
        var data = new List<string>(); //임시로 데이터를 내보낼 변수를 선언한다

        for (var i = 0; i < rawData.Length - index; i++) //비트맵 파일의 끝까지 반복문을 시작한다
        {
            //위 확인결과를 통해 정상적인 데이터라면 rawData 리스트에 추가한다
            try
            {
                if (rawData[index + i + 1] == "")
                    return data;

                if (rawData[index + i + 1] == " ")
                    return data;

                data.Add(rawData[index + i + 1]);
            }
            catch
            {
                // 예외처리하기 귀찮음
            }
        }

        return data;
    }
}