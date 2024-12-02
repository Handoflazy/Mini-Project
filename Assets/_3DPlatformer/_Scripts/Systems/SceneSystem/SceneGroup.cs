using Eflatun.SceneReference;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Systems.SceneManagement
{

    [Serializable]
    public class SceneGroup
    {

        public string GroupName = "New Scene Group";
        public List<SceneData> Scenes;

        public String FindSceneByType(SceneType sceneType) {
            return Scenes.FirstOrDefault(s=>s.SceneType == sceneType)?.Reference.Name;
        }


    }
    [Serializable]
    public class SceneData
    {
        public SceneReference Reference;
        public string Name => Reference.Name;
        public SceneType SceneType;

    }
}