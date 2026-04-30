namespace PayRoleSystem.Http
{
    public class ResponseModel<T>
    {
        public int MessageType { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
        public int HttpStatusCode { get; set; }
        public IList<string> Errors { get; set; }

        public long? AuditId { get; set; }


        public ResponseModel()
        {
            Errors = new List<string>(); 
        }
    }
}
