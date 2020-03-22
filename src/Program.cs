using System;
using System.Collections;
using System.IO;
using System.Linq;

namespace Compilers
{
    public enum TokenValues {
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
        ID,
        ROOT,
        COLON, 
        EQUAL,
        LESSTHAN,
        TRUE,
        FALSE,
        
    }

    class Program
    {
        static int Main(string[] args)
        {
            StreamReader file;
            try
            {
                file = new StreamReader("ExampleProgram2.txt");
            } catch (Exception e) {
                Console.WriteLine(e);
                return 1;
            }

            Hashtable globalST = new Hashtable();
            string text = file.ReadToEnd();
            //while ((line = file.ReadLine()) != null) {
            Tokenizer tokenizer = new Tokenizer(text);
            Parser parser = new Parser(tokenizer);
            Interpreter interpreter = new Interpreter(parser, globalST);
            interpreter.interpret();
            //}
            // Program state at the end
            Console.WriteLine("Global symbol table key-value pairs:");
            foreach (var key in globalST.Keys) {
                Console.WriteLine("Key: " + key + ", Value: " + globalST[key]);
            }
            file.Close();
            return 0;

        }
    }
}
