using System;
using System.Collections;
using System.Text;

namespace Compilers {
    class Tokenizer {
        private String text;
        private int pos;
        private char currentChar;
        private Stack parentheses; 
        public Tokenizer(String text) {
            this.text = text;
            this.pos = 0;
            this.currentChar = text[pos];
            this.parentheses = new Stack();
        }

        public bool forward() {
            this.pos++;
            if (this.pos > (text.Length - 1)) {
                return false;
            } else {
                this.currentChar = this.text[this.pos];
                return true;
            }
        }

        public void ignoreWhitespace() {
            while (this.currentChar == ' ') {
                //check if reached EOF
                if (!forward()) {
                    break;
                }
            }
        }

        public String getIntegerStringValue() {
            String result = "";
            
            while (Char.IsDigit(currentChar)) {
                result += currentChar;
                if (!forward()) {
                    break;
                }
            }
            return result;
        }

        public Token NextToken() {
            Boolean eof = true;
            while (eof) {
                if (this.currentChar == ' ') {
                    this.ignoreWhitespace();
                    continue;
                }

                if (this.currentChar == '(') {
                    this.parentheses.Push(null);
                    eof = forward();
                    continue;
                }

                if (this.currentChar == ')') {
                    if (this.parentheses.Count != 0) {
                        this.parentheses.Pop();
                        eof = forward();
                        continue;
                    } else {
                        throw new InterpreterException("Syntax error: orphan closing parentheses");
                    }
                }

                if (Char.IsDigit(this.currentChar)) {
                    return new Token(TokenValues.INTEGER, getIntegerStringValue());
                }

                if (this.currentChar == '+') {
                    eof = forward();
                    return new Token(TokenValues.PLUS, this.currentChar.ToString());
                }

                if (this.currentChar == '-') {
                    eof = forward();
                    return new Token(TokenValues.MINUS, this.currentChar.ToString());
                }

                if (this.currentChar == '/') {
                    eof = forward();
                    return new Token(TokenValues.DIVISION, this.currentChar.ToString());
                }

                if (this.currentChar == '*') {
                    eof = forward();
                    return new Token(TokenValues.MULTIPLY, this.currentChar.ToString());
                }


                throw new InterpreterException("Syntax Error");
            }
            if (this.parentheses.Count != 0) {
                throw new InterpreterException("Syntax error: Opening parentheses without matching closing parentheses");
            }
            return new Token(TokenValues.EOF, null);
    }
}
}