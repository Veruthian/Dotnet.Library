namespace Veruthian.Library.Readers
{
    public interface IRecollectiveReader<out T> : ISpeculativeReader<T>
    {
        void StoreProgress(object key, bool success, object data = null);

        (bool? success, int Length, object Data) RecallProgress(object key);
    }
}