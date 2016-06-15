#r "Newtonsoft.Json"

using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"ValidateJson was triggered!");
    
    bool valid = true;
    IList<string> messages =null;

    string jsonContent = await req.Content.ReadAsStringAsync();
    dynamic data = JsonConvert.DeserializeObject(jsonContent);
    
    if (data.json == null || data.jsonSchema == null) {
        valid = false;
        messages = new List<string>() { "Please pass json & jsonSchema properties in the input object" };
        return req.CreateResponse(HttpStatusCode.BadRequest, new {
            messages
        });
    }
    
    log.Info(data.json.ToString());
    log.Info(data.jsonSchema.ToString());
    
    JSchema schema = JSchema.Parse(data.jsonSchema.ToString());
    //JObject jsonObject = JObject.Parse(data.json.ToString());
    //valid = jsonObject.IsValid(schema, out messages);

    return !valid 
        ? req.CreateResponse(HttpStatusCode.BadRequest, string.Join(",", messages.ToArray()))
        : req.CreateResponse(HttpStatusCode.OK, "valid json");
}