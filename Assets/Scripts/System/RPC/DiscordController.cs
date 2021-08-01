using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

public class DiscordController : MonoBehaviour
{
	public Discord.Discord discord;
	public ActivityManager activityManager;
	public ActivityAssets assets;

#if !UNITY_EDITOR
		private void Start()
        	{
        		assets.LargeImage = "icontest";
        		discord = new Discord.Discord(865849585778950175, (System.UInt64) CreateFlags.NoRequireDiscord);
        		activityManager = discord.GetActivityManager();
        		var activity = new Activity
        		{
        			State = "개발 중",
        			Details = "비트맵 에디터",
        			Assets = assets
        		};
        		activityManager.UpdateActivity(activity, (res) =>
        		{
        			if (res == Result.Ok)
        			{
        				Debug.Log("Everything is fine!");
        			}
        		});
        	}
        
        	private void Update()
        	{
        		if (!discord.Equals(null))
        			discord.RunCallbacks();
        	}
#endif
}