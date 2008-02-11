namespace Expergent.Collections
{
    public interface IEntry
    {
        object Key { get; }
        IEntry Next { get; set; }
        object Value { get; set; }
        void Clear();
    }
}