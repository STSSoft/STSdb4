namespace STSdb4.Data
{
    public class Data<T> : IData
    {
        public T Value;

        public Data()
        {
        }

        public Data(T value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
