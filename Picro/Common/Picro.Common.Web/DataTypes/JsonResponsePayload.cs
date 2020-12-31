namespace Picro.Common.Web.DataTypes
{
    public class JsonResponsePayload<T>
    {
        public bool Success { get; set; }

        public T? Data { get; set; }
    }

    public class JsonResponsePayload : JsonResponsePayload<object> { }
}