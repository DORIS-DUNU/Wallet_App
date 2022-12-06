using System.Text.Json;
using System.Threading.Tasks;
using WalletApp.Abstractions.Repositories;
using WalletApp.Abstractions.Services;
using WalletApp.Models.DTO;
using WalletApp.Models.Entities;
using WalletApp.Utils;

namespace WalletApp.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public TransactionService(ITransactionRepository transactionRepository, IHttpClientFactory httpClientFactory)
        {
            _transactionRepository = transactionRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<double?> ConvertCurrencyAsync(string currencyA, string currencyB, double amount)
        {
            try
            {
                string conversion_result = string.Empty;
                var httpClient = _httpClientFactory.CreateClient();
                var conversionStr = $"https://v6.exchangerate-api.com/v6/033ce5eb6f40fe61f7080372/pair/{currencyA}/{currencyB}/{amount}";
                using (var response = await httpClient.GetAsync(conversionStr, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();
                    var newConvertDouble = await JsonSerializer.DeserializeAsync<ConvertDoubleDTO>(stream);
                    var newAmount = newConvertDouble.conversion_result;


                    return newAmount;
                }

            }
            catch (Exception)
            {
                return default;
            }
        }

        public async Task<double?> GetRateAsync(string currencyCode, double? amount)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var conversionStr = "https://open.er-api.com/v6/latest/NGN";
                using (var response = await httpClient.GetAsync(conversionStr, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();
                    var newRates = await JsonSerializer.DeserializeAsync<RatesDTO>(stream);
                    var rate = newRates.rates[currencyCode];
                    var convAmount = rate * amount;

                    return convAmount;
                }

            }
            catch (Exception)
            {
                return default;
            }
        }

        public async Task<IEnumerable<TransactionDTO>> GetAllUserTransactionsAsync()
        {
            try
            {

                var result = await _transactionRepository.GetAllUserTransactionsAsync();
                var allTransaction = new List<TransactionDTO>();

                foreach (var items in result)
                {
                    foreach (var item in items)
                    {
                        allTransaction.Add(WalletAppMapper.TransactioToDTO(item));
                    }

                }


                return allTransaction;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<IEnumerable<TransactionDTO>> GetWalletStatementAsync(string walletAddress, int page)
        {
            try
            {
                var result = await _transactionRepository.GetWalletStatementAsync(walletAddress, page);
                var allTransaction = new List<TransactionDTO>();
                foreach (var item in result)
                {
                    allTransaction.Add(WalletAppMapper.TransactioToDTO(item));
                }

                return allTransaction;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}