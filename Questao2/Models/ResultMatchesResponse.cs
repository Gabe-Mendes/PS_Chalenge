using System.Runtime.CompilerServices;

namespace Questao2.Models
{
    public class ResultMatchesResponse
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public List<ResultMatchesData>? data { get; set; }
    }
    
    public class ResultMatchesData
    {
        public string? competition { get; set; }
        public int year { get; set; }
        public string? round { get; set; }
        public string? team1 { get; set; }
        public string? team2 { get; set; }
        public string? team1goals { get; set; }
        public string? team2goals { get; set; }
    }

    
}