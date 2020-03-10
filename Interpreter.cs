using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compilers
{
    class Interpreter
    {
        private Tokenizer tokenizer;
        private Token currentToken;

        public Interpreter(Tokenizer tokenizer) {
            this.tokenizer = tokenizer;
            currentToken = tokenizer.NextToken();
        }

        public void Eat(TokenValues type) {
            if (this.currentToken.GetTokenValueType() == type) {
                this.currentToken = tokenizer.NextToken();
            } else {
                throw new InterpreterException($"Token type did not match, {type} expected, got {this.currentToken.GetTokenValueType()}");
            }
        }

        public string Factor() {
            // INTEGER | LPAREN expr RPAREN
            string result = null;
            if (currentToken.GetTokenValueType() == TokenValues.LPAREN) {
                Eat(TokenValues.LPAREN);
                result = Expr().ToString();
                Eat(TokenValues.RPAREN);
                return result;
            } else if (this.currentToken.GetTokenValueType() == TokenValues.INTEGER) {
                Token token = this.currentToken;
                Eat(TokenValues.INTEGER);
                return token.GetValue();
            }
            return result;
        }

        public int Term() {
            // factor ((MUL|DIV) factor)*
            int result = int.Parse(Factor());

            while (this.currentToken.GetTokenValueType() == TokenValues.DIVISION || this.currentToken.GetTokenValueType() == TokenValues.MULTIPLY) {
                if (currentToken.GetTokenValueType() == TokenValues.MULTIPLY) {
                    Eat(TokenValues.MULTIPLY);
                    result = result * int.Parse(Factor());
                } else {
                    Eat(TokenValues.DIVISION);
                    result = result / int.Parse(Factor());
                }
            }
            return result;
        }

        public int Expr() {
            int result = Term();

            HashSet<TokenValues> values = new HashSet<TokenValues>();
            values.Add(TokenValues.PLUS);
            values.Add(TokenValues.MINUS);
            /*
                expr    : Term ((PLUS|MINUS) Term) *
                Term    : Factor ((MUL|DIV) Factor) * 
                Factor  : INTEGER | LPAREN expr RPAREN
            */
            while(values.Contains(this.currentToken.GetTokenValueType())) {
                TokenValues type = this.currentToken.GetTokenValueType();
                switch (type) {
                    case TokenValues.MINUS:
                        Eat(TokenValues.MINUS);
                        result = result - Term();
                        break;
                    case TokenValues.PLUS:
                        Eat(TokenValues.PLUS);
                        result = result + Term();
                        break;
                    default:
                        throw new InterpreterException("syntax error");
                }
            }

            return result;
        }
    }
}
