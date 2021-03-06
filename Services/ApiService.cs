namespace mynfo.Services
{
    using Domain;
    using Models;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Xamarin.Essentials;

    public class ApiService
    {
        #region connections
        public async Task<Response> CheckConnection()
        {
            var current = Connectivity.NetworkAccess;
            //if (!CrossConnectivity.Current.IsConnected)
            //{
            //    return new Response { IsSuccess = false, Messagge = "No internet connection" };
            //}
            
            if (!CrossConnectivity.Current.IsConnected || current != NetworkAccess.Internet)
            {
                return new Response { IsSuccess = false, Messagge = "No internet connection" };
            }
            //var isReachable = await CrossConnectivity.Current.IsRemoteReachable("http://portal.azure.com");

            var isReachable = await CrossConnectivity.Current.IsReachable("http://portal.azure.com");
            if (!isReachable)
            {
                return new Response { IsSuccess = false, Messagge = "Internet lento, favor de volver a cargar" };
            }

            return new Response { IsSuccess = true };
        }
        #endregion

        #region Delete

        #region Delete
        public async Task<Response> Delete<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            T model)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenType, accessToken);
                var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controller,
                    model.GetHashCode());
                var response = await client.DeleteAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                return new Response
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Messagge = ex.Message,
                };
            }
        }

        public async Task<Response> Delete<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controller,
                    model.GetHashCode());
                var response = await client.DeleteAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                return new Response
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Messagge = ex.Message,
                };
            }
        }

        public async Task<Response> Delete(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controller,
                    id);
                var response = await client.DeleteAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                return new Response
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Messagge = ex.Message,
                };
            }
        }

        public async Task<Response> Delete1(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var model = new ProfileSM
                {
                    ProfileMSId = id,
                };

                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}",
                    servicePrefix,
                    controller);
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }
                return JsonConvert.DeserializeObject<Response>(result);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<T> Delete2<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {

                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controller,
                    id);
                var response = await client.DeleteAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<Response> Delete3<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controller,
                    model);
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                return new Response
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Messagge = ex.Message,
                };
            }
        }
        #endregion

        #region Profiles
        public async Task<Response> DeleteEmail(
           string urlBase,
           string servicePrefix,
           string controller,
           int id)
        {
            try
            {
                var model = new ProfileEmail
                {
                    ProfileEmailId = id,
                };

                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}",
                    servicePrefix,
                    controller);
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }
                return JsonConvert.DeserializeObject<Response>(result);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<Response> DeleteWhatsapp(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var model = new ProfileWhatsapp
                {
                    ProfileWhatsappId = id,
                };

                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}",
                    servicePrefix,
                    controller);
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }
                return JsonConvert.DeserializeObject<Response>(result);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<Response> DeleteRelationPhone(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var model = new ProfilePhone
                {
                    ProfilePhoneId = id,
                };

                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}",
                    servicePrefix,
                    controller);
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }
                return JsonConvert.DeserializeObject<Response>(result);
            }
            catch (Exception ex)
            {
                return default;
            }
        }
        #endregion

        #region Relations
        public async Task<Response> DeleteProfilesEmail_Box(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var model = new Box_ProfileEmail()
                {
                    BoxId = id
                };
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}",
                    servicePrefix,
                    controller);
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }
                return JsonConvert.DeserializeObject<Response>(result);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<Response> DeleteProfilesPhone_Box(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var model = new Box_ProfilePhone()
                {
                    BoxId = id
                };
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}",
                    servicePrefix,
                    controller);
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }
                return JsonConvert.DeserializeObject<Response>(result);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<Response> DeleteProfilesWhatsapp_Box(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var model = new Box_ProfileWhatsapp()
                {
                    BoxId = id
                };
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}",
                    servicePrefix,
                    controller);
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }
                return JsonConvert.DeserializeObject<Response>(result);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<Response> DeletProfilesSM_Box(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var model = new Box_ProfileSM()
                {
                    BoxId = id
                };
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}",
                    servicePrefix,
                    controller);
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }
                return JsonConvert.DeserializeObject<Response>(result);
            }
            catch (Exception ex)
            {
                return default;
            }
        }
        #endregion

        #endregion

        #region Gets

        #region Box
        public async Task<Box> GetBox(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controller,
                    id);
                var response = await client.GetAsync(url);



                if (!response.IsSuccessStatusCode)
                {
                    return new Box();
                }



                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Box>(result);
            }
            catch
            {
                return null;
            }
        }

        public async Task<Box> GetBoxDefault<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                Box model = new Box()
                {
                    UserId = id
                };
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<Box>(result);
                return resp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Box>> GetBoxNoDefault<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                Box model = new Box()
                {
                    UserId = id
                };
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<List<Box>>(result);
                return resp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Box> GetLastBox(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                Box model = new Box()
                {
                    UserId = id
                };
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}",
                    servicePrefix,
                    controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Box>(result);
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> GetBoxCount(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                Box model = new Box()
                {
                    UserId = id
                };
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}",
                    servicePrefix,
                    controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return 0;
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<int>(result);
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region Get
        public async Task<ProfileEmail> Get(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            int id)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controller,
                    id);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new ProfileEmail();
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProfileEmail>(result);
            }
            catch
            {
                return null;
            }
        }
       
        public async Task<Response> Get<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                return new Response
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Messagge = ex.Message,
                };
            }
        }

        public async Task<Response> Get<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            int id)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controller,
                    id);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Messagge = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<T>(result);
                return new Response
                {
                    IsSuccess = true,
                    Messagge = "Ok",
                    Result = model,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Messagge = ex.Message,
                };
            }
        }

        public async Task<RedSocial> GetRS(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}/{2}", servicePrefix, controller, id);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<RedSocial>(result);
                return resp;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<User> GetUser(
           string urlBase,
           string servicePrefix,
           string controller,
           int id)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}{2}", servicePrefix, controller, id);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(result);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region List
        public async Task<List<T>> GetListByUser<T>(
           string urlBase,
           string servicePrefix,
           string controller,
           int id)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}/{2}", servicePrefix, controller, id);
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                List<T> list = JsonConvert.DeserializeObject<List<T>>(result);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<T>> GetBoxListByUser<T>(
           string urlBase,
           string servicePrefix,
           string controller,
           int id)
        {
            try
            {
                Box model = new Box()
                {
                    UserId = id
                };
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                List<T> list = JsonConvert.DeserializeObject<List<T>>(result);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Response> GetList<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Messagge = result,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Messagge = "Ok",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Messagge = ex.Message,
                };
            }
        }

        public async Task<Response> GetList<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            int id)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controller,
                    id);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Messagge = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Messagge = "Ok",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Messagge = ex.Message,
                };
            }
        }

        public async Task<Response> GetList<T>(
            string urlBase,
            string servicePrefix,
            string controller)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Messagge = result,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Messagge = "Ok",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Messagge = ex.Message,
                };
            }
        }
        #endregion

        #region Profiles
        public async Task<List<ProfileSM>> GetProfileByNetWorkT(
            string urlBase,
            string servicePrefix,
            string controller,
            int User,
            int RedSocial)
        {
            try
            {
                var model = new ProfileSM
                {
                    UserId = User,
                    RedSocialId = RedSocial
                };

                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ProfileSM>>(result);
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<ProfileSM>> GetProfileByNetWork(
            string urlBase,
            string servicePrefix,
            string controller,
            int User,
            int RedSocial)
        {
            try
            {
                var model = new ProfileSM
                {
                    UserId = User,
                    RedSocialId = RedSocial
                };

                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ProfileSM>>(result);
            }
            catch
            {
                return null;
            }
        }

        public async Task<User> GetUserByEmail(
            string urlBase,
            string servicePrefix,
            string controller,
            string email)
        {
            try
            {
                var model = new UserRequest
                {
                    Email = email,
                };

                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(result);
            }
            catch
            {
                return null;
            }
        }

        public async Task<ProfileEmail> GetProfileEmail(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var model = new ProfileEmail
                {
                    ProfileEmailId = id,
                };

                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProfileEmail>(result);
            }
            catch
            {
                return null;
            }
        }

        public async Task<ProfilePhone> GetProfilePhone(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var model = new ProfilePhone
                {
                    ProfilePhoneId = id,
                };

                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProfilePhone>(result);
            }
            catch
            {
                return null;
            }
        }

        public async Task<ProfileSM> GetProfileSM(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var model = new ProfileSM
                {
                    ProfileMSId = id,
                };

                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProfileSM>(result);
            }
            catch
            {
                return null;
            }
        }

        public async Task<ProfileWhatsapp> GetProfileWhatsApp(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var model = new ProfileWhatsapp
                {
                    ProfileWhatsappId = id,
                };

                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProfileWhatsapp>(result);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Relation
        public async Task<T> GetIdRelation<T>(
           string urlBase,
           string servicePrefix,
           string controller,
           T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }

                var result = await response.Content.ReadAsStringAsync();
                var result2 = result.Replace("[", string.Empty);
                var result3 = result2.Replace("]", string.Empty);
                return JsonConvert.DeserializeObject<T>(result3);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<List<Box_ProfileEmail>> GetRelationBox_ProfilesEmail(
            string urlBase,
            string servicePrefix,
            string controller,
            int Id)
        {
            try
            {
                var model = new Box_ProfileEmail
                {
                    BoxId = Id
                };

                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    string resul = "0";
                    return JsonConvert.DeserializeObject<List<Box_ProfileEmail>>(resul);
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Box_ProfileEmail>>(result);
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Box_ProfilePhone>> GetRelationBox_ProfilesPhone(
            string urlBase,
            string servicePrefix,
            string controller,
            int Id)
        {
            try
            {
                var model = new Box_ProfilePhone
                {
                    BoxId = Id
                };

                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    string resul = "0";
                    return JsonConvert.DeserializeObject<List<Box_ProfilePhone>>(resul);
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Box_ProfilePhone>>(result);
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Box_ProfileWhatsapp>> GetRelationBox_ProfilesWhatsapp(
            string urlBase,
            string servicePrefix,
            string controller,
            int Id)
        {
            try
            {
                var model = new Box_ProfileWhatsapp
                {
                    BoxId = Id
                };

                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    string resul = "0";
                    return JsonConvert.DeserializeObject<List<Box_ProfileWhatsapp>>(resul);
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Box_ProfileWhatsapp>>(result);
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Box_ProfileSM>> GetRelationBox_ProfilesSM(
            string urlBase,
            string servicePrefix,
            string controller,
            int Id)
        {
            try
            {
                var model = new Box_ProfileSM
                {
                    BoxId = Id
                };

                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    string resul = "0";
                    return JsonConvert.DeserializeObject<List<Box_ProfileSM>>(resul);
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Box_ProfileSM>>(result);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #endregion

        #region Login
        public async Task<Response> LoginMessage(
            string urlBase,
            string servicePrefix,
            string controller,
            string email)
        {
            try
            {
                var userRequest = new UserRequest
                {
                    Email = email
                };
                var request = JsonConvert.SerializeObject(userRequest);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Messagge = response.StatusCode.ToString(),
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Messagge = ex.Message,
                };
            }
        }

        #endregion

        #region LoginUtilities
        public async Task<TokenResponse> GetToken(
            string urlBase, 
            string userName, 
            string password)
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

        public async Task<User> GetUserByEmail(
            string _UrlBase, 
            string _ServicePrefix, 
            string _Controller, 
            string _tokenType, 
            string _accessToken, 
            string _Email)
        {
            try
            {
                var model = new UserRequest { Email = _Email };

                var request = JsonConvert.SerializeObject(model);

                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(_tokenType, _accessToken);

                client.BaseAddress = new Uri(_UrlBase);

                var url = string.Format("{0}{1}", _ServicePrefix, _Controller);

                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<User>(result);
            }
            catch
            {
                return null;
            }
        }

        public async Task<User> GetUserId(
           string urlBase,
           string servicePrefix,
           string controller,
           int id)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controller,
                    id);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(result);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Password
        public async Task<Response> ChangePassword(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            ChangePasswordRequest changePasswordRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(changePasswordRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Messagge = ex.Message,
                };
            }
        }

        public async Task<Response> PasswordRecovery(
            string urlBase,
            string servicePrefix,
            string controller,
            string email)
        {
            try
            {
                var userRequest = new UserRequest
                {
                    Email = email
                };
                var request = JsonConvert.SerializeObject(userRequest);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Messagge = response.StatusCode.ToString(),
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Messagge = ex.Message,
                };
            }
        }
        #endregion

        #region Posts

        #region Post
        public async Task<Response> Post<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request, Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Messagge = "Record added OK",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Messagge = ex.Message,
                };
            }
        }

        public async Task<T> Post<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request, Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<T>(result);
                    return error;
                }

                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return newRecord;
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<Response> Post2<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Messagge = response.StatusCode.ToString(),
                    };
                }


                var result = await response.Content.ReadAsStringAsync();
                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Messagge = "Record added OK",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Messagge = ex.Message,
                };
            }
        }
        #endregion

        #endregion

        #region Puts
        public async Task<T> PutProfile<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}",
                    servicePrefix,
                    controller,
                    model.GetHashCode());
                var response = await client.PutAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<T>(result);
                    //error.IsSuccess = false;
                    return error;
                }

                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return newRecord;
            }
            catch (Exception ex)
            {
                return model;
            }
        }

        public async Task<Response> Put<T>(
           string urlBase,
           string servicePrefix,
           string controller,
           string tokenType,
           string accessToken,
           T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controller,
                    model.GetHashCode());
                var response = await client.PutAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Messagge = ex.Message,
                };
            }
        }

        public async Task<Box> PutBox<Box>(
           string urlBase,
           string servicePrefix,
           string controller,
           Box model,
           int id)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controller,
                    id);
                var response = await client.PutAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return model;
                }

                var newRecord = JsonConvert.DeserializeObject<Box>(result);

                return newRecord;
            }
            catch (Exception ex)
            {
                return model;
            }
        }

        public async Task<Response> Put<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controller,
                    model.GetHashCode());
                var response = await client.PutAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Messagge = ex.Message,
                };
            }
        }
        #endregion

    }
}