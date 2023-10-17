using System.Collections.Generic;
namespace APIServices.Conflux.Models.User.Response
{
    public class UserReponse
    {
        public bool IsSuccess { get; set; } = false;
        public KeyValuePair<string, string> Error { get; set; }
    }
}
