using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public enum DataType
{
    Info,
    Timings,
    Events,
    Note
}

public static class FileReader
{
    private const string LineSplitRe = @"\r\n|\n\r|\n|\r";

    public static List<string> ReadLevelData(string file, DataType type)
    {
        var rawData = Regex.Split(file, LineSplitRe);

        var index = 0;
        
        if (type == DataType.Info)
        {
            index = Array.IndexOf(rawData, "[Info]");
        }
        if (type == DataType.Timings)
        {
            index = Array.IndexOf(rawData, "[Timings]");
        }
        if (type == DataType.Events)
        {
            index = Array.IndexOf(rawData, "[Events]");
        }
        if (type == DataType.Note)
        {
            index = Array.IndexOf(rawData, "[Note]");
        }

        var data = new List<string>(); //임시로 데이터를 내보낼 변수를 선언한다

        for (var i = 0; i < rawData.Length - index; i++) //비트맵 파일의 끝까지 반복문을 시작한다
        {
            //위 확인결과를 통해 정상적인 데이터라면 rawData 리스트에 추가한다
            try
            {
                if (rawData[index + i + 1] == "")
                {
                    return data;
                }
                
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