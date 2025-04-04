using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCTechSpring2025MovieApp.Shared.Wrapper;

public class Response
{
    public bool Succeeded { get; set; }
    public string Message { get; set; } = string.Empty;
    public Dictionary<string, string[]> Errors { get; set; } = new();

    public Response()
    {
        Errors = new Dictionary<string, string[]>();
    }

    public Response(bool succeeded, string message)
    {
        Errors = new Dictionary<string, string[]>();
        Succeeded = succeeded;
        Message = message;
    }

    public Response(bool succeeded, string message, Dictionary<string, string[]> errors)
    {
        Succeeded = succeeded;
        Message = message;
        Errors = errors;
    }

    public static Response Success(string message)
    {
        return new Response(false, message);
    }
}
