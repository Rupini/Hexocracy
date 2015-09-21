namespace Hexocracy
{
    public enum MoveResult : byte
    {
        Ok = 0,
        NotEnoughMovePoints = 1,
        UnallowableHeight = 2,
        Impassable = 3,
        BadDestination = 4,
        AlreadyInFlight
    }
}
