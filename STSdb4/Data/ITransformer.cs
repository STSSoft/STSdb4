namespace STSdb4.Data
{
    public interface ITransformer<T1, T2>
    {
        T2 To(T1 value1);
        T1 From(T2 value2);
    }
}
