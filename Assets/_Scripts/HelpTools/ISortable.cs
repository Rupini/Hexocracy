namespace Hexocracy.HelpTools
{
    public interface IRelaxable<T>
    {
        T Value { get; set; }
        int Cost { get; set; }
    }
}
