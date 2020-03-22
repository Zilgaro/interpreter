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
                     | STRING
                     | TRUE
                     | FALSE
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
            } else if (this.currentToken.GetTokenValueType() == TokenValues.STRING) {
                Token token = this.currentToken;
                Eat(TokenValues.STRING);
                return new Str(token);
            } else if (this.currentToken.GetTokenValueType() == TokenValues.TRUE) {
                Token token = this.currentToken;
                Eat(TokenValues.TRUE);
                return new BoolNode(token);
            }else if (this.currentToken.GetTokenValueType() == TokenValues.FALSE) {
                Token token = this.currentToken;
                Eat(TokenValues.FALSE);
                return new BoolNode(token);
            } else {
                return Variable();
            }
        }

        public dynamic Term() {
            //Term: Factor ((MUL|DIV) factor)*
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
            values.Add(TokenValues.EQUAL);
            values.Add(TokenValues.LESSTHAN);
            /*
                expr    : Term ((PLUS|MINUS|EQUAL|LESSTHAN) Term) *
                Term    : Factor ((MUL|DIV) Factor) * 
                Factor  : INTEGER | LPAREN expr RPAREN | Variable | STRING | TRUE | FALSE
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
                    case TokenValues.EQUAL:
                        Eat(TokenValues.EQUAL);
                        break;
                    case TokenValues.LESSTHAN:
                        Eat(TokenValues.LESSTHAN);
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
            List<VisitableNode> nodes = Statements();
            Root root = new Root();
            for (int i = 0; i < nodes.Count; i++) {
                root.addChild(nodes[i]);
            }
            return root;
        }

        public List<VisitableNode> Statements() {
            /* Statements : Statement SEMI
            *             | Statement SEMI Statements
            */
            VisitableNode node = Statement();

            List<VisitableNode> results = new List<VisitableNode>();
            results.Add(node);

            while (this.currentToken.GetTokenValueType() == TokenValues.SEMI) { 
                Eat(TokenValues.SEMI);
                results.Add(Statement());
            }
            if (this.currentToken.GetTokenValueType() == TokenValues.ID) {
                throw new InterpreterException("Syntax error");
            }
            return results; 
        }

        public VisitableNode Statement() {
            /* For now only assignment statements and empties
            Statement : AssignmentStatement
                      | VariableDeclaration
                      | Print
                      | Empty
                      | AssertionStatement
                      | ReadStatement
                      | ForStatement
            */
            VisitableNode result = null;

                if (this.currentToken.GetTokenValueType() == TokenValues.ID) {
                    result = AssignmentStatement();
                } else if (this.currentToken.GetTokenValueType() == TokenValues.VAR) {
                    Eat(TokenValues.VAR);
                    result = VariableDeclaration();
                } else if (this.currentToken.GetTokenValueType() == TokenValues.PRINT) {
                    Eat(TokenValues.PRINT);
                    result = Print();
                } else if (this.currentToken.GetTokenValueType() == TokenValues.ASSERT) {
                    result = AssertionStatement();
                } else if (this.currentToken.GetTokenValueType() == TokenValues.READ) {
                    result = ReadStatement();
                } else if (this.currentToken.GetTokenValueType() == TokenValues.FOR) {
                    result = ForStatement();
                } else {
                    result = Empty();
                } 

            return result;
        }

        public Print Print() {
            /*
            * Print: PRINT Expr
            */
            VisitableNode n = Expr();
            return new Print(n);
        }

        public Assert AssertionStatement() {
            /*
            * AssertionStatement: ASSERT LPAREN expr RPAREN
            */
            Eat(TokenValues.ASSERT);
            Eat(TokenValues.LPAREN);
            VisitableNode n = Expr();
            Eat(TokenValues.RPAREN);
            return new Assert(n);

        }

        public ForNode ForStatement() {
            /*
            * ForStatement: FOR Var IN Expr DDOT Expr DO Statements END FOR
            */
            Eat(TokenValues.FOR);
            Var v = Variable();
            Eat(TokenValues.IN);
            VisitableNode from = Expr();
            Eat(TokenValues.DDOT);
            VisitableNode to = Expr();
            Eat(TokenValues.DO);
            List<VisitableNode> s = Statements();
            Eat(TokenValues.END);
            Eat(TokenValues.FOR);
            return new ForNode(v,from,to,s);
        }

        public ReadNode ReadStatement() {
           /*
           * ReadStatement: READ Variable
           */
           Eat(TokenValues.READ);
           Var v = Variable();
           return new ReadNode(v);
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

        public Assign DecAssign(Var left) {
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

        public VarDecl VariableDeclaration() {
            /*
            * VariableDeclaration : VAR COLON TypeSpec (ASSIGN Expr)?
            */
            Var n = Variable();
            Eat(TokenValues.COLON);

            TypeNode t = TypeSpec();

            if (this.currentToken.GetTokenValueType() == TokenValues.ASSIGN) {
                Assign a = DecAssign(n);
                return new VarDecl(n, t ,a);
            }
            return new VarDecl(n, t);
        }

        public TypeNode TypeSpec() {
            /*
            * TypeSpec:   INT 
                        | STRING
                        | BOOL

            */
            Token token = this.currentToken;

            if (this.currentToken.GetTokenValueType() == TokenValues.INT) {
                Eat(TokenValues.INT);
            } else if (this.currentToken.GetTokenValueType() == TokenValues.STRING) {
                Eat(TokenValues.STRING);
            } else if (this.currentToken.GetTokenValueType() == TokenValues.BOOL) {
                Eat(TokenValues.BOOL);
            }
            return new TypeNode(token);
        }

        public NoOp Empty() {
            return new NoOp();
        }

        public Root parse() {
            Root node = Program();
            if (this.currentToken.GetTokenValueType() != TokenValues.EOF) {
                throw new InterpreterException("EOF expected, probably missing a semicolon");
            }
            return node;
        }
    }
}
