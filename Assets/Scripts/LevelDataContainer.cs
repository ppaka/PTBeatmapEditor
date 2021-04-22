using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class LevelDataContainer : MonoBehaviour
{
    private const string LineSplitRe = @"\r\n|\n\r|\n|\r";

    public List<string> level;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void SetLevel(string file)
    {
        var rawData = Regex.Split(file, LineSplitRe);
        var data = new List<string>(); //임시로 데이터를 내보낼 변수를 선언한다

        for (var i = 0; i < rawData.Length; i++) //비트맵 파일의 끝까지 반복문을 시작한다
        {
            //위 확인결과를 통해 정상적인 데이터라면 rawData 리스트에 추가한다
            try
            { 
                data.Add(rawData[i]);
            }
            catch
            {
                // 예외처리하기 귀찮음
            }
        }

        level = data;
    }
}