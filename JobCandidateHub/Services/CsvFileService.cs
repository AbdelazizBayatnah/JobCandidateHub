using CsvHelper;
using CsvHelper.Configuration;
using JobCandidateHub.Models;
using System.Globalization;

namespace JobCandidateHub.Services
{
    public interface ICsvFileService
    {
        Task<List<CandidateDetails>> GetAllCandidateDetails();

        Task SaveCandidateDetails(List<CandidateDetails> candidateDetailsModels);
    }

    public class CsvFileService : ICsvFileService
    {
        private readonly string _csvFilePath;

        public CsvFileService(string csvFilePath)
        {
            _csvFilePath = csvFilePath;
        }

        public async Task<List<CandidateDetails>> GetAllCandidateDetails()
        {
            if (!File.Exists(_csvFilePath))
            {
                return new List<CandidateDetails>();
            }
            using var reader = new StreamReader(_csvFilePath);

            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

            return await Task.Run(() => csv.GetRecords<CandidateDetails>().ToList());
        }

        public async Task SaveCandidateDetails(List<CandidateDetails> candidateDetailsModels)
        {
            await using var writer = new StreamWriter(_csvFilePath, false);

            await using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));

            await Task.Run(() => csv.WriteRecords(candidateDetailsModels));
        }
    }
}