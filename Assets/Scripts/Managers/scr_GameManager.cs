using System;
using UnityEngine;

public class scr_GameManager : Singleton<scr_GameManager>
{
    public int StageNum = 0;
    [SerializeField] private scr_MusicZone[] _musicZones = new scr_MusicZone[5];

    private void Start()
    {
        ActivateStage(0);
    }

    public void NextStage()
    {
        _musicZones[StageNum].active = false;
        _musicZones[StageNum].OpenGates();
        StageNum++;
        ActivateStage(StageNum);
    }

    private void ActivateStage(int stageNum)
    {
        scr_MusicManager.Instance.PlayMusic(_musicZones[stageNum].music);
        _musicZones[stageNum].active = true;
        _musicZones[stageNum].HintMusic();
    }
}
