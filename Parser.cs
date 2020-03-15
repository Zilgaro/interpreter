using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compilers
{
    class Parser
    {
        private Tokenizer tokenizer;
        private Token currentToken;

        public Parser(Tokenizer tokenizer) {
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

        public AST Factor() {
            // INTEGER | LPAREN expr RPAREN
            AST result = null;
            if (currentToken.GetTokenValueType() == TokenValues.LPAREN) {
                Eat(TokenValues.LPAREN);
                result = Expr();
                Eat(TokenValues.RPAREN);
                return result;
            } else if (this.currentToken.GetTokenValueType() == TokenValues.INTEGER) {
                Token token = this.currentToken;
                Eat(TokenValues.INTEGER);
                return new AST(token, new AST[2]);
            }
            return result;
        }

        public AST Term() {
            // factor ((MUL|DIV) factor)*
            //int result = int.Parse(Factor());
            AST result = Factor();

            while (this.currentToken.GetTokenValueType() == TokenValues.DIVISION || this.currentToken.GetTokenValueType() == TokenValues.MULTIPLY) {
                if (currentToken.GetTokenValueType() == TokenValues.MULTIPLY) {
                    Eat(TokenValues.MULTIPLY);
                    AST[] children = {result, this.Factor()};
                    result = new AST(new Token(TokenValues.MULTIPLY, "*"), children);
                } else {
                    Eat(TokenValues.DIVISION);
                    AST[] children = {result, this.Factor()};
                    result = new AST(new Token(TokenValues.DIVISION, "/"), children);
                }
            }

            
            return result;
        }

        public AST Expr() {
            AST result = Term();

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
                Token curr = this.currentToken;
                switch (type) {
                    case TokenValues.MINUS:
                        Eat(TokenValues.MINUS);
                        AST[] children = {result, this.Term()};
                        result = new AST(new Token(TokenValues.MINUS, "-"), children);
                        break;
                    case TokenValues.PLUS:
                        Eat(TokenValues.PLUS);
                        AST[] children1 = {result, this.Term()};
                        result = new AST(new Token(TokenValues.PLUS, "+"), children1);
                        break;
                    default:
                        throw new InterpreterException("syntax error");
                }
            }
            return result;
        }
        public AST parse() {
            return this.Expr();
        }
    }
}
