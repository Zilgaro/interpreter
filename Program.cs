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
                Parser parser = new Parser(tokenizer);
                Interpreter interpreter = new Interpreter(parser);
                int result = interpreter.interpret();
                Console.WriteLine(result);
            }
        }
    }
}
