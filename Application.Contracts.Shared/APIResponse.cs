namespace Application.Contracts.Shared
{
    /// <summary>
    /// to standardize the response from the API
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class APIResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }
}
