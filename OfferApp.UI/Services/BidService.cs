using OfferApp.Shared.DTO;
using System.Net.Http.Json;

namespace OfferApp.UI.Services
{
    public class BidService : IBidService
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "/api/bids";

        public BidService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BidDto> AddBid(BidDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync(BASE_URL, dto);
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadFromJsonAsync<BidDto>())
                    ?? throw new InvalidOperationException("Received null Bid");
            }

            if ((int)response.StatusCode >= 400 && (int)response.StatusCode <= 499)
            {
                var dictionary = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>()
                    ?? throw new Exception("Invalid payload");
                throw new InvalidOperationException(dictionary["Error"]);
            }

            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<BidPublishedDto> BidUp(BidUpDto bidUp)
        {
            var response = await _httpClient.PatchAsJsonAsync($"{BASE_URL}/{bidUp.Id}/bid-up", bidUp);
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadFromJsonAsync<BidPublishedDto>())
                    ?? throw new InvalidOperationException("Received null Bid");
            }

            if ((int)response.StatusCode >= 400 && (int)response.StatusCode <= 499)
            {
                var dictionary = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>()
                    ?? throw new Exception("Invalid payload");
                throw new InvalidOperationException(dictionary["Error"]);
            }

            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task DeleteBid(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BASE_URL}/{id}");
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            if ((int)response.StatusCode >= 400 && (int)response.StatusCode <= 499)
            {
                var dictionary = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>()
                    ?? throw new Exception("Invalid payload");
                throw new InvalidOperationException(dictionary["Error"]);
            }

            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<IReadOnlyList<BidDto>> GetAllBids()
        {
            return (await _httpClient.GetFromJsonAsync<IReadOnlyList<BidDto>>(BASE_URL))
                ?? throw new InvalidOperationException("Received null Bids");
        }

        public async Task<IReadOnlyList<BidPublishedDto>> GetAllPublishedBids()
        {
            return (await _httpClient.GetFromJsonAsync<IReadOnlyList<BidPublishedDto>>($"{BASE_URL}/published"))
                ?? throw new InvalidOperationException("Received null Bids");
        }

        public Task<BidDto?> GetBidById(int id)
        {
            return _httpClient.GetFromJsonAsync<BidDto>($"{BASE_URL}/{id}");
        }

        public async Task<bool> Published(int id)
        {
            var response = await _httpClient.PatchAsync($"{BASE_URL}/{id}/publish", null);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            if ((int)response.StatusCode >= 400 && (int)response.StatusCode <= 499)
            {
                var dictionary = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>()
                    ?? throw new Exception("Invalid payload");
                throw new InvalidOperationException(dictionary["Error"]);
            }

            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<bool> Unpublished(int id)
        {
            var response = await _httpClient.PatchAsync($"{BASE_URL}/{id}/unpublish", null);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            if ((int)response.StatusCode >= 400 && (int)response.StatusCode <= 499)
            {
                var dictionary = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>()
                    ?? throw new Exception("Invalid payload");
                throw new InvalidOperationException(dictionary["Error"]);
            }

            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<BidDto> UpdateBid(BidDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BASE_URL}/{dto.Id}", dto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<BidDto>()
                    ?? throw new InvalidOperationException("Received null Bid");
            }

            if ((int)response.StatusCode >= 400 && (int)response.StatusCode <= 499)
            {
                var dictionary = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>()
                    ?? throw new Exception("Invalid payload");
                throw new InvalidOperationException(dictionary["Error"]);
            }

            throw new Exception(await response.Content.ReadAsStringAsync());
        }
    }
}
