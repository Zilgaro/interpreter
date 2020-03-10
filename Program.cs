using System;
using System.Collections;

namespace Compilers
{
    enum TokenValues {
        INTEGER,
        PLUS,
        MINUS,
        MULTIPLY,
        DIVISION,
        LPAREN,
        RPAREN,
        EOF,
        WS_NSTRING,
        VAR,
        INT,
        FOR,
        END,
        IN,
        DO,
        READ,
        PRINT,
        STRING,
        BOOL,
        ASSERT,
        ASSIGN,
        SEMI,
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
