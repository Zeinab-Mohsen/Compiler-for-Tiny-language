using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public enum Token_Class
{
    Int, Float, String, Read, Write, Repeat, Until, If, Elseif, Else, Then, Return, Endl,
    End, Main,
    LParanthesis, RParanthesis, LBraces, RBraces, EqualOp, LessThanOp, GreaterThanOp, PlusOp, MinusOp, MultiplyOp,
    DivideOp, NotequalOp, Semicolon, Comma, AssignOp, AndOp, OrOP, Idenifier, Constant, Unrecognized,s
}
namespace JASON_Compiler
{


    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {

            ReservedWords.Add("int", Token_Class.Int);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("elseif", Token_Class.Elseif);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add("main", Token_Class.Main);

            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("{", Token_Class.LBraces);
            Operators.Add("}", Token_Class.RBraces);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("<>", Token_Class.NotequalOp);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add(":=", Token_Class.AssignOp);
            Operators.Add("&&", Token_Class.AndOp);
            Operators.Add("||", Token_Class.OrOP);

        }

        public void StartScanning(string SourceCode)
        {
            SourceCode += ' ';
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                // if the entered value is space or any other character it will 
                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;
                // if you read a character
                if ((CurrentChar >= 'A' && CurrentChar <= 'Z') || (CurrentChar >= 'a' && CurrentChar <= 'z'))
                {
                    j++;
                    if (j != SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                    }
                    while (((CurrentChar >= 'A' && CurrentChar <= 'Z') || (CurrentChar >= 'a' && CurrentChar <= 'z') || CurrentChar >= '0' && CurrentChar <= '9') && j != SourceCode.Length)
                    {

                        CurrentLexeme += SourceCode[j++];
                        CurrentChar = SourceCode[j];

                    }
                    j -= 1;
                }
                // if you read a number
                else if (CurrentChar >= '0' && CurrentChar <= '9')
                {
                    j++;
                    if (j != SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                    }

                    while (((CurrentChar >= 'A' && CurrentChar <= 'Z') || (CurrentChar >= 'a' && CurrentChar <= 'z') || CurrentChar >= '0' && CurrentChar <= '9' || CurrentChar == '.') && j != SourceCode.Length)
                    {

                        CurrentLexeme += SourceCode[j++];
                        CurrentChar = SourceCode[j];

                    }

                    j -= 1;
                }
                //if you read a comment
                else if ((CurrentChar == '/' && SourceCode[j + 1] == '*') && j < SourceCode.Length)
                {
                    j++;
                    while (j < SourceCode.Length)
                    {

                        CurrentChar = SourceCode[j];
                        if (CurrentChar == '*' && SourceCode[j + 1] == '/')
                        {
                            CurrentLexeme += SourceCode[j++];
                            CurrentLexeme += SourceCode[j];
                            break;
                        }

                        CurrentLexeme += SourceCode[j++];

                    }

                }
                //if you read assign, and or OR operators
                else if ((CurrentChar == ':' && SourceCode[j + 1] == '=') || (CurrentChar == '&' && SourceCode[j + 1] == '&') || (CurrentChar == '|' && SourceCode[j + 1] == '|'))
                {
                    CurrentLexeme += SourceCode[j + 1];
                    j++;
                }
                // if you read any unusual operator
                else if ((CurrentChar == '<' || CurrentChar == '=' || CurrentChar == '>') && (SourceCode[j + 1] == '=' || SourceCode[j + 1] == '<' || SourceCode[j + 1] == '>'))
                {
                    CurrentLexeme += SourceCode[j + 1];
                    j++;
                }
                // if your read a string
                else if (CurrentChar == '"')
                {
                    while (j != SourceCode.Length - 1)
                    {
                        j++;
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar;
                        if (CurrentChar == '"')
                        {
                            break;
                        }
                    }
                }
                // if you read anything starts with a '.' followed by any compination of characters or letters
                else if (CurrentChar == '.')
                {
                    j++;
                    if (j != SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                    }
                    while (((CurrentChar >= '0' && CurrentChar <= '9') || (CurrentChar >= 'A' && CurrentChar <= 'Z') || (CurrentChar >= 'a' && CurrentChar <= 'z') || CurrentChar == '.') && j != SourceCode.Length)
                    {
                        CurrentLexeme += SourceCode[j++];
                        CurrentChar = SourceCode[j];

                    }
                    j -= 1;
                }
                // does nothing but it can 
                else
                {

                }
                // after finishing a lexeme we assign the valu of j to i
                i = j;
                FindTokenClass(CurrentLexeme);
            }

            TinyCompiler.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?
            if (ReservedWords.Keys.Contains(Lex))
            {
                TC = ReservedWords[Lex];
                Tok.token_type = TC;
                Tokens.Add(Tok);
            }

            //Is it an identifier?
            else if (isIdentifier(Lex))
            {
                TC = Token_Class.Idenifier;
                Tok.token_type = TC;
                Tokens.Add(Tok);
            }

            //Is it a Constant?
            else if (isConstant(Lex))
            {
                TC = Token_Class.Constant;
                Tok.token_type = TC;
                Tokens.Add(Tok);
            }
            //Is it an operator?
            else if (Operators.Keys.Contains(Lex))
            {
                TC = Operators[Lex];
                Tok.token_type = TC;
                Tokens.Add(Tok);
            }
            //Is it a Comment
            else if (isComment(Lex))
            {
                //skip
            }
            //Is it a String
            else if (isString(Lex))
            {
                TC = Token_Class.s;
                Tok.token_type = TC;
                Tokens.Add(Tok);
            }
            // Is it an unrecognized token (1res)?
            else if (isUnrecognized(Lex))
            {
                //Errors.Error_List.Add(Lex);  // Add it to the error list
                TC = Token_Class.Unrecognized;  // Mark as unrecognized token
                Tok.token_type = TC;
                Tokens.Add(Tok);
            }
            // Is it an unrecognized token (.0)?
            else if (isUnrecognized1(Lex))
            {
                //Errors.Error_List.Add(Lex);  // Add it to the error list
                TC = Token_Class.Unrecognized;  // Mark as unrecognized token
                Tok.token_type = TC;
                Tokens.Add(Tok);
            }
            //Is it an undefined?
            else
            {
                Errors.Error_List.Add(Lex);
            }
        }


        // Check if the lex is an identifier or not.
        bool isIdentifier(string lex)
        {
            bool isValid = true;
            var id = new Regex("^[a-zA-Z]([a-zA-Z0-9])*$", RegexOptions.Compiled);
            if (!id.IsMatch(lex))
            {
                isValid = false;
            }
            return isValid;
        }
        // Check if the lex is a Number or not.
        bool isConstant(string lex)
        {
            bool isValid = true;
            var constant = new Regex(@"^[0-9]+(?:\.[0-9]*)?$", RegexOptions.Compiled);
            if (!constant.IsMatch(lex))
            {
                isValid = false;
            }

            return isValid;
        }
        // Check if the lex is a Comment or not.
        bool isComment(string lex)
        {
            bool isValid = true;
            var Comment = new Regex(@"/\*(.)*\*/$", RegexOptions.Compiled);
            if (!Comment.IsMatch(lex))
            {
                isValid = false;
            }
            return isValid;
        }
        // Check if the lex is a String or not.
        bool isString(string lex)
        {
            bool isValid = true;
            var STR = new Regex("\"[^\"]+\"$", RegexOptions.Compiled);
            if (!STR.IsMatch(lex))
            {
                isValid = false;
            }
            return isValid;
        }
        // Check if the lex is (1res)form or not.
        bool isUnrecognized(string lex)
        {
            bool isValid = true;
            var rec = new Regex(@"^[0-9]+(?:[a-zA-Z0-9]+)?$", RegexOptions.Compiled);
            if (!rec.IsMatch(lex))
            {
                isValid = false;
            }

            return isValid;
        }
        // Check if the lex is (.0.0.0)form or not.
        bool isUnrecognized1(string lex)
        {
            bool isValid = true;
            var rec1 = new Regex(@"\..+$", RegexOptions.Compiled);
            if (!rec1.IsMatch(lex))
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
