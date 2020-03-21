using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Compilers {
    class Tokenizer {
        private String text;
        private int pos;
        private char currentChar;
        private Dictionary<String, Token> reservedKeywords;
        public Tokenizer(String text) {
            this.reservedKeywords = new Dictionary<String, Token>();
            reservedKeywords.Add("var", new Token(TokenValues.ID, "var"));
            reservedKeywords.Add("int", new Token(TokenValues.ID, "int"));
            reservedKeywords.Add("bool", new Token(TokenValues.ID, "bool"));
            reservedKeywords.Add("for", new Token(TokenValues.ID, "for"));
            reservedKeywords.Add("end", new Token(TokenValues.ID, "end"));
            reservedKeywords.Add("print", new Token(TokenValues.ID, "print"));
            reservedKeywords.Add("string", new Token(TokenValues.ID, "string"));
            reservedKeywords.Add("in", new Token(TokenValues.ID, "in"));
            reservedKeywords.Add("do", new Token(TokenValues.ID, "do"));
            reservedKeywords.Add("read", new Token(TokenValues.ID, "read"));
            reservedKeywords.Add("assert", new Token(TokenValues.ID, "assert"));
            this.text = text;
            this.pos = 0;
            this.currentChar = text[pos];
        }

        public bool forward() {
            this.pos++;
            if (this.pos > (text.Length - 1)) {
                this.currentChar = '\0';
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
            if (this.reservedKeywords.ContainsKey(result)) {
                return this.reservedKeywords[result];
            } else {
                this.reservedKeywords.Add(result, new Token(TokenValues.ID, result));
                return this.reservedKeywords[result];
            }
        }

        public Token NextToken() {
            Boolean eof = true;
            while (eof) {

                if (this.currentChar == '\0') {
                    break;
                }

                if (char.IsLetter(this.currentChar)) {
                    return this.id();
                }
                
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

                if (this.currentChar == ':' && this.lookAhead() == '=') {
                    eof =forward();
                    eof =forward();
                    return new Token(TokenValues.ASSIGN, ":=");
                }

                if (this.currentChar == ';') {
                    eof = forward();
                    return new Token(TokenValues.SEMI, ";");
                }
                throw new InterpreterException("Syntax Error");
            }
            return new Token(TokenValues.EOF, null);
    }
}
}