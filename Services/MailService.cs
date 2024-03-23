using maturitetna.Models;
using Newtonsoft.Json;
using System.Text;

namespace maturitetna.Services
{
    public class MailService
    {
        const string apiUrl = "https://api.mailgun.net/v3/sandbox55e42ae4cba347f48f1202038f15d0d7.mailgun.org/messages";
        const string API_KEY = "2303818152cc3868e22f98be90ca3408-309b0ef4-b47cb0b5";
        // const string SANDOX_DOMAIN = "sandbox4ad7f4449d8a4e5aab1e251e24774f80.mailgun.org";
        static string authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"api:{API_KEY}"));

        public static async Task SendConfirmAppointment(appointmentEntity appointment)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var templateVariables = new Dictionary<string, string>
                    {
                        { "name", appointment.user.name },
                        { "lastname", appointment.user.lastname },
                        { "day", appointment.appointmentTime.ToShortDateString() },
                        { "time", appointment.appointmentTime.ToShortTimeString() }
                    };
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader);
                    var formData = new MultipartFormDataContent();

                    formData.Add(new StringContent("testiramo@gmail.com"), "from");
                    formData.Add(new StringContent(appointment.user.email), "to");
                    // formData.Add(new StringContent("Potrditev termina"), "subject");
                    formData.Add(new StringContent("Confirmed reservation"), "template");
                    formData.Add(new StringContent(JsonConvert.SerializeObject(templateVariables)), "h:X-Mailgun-Variables");

                    HttpResponseMessage response = await client.PostAsync(apiUrl, formData);

                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static async Task SendCancelAppointment(appointmentEntity appointment)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var templateVariables = new Dictionary<string, string>
                    {
                        { "name", appointment.user.name },
                        { "lastname", appointment.user.lastname },
                        { "day", appointment.appointmentTime.ToShortDateString() },
                        { "time", appointment.appointmentTime.ToShortTimeString() }
                    };
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader);
                    var formData = new MultipartFormDataContent();

                    formData.Add(new StringContent("testiramo@gmail.com"), "from");
                    formData.Add(new StringContent(appointment.user.email), "to");
                    // formData.Add(new StringContent("Potrditev termina"), "subject");
                    formData.Add(new StringContent("Cancel appointment"), "template");
                    formData.Add(new StringContent(JsonConvert.SerializeObject(templateVariables)), "h:X-Mailgun-Variables");

                    HttpResponseMessage response = await client.PostAsync(apiUrl, formData);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
