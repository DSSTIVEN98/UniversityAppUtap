using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UniversityApp.BL.Services.Implements
{
    public class GenericRest<TEntity> : IGenericRest<TEntity>
    {

        #region StringToNIntConverter
        private class StringToNIntConverter : JsonConverter
        {

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(int?);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null)
                    return null;
                if (reader.TokenType == JsonToken.Integer)
                    return reader.Value;

                if (reader.TokenType == JsonToken.String)
                {
                    if (string.IsNullOrEmpty((string)reader.Value))
                        return null;
                    int num;
                    if (int.TryParse((string)reader.Value, out num))
                        return num;

                    throw new JsonReaderException(string.Format("Expected integer, got {0}", reader.Value));
                }
                throw new JsonReaderException(string.Format("Unexcepted token {0}", reader.TokenType));
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(value);
            }
        }
        #endregion
        //  Tutorial 56 - Parte 10 - Validación de conexión a internet
        //  Nugget: Xam.Plugin.Connectivity (Android,iOS, BLL, Current)
        /// <summary>
        /// CheckConnection
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("https://www.google.com/");

            if (!isReachable)
                return false;

            return true;
        }

        public async Task<TEntity> GetById(string url, int id)
        {
            HttpResponseMessage response = await RestClientSingleton.Instance().GetAsync(url + id);
            string responseText = await response.Content.ReadAsStringAsync();

            response.Dispose();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseText);

            return JsonConvert.DeserializeObject<TEntity>(responseText, new StringToNIntConverter());
        }

        public async Task<IEnumerable<TEntity>> GetAll(string url)
        {
            HttpResponseMessage response = await RestClientSingleton.Instance().GetAsync(url);
            string responseText = await response.Content.ReadAsStringAsync();

            response.Dispose();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseText);

            return JsonConvert.DeserializeObject<List<TEntity>>(responseText,new StringToNIntConverter());
        }

        public async Task<TEntity> Create(string url, TEntity entity)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await RestClientSingleton.Instance().PostAsync(url, content);
            string responseText = await response.Content.ReadAsStringAsync();

            response.Dispose();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseText);

            return JsonConvert.DeserializeObject<TEntity>(responseText);
        }

        public async Task Update(string url, int id, TEntity entity)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await RestClientSingleton.Instance().PutAsync(url + id, content);

            string responseText = await response.Content.ReadAsStringAsync();

            response.Dispose();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseText);
        }

        public async Task Delete(string url, int id)
        {
            HttpResponseMessage response = await RestClientSingleton.Instance().DeleteAsync(url + id);
            string responseText = await response.Content.ReadAsStringAsync();

            response.Dispose();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseText);
        }
    }
}
