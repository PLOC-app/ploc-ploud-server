namespace Ploc.Ploud.Library
{
    public enum WineSheetState
    {
        None = 0,

        Pending = 1,

        Cancelling = 2,

        Canceled = 4,

        Completed = 8,

        Sending = 16
    }
}
