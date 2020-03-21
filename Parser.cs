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

        public dynamic Factor() {
            /* Factor: INTEGER 
                     | LPAREN expr RPAREN
                     | Variable
            *           
            */
            if (currentToken.GetTokenValueType() == TokenValues.LPAREN) {
                Eat(TokenValues.LPAREN);
                BinOp result = Expr();
                Eat(TokenValues.RPAREN);
                return result;
            } else if (this.currentToken.GetTokenValueType() == TokenValues.INTEGER) {
                Token token = this.currentToken;
                Eat(TokenValues.INTEGER);
                return new Num(token);
            } else {
                return Variable();
            }
        }

        public dynamic Term() {
            // factor ((MUL|DIV) factor)*
            //int result = int.Parse(Factor());
            var result = Factor();
            while (this.currentToken.GetTokenValueType() == TokenValues.DIVISION || this.currentToken.GetTokenValueType() == TokenValues.MULTIPLY) {
                Token token = this.currentToken;
                if (currentToken.GetTokenValueType() == TokenValues.MULTIPLY) {
                    Eat(TokenValues.MULTIPLY);
                } else {
                    Eat(TokenValues.DIVISION);
                }
                result = new BinOp(result, token, this.Factor());
            }

            
            return result;
        }

        public dynamic Expr() {
            VisitableNode result = Term();

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
                        break;
                    case TokenValues.PLUS:
                        Eat(TokenValues.PLUS);
                        break;
                    default:
                        throw new InterpreterException("syntax error");
                }
                result = new BinOp(result, curr, this.Term());
            }
            return result;
        }

        public Root Program() {
            // Program : Statements
            VisitableNode[] nodes = Statements();
            Root root = new Root();
            for (int i = 0; i < nodes.Length; i++) {
                root.addChild(nodes[i]);
            }
            return root;
        }

        public VisitableNode[] Statements() {
            /* Statements : Statement SEMI
            *             | Statement SEMI Statements
            */
            VisitableNode node = Statement();

            VisitableNode[] results = {node};
            
            while (this.currentToken.GetTokenValueType() == TokenValues.SEMI) { 
                Eat(TokenValues.SEMI);
                results.Append(Statement());
            }
            if (this.currentToken.GetTokenValueType() == TokenValues.ID) {
                throw new InterpreterException("Syntax error");
            }
            return results; 
        }

        public VisitableNode Statement() {
            /* For now only assignment statements and empties
            Statement : AssignmentStatement
                      //| NormalStatement
                      | Empty
            */
            VisitableNode result = null;

                if (this.currentToken.GetTokenValueType() == TokenValues.ID) {
                    result = AssignmentStatement();
                } else {
                    result = Empty();
                } 

            return result;
        }

        public Assign AssignmentStatement() {
            /*
            * AssignmentStatement : Variable ASSIGN Expr 
            */
            Var left = Variable();
            Token token = currentToken;
            Eat(TokenValues.ASSIGN);
            var right = Expr();
            return new Assign(left, token, right);
        }

        public Var Variable() {
            /*
            * Variable : ID 
            */
            Var res = new Var(currentToken);
            Eat(TokenValues.ID);
            return res;
        }

        public NoOp Empty() {
            return new NoOp();
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
