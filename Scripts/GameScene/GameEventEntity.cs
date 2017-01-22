using System;

[System.Serializable]
public class Option{
	public string text;
	public int tag;
}
[System.Serializable]
public class GameEventEntity
{

	public enum EventType{
		Start = 0,
		Option = 1,
		NotOption = 2,
		End = -1
	};

	public int tag;
	public int type;
	public string[] characters;
	public string background;
	public string[] textList;

	public Option[] options;

	public int nextTag;
	public GameEventEntity ()
	{
	}
}


