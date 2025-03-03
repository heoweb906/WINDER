using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPiece : CarriedObject
{
    public int iSoundPieceNum;
    public string sSoundName;
    

    public void PlayingPitchSound()
    {
        SoundAssistManager.Instance.GetSFXAudioBlock(sSoundName, gameObject.transform);
    }


}
