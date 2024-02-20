using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JASON_Compiler
{
    public class Node
    {
        public List<Node> Children = new List<Node>();
        
        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public  Node root;
        
        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }
        void printError(string Expected, int inputPointer = -1)
        {
            /*if (inputPointer == -1)
                inputPointer = InputPointer;
            Errors.Error_List.Add("Parsing Error: Expected "
                        + Expected + " and " +
                        TokenStream[inputPointer].token_type.ToString() +
                        "  found\r\n");
            InputPointer++;*/
        }
        Node Program()
        {
            Node program = new Node("Program");
            Node FunctionStatements = new Node("Function Statements");
            program.Children.Add(FunctionStatements);
            List<Node> Child = new List<Node>();
            Child = Function_Statements();
            for(int i= 0; i<Child.Count; i++)
            {
                FunctionStatements.Children.Add(Child[i]);

            }
            program.Children.Add(Main_Fuction());
            MessageBox.Show("Success");
            return program;
        }

        List<Node> Function_Statements()
        {
            List<Node> Function_Statements = new List<Node>();
            // write your code here to check the header sructure
            for(int i = 0; InputPointer < TokenStream.Count &&( TokenStream[InputPointer].token_type==Token_Class.Float|| TokenStream[InputPointer].token_type == Token_Class.Int|| TokenStream[InputPointer].token_type == Token_Class.String) ; i++)
            {
                if( TokenStream[InputPointer + 1 ].token_type == Token_Class.Main)
                {
                    break;
                }
                Function_Statements.Add(Function_Statement());
            }
            
            return Function_Statements;
        }

        Node Function_Declaretion()
        {
            Node Function_Declaretion = new Node("Function Declaretion");
            Function_Declaretion.Children.Add(Datatype());
            Function_Declaretion.Children.Add(match(Token_Class.Idenifier));
            Function_Declaretion.Children.Add(match(Token_Class.LParanthesis));
            Function_Declaretion.Children.Add(ArgList());
            Function_Declaretion.Children.Add(match(Token_Class.RParanthesis));
            return Function_Declaretion;
        }

        Node ArgList()
        {
            Node ArgList = new Node("ArgList");
            if(InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.String || TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.Int))
            {
                ArgList.Children.Add(Arguments());
            }
            else
            {
                return null;
            }
            return ArgList; 
        }

        Node Arguments()
        {
            Node Arguments = new Node("Arguments");
            Arguments.Children.Add(Argument());
            Arguments.Children.Add(Argg());
  
            return Arguments;
        }

        Node Argument()
        {
            Node Argument = new Node("Argument");
            Argument.Children.Add(Datatype());
            Argument.Children.Add(match(Token_Class.Idenifier));
            return Argument;
        }

        Node Argg()
        {
            Node Arg = new Node("Arg");
            if(InputPointer<TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                Arg.Children.Add(match(Token_Class.Comma));
                Arg.Children.Add(Argument());
                Arg.Children.Add(Argg());
                return Arg;

            }
            else
            {
                return null;
            }
            
        }
        Node Function_Statement()
        {
            Node Function_Statement = new Node("Function Statement");
            Function_Statement.Children.Add(Function_Declaretion());
            Function_Statement.Children.Add(Function_Body());
            return Function_Statement;
        }

        Node Function_Body()
        {
            Node Function_Body = new Node("Function Body");
            Node FucntionStatements = new Node("Function Statements");
            List<Node> FB = new List<Node>();
            Function_Body.Children.Add(match(Token_Class.LBraces));
            FB = Statements();
            
            for(int i = 0; i < FB.Count; i++)
            {
                FucntionStatements.Children.Add(FB[i]);
            }
            Function_Body.Children.Add(FucntionStatements);
            Function_Body.Children.Add(ReturnStatement());
            Function_Body.Children.Add(match(Token_Class.RBraces));
            return Function_Body;
        }
        

        Node Datatype()
        {
            Node Datatype = new Node("Datatype");
            if(InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Int)
            {
                Datatype.Children.Add(match(Token_Class.Int));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.String)
            {
                Datatype.Children.Add(match(Token_Class.String));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Float)
            {
                Datatype.Children.Add(match(Token_Class.Float));
            }
            return Datatype;
        }
        Node Main_Fuction()
        {
            Node Main = new Node("Main Function");
            // write your code here to check atleast the declare sturcure 
            Main.Children.Add(Datatype());
            Main.Children.Add(match(Token_Class.Main));
            Main.Children.Add(match(Token_Class.LParanthesis));
            Main.Children.Add(match(Token_Class.RParanthesis));
            Main.Children.Add(Function_Body());
            // without adding procedures
            return Main;
        }

        Node Assignment_Statment()
        {
            Node assign = new Node("Assignment Statment");
            assign.Children.Add(match(Token_Class.Idenifier));
            assign.Children.Add(match(Token_Class.AssignOp));
            assign.Children.Add(Expression());
            return assign;
        }

        Node ReturnStatement()
        {
            Node returnStatement = new Node("Return Statement");
            returnStatement.Children.Add(match(Token_Class.Return));
            returnStatement.Children.Add(Expression());
            returnStatement.Children.Add(match(Token_Class.Semicolon));
            return returnStatement;
        }
        List<Node> Statements()
        {
            List<Node> Statements = new List<Node>();
            for (int i = InputPointer; InputPointer < TokenStream.Count; i++)
            {
                if(TokenStream[InputPointer].token_type == Token_Class.Idenifier || TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.String || TokenStream[InputPointer].token_type == Token_Class.Write || TokenStream[InputPointer].token_type == Token_Class.Read || TokenStream[InputPointer].token_type == Token_Class.If || TokenStream[InputPointer].token_type == Token_Class.Repeat)
                {
                    Statements.Add(State());
                }
                else 
                { 
                    break; 
                }
            }
            
            return Statements;
        }

        Node State()
        {
            Node Statee = new Node("Statement");
            if (InputPointer < TokenStream.Count )
            {
                if (TokenStream[InputPointer].token_type == Token_Class.Idenifier || TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.String || TokenStream[InputPointer].token_type == Token_Class.Write || TokenStream[InputPointer].token_type == Token_Class.Read || TokenStream[InputPointer].token_type == Token_Class.If || TokenStream[InputPointer].token_type == Token_Class.Repeat)
                {
                    Statee.Children.Add(Statement());
                    if(InputPointer < TokenStream.Count)
                    {
                    if(TokenStream[InputPointer].token_type != Token_Class.End)
                    {
                        Statee.Children.Add(match(Token_Class.Semicolon));
                    }
                    Statee.Children.Add(State());

                    }
                    return Statee;
                }
                else
                {
                    return Statee;
                }

                
                
            }
            else
            {
                return null;
            }
            
        }

        Node Statement()
        {
            Node Statement = new Node("Statement");
            if(InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
                {
                    if (TokenStream[InputPointer + 1].token_type == Token_Class.AssignOp)
                    {
                        Statement.Children.Add(Assignment_Statment());
                    }
                    else if (TokenStream[InputPointer + 1].token_type == Token_Class.LParanthesis)
                    {
                        Statement.Children.Add(Function_Call());
                    }
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.String)
                {
                    Statement.Children.Add(Declaration_Statement());
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.Write)
                {
                    Statement.Children.Add(Write_Statement());
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.Read)
                {
                    Statement.Children.Add(Read_Statement());
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.If)
                {
                    Statement.Children.Add(If_Statement());
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.Repeat)
                {
                    Statement.Children.Add(Repeat_Statement());
                }
                else
                {
                    return null;
                }
                return Statement;
            }
            else
            {
                printError("Out of index");
            }
            
            return Statement;
        }

        Node Function_Call()
        {
            Node Function_Call = new Node("Function Call");
            Function_Call.Children.Add(match(Token_Class.Idenifier));
            Function_Call.Children.Add(match(Token_Class.LParanthesis));
            Function_Call.Children.Add(ParameterList());
            Function_Call.Children.Add(match(Token_Class.RParanthesis));
            return Function_Call;
        }

        Node ParameterList()
        {
            Node PList = new Node("ParameterList");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                List<Node> es = new List<Node>();
                es = Parameters();
                for (int i = 0; i < es.Count; i++)
                {
                    PList.Children.Add(es[i]);
                }
                return PList;
            }
            else
                return null;
        }
        List<Node> Parameters()
        {
            List<Node> Parameters = new List<Node>();
            for(int i = 0; (TokenStream[InputPointer + 1].token_type == Token_Class.Comma && TokenStream[InputPointer].token_type == Token_Class.Idenifier) && InputPointer < TokenStream.Count; i++)
            {
                Parameters.Add(match(Token_Class.Idenifier));
                Parameters.Add(match(Token_Class.Comma));
            }

            Parameters.Add(match(Token_Class.Idenifier));
            return Parameters;
        }
        Node If_Statement()
        {
            Node ifStatement = new Node("If Statement");
            ifStatement.Children.Add(match(Token_Class.If));
            ifStatement.Children.Add(Condition_Statement());
            ifStatement.Children.Add(match(Token_Class.Then));
            List<Node> es = new List<Node>();
            es = Statements();
            for (int i = 0; i < es.Count; i++)
            {
                ifStatement.Children.Add(es[i]);
            }
            ifStatement.Children.Add(If_end());

            return ifStatement;
        }

        Node If_end()
        {
            Node If_end = new Node("If_end");
            if (InputPointer < TokenStream.Count)
            {
                if(TokenStream[InputPointer].token_type == Token_Class.Elseif)
                {
                    If_end.Children.Add(Elseif_Statement());
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.Else)
                {
                    If_end.Children.Add(Else_Statement());
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.End)
                {
                    If_end.Children.Add(match(Token_Class.End));
                }
                else
                {
                    printError("wrong if end");
                }
            }
            else
            {
                printError("Out of range");
            }
            return If_end;
        }

        Node Elseif_Statement()
        {
            Node Elseif_Statement = new Node("Elseif Statement");
              
            Elseif_Statement.Children.Add(match(Token_Class.Elseif));
            Elseif_Statement.Children.Add(Condition_Statement());
            Elseif_Statement.Children.Add(match(Token_Class.Then));
            List<Node> es = new List<Node>();
            es = Statements();
            for (int i = 0; i < es.Count; i++)
            {
                Elseif_Statement.Children.Add(es[i]);
            }
            
            Elseif_Statement.Children.Add(If_end());
            return Elseif_Statement;
        }

        Node Else_Statement()
        {
            Node Else_Statement = new Node("Elseif Statement");
            Else_Statement.Children.Add(match(Token_Class.Else));
            List<Node> es = new List<Node>();
            es = Statements();
            for (int i = 0; i < es.Count; i++)
            {
                Else_Statement.Children.Add(es[i]);
            }
            Else_Statement.Children.Add(match(Token_Class.End));
            return Else_Statement;
        }

        Node Repeat_Statement()
        {
            Node repeatStatement = new Node("Repeat Statement");
            repeatStatement.Children.Add(match(Token_Class.Repeat));
            List<Node> es = new List<Node>();
            es = Statements();
            for (int i = 0; i < es.Count; i++)
            {
                repeatStatement.Children.Add(es[i]);
            }
            repeatStatement.Children.Add(match(Token_Class.Until));
            repeatStatement.Children.Add(Condition_Statement());
            return repeatStatement;
        }

        Node Condition_Statement()
        {
            Node Condition_Statement = new Node("Condition Statement");
            Condition_Statement.Children.Add(Condition());
            Condition_Statement.Children.Add(Multi_Conditions());
            return Condition_Statement;
        }

        Node Multi_Conditions()
        {
            Node Multi_Conditionss = new Node("Multi Conditions");
            if(InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.AndOp || TokenStream[InputPointer].token_type == Token_Class.OrOP))
            {
                Multi_Conditionss.Children.Add(Boolean_Op());
                Multi_Conditionss.Children.Add(Condition());
                Multi_Conditionss.Children.Add(Multi_Conditions());
                return Multi_Conditionss;
            }
            else
            {
                return null;
            }

        }

        Node Boolean_Op()
        {
            Node Boolean_Op = new Node("Boolean Op");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.AndOp)
            {
                Boolean_Op.Children.Add(match(Token_Class.AndOp));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.OrOP)
            {
                Boolean_Op.Children.Add(match(Token_Class.OrOP));
            }
            else
                printError("Boolean Operator");
            return Boolean_Op;
        }
        Node Condition()
        {
            Node Condition = new Node("Condition");
            Condition.Children.Add(match(Token_Class.Idenifier));
            Condition.Children.Add(Condition_Op());
            Condition.Children.Add(Term());
            return Condition;
        }

        Node Term()
        {
            Node Term = new Node("Term");
            if(InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.Constant)
                {
                    Term.Children.Add(match(Token_Class.Constant));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
                {
                    if(TokenStream[InputPointer+1].token_type == Token_Class.LParanthesis)
                    {
                        Term.Children.Add(Function_Call());
                    }
                    else
                    {
                        Term.Children.Add(match(Token_Class.Idenifier));
                    }
                }
            }
            else
            {
                printError("Out of Range");
            }
            return Term;
        }

        Node Condition_Op()
        {
            Node Condition_Op = new Node("Condition Operator");
            if (InputPointer < TokenStream.Count)
            {
                if(TokenStream[InputPointer].token_type == Token_Class.LessThanOp)
                {
                    Condition_Op.Children.Add(match(Token_Class.LessThanOp));   
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.GreaterThanOp)
                {
                    Condition_Op.Children.Add(match(Token_Class.GreaterThanOp));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.EqualOp)
                {
                    Condition_Op.Children.Add(match(Token_Class.EqualOp));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.NotequalOp)
                {
                    Condition_Op.Children.Add(match(Token_Class.NotequalOp));
                }
                else
                {
                    printError("Wrong OP");
                }
            }
            else
            {
                printError("Out of range");
            }
            return Condition_Op;
        }
        Node Read_Statement()
        {
            Node readStatement = new Node("Read Statement");
            readStatement.Children.Add(match(Token_Class.Read));
            readStatement.Children.Add(match(Token_Class.Idenifier));
            return readStatement;
        }

        Node Declaration_Statement()
        {
            Node declarationStatement = new Node("Declaration Statement");
            declarationStatement.Children.Add(Datatype());
            declarationStatement.Children.Add(ID());
            List<Node> es = new List<Node>();
            es = IdList();
            for (int i = 0; i < es.Count; i++)
            {
                declarationStatement.Children.Add(es[i]);
            }
            return declarationStatement;
        }
        List<Node> IdList()
        {
            List<Node> IdList = new List<Node>();
            
            IdList.Add(IdenList());
            return IdList;
        }
        
        Node IdenList()
        {
            Node IdenListt = new Node("IdenList");
            if(TokenStream[InputPointer].token_type == Token_Class.Comma && InputPointer < TokenStream.Count)
            {
                IdenListt.Children.Add(match(Token_Class.Comma));
                IdenListt.Children.Add(ID());
                IdenListt.Children.Add(IdenList());
                return IdenListt;
            }
            else
            {
                return null;
            }
            
        }

        Node ID()
        {
            Node ID = new Node("ID");
            if ((TokenStream[InputPointer].token_type == Token_Class.Idenifier && TokenStream[InputPointer+1].token_type == Token_Class.AssignOp) && InputPointer < TokenStream.Count)
            {
                ID.Children.Add(Assignment_Statment());
            }
            else if ((TokenStream[InputPointer].token_type == Token_Class.Idenifier && TokenStream[InputPointer + 1].token_type != Token_Class.AssignOp) && InputPointer < TokenStream.Count)
            {
                ID.Children.Add(match(Token_Class.Idenifier));  
            }
            else
            {
                printError("wrong ID");
            }

                return ID;
        }
        Node Write_Statement()
        {
            Node write_Statement = new Node("Write Statement");
            write_Statement.Children.Add(match(Token_Class.Write));
            write_Statement.Children.Add(Write_State());
            return write_Statement;
        }

        Node Write_State()
        {
            Node Write_State = new Node("WriteState");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Endl)
            {
                Write_State.Children.Add(match(Token_Class.Endl));
            }
            else if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.s ||
                TokenStream[InputPointer].token_type == Token_Class.LParanthesis || (TokenStream[InputPointer].token_type == Token_Class.Constant || TokenStream[InputPointer].token_type == Token_Class.Idenifier)))
            {
                Write_State.Children.Add(Expression());
            }
            return Write_State;
        }

        Node Expression()
        {
            Node Expression = new Node("Expression");
            if (InputPointer < TokenStream.Count)
            {
                if(TokenStream[InputPointer].token_type == Token_Class.s)
                {
                    Expression.Children.Add(match(Token_Class.s));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.Constant || TokenStream[InputPointer].token_type == Token_Class.Idenifier)
                {
                    if (TokenStream[InputPointer + 1].token_type == Token_Class.LParanthesis)
                    {
                        Expression.Children.Add(Term());

                    }
                    else 
                    {
                        if(TokenStream[InputPointer].token_type == Token_Class.Constant)
                        {
                            Expression.Children.Add(match(Token_Class.Constant));
                        }
                        else if(TokenStream[InputPointer + 1].token_type == Token_Class.PlusOp || TokenStream[InputPointer + 1].token_type == Token_Class.MinusOp|| TokenStream[InputPointer + 1].token_type == Token_Class.MultiplyOp|| TokenStream[InputPointer + 1].token_type == Token_Class.DivideOp)
                        {
                            Expression.Children.Add(Equationn());
                        }
                        else
                        {
                            Expression.Children.Add(match(Token_Class.Idenifier));
                        }

                    }
                }

                else
                {
                    Expression.Children.Add(Equationn());
                }

            }
            else
            {
                printError("out of range");
            }
            return Expression;
        }
        Node Equationn()
        {
            Node Equation = new Node("Equation");
            if (InputPointer < TokenStream.Count)
            {
                if(TokenStream[InputPointer].token_type == Token_Class.Constant || TokenStream[InputPointer].token_type == Token_Class.Idenifier || (TokenStream[InputPointer].token_type == Token_Class.Idenifier && TokenStream[InputPointer+1].token_type == Token_Class.LParanthesis))
                {
                    Equation.Children.Add(Term());
                    Equation.Children.Add(Eq());
                }
                else if(TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
                {
                    Equation.Children.Add(match(Token_Class.LParanthesis));
                    Equation.Children.Add(Equationn());
                    Equation.Children.Add(Arth_Op());
                    Equation.Children.Add(Equationn());
                    Equation.Children.Add(match(Token_Class.RParanthesis));
                    Equation.Children.Add(Eq());

                }
            }
            return Equation;
        }
        Node Eq()
        {
            Node eq = new Node("Eq");
            if(InputPointer < TokenStream.Count)
            {
                if(TokenStream[InputPointer].token_type == Token_Class.PlusOp || TokenStream[InputPointer].token_type == Token_Class.DivideOp|| TokenStream[InputPointer].token_type == Token_Class.MultiplyOp|| TokenStream[InputPointer].token_type == Token_Class.MinusOp)
                {
                    eq.Children.Add(Arth_Op());
                    eq.Children.Add(EqDash());
                }
                else
                {
                    return null;
                }
            }
            return eq;
        }

        Node EqDash()
        {
            Node EgDash = new Node("EqDash");
            if (TokenStream[InputPointer].token_type == Token_Class.Constant || TokenStream[InputPointer].token_type == Token_Class.Idenifier || TokenStream[InputPointer].token_type == Token_Class.Constant)
            {
                EgDash.Children.Add(Equationn());
                EgDash.Children.Add(Eq());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                EgDash.Children.Add(match(Token_Class.LParanthesis));
                EgDash.Children.Add(Equationn());
                EgDash.Children.Add(Arth_Op());
                EgDash.Children.Add(Equationn());
                EgDash.Children.Add(match(Token_Class.RParanthesis));
                EgDash.Children.Add(Eq());

            }
            return EgDash;
        }

        Node Arth_Op()
        {
            Node Arth_Op = new Node("Arth_Op");
            if(InputPointer < TokenStream.Count)
            {
                if(TokenStream[InputPointer].token_type == Token_Class.PlusOp)
                {
                    Arth_Op.Children.Add(match(Token_Class.PlusOp));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.MinusOp)
                {
                    Arth_Op.Children.Add(match(Token_Class.MinusOp));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.DivideOp)
                {
                    Arth_Op.Children.Add(match(Token_Class.DivideOp));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.MultiplyOp)
                {
                    Arth_Op.Children.Add(match(Token_Class.MultiplyOp));
                }

            }
            return Arth_Op;
        }
        // Implement your logic here

        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;

                }

                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString()  + "\r\n");
                InputPointer++;
                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
