using MoneyManager.Core;
using MoneyManager.FileSystem;
using MoneyManager.REPL.Commands;

namespace MoneyManager.REPL
{
    /// <summary>
    /// A singleton class representing the MoneyManager REPL user interface.
    /// </summary>
    public class REPL
    {
        /// <summary>
        /// The singleton instance of the <see cref="REPL"/> class.
        /// </summary>
        public static REPL Instance
        {
            get
            {
                instance ??= new REPL();
                return Instance;
            }
        }
        private static REPL? instance;

        /// <summary>
        /// The currenty open <see cref="AccountBook"/>.
        /// </summary>
        internal AccountBook CurrentAccountBook { get; set; }
        /// <summary>
        /// The currently open <see cref="AccountBookFile"/>.
        /// </summary>
        internal AccountBookFile? CurrentAccountBookFile { get; set; }
        /// <summary>
        /// The current context of the <see cref="REPL"/>. Must be either an <see cref="Account"/> or a <see cref="Category"/>.
        /// </summary>
        internal object CurrentContext { get; set; }

        /// <summary>
        /// The list of the REPL's top-level commands.
        /// </summary>
        internal Command[] TopLevelCommands => [
            new DeleteCommand([]),
            new EditCommand([]),
            new EnterCommand([]),
            new ExitCommand([]),
            new GetCommand([]),
            new NewCommand([]),
            new SaveCommand([]),
            new SetCommand([]),
            new TransferCommand([]),
            ];

        /// <summary>
        /// Gets if the REPL is currently running. Set to false to end the loop.
        /// </summary>
        public bool IsRunning { get; internal set; } = false;

        private REPL()
        {
            CurrentAccountBook = new AccountBook(); // Start REPL with new empty accountbook
            CurrentContext = CurrentAccountBook;
        }

        /// <summary>
        /// Starts the REPL interface. Set <see cref="IsRunning"/> to false to end.
        /// </summary>
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

        /// <summary>
        /// Parses a user inputted string.
        /// </summary>
        /// <param name="userInput"></param>
        /// <exception cref="REPLSemanticErrorException"></exception>
        /// <exception cref="REPLCommandNotFoundException"></exception>
        private void Parse(string userInput)
        {
            // Get a list of command matches for the input
            var commandMatches = TopLevelCommands.Where(x => x.MatchStr(userInput));

            // Case: more than one match -> there's something wrong with my code!
            if (commandMatches.Count() > 1)
                throw new REPLSemanticErrorException($"Multiple top-level commands match str: \"{userInput.Split().First()}\"");

            // Case: exactly one match -> this is our command
            if (commandMatches.Count() == 1)
            {
                commandMatches.First().Parse(userInput, new ArgumentValueCollection());
                return;
            }

            // Otherwise if we find nothing, then the command mustn't exist.
            throw new REPLCommandNotFoundException($"Couldn't find command: \"{userInput.Split().First()}\"");
        }
    }
}