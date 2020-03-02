using System;

public class InterpreterException : Exception
{
    public InterpreterException()
    {
    }

    public InterpreterException(string message) : base(message)
    {
        
    }
}