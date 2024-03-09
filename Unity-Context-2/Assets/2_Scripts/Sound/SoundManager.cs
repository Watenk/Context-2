using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : IFixedUpdateable
{
    private List<LoopingSound> loopingSounds = new List<LoopingSound>();
    private List<Sound> sounds = new List<Sound>();
    private List<Sound> soundsGC = new List<Sound>();
    private GameObject soundPrefab;
    private List<PlayerSoundData> playerChimes;
    private List<NPCSoundData> npcChimes;

    //-------------------------------------------------------

    public SoundManager(){
        soundPrefab = SoundSettings.Instance.SoundPrefab;
        playerChimes = SoundSettings.Instance.PlayerChimes;
        npcChimes = SoundSettings.Instance.NPCChimes;

        #if UNITY_EDITOR
            if (soundPrefab == null) { Debug.LogWarning("SoundPrefab is null in AudioSettings"); }
        #endif
    }

    public void OnFixedUpdate(){
        foreach (Sound currentSound in sounds){
            currentSound.OnFixedUpdate();
        }

        // GC
        foreach (Sound currentSound in soundsGC){
            sounds.Remove(currentSound);
        }
        soundsGC.Clear();
    }

    public LoopingSound PlayLoopingSound(PlayerSoundData loopingSoundData, Vector3 pos){
        GameObject soundGameObject = GameObject.Instantiate(soundPrefab, pos, Quaternion.identity);
        soundGameObject.transform.SetParent(GameManager.Instance.transform);

        LoopingSound newLoopingSound = new LoopingSound(this, soundGameObject, loopingSoundData.StartWwiseEvent, loopingSoundData.StopWwiseEvent);
        loopingSounds.Add(newLoopingSound);
        return newLoopingSound;
    }

    public Sound PlaySound(NPCSoundData soundData, Vector3 pos){
        GameObject soundGameObject = GameObject.Instantiate(soundPrefab, pos, Quaternion.identity);
        soundGameObject.transform.SetParent(GameManager.Instance.transform);

        Sound newSound = new Sound(this, soundGameObject, soundData.WwiseEvent);
        sounds.Add(newSound);
        return newSound;
    }

    public void StopLoopingSound(LoopingSound sound){
        GameObject.Destroy(sound.soundGameObject);
        loopingSounds.Remove(sound);
    }

    // You dont have to call StopSound Manually
    public void StopSound(Sound sound){
        GameObject.Destroy(sound.SoundGameObject);
        soundsGC.Add(sound);
    }

    public PlayerSoundData GetPlayerSound(ChimeInputs chimeInput){
        return playerChimes.Find(chime => chime.ChimeInput == chimeInput);
    }

    public NPCSoundData GetNPCSound(ChimeTasks chimeTask){
        return npcChimes.Find(chime => chime.ChimeTask == chimeTask);
    }
}
