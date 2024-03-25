// See https://aka.ms/new-console-template for more information
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using File = System.IO.File;

var clientId = "<Client App ID>";
var clientSecret = "<Client App Secret>";
var tenantName = "<tenant>.onmicrosoft.com";  //for example contosob2c.onmicrosoft.com
var PolicySignUpSignIn = "<sign-up or sign-in userflow>"; //for example b2c_1_susi
var AuthorityBase = $"https://<tenant>.b2clogin.com/tfp/{tenantName}/";
var Authority = $"{AuthorityBase}{PolicySignUpSignIn}";

string[] scopes = { $"<web API URI>/.default" };
string webApiUrl = "https://localhost:7137/WeatherForecast";
string accessToken = "";

// for logging purpose
/*
void Log(LogLevel level, string message, bool containsPii)
{
    string logs = $"{level} {message}{Environment.NewLine}";
    File.AppendAllText(System.Reflection.Assembly.GetExecutingAssembly().Location + ".msalLogs.txt", logs);
}
*/

IConfidentialClientApplication app;
app = ConfidentialClientApplicationBuilder.Create(clientId)
               .WithClientSecret(clientSecret)
               .WithB2CAuthority(Authority)
               // for logging purpose
               // .WithLogging(Log, LogLevel.Verbose, true)
               .Build();

try
{
    var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
    accessToken = result.AccessToken;
}
catch (Exception ex)
{
    Console.WriteLine (ex.Message);
    Console.WriteLine(ex.StackTrace);

}


var httpClient = new HttpClient();
HttpResponseMessage response;
try
{
    var request = new HttpRequestMessage(HttpMethod.Get, webApiUrl);
    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    response = await httpClient.SendAsync(request);
    var content = await response.Content.ReadAsStringAsync();
    Console.WriteLine("Http Response Code: " + response.StatusCode.ToString());
    Console.WriteLine("Http Respone: " + content);
}
catch (Exception ex)
{
   Console.WriteLine(ex.ToString());
}



