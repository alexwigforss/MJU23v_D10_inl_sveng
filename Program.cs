using static System.Console;
namespace MJU23v_D10_inl_sveng
{
    internal class Program
    {
        static List<SweEngGloss> dictionary;
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
            string defaultFile = "..\\..\\..\\dict\\sweeng.lis";
            WriteLine("Welcome to the dictionary app!");
            do
            {
                Write("> ");
                string[] argument = ReadLine().Split();
                string command = argument[0];
                if (command == "quit")
                {
                    WriteLine("Goodbye!");
                    // FIXME: sätt runningvariabel till false så den stängs
                }
                else if (command == "load")
                {
                    if (argument.Length == 2)
                    {   // TBD: Faktorisera ut till metod LoadDict()
                        // FIXME: Bygg om om pathSträngen så att användaren kan skriva enbart filnamnet
                        using (StreamReader sr = new StreamReader(argument[1]))
                        {
                            dictionary = new List<SweEngGloss>(); // Empty it!
                            string line = sr.ReadLine();
                            while (line != null)
                            {
                                SweEngGloss gloss = new SweEngGloss(line);
                                dictionary.Add(gloss);
                                line = sr.ReadLine();
                            }
                        }
                    }
                    else if (argument.Length == 1)
                    {   // TBD: Faktorisera ut till metod LoadDict()
                        using (StreamReader sr = new StreamReader(defaultFile))
                        {
                            dictionary = new List<SweEngGloss>(); // Empty it!
                            string line = sr.ReadLine();
                            while (line != null)
                            {
                                SweEngGloss gloss = new SweEngGloss(line);
                                dictionary.Add(gloss);
                                line = sr.ReadLine();
                            }
                        }
                    }
                }
                else if (command == "list")
                {
                    foreach (SweEngGloss gloss in dictionary)
                    {
                        WriteLine($"{gloss.word_swe,-10}  - {gloss.word_eng,-10}");
                    }
                }
                else if (command == "new")
                {
                    if (argument.Length == 3)
                    {
                        dictionary.Add(new SweEngGloss(argument[1], argument[2]));
                    }
                    else if (argument.Length == 1)
                    {
                        // TBD: Faktorisera ut till WordPrompt()
                        WriteLine("Write word in Swedish: ");
                        string s = ReadLine();
                        Write("Write word in English: ");
                        string e = ReadLine();

                        dictionary.Add(new SweEngGloss(s, e));
                    }
                }
                else if (command == "delete")
                {
                    if (argument.Length == 3)
                    {
                        // TBD: Faktorisera ut till DeleteWord()
                        int index = -1;
                        for (int i = 0; i < dictionary.Count; i++)
                        {
                            SweEngGloss gloss = dictionary[i];
                            if (gloss.word_swe == argument[1] && gloss.word_eng == argument[2])
                                index = i;
                        }
                        dictionary.RemoveAt(index);
                    }
                    else if (argument.Length == 1)
                    {
                        // TBD: Faktorisera ut till WordPrompt()
                        WriteLine("Write word in Swedish: ");
                        string s = ReadLine();
                        Write("Write word in English: ");
                        string e = ReadLine();

                        // TBD: Faktorisera ut till DeleteWord()
                        int index = -1;
                        for (int i = 0; i < dictionary.Count; i++)
                        {
                            SweEngGloss gloss = dictionary[i];
                            if (gloss.word_swe == s && gloss.word_eng == e)
                                index = i;
                        }
                        // FIXME: Index was out of range (om input saknas i listan)
                        dictionary.RemoveAt(index);
                    }
                }
                else if (command == "translate")
                {
                    if (argument.Length == 2)
                    {
                        // TBD: Bryt ut till TranslateWord()
                        foreach (SweEngGloss gloss in dictionary)
                        {
                            if (gloss.word_swe == argument[1])
                                WriteLine($"English for {gloss.word_swe} is {gloss.word_eng}");
                            if (gloss.word_eng == argument[1])
                                WriteLine($"Swedish for {gloss.word_eng} is {gloss.word_swe}");
                        }
                    }
                    else if (argument.Length == 1)
                    {
                        WriteLine("Write word to be translated: ");
                        string s = ReadLine();
                        // TBD: Bryt ut till TranslateWord()
                        // FIXME:  System.NullReferenceException (om dictionary inte laddats)
                        // TBD: Informera användaren om ordet saknas i listan.
                        foreach (SweEngGloss gloss in dictionary)
                        {
                            if (gloss.word_swe == s)
                                WriteLine($"English for {gloss.word_swe} is {gloss.word_eng}");
                            if (gloss.word_eng == s)
                                WriteLine($"Swedish for {gloss.word_eng} is {gloss.word_swe}");
                        }
                    }
                }
                else
                {
                    if (command.ToLower() != "help") WriteLine($"Unknown command: '{command}'\n");
                    WriteLine(AvailableCommands());
                    //DID: Skriv ut StringOfAvailableCommands
                }
            }
            while (true);
        } // End Main
          // DOIN: StringOfAvailableCommands
        static String AvailableCommands() => "quit, load, load filename\n" +
                                             "list, new, new [swe] [eng]\n" +
                                             "delete, delete [swe] [eng]\n" +
                                             "translate, translate [swe] [eng]";
    } // End Program
}
/*
 *  TASK: s (Pasted from assignment)
 *  Notera eventuella fel! Lägg in dem som // FIXME-kommentarer, [Done]
 *  om ni vill lägga in en helpfunktion []
 *  TODO: enbokstavsvariabler skall döpas om []
 *  TODO: koddubbletter skall bort, [] 
 *  Rekomenderade tester [Done] Resulterat i Fixme's
 */
