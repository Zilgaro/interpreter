using System;
using System.Collections;
using System.Text;

namespace Compilers {
    class Tokenizer {
        private String text;
        private int pos;
        private char currentChar;
        private Hashtable reservedKeywords;
        public Tokenizer(String text) {
            this.reservedKeywords = new Hashtable();
            reservedKeywords.Add("var", new Token(TokenValues.VAR, "var"));
            reservedKeywords.Add("int", new Token(TokenValues.INT, "int"));
            reservedKeywords.Add("bool", new Token(TokenValues.BOOL, "bool"));
            reservedKeywords.Add("for", new Token(TokenValues.FOR, "for"));
            reservedKeywords.Add("end", new Token(TokenValues.END, "end"));
            reservedKeywords.Add("print", new Token(TokenValues.PRINT, "print"));
            reservedKeywords.Add("string", new Token(TokenValues.STRING, "string"));
            reservedKeywords.Add("in", new Token(TokenValues.IN, "in"));
            reservedKeywords.Add("do", new Token(TokenValues.DO, "do"));
            reservedKeywords.Add("read", new Token(TokenValues.READ, "read"));
            reservedKeywords.Add("assert", new Token(TokenValues.ASSERT, "assert"));
            this.text = text;
            this.pos = 0;
            this.currentChar = text[pos];
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

        public char lookAhead() {
            if ((this.pos+1) > (text.Length - 1)) {
                return '\0';
            } else {
                return this.text[this.pos+1];
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

        public void ignoreSingleLineComment() {
            while (this.currentChar != '\n') {
                if (!forward()) {
                    break;
                }
            }
        }

        public void ignoreMultiLineComment() {
            while (this.currentChar != '*') {
                //check if reached EOF
                if (!forward()) {
                    break;
                }
            }

            if (this.currentChar == '*') {
                if (forward()) {
                    if (this.currentChar != '/') {
                        ignoreMultiLineComment();
                    }
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

        public Token id() {
            string result = "";
            while (char.IsLetterOrDigit(this.currentChar)) {
                result += this.currentChar;
                forward();
            }
            return (Token)this.reservedKeywords[result];
        }

        public Token NextToken() {
            Boolean eof = true;
            while (eof) {
                
                if (this.currentChar == ' ') {
                    this.ignoreWhitespace();
                    continue;
                }

                if (this.currentChar == '(') {
                    eof = forward();
                    return new Token(TokenValues.LPAREN, this.currentChar.ToString());
                }

                if (this.currentChar == ')') {
                    eof = forward();
                    return new Token(TokenValues.RPAREN, this.currentChar.ToString());
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
                /*
                    Ignoring comments done here for now
                */
                if (this.currentChar == '/') {
                    if (lookAhead() != '/') {
                        eof = forward();
                        return new Token(TokenValues.DIVISION, this.currentChar.ToString());    
                    } else if (lookAhead() != '*') {
                        ignoreMultiLineComment();
                        continue;
                    } else {
                        ignoreSingleLineComment();
                        continue;
                    }
                    
                }

                if (this.currentChar == '*') {
                    eof = forward();
                    return new Token(TokenValues.MULTIPLY, this.currentChar.ToString());
                }

                if (char.IsLetter(this.currentChar)) {
                    return this.id();
                }

                if (this.currentChar == ':' && this.lookAhead() == '=') {
                    forward();
                    forward();
                    return new Token(TokenValues.ASSIGN, ":=");
                }

                if (this.currentChar == ';') {
                    forward();
                    return new Token(TokenValues.SEMI, ";");
                }

                throw new InterpreterException("Syntax Error");
            }
            return new Token(TokenValues.EOF, null);
    }
}
}