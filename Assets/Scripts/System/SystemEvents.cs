using System;

public static class SystemEvents
{
	public static Action audioLoadComplete;
	public static Action levelLoadComplete;
	public static Action<Notes> noteAdded;
	public static Action<Notes> noteRemoved;
	public static Action<Notes> noteEdited;
	public static Action noteRemoveAll;
	public static Action<int, float, string> timingChanged;
}