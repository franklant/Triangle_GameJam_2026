## PuzzleMananger.cs: 
- **TransitionToNextArea()**: 

```cs
if (targetSpawnID < 5)
{
	PlayerPrefs.SetInt("MusicLevel", targetSpawnID);	// change the level of music to level 2
}
```

## AudioManager Object
- *Audio Source* Component.
- *'audioController.cs'* component script.

## Reset Music Object
- *'resetMusic.cs'* component script.

# Additions
- [x] Added music to the *reality* and *mind* scene.
- [x] Added music to all *puzzle* scenes.
- [x] Added proper music progression to switch in between each layer.
