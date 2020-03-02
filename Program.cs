using System;

namespace Compilers
{
    enum TokenValues {
        INTEGER,
        PLUS,

        MINUS,

        MULTIPLY,
        DIVISION,
        
        EOF,
        WS_NSTRING,
    }

    class Program
    {
        static void Main(string[] args)
        {
            string line;
            while ((line = Console.ReadLine()) != null) {
                Tokenizer tokenizer = new Tokenizer(line);
                Interpreter intrp = new Interpreter(tokenizer);
                int result = intrp.Expr();
                Console.WriteLine(result);
            }
        }
    }
}