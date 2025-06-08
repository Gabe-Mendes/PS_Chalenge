using System.Text.Json;
using Questao2.Models;

namespace Questao2
{
    class DataTeam
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public int ScoredGoals { get; set; }

        public DataTeam(string _name, int _year)
        {
            Name = _name;
            Year = _year;
            ScoredGoals = GetTotalScoredGoals().GetAwaiter().GetResult();
        }

        public void ShowDetails()
        {
            Console.WriteLine();
            Console.WriteLine($"Team {Name} scored {ScoredGoals} goals in {Year}.");
            Console.WriteLine();
        }

        public async Task<int> ReturnGoalTotalTeamBySide(bool home)
        {
            int currentPage = 1;
            int lenghtPages = 1;
            int totalGoals = 0;
            string baseUrl = $"https://jsonmock.hackerrank.com/api/football_matches";            
            string teamNameParam = home ? "team1" : "team2";
            List<ResultMatchesData> allResults = new();
            
            try
            {
                using var httpClient = new HttpClient();
                do
                {
                    string url = $"{baseUrl}?year={this.Year}&{teamNameParam}={Uri.EscapeDataString(this.Name)}&page={currentPage}";
                    var response = await httpClient.GetAsync(url);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new InvalidOperationException($"Error fetching game results: {responseContent}");
                    }
                    var resultContent = JsonSerializer.Deserialize<ResultMatchesResponse>(responseContent);

                    if (resultContent?.data != null)
                    {
                        foreach (var result in resultContent.data)
                        {
                            var goalsStr = home ? result.team1goals : result.team2goals;
                            if (goalsStr != null && int.TryParse(goalsStr.ToString(), out int gols) && gols > 0)
                            {
                                totalGoals += gols;
                            }
                        }
                    }

                    lenghtPages = resultContent?.total_pages ?? 1;
                    currentPage++;
                }
                while (currentPage <= lenghtPages);

            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.ToString}");
            }

            return totalGoals;
        }
        public async Task<int> GetTotalScoredGoals()
        {
            int totalGoals = await ReturnGoalTotalTeamBySide(true);
            totalGoals += await ReturnGoalTotalTeamBySide(false);
            return totalGoals;
        }        
    }
}