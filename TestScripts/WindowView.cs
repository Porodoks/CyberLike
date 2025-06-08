namespace Assets.Test
{
    public class WindowView : BuildingPartView
    {
        public WindowData GetData()
        {
            return new WindowData(this);
        }
    }
}
