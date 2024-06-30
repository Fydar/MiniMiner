public partial class GameboyInput
{
    private static GameboyInput instance;

    public static GameboyInput Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameboyInput();
                instance.GameboyControls.Enable();
            }
            return instance;
        }
    }
}
