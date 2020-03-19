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
            if (this.currentToken == null) {
                throw new InterpreterException("first token null");
            }
        }

        public void Eat(TokenValues type) {
            if (this.currentToken.GetTokenValueType() == type) {
                this.currentToken = tokenizer.NextToken();
            } else {
                throw new InterpreterException($"Token type did not match, {type} expected, got {this.currentToken.GetTokenValueType()}");
            }
        }

        public AST Factor() {
            /* Factor: INTEGER 
                     | LPAREN expr RPAREN
                     | Variable
            *           
            */
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
            } else {
                return Variable();
            }
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

        public AST Program() {
            // Program : Statements
            AST[] nodes = Statements();
            AST[] children = {};
            AST root = new AST(new Token(TokenValues.ROOT, null), children);

            for (int i = 0; i < nodes.Length; i++) {
                root.GetChildren().Append(nodes[i]);
            }
            return root;
        }

        public AST[] Statements() {
            /* Statements : Statement SEMI
            *             | Statement SEMI Statements
            */
            AST node = Statement();

            AST[] results = {node};
            
            while (this.currentToken.GetTokenValueType() == TokenValues.SEMI) { 
                Eat(TokenValues.SEMI);
                results.Append(Statement());
            }
            if (this.currentToken.GetTokenValueType() == TokenValues.ID) {
                throw new InterpreterException("Syntax error");
            }
            return results; 
        }

        public AST Statement() {
            /* For now only assignment statements and empties
            Statement : AssignmentStatement
                      //| NormalStatement
                      | Empty
            */
            AST result = null;
            try {
                if (this.currentToken.GetTokenValueType() == TokenValues.ID) {
                    result = AssignmentStatement();
                } else {
                    result = Empty();
                }
            } catch (Exception e) {
                result = Empty();
            }
            return result;
        }

        public Assign AssignmentStatement() {
            /*
            * AssignmentStatement : Variable ASSIGN Expr 
            */
            Var left = Variable();
            Eat(TokenValues.ASSIGN);
            AST right = Expr();
            AST[] children = {left, right};
            return new AST(this.currentToken, children);
        }

        public Var Variable() {
            /*
            * Variable : ID 
            */
            Eat(TokenValues.ID);
            return new Var(currentToken, null);
        }

        public AST Empty() {
            return new AST(null, null);
        }
        public Root parse() {
            Root node = Program();
            if (this.currentToken.GetTokenValueType() != TokenValues.EOF) {
                throw new InterpreterException("EOF expected");
            }
            return node;
        }
    }
}
