using UnityEngine;

namespace MiniMinerUnity
{
    [CreateAssetMenu]
    public class EquipmentItemTemplate : ScriptableObject
    {
        public string Identifier;
        public string DisplayName;
        public SfxGroup UpgradeSound;

        [Space]
        public Sprite TinyIcon;
        public Sprite SmallIcon;

        [Space]
        public int StartingLevel = -1;
        public EquipmentItemLevelData[] Levels = new EquipmentItemLevelData[1];
    }
}