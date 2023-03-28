using static System.Console;
namespace MJU23v_D10_inl_sveng
{
    internal class Program
    {
        static List<SweEngGloss>? dictionary;

        public Program()
        {
            // ↓ QuickFix So We Can Add New To Empty
            // ↓ If this is removed it should get catched by
            // ↓ NullReferenceException in Main Try
            dictionary = new List<SweEngGloss>();
        }

        class SweEngGloss
        {
            public string word_swe, word_eng;
            public SweEngGloss(string word_swe, string word_eng)
            {
                this.word_swe = word_swe; this.word_eng = word_eng;
            }
            public SweEngGloss(string line)
            {
                string[] words = line.Split('|');
                this.word_swe = words[0]; this.word_eng = words[1];
            }
        }
        static void Main(string[] args)
        {
            bool running = true;
            string defaultFile = "..\\..\\..\\dict\\sweeng.lis";
            WriteLine("Welcome to the dictionary app!");
            running = MainLogic(running, defaultFile);
            static bool MainLogic(bool running, string defaultFile)
            {
                try // OUTER TRY
                {
                    do
                    {
                        Write("> ");
                        string[] argument = ReadLine()!.Split();
                        string command = argument[0];
                        if (command.ToLower() == "quit" || command.ToLower() == "exit")
                        {
                            running = SafeExit();
                        }
                        else if (command == "load")
                        {
                            if (argument.Length >= 2) LoadTry(BuildPath(argument));
                            else if (argument.Length == 1) LoadTry(defaultFile);
                        }
                        else if (command == "list")
                        {
                            ListDict();
                        }
                        else if (command == "new")
                        {
                            if (argument.Length == 3) dictionary!.Add(new SweEngGloss(argument[1], argument[2]));
                            else if (argument.Length == 1) NewNoParams();
                        }
                        else if (command == "delete")
                        {
                            if (argument.Length == 2) DeleteWords(argument[1]);
                            if (argument.Length == 3) DeleteWords(argument[1], argument[2]);
                            else if (argument.Length == 1) Delete();
                        }
                        else if (command == "translate")
                        {
                            if (argument.Length >= 2) TranslateWord(argument[1]);
                            else if (argument.Length == 1) TranslateWord(ProptWord());
                        }
                        else
                        {
                            if (command.ToLower() != "help") WriteLine($"Unknown command: '{command}'\n");
                            WriteLine(AvailableCommands());
                        }
                    } while (running);
                }
                catch (NullReferenceException e)
                {
                    WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    WriteLine(e);
                }
                return running;
            }
            static string BuildPath(string[] argument)
            {
                string path = $"..\\..\\..\\dict\\{argument[1]}";
                if (!path.Contains(".lis"))
                    path += ".lis";
                return path;
            }

            static void NewNoParams()
            {
                PromptWords(out string? swe, out string? eng);
                if ((dictionary != null) && (swe != null) && (eng != null))
                {
                    dictionary.Add(new SweEngGloss(swe, eng));
                }
            }

            static void Delete()
            {
                PromptWords(out string? swe, out string? eng);
                if (swe != null)
                {
                    DeleteWords(swe, eng);
                }
            }

            static string ProptWord()
            {
                string? swe_word;
                do
                {
                    WriteLine("Write word to be translated: ");
                    swe_word = ReadLine();
                }
                while (swe_word == null);
                return swe_word;
            }
        } // End Main

        private static void TranslateWord(string word)
        {
            if (IsInList(word))
                return;
            else
                WriteLine($"{word} could not be found.");

            static bool IsInList(in string word)
            {
                bool result = false;
                foreach (SweEngGloss gloss in dictionary!)
                {
                    if (gloss.word_swe == word)
                    {
                        WriteLine($"English for {gloss.word_swe} is {gloss.word_eng}");
                        result = true;
                    }
                    if (gloss.word_eng == word)
                    {
                        WriteLine($"Swedish for {gloss.word_eng} is {gloss.word_swe}");
                        result = true;
                    }
                }
                return result;
            }
        }

        private static void ListDict()
        {
            if (dictionary != null)
            {
                foreach (SweEngGloss gloss in dictionary)
                {
                    WriteLine($"{gloss.word_swe,-10}  - {gloss.word_eng,-10}");
                }
            }
        }

        private static bool SafeExit()
        {
            bool running;
            WriteLine("Goodbye!");
            running = false;
            return running;
        }
        private static void PromptWords(out string? swe, out string? eng)
        {
            do
            {
                WriteLine("Write word in Swedish: ");
                swe = ReadLine();
            }
            while (swe == null);
            do
            {
                Write("Write word in English: ");
                eng = ReadLine();
            }
            while (swe == null);
        }

        private static void DeleteWords(string swe, string? eng = null)
        {
            int index = -1;
            if (eng == null)
                for (int i = 0; i < dictionary!.Count; i++)
                {
                    SweEngGloss gloss = dictionary[i];
                    if (gloss.word_swe == swe)
                        index = i;
                }
            else
                for (int i = 0; i < dictionary!.Count; i++)
                {
                    SweEngGloss gloss = dictionary[i];
                    if (gloss.word_swe == swe && gloss.word_eng == eng)
                        index = i;
                }
            try // INNER TRY check if trying to remove word not in list
            {
                dictionary.RemoveAt(index);
            }
            catch (ArgumentOutOfRangeException)
            {
                WriteLine($"Could not find {swe} {eng} in list.");
            }
            catch (Exception exception)
            {
                WriteLine(exception);
            }
        }


        private static void LoadTry(string path)
        {
            if (File.Exists(path)) LoadDict(path);
            else WriteLine($"Failed to load from. {Path.GetFullPath(path)}");
        }

        private static void LoadDict(string argument)
        {
            using (StreamReader sr = new StreamReader(argument))
            {
                dictionary = new List<SweEngGloss>(); // Empty it!
                string? line = sr.ReadLine();
                while (line != null)
                {
                    SweEngGloss gloss = new SweEngGloss(line);
                    dictionary.Add(gloss);
                    line = sr.ReadLine();
                }
            }
        }

        static String AvailableCommands() => "quit, load, load filename\n" +
                                             "list, new, new [swe] [eng]\n" +
                                             "delete, delete [swe] [eng]\n" +
                                             "translate, translate [swe] [eng]";
    } // End Program
}
/*
 *  TASKs (Pasted from assignment)
 *  DID: Notera eventuella fel! Lägg in dem som // FIXME-kommentarer, [X]
 *  DID: om ni vill lägga in en helpfunktion [X]
 *  DID: enbokstavsvariabler skall döpas om [X]
 *  DID: koddubbletter skall bort, [X] 
 */
// DID: Running = false för att avsluta
// DID: StringOfAvailableCommands
// DID: Skriv ut StringOfAvailableCommands
// DID: Faktoriserat ut till metod LoadDict()
// DID: Byggt om om pathSträngen så att användaren kan skriva enbart filnamnet
// DID: Quick n dirty check if file exist.
// DID: Faktoriserat ut till PromptWords()
// DID: Faktorisera ut till DeleteWords()
// DID: Namngivit enTeckensVariabler s & e
// DID: Bryt ut till TranslateWord()
// FIXED: FileNotFoundException (In LoadTry)
// FIXED: Index was out of range (om input saknas i listan) (In DeleteWords)
// DID:  Informera användaren om ordet saknas i listan.(Translate)
// DID: Städat lite
// FIXED: Added CatchBlock för om dictionary saknas vid new
// DID: Flyttat new dictionarey till constructorn för program
// DID: Gått igenom de "gröna ormarna" och sidtacklat dem varsamt med (?) (!) och (if / while != null)