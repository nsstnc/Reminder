using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Reminder
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void Send_Request(object sender, EventArgs e)
        {
           
            // токен из личного кабинета
            string apiKey = "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";
            // адрес api для взаимодействия с чат-ботом
            string endpoint = "https://api.openai.com/v1/chat/completions";
            // набор соообщений диалога с чат-ботом
            List<Message> messages = new List<Message>();
            // HttpClient для отправки сообщений
            var httpClient = new HttpClient();
            // устанавливаем отправляемый в запросе токен
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");




            // запрос из поля
            var content = RequestField.Text.ToString();

            // формируем отправляемое сообщение
            var message = new Message() { Role = "user", Content = content };
            // добавляем сообщение в список сообщений
            messages.Add(message);

            // формируем отправляемые данные
            var requestData = new Request()
            {
                ModelId = "gpt-3.5-turbo",
                Messages = messages
            };
            // отправляем запрос
            using var response = httpClient.PostAsJsonAsync(endpoint, requestData);

            // если произошла ошибка, выводим сообщение об ошибке на консоль
            if (!response.IsCompletedSuccessfully)
            {
                Answer.Text = $"{(int)response.Result.StatusCode} {response.Status}";
            }
            // получаем данные ответа
            // ResponseData? responseData = response.Content.ReadFromJsonAsync<ResponseData>();
            Task<ResponseData?> responseData = response.Result.Content.ReadFromJsonAsync<ResponseData>();

            var choices = responseData?.Result.Choices ?? new List<Choice>();
            if (choices.Count == 0)
            {
                Answer.Text = "No choices were returned by the API";
            }

            var choice = choices[0];
            var responseMessage = choice.Message;
            // добавляем полученное сообщение в список сообщений
            messages.Add(responseMessage);
            var responseText = responseMessage.Content.Trim();
            Answer.Text = responseText;
        }
    }
}