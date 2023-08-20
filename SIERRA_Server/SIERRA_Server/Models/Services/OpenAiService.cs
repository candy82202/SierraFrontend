using Microsoft.Extensions.Options;
using OpenAI_API.Models;
using SIERRA_Server.Configurations;
using SIERRA_Server.Models.DTOs.Desserts;
using SIERRA_Server.Models.Interfaces;
using System.Text;

namespace SIERRA_Server.Models.Services
{
    public class OpenAiService : IOpenAiService
    {
        private readonly OpenAiConfig _openAiConfig;
        private readonly IDessertRepository _repo;
        private readonly IDessertCategoryRepository _dessertCategoryRepository;
        public OpenAiService(IOptionsMonitor<OpenAiConfig> optionsMonitor, IDessertRepository repo, IDessertCategoryRepository dessertCategoryRepository)
        {
            _openAiConfig = optionsMonitor.CurrentValue;
            _repo = repo;
            _dessertCategoryRepository = dessertCategoryRepository;
        }

        public async Task<string> CheckProgramingLanguage(string language)
        {
            //api instance
            var api = new OpenAI_API.OpenAIAPI(_openAiConfig.Key);
            var chat = api.Chat.CreateConversation();

            chat.AppendSystemMessage("You are a teacher who help new programmers understand things are programming language or not. If the usr tells you a programming language respond with yes, if a user tells you something which is not a programing language respond with no. you will only respond with yes or no. you do not say anything else.");

            chat.AppendUserInput(language);

            var response = await chat.GetResponseFromChatbotAsync();
            return response;
        }
        //public async Task<string> AskDessertQuestion(string text)
        //{
        //    //api instance
        //    var api = new OpenAI_API.OpenAIAPI(_openAiConfig.Key);
        //    var chat = api.Chat.CreateConversation();

        //    chat.AppendSystemMessage("你是一個我們甜點店做專業客戶服務的，我們的甜點店有賣甜點類別有\"整模蛋糕\",\"常溫蛋糕\",\"長條蛋糕\",\"點心\",\"禮盒\"，經典商品是\"波士頓蛋糕\"，如果有人詢問你有甚麼蛋糕類別，請回答\"整模蛋糕\",\"常溫蛋糕\",\"長條蛋糕\",\"點心\",\"禮盒\"，除了蛋糕以外還有課程銷售，可以選日期推薦");

        //    chat.AppendUserInput(text);

        //    var response = await chat.GetResponseFromChatbotAsync();
        //    return response;
        //}
        //public async Task<string> AskDessertQuestion(string text)
        //{
        //    var api = new OpenAI_API.OpenAIAPI(_openAiConfig.Key);

        //    //get dessert
        //    var hotdessert = await _repo.GetHotProductsAsync();
        //    var moldCake = await _repo.GetMoldCake();
        //    var roomTemperature = await _repo.GetRoomTemperature();
        //    var snack = await _repo.GetSnack();
        //    var longCake = await _repo.GetLongCake();
        //    var presents = await _repo.GetPresents();
        //    var chat = api.Chat.CreateConversation();

        //    
        //    var systemMessage = new StringBuilder();
        //    systemMessage.AppendLine("你是一個我們甜點店做專業客戶服務的人員，我們的甜點店有賣甜點類別有\"整模蛋糕\",\"常溫蛋糕\",\"長條蛋糕\",\"點心\",\"禮盒\"，");
        //    systemMessage.AppendLine("熱銷商品前三名分别是：");
        //    for (int i = 0; i < hotdessert.Count && i < 3; i++)
        //    {
        //        systemMessage.AppendLine($"{i + 1}. {hotdessert[i].DessertName}");
        //    }
        //    systemMessage.AppendLine("整模蛋糕有：");
        //    for (int i = 0; i < moldCake.Count ; i++)
        //    {
        //        systemMessage.AppendLine($" {moldCake[i].DessertName}");
        //    }
        //    systemMessage.AppendLine("常溫蛋糕有：");
        //    for (int i = 0; i < roomTemperature.Count; i++)
        //    {
        //        systemMessage.AppendLine($" {roomTemperature[i].DessertName}");
        //    }
        //    systemMessage.AppendLine("點心有：");
        //    for (int i = 0; i < snack.Count; i++)
        //    {
        //        systemMessage.AppendLine($" {snack[i].DessertName}");
        //    }
        //    systemMessage.AppendLine("長條蛋糕有：");
        //    for (int i = 0; i < longCake.Count; i++)
        //    {
        //        systemMessage.AppendLine($" {longCake[i].DessertName}");
        //    }
        //    systemMessage.AppendLine("禮盒有：");
        //    for (int i = 0; i < presents.Count; i++)
        //    {
        //        systemMessage.AppendLine($" {presents[i].DessertName}");
        //    }
        //    systemMessage.AppendLine("如果有人詢問你有什麼蛋糕類別，請回答\"整模蛋糕\",\"常溫蛋糕\",\"長條蛋糕\",\"點心\",\"禮盒\"，");
        //    systemMessage.AppendLine("除了蛋糕以外還有課程銷售，可以選日期推薦。");

        //    chat.AppendSystemMessage(systemMessage.ToString());
        //    chat.AppendUserInput(text);
        //    var response = await chat.GetResponseFromChatbotAsync();
        //    return response;
        //}
        public async Task<string> AskDessertQuestion(string text)
        {
            var api = new OpenAI_API.OpenAIAPI(_openAiConfig.Key);
            var chat = api.Chat.CreateConversation();
            var hotdessert = await _repo.GetHotProductsAsync();
            var dessertCategories = new Dictionary<string, Func<Task<List<DessertListDTO>>>>
    {
        { "整模蛋糕", async () => await GetDessertsByCategoryAsync(1) },
        { "常溫蛋糕", async () => await GetDessertsByCategoryAsync(2) },
        { "點心", async () => await GetDessertsByCategoryAsync(3) },
        { "長條蛋糕", async () => await GetDessertsByCategoryAsync(4) },
        { "禮盒", async () => await GetDessertsByCategoryAsync(5) }
    };

            // Construct system message
            var systemMessage = new StringBuilder();
            systemMessage.AppendLine("你是一個我們甜點店做專業客戶服務的人員，我們的甜點店有賣甜點類別有：");

            foreach (var category in dessertCategories)
            {
                var desserts = await category.Value();
                if (desserts.Any())
                {
                    systemMessage.AppendLine(category.Key + "：");
                    foreach (var dessert in desserts)
                    {
                        systemMessage.AppendLine($" {dessert.DessertName}");
                    }
                }
            }

            systemMessage.AppendLine("熱銷商品前三名分别是：");
            for (int i = 0; i < hotdessert.Count && i < 3; i++)
            {
                systemMessage.AppendLine($"{i + 1}. {hotdessert[i].DessertName}");
            }
            if (text.Contains("價格"))
            {
                var dessertName = await ExtractDessertNameFromText(text); // Extract dessert name from user input
                var price = await GetDessertPriceAsync(dessertName);

                if (price != null)
                {
                    systemMessage.AppendLine($"{dessertName}的價格是{price}元。");
                }
                else
                {
                    systemMessage.AppendLine($"很抱歉，找不到{dessertName}的價格信息。");
                }
            }
            if (text.Contains("宅配"))
            {
                systemMessage.AppendLine("目前與黑貓宅配物流配合全程低溫冷凍配送，因週日黑貓宅配無提供配送服務，SIERRA不負擔黑貓宅配物流延遲的責任，包裹出貨後配送狀況依物流中心、當區配送營業所為主，詳情請至黑貓官網查詢或撥打黑貓客服。運送過程中，如包裹在物流公司分流理貨當中有解凍到、失溫，都很有可能會因為配送人員與路況的不同，導致蛋糕位移、變形、或是盒內四周沾到黏膩果膠等損壞的風險，請務必謹慎評估風險後再下單。");
                systemMessage.AppendLine("如果有人詢問你有什麼蛋糕類別，請回答\"整模蛋糕\",\"常溫蛋糕\",\"長條蛋糕\",\"點心\",\"禮盒\"。");
                systemMessage.AppendLine("如果有人詢問你可以指定時間送達蛋糕嗎，請回答目前無接受指定送達時段哦！實際送達時間需依照當區物流司機安排的路線為主。");

            }
            if (text.Contains("類別"))
            {
                systemMessage.AppendLine("如果有人詢問你可以貨到付款嗎?，請回答目前線上訂購的所有商品皆為接單製作生產，故無提供貨到付款服務。");

            }

            systemMessage.AppendLine("除了蛋糕以外還有課程銷售，可以選日期推薦。");
            systemMessage.AppendLine("如果有人詢問訂單什麼時候會出貨，請回答 除以下特殊節慶之外的正常時間，我們都是在您選擇的希望到貨日期的前一天才會寄出包裹，逢各大節日前(年節、中秋節等節慶)，物流包裹量通常都會暴增，需請您自行將希望到貨日期選擇在您要送禮/慶祝前的兩~三天，提前收到比較不會因為物流延遲而耽誤您的送禮/慶祝安排。如遇母親節、父親節，將統一由我們主動提前出貨");
            systemMessage.AppendLine($"如果有人詢問你可以寄便利商店店到店嗎?請回答不好意思，目前我們僅配合黑貓低溫宅配寄送， 無提供超商取貨、店到店服務哦！");
            chat.AppendSystemMessage(systemMessage.ToString());
            chat.AppendUserInput(text);
            var response = await chat.GetResponseFromChatbotAsync();
            return response;
        }
        //private async Task<decimal?> GetDessertPriceAsync(string dessertName)
        //{
        //    var dessert = await _repo.GetDessertByName(dessertName);
        //    if (dessert != null && dessert.Any())
        //    {
        //        var firstDessert = dessert.First(); // Assuming GetDessertByName returns a list of desserts
        //        return firstDessert.UnitPrice;
        //    }
        //    return null;
        //}

        //private async Task<string> ExtractDessertNameFromText(string text)
        //{
        //    var dessertNames = await _repo.GetDessertNames();
        //    foreach (var dessertName in dessertNames)
        //    {
        //        if (text.Contains(dessertName))
        //        {
        //            return dessertName;
        //        }
        //    }

        //    return null; // Return null if no dessert name is extracted or matched
        //}
        private async Task<decimal?> GetDessertPriceAsync(string dessertName)
        {
            var dessert = await _repo.GetDessertByName(dessertName);
            if (dessert != null && dessert.Any())
            {
                var firstDessert = dessert.First();
                return firstDessert.UnitPrice;
            }
            return null;
        }

        private async Task<string> ExtractDessertNameFromText(string text)
        {
            var dessertNames = await _repo.GetDessertNames();
            foreach (var dessertName in dessertNames)
            {
                if (text.Contains(dessertName))
                {
                    return dessertName;
                }
            }

            return null;
        }

        private async Task<List<DessertListDTO>> GetDessertsByCategoryAsync(int categoryId)
        {
            var desserts = await _dessertCategoryRepository.GetDessertsByCategoryId(categoryId);
            return desserts.Select(d => d.ToDListDto()).ToList();
        }

        public async Task<string> CompleteSentenceAdvance(string text)
        {
            //throw new NotImplementedException();
            var api = new OpenAI_API.OpenAIAPI(_openAiConfig.Key);
            var result = await api.Completions.CreateCompletionAsync(
                new OpenAI_API.Completions.CompletionRequest(text, model: Model.CurieText, temperature: 0.1));
            return result.Completions[0].Text;
        }

        public async Task<string> CompleteSentence(string text)
        {
            throw new NotImplementedException();
        }
    }
}
