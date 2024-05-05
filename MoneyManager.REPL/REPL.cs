using MoneyManager.Core;

namespace MoneyManager.REPL;

public class REPL
{
    public static REPL Instance
    {
        get
        {
            instance ??= new REPL();
            return Instance;
        }
    }
    private static REPL? instance;

    internal AccountBook CurrentAccountBook { get; set; }
    internal object CurrentContext { get; set; }

    internal Command[] TopLevelCommands => [];

    public bool IsRunning { get; private set; } = false;

    private REPL()
    {
        CurrentAccountBook = new AccountBook(); // Start REPL with new empty accountbook
        CurrentContext  = CurrentAccountBook;
    }

    public void Run()
    {
        // Begin REPL Loop
    }

    private void Parse(string userInput)
    {
        // Parse given user input
    }
}