using System;
using System.Collections;

namespace Compilers {
    class Interpreter : Visitor {
        public Hashtable globalST; // Global scope symbol table
        private Parser parser;
        public Interpreter(Parser parser, Hashtable globalST) {
            this.parser = parser;
            this.globalST = globalST;
        } 
        /*
        public int visitNum(AST node) {
            if (node.GetToken().GetTokenValueType() == TokenValues.INTEGER) {
                return Int32.Parse(node.GetToken().GetValue());
            } else {
                throw(new InterpreterException("Invalid syntax"));
            }
        }

        public int visitBinOp(AST node) {
            int left = this.visit(node.GetChildren()[0]);
            int right = this.visit(node.GetChildren()[1]);   

            switch (node.GetToken().GetTokenValueType()) {
                case TokenValues.PLUS:
                    return left + right;
                case TokenValues.MINUS:
                    return left - right;
                case TokenValues.MULTIPLY:
                    return left * right;
                case TokenValues.DIVISION:
                    return left / right;
                default:
                    throw(new InterpreterException("Invalid syntax"));
            }
        }

        public int visit (AST node) {
            TokenValues v = node.GetToken().GetTokenValueType();
            Console.WriteLine("VISITING: " + v);
            if (v == TokenValues.INTEGER) {
                return this.visitNum(node);
            } else if (v == TokenValues.MULTIPLY || v == TokenValues.MINUS || v == TokenValues.PLUS || v ==  TokenValues.DIVISION) {
                return this.visitBinOp(node);
            } else if (v == TokenValues.ASSIGN) {
                this.visitAssign(node);
                return 0;
            } else if (v == TokenValues.ID) {
                return this.visitVar(node);
            } else {
                throw new InterpreterException("syntax error");
            }
        }

        public void visitProgram(AST node) {
            for (int i = 0; i < node.GetChildren().Length; i++) {
                visit(node.GetChildren()[i]);
            }
        }

        public void visitNoOp(AST node) {}

        public void visitAssign(AST node) {
            String name = node.GetLeft().GetToken().GetValue();
            globalST.Add(name, visit(node.GetRight()));
        }

        public int visitVar(AST node) {
            String name = node.GetToken().GetValue();
            AST value = (AST) globalST[name];
            if (value == null) {
                throw new InterpreterException(string.Format("Error accessing {0}", name));
            } else {
                Console.WriteLine("We here: " + Int32.Parse(value.GetToken().GetValue()));
                return Int32.Parse(value.GetToken().GetValue());
            }
        }
        */

        public void visit(Root root) {
            root.getChild(0).accept(this);
        }

        public void visit(Assign assign) {
            string name = assign.GetLeft().GetValue();
            Type t = assign.GetRight().GetType();
            if (t == typeof(Num)) {
                this.globalST[name] = this.visit((Num) assign.GetRight());
            } else if (t == typeof(BinOp)) {
                this.globalST[name] = this.visit((BinOp) assign.GetRight());
            } else if (t == typeof(Var)) {
                this.globalST[name] = this.visit((Var) assign.GetRight());
            } else {
                throw new InterpreterException("Unsupported assign visit");
            }
        }

        public int visit(Num num) {
            return Int32.Parse(num.getValue());
        }

        public int visit(Var var) {
            String name = var.GetValue();
        
            if (globalST.ContainsKey(name)) {
                return (int) globalST[name];
            } else {
                throw new InterpreterException("Undeclared variable");
            }
        }

        public int visit(BinOp binOp) {
            int left;
            int right;
            Type leftType = binOp.getLeft().GetType();
            Type rightType = binOp.getRight().GetType();
            if (leftType == typeof(Num)) {
                left = this.visit((Num)binOp.getLeft());
            } else if (leftType == typeof(BinOp)) {
                left = this.visit((BinOp)binOp.getLeft());
            } else {
                left = this.visit((Var)binOp.getLeft());
            }
            
            if (rightType == typeof(Num)) {
                right = this.visit((Num)binOp.getRight());
            } else if (rightType == typeof(BinOp)) {
                right = this.visit((BinOp)binOp.getRight());
            } else {
                right = this.visit((Var)binOp.getRight());
            }
               

            switch (binOp.getOp().GetTokenValueType()) {
                case TokenValues.PLUS:
                    return left + right;
                case TokenValues.MINUS:
                    return left - right;
                case TokenValues.MULTIPLY:
                    return left * right;
                case TokenValues.DIVISION:
                    return left / right;
                default:
                    throw(new InterpreterException("Invalid syntax"));
            }
        }

        public void visit(NoOp noOp) {}

        public void interpret() {
            Root root = this.parser.parse();
            this.visit(root);

            //return result; 
        }
    }
}