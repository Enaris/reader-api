using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.Services.Helpers
{
    public static class ErrorsHelper
    {
        public static KeyValuePair<string, string> FieldError(string field, string error)
        {
            return new KeyValuePair<string, string>(field, error);
        }

        public static KeyValuePair<string, IEnumerable<string>> FieldErrors(string field, IEnumerable<string> errors)
        {
            return new KeyValuePair<string, IEnumerable<string>>(field, new List<string>(errors));
        }

        public static OtherErrorsResponse DuplicateEmail()
            => new OtherErrorsResponse { Errors = OtherErrors.DuplicateEmail() };

        public static OtherErrorsResponse SthWrongHappend()
            => new OtherErrorsResponse { Errors = OtherErrors.SthWrongHappend() };

        public static OtherErrorsResponse BadLoginOrPassword()
            => new OtherErrorsResponse { Errors = OtherErrors.BadLoginOrPassword() };

    }

    public class OtherErrors
    {
        public IEnumerable<string> Login { get; set; }
        public IEnumerable<string> Email { get; set; }
        public IEnumerable<string> Others { get; set; }

        public static OtherErrors DuplicateEmail()
            => new OtherErrors { Email = new List<string> { "Email address already in database" } };

        public static OtherErrors SthWrongHappend()
            => new OtherErrors { Others = new List<string> { "Something wrong happend. Try again later" } };

        public static OtherErrors BadLoginOrPassword()
            => new OtherErrors { Login = new List<string> { "Login or password is invalid" } };
    }

    public class OtherErrorsResponse
    {
        public OtherErrors Errors { get; set; }
    }

}
