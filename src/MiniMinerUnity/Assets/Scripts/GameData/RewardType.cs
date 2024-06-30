using System.Linq;
using UnityEngine;

namespace MiniMinerUnity
{
    [CreateAssetMenu]
    public class RewardType : ScriptableObject
    {
        public int CurrencyValue;
        public int StarRating;
        public string DisplayName;
        public Sprite IconGraphic;
        public Sprite MiningGraphic;
        public SfxGroup FindSound;
        public RewardDigPosition[] DigPositions;

        [Space]
        [Range(0.0f, 1.0f)]
        public float ChanceOfApperence = 1.0f;
        public int MinimumGems = 1;

        public int Width => DigPositions.Select(dig => dig.Offset.x).Max() + 1;

        public int Height => DigPositions.Select(dig => dig.Offset.y).Max() + 1;

        public int Footprint => Width * Height;
    }
}