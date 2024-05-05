using MoneyManager.Core;
using MoneyManager.FileSystem;

namespace MoneyManager.REPL
{
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
        internal AccountBookFile? CurrentAccountBookFile { get; set; }
        internal object CurrentContext { get; set; }

        internal Command[] TopLevelCommands => [];

        public bool IsRunning { get; internal set; } = false;

        private REPL()
        {
            CurrentAccountBook = new AccountBook(); // Start REPL with new empty accountbook
            CurrentContext = CurrentAccountBook;
        }

        public void Run()
        {
            // Print REPL Header
            Terminal.Message(
                "--- MoneyManager v0.0.0 REPL ---\n" +
                "Type 'help' for a list of command paths."
                );
            
            // Set IsRunning and begin loop
            IsRunning = true;

            while (IsRunning)
            {
                // Setup prefix for terminal prompt -> AccountBookName/CurrentContext>>
                string promptPrefix = Path.GetFileName(CurrentAccountBookFile?.Path) ?? "newAccountBook";
                if (CurrentContext != CurrentAccountBook) promptPrefix +=
                        CurrentContext is Account acc ? $"/{acc.Name}" :
                        CurrentContext is Category cat ? $"/{cat.Name}" :
                        CurrentContext.ToString();
                
                // Get user input
                string userInput = Terminal.Prompt($"{promptPrefix}>> ");

                // Try to parse and handle any exceptions
                try
                {
                    Parse(userInput);
                }
                catch(REPLException e)
                {
                    Terminal.Message(e.Message, ConsoleColor.Red);
                }
            }
        }

        private void Parse(string userInput)
        {
            // Parse given user input

        }
    }
}