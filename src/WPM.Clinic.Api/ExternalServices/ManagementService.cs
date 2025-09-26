namespace WPM.Clinic.ExternalServices
{
    public class ManagementService
    {
        private HttpClient _httpClient;
        public ManagementService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<PetInfo> GetPetInfo(int Id)
        {
            var petInfo = await _httpClient.GetFromJsonAsync<PetInfo>($"/api/pets/{Id}");
            return petInfo;
        }
    }
    public record PetInfo(int Id, string Name, int Age, int BreedId);
}
