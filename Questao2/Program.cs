using Questao2;

public interface IProgram
{
    // Interface members (if any) go here. Removed static abstract Main for compatibility.
}

/// <summary>
/// The entry point of the application.
/// Initializes instances of <see cref="DataTeam"/> with specified team names and years,
/// and displays their details using the <c>ShowDetails</c> method.
/// </summary>
public class Program : IProgram
{

    public static void Main()
    {
        DataTeam dataTeam = new DataTeam("Paris Saint-Germain", 2013);
        dataTeam.ShowDetails();

        DataTeam dataTeam2 = new DataTeam("Chelsea", 2014);
        dataTeam2.ShowDetails();
    }
}

// Output expected:
    // Team Paris Saint - Germain scored 109 goals in 2013
    // Team Chelsea scored 92 goals in 2014