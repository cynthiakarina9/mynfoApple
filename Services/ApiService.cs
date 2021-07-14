using Foundation;
using mynfoApple.Domain;
using mynfoApple.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace mynfoApple.Services
{
    public class ApiService
    {
        #region connections
        public async Task<Response> CheckConnection()
        {
            if(!CrossConnectivity.Current.IsConnected)
            {
                return new Response { IsSuccess = false, Messagge = "No internet connection" };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("http://portal.azure.com");

            if (!isReachable)
            {
                return new Response { IsSuccess = false, Messagge = "No internet connection" };
            }

            return new Response { IsSuccess = true };
        }
        #endregion

        #region LoginUtilities
        public async Task<TokenResponse> GetToken(string urlBase, string userName, string password)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);

                var response = await client.PostAsync("Token", 
                                                        new StringContent(
                                                            string.Format("grant_type=password&username={0}&password={1}", 
                                                            userName, 
                                                            password), Encoding.UTF8,"application/x-www-form-urlencoded"));

                var resultJason = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<TokenResponse>(resultJason);

                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<User>
        #endregion
    }
}