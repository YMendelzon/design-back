//using Newtonsoft.Json;
//using System.Text;

//public class MailchimpService
//{
//    private readonly HttpClient _httpClient;
//    private readonly string _apiKey = "1f2a9a9ad444bad3c3271529e147c70a-us17";
//    private readonly string _serverPrefix = "us17";

//    private readonly string _listId = "e1737e366f";
//    private readonly string _fromName = "Designery";
//    private readonly string _replyTo = "d32193412@gmail.com";

//    public MailchimpService(HttpClient httpClient)
//    {
//        _httpClient = httpClient;
//        _httpClient.BaseAddress = new Uri($"https://{_serverPrefix}.api.mailchimp.com/3.0/");
//        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"anystring:{_apiKey}")));
//    }

//    public async Task AddEmailToListAsync(string email, string firstName)
//    {
//        var subscriber = new
//        {
//            email_address = email,
//            status = "subscribed", // אפשרויות נוספות: "pending", "unsubscribed"

//            merge_fields = new
//            {
//                FNAME = firstName
//            }

//        };

//        var content = new StringContent(JsonConvert.SerializeObject(subscriber), Encoding.UTF8, "application/json");

//        var response = await _httpClient.PostAsync($"lists/{_listId}/members", content);

//        // הוספת לוגים לתגובה
//        var responseBody = await response.Content.ReadAsStringAsync();
//        Console.WriteLine($"Status Code: {response.StatusCode}");
//        Console.WriteLine($"Response Body: {responseBody}");

//        response.EnsureSuccessStatusCode();
//    }


//    public async Task<List<string>> GetSubscribersAsync()
//    {
//        var response = await _httpClient.GetAsync($"lists/{_listId}/members");
//        response.EnsureSuccessStatusCode();

//        var jsonString = await response.Content.ReadAsStringAsync();
//        dynamic data = JsonConvert.DeserializeObject(jsonString);

//        var subscribers = new List<string>();
//        foreach (var member in data.members)
//        {
//            subscribers.Add(member.email_address.ToString());
//        }

//        return subscribers;
//    }
//    public async Task<List<string>> GetListsAsync()
//    {
//        var response = await _httpClient.GetAsync("lists");
//        response.EnsureSuccessStatusCode();

//        var jsonString = await response.Content.ReadAsStringAsync();
//        dynamic data = JsonConvert.DeserializeObject(jsonString);

//        var lists = new List<string>();
//        foreach (var list in data.lists)
//        {
//            lists.Add(list.id.ToString());
//        }

//        return lists;
//    }
//    //////////////////////////////////////////
//    ///
//    public async Task<string> CreateCampaignAsync(string subject)
//    {


//        var campaign = new
//        {
//            type = "regular",
//            recipients = new
//            {
//                list_id = _listId
//            },
//            settings = new
//            {
//                subject_line = subject,
//                from_name = _fromName,
//                reply_to = _replyTo
//            }
//        };

//        var content = new StringContent(JsonConvert.SerializeObject(campaign), Encoding.UTF8, "application/json");

//        var response = await _httpClient.PostAsync("campaigns", content);
//        response.EnsureSuccessStatusCode();

//        var jsonString = await response.Content.ReadAsStringAsync();
//        dynamic data = JsonConvert.DeserializeObject(jsonString);

//        return data.id;
//    }

//    public async Task AddContentToCampaignAsync(string campaignId, string htmlContent)
//    {
//        var content = new
//        {
//            html = htmlContent
//        };

//        var httpContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

//        var response = await _httpClient.PutAsync($"campaigns/{campaignId}/content", httpContent);
//        response.EnsureSuccessStatusCode();
//    }

//    public async Task SendCampaignAsync(string campaignId)
//    {
//        var response = await _httpClient.PostAsync($"campaigns/{campaignId}/actions/send", null);
//        response.EnsureSuccessStatusCode();
//    }
//}


using Newtonsoft.Json;
using System.Text;
using DotNetEnv;

public class MailchimpService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _serverPrefix;

    private readonly string _listId = "e1737e366f";
    private readonly string _fromName = "Designery";
    private readonly string _replyTo = "d32193412@gmail.com";

    public MailchimpService(HttpClient httpClient)
    {
        _httpClient = httpClient;

        // טען את הערכים מקובץ .env
        Env.Load();
        _apiKey = Environment.GetEnvironmentVariable("MAILCHIMP_API_KEY");
        _serverPrefix = Environment.GetEnvironmentVariable("MAILCHIMP_SERVER_PREFIX");

        _httpClient.BaseAddress = new Uri($"https://{_serverPrefix}.api.mailchimp.com/3.0/");
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"anystring:{_apiKey}")));
    }

    public async Task AddEmailToListAsync(string email, string firstName)
    {
        var subscriber = new
        {
            email_address = email,
            status = "subscribed", // אפשרויות נוספות: "pending", "unsubscribed"
            merge_fields = new
            {
                FNAME = firstName
            }
        };

        var content = new StringContent(JsonConvert.SerializeObject(subscriber), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"lists/{_listId}/members", content);

        // הוספת לוגים לתגובה
        var responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Status Code: {response.StatusCode}");
        Console.WriteLine($"Response Body: {responseBody}");

        response.EnsureSuccessStatusCode();
    }

    public async Task<List<string>> GetSubscribersAsync()
    {
        var response = await _httpClient.GetAsync($"lists/{_listId}/members");
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        dynamic data = JsonConvert.DeserializeObject(jsonString);

        var subscribers = new List<string>();
        foreach (var member in data.members)
        {
            subscribers.Add(member.email_address.ToString());
        }

        return subscribers;
    }

    public async Task<List<string>> GetListsAsync()
    {
        var response = await _httpClient.GetAsync("lists");
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        dynamic data = JsonConvert.DeserializeObject(jsonString);

        var lists = new List<string>();
        foreach (var list in data.lists)
        {
            lists.Add(list.id.ToString());
        }

        return lists;
    }

    public async Task<string> CreateCampaignAsync(string subject)
    {
        var campaign = new
        {
            type = "regular",
            recipients = new
            {
                list_id = _listId
            },
            settings = new
            {
                subject_line = subject,
                from_name = _fromName,
                reply_to = _replyTo
            }
        };

        var content = new StringContent(JsonConvert.SerializeObject(campaign), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("campaigns", content);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        dynamic data = JsonConvert.DeserializeObject(jsonString);

        return data.id;
    }

    public async Task AddContentToCampaignAsync(string campaignId, string htmlContent)
    {
        var content = new
        {
            html = htmlContent
        };

        var httpContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"campaigns/{campaignId}/content", httpContent);
        response.EnsureSuccessStatusCode();
    }

    public async Task SendCampaignAsync(string campaignId)
    {
        var response = await _httpClient.PostAsync($"campaigns/{campaignId}/actions/send", null);
        response.EnsureSuccessStatusCode();
    }
}
