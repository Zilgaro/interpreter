using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Compilers {
    class Interpreter : Visitor {
        public Hashtable globalST; // Global scope symbol table
        private Parser parser;
        public Interpreter(Parser parser, Hashtable globalST) {
            this.parser = parser;
            this.globalST = globalST;
        } 
    
        public void visit(Root root) {
            foreach(VisitableNode n in root.getChildren()) {
                n.accept(this);
            }
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
            } else if (t == typeof(Str)){
                this.globalST[name] = this.visit((Str) assign.GetRight());
            } else if (t == typeof(BoolNode)){
                this.globalST[name] = this.visit((BoolNode) assign.GetRight());
            } else {
                throw new InterpreterException("Unsupported type assignment");
            }
        }

        public int visit(Num num) {
            return Int32.Parse(num.getValue());
        }

        public string visit(Str str) {
            return str.getValue();
        }

        public bool visit(BoolNode boolNode) {
            return bool.Parse(boolNode.getValue());
        }

        public dynamic visit(Var var) {
            String name = var.GetValue();
        
            if (globalST.ContainsKey(name)) {
                return globalST[name];
            } else {
                throw new InterpreterException("Undeclared variable");
            }
        }

        // A hot mess but gets the return value based on types given left and right
        public dynamic visit(BinOp binOp) {
            int left = 0;
            int right = 0;
            Type leftType = binOp.getLeft().GetType();
            Type rightType = binOp.getRight().GetType();
            if (leftType == typeof(Num)) {
                left = this.visit((Num)binOp.getLeft());
            } else if (leftType == typeof(BinOp)) {
                left = this.visit((BinOp)binOp.getLeft());
            } else if (leftType == typeof(Var)) {
                left = this.visit((Var)binOp.getLeft());
            }
            
            if (rightType == typeof(Num)) {
                right = this.visit((Num)binOp.getRight());
            } else if (rightType == typeof(BinOp)) {
                right = this.visit((BinOp)binOp.getRight());
            } else if (rightType == typeof(Var)) {
                right = this.visit((Var)binOp.getRight());
            }

            if (leftType == typeof(Str)) {
                if (rightType == typeof(Str)) {
                    if (binOp.getOp().GetTokenValueType() == TokenValues.PLUS) {
                        return this.visit((Str)binOp.getLeft()) + this.visit((Str)binOp.getRight());
                    }
                } else {
                    throw new InterpreterException(String.Format("Incompatible types for operation {0}: {1} {2}", binOp.getOp().GetTokenValueType(), leftType, rightType));
                }
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
                case TokenValues.EQUAL:
                    return left == right;
                case TokenValues.LESSTHAN:
                    return left < right;
                default:
                    throw(new InterpreterException("Invalid syntax"));
            }
        }

        public void visit(NoOp noOp) {}
        public void visit(TypeNode typeNode) {}
        public void visit(VarDecl varDecl) {
            if (varDecl.GetAssign() != null) {
                visit(varDecl.GetAssign());
            }
        }

        /*
        * This is again horribly inefficient but my C# skills are lacking to do this nicer
        */
        public void visit(Print print) {
            Type printType = print.GetNode().GetType();
            if (printType == typeof(BinOp)) {
                Console.WriteLine(visit((BinOp)print.GetNode()));
            } else if (printType == typeof(Num)) {
                Console.WriteLine(visit((Num)print.GetNode()));
            } else if (printType == typeof(Var)) {
                Console.WriteLine(visit((Var)print.GetNode()));
            } else if (printType == typeof(Str)) {
                Console.WriteLine(visit((Str)print.GetNode()));
            }
        }

        public void visit(Assert assert) {
            Type assertType = assert.GetNode().GetType();

            if (assertType == typeof(BinOp)) {
                bool result = visit((BinOp)assert.GetNode());
                if (result == false) {
                    throw new InterpreterException("Assertion failed");
                }
            } else if (assertType == typeof(BoolNode)) {
                bool result = visit((BoolNode)assert.GetNode());
                if (result == false) {
                    throw new InterpreterException("Assertion failed");
                }
            }
        }

        public void visit(ReadNode readNode) {
            int read = Int32.Parse(Console.ReadLine());
            globalST[readNode.GetNode().GetValue()] = read;
        }

        public void visit(ForNode forNode) {
            int from;
            int to;
            Type fromType = forNode.getFrom().GetType();
            
            if (fromType == typeof(Num)) {
                from = visit((Num)forNode.getFrom());
            } else if (fromType == typeof(Var)) {
                from = (int) visit((Var)forNode.getFrom());
            } else if (fromType == typeof(BinOp)) {
                from = (int) visit((BinOp)forNode.getFrom());
            } else {
                throw new InterpreterException("Syntax error on For loop condition FROM");
            }

            Type toType = forNode.getTo().GetType();
            if (toType == typeof(Num)) {
                to = visit((Num)forNode.getTo());
            } else if (toType == typeof(Var)) {
                to = (int) visit((Var)forNode.getTo());
            } else if (toType == typeof(BinOp)) {
                to = (int) visit((BinOp)forNode.getTo());
            } else {
                throw new InterpreterException("Syntax error on For loop condition TO");
            }
            Root statements = new Root();
            statements.setChildren(forNode.getStatements());
            IEnumerable<int> numbers = Enumerable.Range(to,from);
            foreach (var index in numbers) {
                // Update value of the 'counter'
                globalST[forNode.getCounter().GetValue()] = index;
                visit(statements);
            } 
        }

        public void interpret() {
            Root root = this.parser.parse();
            this.visit(root);
        }
    }
}