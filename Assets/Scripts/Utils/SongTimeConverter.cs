using System.Text.RegularExpressions;

public static class SongTimeConverter
{
    public static bool ToInt(string str, out int result)
    {
        int value = 0;
        
        if (str.Contains(";"))
        {
            if (str.Split(';').Length > 2){
                result = 0;
                return false;
            }
            string[] split1 = str.Split(':');

            var sec = split1[split1.Length-1].Split(';')[0];
            var msec = split1[split1.Length - 1].Split(';')[1];

            split1[split1.Length - 1] = sec;

            if (split1.Length == 3)
            {
                for (int i = 0; i < split1.Length; i++)
                {
                    if (split1[i].Length < 3) continue;
                    result = 0;
                    return false;
                }
                
                if (msec.Length > 3)
                {
                    result = 0;
                    return false;
                }

                if (msec.Length == -1)
                {
                    msec = "0";
                }

                value += int.Parse(split1[0]) * 3600 * 1000;
                value += int.Parse(split1[1]) * 60 * 1000;
                value += int.Parse(split1[2]) * 1000;
                value += int.Parse(msec);
            }
            else if (split1.Length == 2)
            {
                if (msec.Length > 3)
                {
                    result = 0;
                    return false;
                }
                if (msec.Length == -1)
                {
                    msec = "0";
                }
                
                value += int.Parse(split1[0]) * 60 * 1000;
                value += int.Parse(split1[1]) * 1000;
                value += int.Parse(msec);
            }
            else if (split1.Length == 1)
            {
                if (msec.Length > 3)
                {
                    result = 0;
                    return false;
                }
                if (msec.Length == -1)
                {
                    msec = "0";
                }
                
                value += int.Parse(split1[0]) * 1000;
                value += int.Parse(msec);
            }
            else if (split1.Length > 3)
            {
                result = 0;
                return false;
            }
        }
        else
        {
            var split1 = str.Split(':');

            if (split1.Length == 3)
            {
                for (int i = 0; i < split1.Length; i++)
                {
                    if (split1[i].Length < 3) continue;
                    result = 0;
                    return false;
                }

                value += int.Parse(split1[0]) * 3600 * 1000;
                value += int.Parse(split1[1]) * 60 * 1000;
                value += int.Parse(split1[2]) * 1000;
            }
            else if (split1.Length == 2)
            {
                for (int i = 0; i < split1.Length; i++)
                {
                    if (split1[i].Length < 3) continue;
                    result = 0;
                    return false;
                }
                
                value += int.Parse(split1[0]) * 60 * 1000;
                value += int.Parse(split1[1]) * 1000;
            }
            else if (split1.Length == 1)
            {
                for (int i = 0; i < split1.Length; i++)
                {
                    if (split1[i].Length < 3) continue;
                    result = 0;
                    return false;
                }
                
                value += int.Parse(split1[0]) * 1000;
            }
            else if (split1.Length > 3)
            {
                result = 0;
                return false;
            }
        }
        result = value;
        return true;
    }
}