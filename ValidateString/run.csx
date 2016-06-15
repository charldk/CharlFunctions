#r "Newtonsoft.Json"

using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"ValidateString was triggered!");
    
    bool valid = true;
    IList<string> messages =null;

    string jsonContent = await req.Content.ReadAsStringAsync();
    dynamic data = JsonConvert.DeserializeObject(jsonContent);
    
    if (data.myString == null) {
        valid = false;
        messages = new List<string>() { "Please pass myString in the input object" };
        return req.CreateResponse(HttpStatusCode.BadRequest, new {
            messages
        });
    }
    string myString = data.myString;
    
    log.Info("Data passed in: " + myString);
    
    valid = myString.IndexOf("charl") >= 0;
    
    log.Info("Data passed in is " + valid ? "valid": "not valid");

    return !valid 
        ? req.CreateResponse(HttpStatusCode.BadRequest, string.Join(",", messages.ToArray()))
        : req.CreateResponse(HttpStatusCode.OK, "valid string");
}