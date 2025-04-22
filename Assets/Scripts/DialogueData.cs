using System;
using System.Collections.Generic;

[System.Serializable]
public class DialogueData
{
    public List<Stage> stage01;
}

[System.Serializable]
public class Stage
{
    public string girlName;
    public List<StageText> stagetext;
}

[System.Serializable]
public class StageText
{
    public List<string> stagephase00text;
}
