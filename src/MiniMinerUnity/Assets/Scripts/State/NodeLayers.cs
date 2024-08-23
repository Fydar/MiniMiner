namespace MiniMinerUnity
{
    public struct NodeLayers
    {
        private byte data;

        public bool Surface
        {
            get => GetBit(ref data, 0);
            set => SetBit(ref data, 0, value);
        }

        public bool Gravel
        {
            get => GetBit(ref data, 1);
            set => SetBit(ref data, 1, value);
        }

        public bool Rock
        {
            get => GetBit(ref data, 2);
            set => SetBit(ref data, 2, value);
        }

        private static bool GetBit(ref byte source, byte index)
        {
            return (source & (1 << index)) != 0;
        }

        private static void SetBit(ref byte data, byte index, bool value)
        {
            data = (byte)(value
                ? data | (1 << index)
                : data & ~(1 << index));
        }

        public NodeLayers RemoveDestructableLayer()
        {
            if (Surface)
            {
                return new NodeLayers()
                {
                    data = data,
                    Surface = false
                };
            }
            if (Gravel)
            {
                return new NodeLayers()
                {
                    data = data,
                    Gravel = false
                };
            }
            return this;
        }
    }
}
