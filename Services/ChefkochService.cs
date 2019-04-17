using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace ChefkochApp.Services
{
    public class ChefkochService
    {
        private IHttpClientFactory m_ClientFactory;
        private string m_Filename;
        private JsonSerializer m_JsonSerializer;
        private List<ChefkochRecipe> m_Data;
        private const string DataFilename = "Rezepte.json";

        public ChefkochService(IHttpClientFactory clientFactory, IHostingEnvironment env)
        {
            m_ClientFactory = clientFactory;
            m_Filename = Path.Combine(env.WebRootPath, DataFilename);

            // Prüfen, ob die gespeicherten Rezepte bereits vorhanden sind
            if (!File.Exists(m_Filename))
            {
                File.Create(m_Filename);
                m_Data = new List<ChefkochRecipe>();
                return;
            }

            // Daten von der Festplatte laden
            LoadData();
        }

        public List<ChefkochRecipe> GetData()
        {
            return m_Data;
        }

        private ChefkochRecipe DownloadRecipie(string id)
        {

            HttpClient client = m_ClientFactory.CreateClient();
            var task = client.GetStringAsync($"http://api.chefkoch.de/v2/recipes/{id}");
            task.Wait();
            return JsonConvert.DeserializeObject<ChefkochRecipe>(task.Result);
        }

        public ChefkochRecipe AddRecipie(string id)
        {
            ChefkochRecipe result = m_Data.FirstOrDefault(x => { return x.id == id; });
            if (result == null)
            {
                result = DownloadRecipie(id);
                m_Data.Add(result);
                SaveData();
            }
            return result;
        }

        public ChefkochRecipe GetRecipie(string id)
        {
            ChefkochRecipe result = m_Data.FirstOrDefault(x => { return x.id == id; });
            if(result == null)
            {
                return AddRecipie(id);
            }
            return result;
        }

        private void LoadData()
        {
            m_Data = JsonConvert.DeserializeObject<List<ChefkochRecipe>>(File.ReadAllText(m_Filename));
            if (m_Data == null)
            {
                m_Data = new List<ChefkochRecipe>();
            }
        }

        private void SaveData()
        {
            File.WriteAllText(m_Filename, JsonConvert.SerializeObject(m_Data));
        }
    }
}
