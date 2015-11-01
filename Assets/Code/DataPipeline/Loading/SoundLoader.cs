using Assets.Code.DataPipeline.Providers;
using Assets.Code.Logic.Logging;
using Assets.Code.Utilities;

namespace Assets.Code.DataPipeline.Loading
{
    class SoundLoader
    {
        public static void LoadSounds(Logger logger, SoundProvider soundProvider, string folderLocation)
        {
            foreach (var fileName in FileServices.GetResourceFiles(folderLocation, ".ogg", ".wav", ".flac", ".mp3"))
            {
                var sound = FileServices.LoadAudioResource(fileName);

                if(sound != null){
                    sound.name = FileServices.GetEndOfResourcePath(fileName);
                    soundProvider.AddSound(sound);
				}
				else
                    logger.Log("WARNING! tried to load a sound into the provider but it was null! (file : " + fileName + ")");
            }
        }
    }
}
